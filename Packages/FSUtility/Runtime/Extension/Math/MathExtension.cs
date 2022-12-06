using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FunS.Utility
{
    public static class MathExtension
    {
        public static readonly Vector4 Matrix2x2Identity = new Vector4(1, 0, 0, 1);

        public static float GetSqrDistance(Vector3 a, Vector3 b)
        {
            return (a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y) + (a.z - b.z) * (a.z - b.z);
        }

        public static bool CheckInSqrDistance(float squDistance, float target)
        {
            return squDistance < target * target;
        }

        public static int PowerInt(int value, int exp)
        {
            int output = 1;
            for (int i = 0; i < exp; i++)
            {
                output *= value;
            }
            return output;
        }

        public static int Power2Int(int exp)
        {
            int output = 1;
            for (int i = 0; i < exp; i++)
            {
                output *= 2;
            }
            return output;
        }

        //https://stackoverflow.com/questions/15967240/fastest-implementation-of-log2int-and-log2float
        public static int Log2(int v)
        {
            int r = 0xFFFF - v >> 31 & 0x10;
            v >>= r;
            int shift = 0xFF - v >> 31 & 0x8;
            v >>= shift;
            r |= shift;
            shift = 0xF - v >> 31 & 0x4;
            v >>= shift;
            r |= shift;
            shift = 0x3 - v >> 31 & 0x2;
            v >>= shift;
            r |= shift;
            r |= (v >> 1);
            return r;
        }

        //https://forum.unity.com/threads/solved-how-to-get-rotation-value-that-is-in-the-inspector.460310/#post-2989387
        public static float WrapAngle(float angle)
        {
            angle %= 360;
            if (angle > 180)
                return angle - 360;

            return angle;
        }

        public static float UnwrapAngle(float angle)
        {
            if (angle >= 0)
                return angle;

            angle = -angle % 360;

            return 360 - angle;
        }

        /// <summary>
        /// Get Gaze Angle(without Acos)
        /// </summary>
        /// <returns>-1~0~1</returns>
        public static float FastGetGazeAngle(Vector3 forward, Vector3 targetDir)
        {
            return Vector3.Dot(forward, targetDir);
        }
    }
}
