using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FunS.Utility.Editor.Image
{
    public class ImageEncode
    {
        public static byte[] EncodeTexture2DTo(Texture2D tex, ImageType type, int? jpgQuality = null)
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
    }
}