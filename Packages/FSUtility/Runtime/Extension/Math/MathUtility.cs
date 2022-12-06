using UnityEngine;

namespace FunS.Utility
{
    public static class MathUtility
    {
        public static float PowerInt(this float value, int exp)
        {
            float output = 1f;
            while (exp > 0) 
            {
                exp--;
                output *= value;
            }
            return output;
        }

        //https://stackoverflow.com/questions/15967240/fastest-implementation-of-log2int-and-log2float
        public static int Log2(this int value)
        {
            int r = 0xFFFF - value >> 31 & 0x10;
            value >>= r;
            int shift = 0xFF - value >> 31 & 0x8;
            value >>= shift;
            r |= shift;
            shift = 0xF - value >> 31 & 0x4;
            value >>= shift;
            r |= shift;
            shift = 0x3 - value >> 31 & 0x2;
            value >>= shift;
            r |= shift;
            r |= (value >> 1);
            return r;
        }

        //https://forum.unity.com/threads/solved-how-to-get-rotation-value-that-is-in-the-inspector.460310/#post-2989387
        public static float WrapAngle(this float angle)
        {
            angle %= 360;
            if (angle > 180)
                return angle - 360;

            return angle;
        }

        public static float UnwrapAngle(this float angle)
        {
            if (angle >= 0)
                return angle;

            angle = -angle % 360;

            return 360 - angle;
        }
    }
}
