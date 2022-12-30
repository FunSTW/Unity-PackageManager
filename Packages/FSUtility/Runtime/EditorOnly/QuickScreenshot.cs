#if UNITY_EDITOR
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using UnityEngine;
using UnityEditor;
using System;

[RequireComponent(typeof(Camera))]
#if ODIN_INSPECTOR
[InfoBox("It's editor only tool.")]
#endif
public class QuickScreenshot : MonoBehaviour
{
#if ODIN_INSPECTOR
    [Title("Name")]
#else
    [Header("It's a editor only tool, \nRight-Click this script topbar and click 'CaptureScreenshot' to save image.")]
    [Header("Name")]
#endif
    new public string name = "Screenshot";
    public int nameOfCount = 0;

#if ODIN_INSPECTOR
    [Title("Quality")]
#else
    [Header("Quality")]
#endif
    public ImageType imageType;
    [Range(1, 100)] public int JPGQuality = 90;
    [Range(0.25f, 3)] public float renderScale = 1;
    [Range(0, 8)] public int antiAliasing = 8;

#if ODIN_INSPECTOR
    [Title("ReadOnly")]
    [ReadOnly]
#else
    [Header("ReadOnly")]
#endif
    new public Camera camera;

#if ODIN_INSPECTOR
    [Title("File")]
    [ShowInInspector]
#endif
    public string Name => $"{name}{nameOfCount:D3}";

#if ODIN_INSPECTOR
    [ShowInInspector]
#endif
    public string SizeInfo => $"W:{(int)(camera.pixelWidth * renderScale)} / H:{(int)(camera.pixelHeight * renderScale)}";

    public enum ImageType
    {
        jpg,
        png,
        tga
    }

#if ODIN_INSPECTOR
    [Button(size: ButtonSizes.Large), PropertyOrder(-1)]
#endif 
    [ContextMenu("CaptureScreenshot")]
    public void CaptureScreenshot()
    {
        int width = (int)(camera.pixelWidth * renderScale);
        int height = (int)(camera.pixelHeight * renderScale);

        Texture2D tex = RTImage(camera, width, height);

        nameOfCount++;
        string outName = Name;
        string relativePath = $"{outName}.{Enum.GetName(typeof(ImageType), imageType)}";
        string assetRelativePath = $"Assets/{relativePath}";
        string path = $"{Application.dataPath}/{relativePath}";

        Debug.Log($"Capture image '{outName}', save in '{assetRelativePath}'");

        System.IO.File.WriteAllBytes(path, EncodeTo(tex, imageType, JPGQuality));
        AssetDatabase.Refresh();

        var pingObj = AssetDatabase.LoadMainAssetAtPath(assetRelativePath);
        EditorGUIUtility.PingObject(pingObj);
    }

    private Texture2D RTImage(Camera mCamera, int mWidth, int mHeight)
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

    private static byte[] EncodeTo(Texture2D tex, ImageType type, int? jpgQuality = null)
    {
        switch (type)
        {
            case ImageType.jpg:
                if (jpgQuality != null)
                {
                    return tex.EncodeToJPG(jpgQuality.Value);
                }
                else
                {
                    return tex.EncodeToJPG();
                }
            case ImageType.png:
                return tex.EncodeToPNG();
            case ImageType.tga:
                return tex.EncodeToTGA();
            default:
                return null;
        }
    }
    private void OnValidate()
    {
        if (camera == null) camera = GetComponent<Camera>();

    }
}
#endif