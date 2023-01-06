using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FunS.Utility
{
    using FunS.Utility.Image;
    public class CameraCapture : ScriptableWizard
    {
        [Header("Require")]
        public Camera camera;
        [Header("Name")]
        new public string name = "Screenshot";
        public int nameOfCount = 0;
        [Header("Quality")]
        public Image2DType imageType;
        [Range(1, 100)] public int JPGQuality = 90;
        [Range(0.25f, 3)] public float renderScale = 1;
        [Range(0, 8)] public int antiAliasing = 8;

        private string Name => $"{name}{nameOfCount:D3}";
        private string SizeInfo => $"W:{(int)(camera.pixelWidth * renderScale)} / H:{(int)(camera.pixelHeight * renderScale)}";

        private string Message;

        private void OnValidate()
        {
            RefreshMessage();
        }

        void OnWizardUpdate()
        {
            if (camera != null)
            {
                isValid = true;
            }
            else
            {
                isValid = false;
                if ((Selection.activeObject as GameObject).TryGetComponent(out camera))
                {
                    isValid = true;
                }
            }
        }

        void OnWizardCreate()
        {

        }

        void OnWizardOtherButton()
        {
            int width = (int)(camera.pixelWidth * renderScale);
            int height = (int)(camera.pixelHeight * renderScale);

            Texture2D tex = RenderCameraTexture(camera, width, height);

            nameOfCount++;
            string outName = Name;
            string relativePath = $"{outName}.{Enum.GetName(typeof(Image2DType), imageType)}";
            string assetRelativePath = $"Assets/{relativePath}";
            string path = $"{Application.dataPath}/{relativePath}";

            System.IO.File.WriteAllBytes(path, ImageEncode.EncodeTexture2DTo(tex, imageType, JPGQuality));
            AssetDatabase.Refresh();

            var pingObj = AssetDatabase.LoadMainAssetAtPath(assetRelativePath);
            EditorGUIUtility.PingObject(pingObj);

            Message = $"Save in '{assetRelativePath}'";
            RefreshMessage();
        }

        [MenuItem("CONTEXT/Camera/Capture to image")]
        static void CaptureToImage()
        {
            ScriptableWizard.DisplayWizard<CameraCapture>("FSUtility Camera Capture", "Close", "Capture!");
        }

        private void RefreshMessage()
        {
            if (camera != null)
            {
                helpString =
                    $"Capture Info :" +
                    $"\n\tName : {Name}" +
                    $"\n\tSize : {SizeInfo}";
                if (!String.IsNullOrEmpty(Message))
                    helpString += $"\n\n\t{Message}";
            }
        }

        private Texture2D RenderCameraTexture(Camera mCamera, int mWidth, int mHeight)
        {
            Rect rect = new Rect(0, 0, mWidth, mHeight);
            RenderTexture renderTexture = new RenderTexture(mWidth, mHeight, 24);
            renderTexture.name = $"[EditorOnly] CaptureScreenshot {Name}";
            renderTexture.antiAliasing = antiAliasing;
            Texture2D screenShot = new Texture2D(mWidth, mHeight, TextureFormat.RGBA32, false);

            mCamera.targetTexture = renderTexture;
            mCamera.Render();

            RenderTexture.active = renderTexture;
            screenShot.ReadPixels(rect, 0, 0);

            mCamera.targetTexture = null;
            RenderTexture.active = null;

            DestroyImmediate(renderTexture);
            renderTexture = null;
            GC.Collect();
            return screenShot;
        }
    }
}