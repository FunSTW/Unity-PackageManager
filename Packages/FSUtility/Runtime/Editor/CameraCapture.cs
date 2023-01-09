using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FunS.Utility.Editor
{
    using FunS.Utility.Editor.Image;

    public class CameraCapture : ScriptableWizard
    {
        [Header("Require")]
        public Camera camera;

        [Header("Setting")]
        public bool renderCubemap;

        [Header("File")]
        new public string name = "Capture";
        public string data = "yyyy-MM-dd_HH-mm-ss";

        [Header("Quality")]
        public ImageType imageType;
        [Range(1, 100)] public int jpgQuality = 90;
        [Range(0, 8)] public int antiAliasing = 8;

        [Header("2D")]
        [Range(0.25f, 3)] public float renderScale = 1;

        [Header("Cube")]
        public ImageCubeSize imageCubeSize = ImageCubeSize._512;

        private string Name => $"{name}_{DateTime.Now.ToString(data)}";
        private bool IsGammaSpace => PlayerSettings.colorSpace == ColorSpace.Gamma;

        //For gamma correction color distortion.
        private TextureFormat OutputTempTextureFormat
        {
            get
            {
                bool supportRGBA64 = SystemInfo.IsFormatSupported(UnityEngine.Experimental.Rendering.GraphicsFormat.R16G16B16A16_UNorm, UnityEngine.Experimental.Rendering.FormatUsage.GetPixels);

                if (!IsGammaSpace && !supportRGBA64)
                {
                    ErrorMessage =
                        "You are using linear color space to capture, the image will Gamma corrected, " +
                        "But Unity 2020.3 and below do not support ReadPixels in TextureFormat.RGBA64, " +
                        "This will distort the color of the image during gamma correction. " +
                        "(https://forum.unity.com/threads/screenshot-encoding-very-dark-in-linear-color-space.865147/#post-8712648)";
                }

                return (IsGammaSpace || !supportRGBA64) ? TextureFormat.RGBA32 : TextureFormat.RGBA64;
            }
        }

        private string Message;
        private string ErrorMessage;

        [MenuItem("CONTEXT/Camera/Capture To Image", priority = 500)]
        private static void CaptureToImage()
        {
            var window = ScriptableWizard.DisplayWizard<CameraCapture>("FSUtility Camera Capture", "Close", "Capture!");
            window.Init();
        }

        [MenuItem("CONTEXT/Camera/Snapshot Capture", priority = 501)]
        private static void SnapshotCapture()
        {
            var window = new CameraCapture();
            window.Init();
            window.name = "Snapshot";
            window.OnWizardOtherButton();
        }

        [MenuItem("CONTEXT/Camera/Snapshot Capture Cubemap", priority = 502)]
        private static void SnapshotCaptureCubemap()
        {
            var window = new CameraCapture();
            window.Init();
            window.name = "Snapshot";
            window.renderCubemap = true;
            window.OnWizardOtherButton();
        }


        private void Init()
        {
            isValid = (Selection.activeObject as GameObject).TryGetComponent(out camera);
            RefreshMessage();
        }

        private void OnValidate()
        {
            RefreshMessage();
        }

        private void OnWizardUpdate()
        {
            isValid = camera != null;
            RefreshMessage();
        }

        private void OnWizardCreate()
        {
            //Just close.
        }

        private void OnWizardOtherButton()
        {
            Texture2D tex;

            if (!renderCubemap) tex = RenderTexture2D(camera, Mathf.RoundToInt(camera.pixelWidth * renderScale), Mathf.RoundToInt(camera.pixelHeight * renderScale));
            else tex = RenderCubemap(camera, (int)imageCubeSize);

            if (!IsGammaSpace) tex.ToGamma();

            string outName = Name;
            string relativePath = $"{outName}.{Enum.GetName(typeof(ImageType), imageType)}";
            string assetRelativePath = $"Assets/{relativePath}";
            string path = $"{Application.dataPath}/{relativePath}";

            System.IO.File.WriteAllBytes(path, ImageEncode.EncodeTexture2DTo(tex, imageType, jpgQuality));
            AssetDatabase.Refresh();

            //https://forum.unity.com/threads/how-to-use-textureimporter-to-change-textures-format-and-re-import-again.86177/#post-3494294
            TextureImporter textureImporter = (TextureImporter)AssetImporter.GetAtPath(assetRelativePath);
            textureImporter.npotScale = TextureImporterNPOTScale.None;
            EditorUtility.SetDirty(textureImporter);
            textureImporter.SaveAndReimport();

            EditorGUIUtility.PingObject(textureImporter);

            Message = $"Success! Save image to '{assetRelativePath}'";
            RefreshMessage();

            DestroyImmediate(tex);
        }

        private void RefreshMessage()
        {
            if (camera != null)
            {
                string Size2DInfo = $"{(int)(camera.pixelWidth * renderScale)},{(int)(camera.pixelHeight * renderScale)} => GameView Size * Render Scale";
                string SizeCubeInfo = $"{(int)imageCubeSize * 2},{(int)imageCubeSize} => Equirectangular 2:1";

                errorString = ErrorMessage;
                helpString =
                    $"Capture Info :" +
                    $"\n\tName : {Name}" +
                    $"\n\tWidth,Height : {(renderCubemap ? SizeCubeInfo : Size2DInfo)}";
                if (!String.IsNullOrEmpty(Message))
                    helpString += $"\n\n\t{Message}";
            }
            else
            {
                errorString = "Camera is missing, did you delete it?";
                helpString = String.Empty;
            }
        }

        private Texture2D RenderTexture2D(Camera mCamera, int mWidth, int mHeight)
        {
            RenderTexture rt = new RenderTexture(mWidth, mHeight, 0, RenderTextureFormat.DefaultHDR);
            rt.antiAliasing = antiAliasing;

            mCamera.targetTexture = rt;
            mCamera.Render();

            RenderTexture.active = rt;
            Texture2D output = new Texture2D(mWidth, mHeight, OutputTempTextureFormat, false);
            output.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);

            output.Apply();
            mCamera.targetTexture = null;
            RenderTexture.active = null;

            rt.Release();
            DestroyImmediate(rt);

            return output;
        }

        //https://forum.unity.com/threads/camera-rendertocubemap-including-orientation.534469/
        public Texture2D RenderCubemap(Camera mCamera, int mSize)
        {
            RenderTexture cubemapRt = new RenderTexture(mSize, mSize, 0, RenderTextureFormat.Default);
            cubemapRt.dimension = UnityEngine.Rendering.TextureDimension.Cube;
            cubemapRt.antiAliasing = antiAliasing;

            mCamera.RenderToCubemap(cubemapRt, 0b111111, Camera.MonoOrStereoscopicEye.Mono);
            RenderTexture equirectRt = new RenderTexture(mSize * 2, mSize, 0, RenderTextureFormat.Default);
            cubemapRt.ConvertToEquirect(equirectRt, Camera.MonoOrStereoscopicEye.Mono);

            RenderTexture.active = equirectRt;
            Texture2D output = new Texture2D(equirectRt.width, equirectRt.height, OutputTempTextureFormat, false);
            output.ReadPixels(new Rect(0, 0, equirectRt.width, equirectRt.height), 0, 0);
            output.Apply();
            RenderTexture.active = null;

            cubemapRt.Release();
            equirectRt.Release();
            DestroyImmediate(cubemapRt);
            DestroyImmediate(equirectRt);

            return output;
        }
    }
}