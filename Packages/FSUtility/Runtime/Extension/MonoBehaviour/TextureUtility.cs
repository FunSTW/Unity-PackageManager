using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FunS.Utility
{
    public static class TextureUtility
    {
        public static void ToGamma(this Texture2D scr)
        {
            var colors = scr.GetPixels();
            for (int i = 0; i < colors.Length; i++) colors[i] = colors[i].gamma;
            scr.SetPixels(colors);
            scr.Apply();
        }

        public static void ToLinear(this Texture2D scr)
        {
            var colors = scr.GetPixels();
            for (int i = 0; i < colors.Length; i++) colors[i] = colors[i].linear;
            scr.SetPixels(colors);
            scr.Apply();
        }
    }
}
