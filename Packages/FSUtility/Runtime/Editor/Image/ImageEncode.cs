using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FunS.Utility.Image
{
    public class ImageEncode
    {
        public static byte[] EncodeTexture2DTo(Texture2D tex, Image2DType type, int? jpgQuality = null)
        {
            switch (type)
            {
                case Image2DType.jpg:
                    if (jpgQuality != null)
                    {
                        return tex.EncodeToJPG(jpgQuality.Value);
                    }
                    else
                    {
                        return tex.EncodeToJPG();
                    }
                case Image2DType.png:
                    return tex.EncodeToPNG();
                case Image2DType.tga:
                    return tex.EncodeToTGA();
                default:
                    return null;
            }
        }
    }
}