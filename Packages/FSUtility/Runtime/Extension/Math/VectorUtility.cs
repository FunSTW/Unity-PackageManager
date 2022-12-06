using UnityEngine;

namespace FunS.Utility
{
    public static class VectorUtility
    {
        /// <summary>
        /// Get distance without square root.
        /// </summary>
        public static float GetSqrDistance(this Vector3 a, Vector3 b)
        {
            return (a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y) + (a.z - b.z) * (a.z - b.z);
        }

        /// <summary>
        /// Get gazing target angle without Acos.
        /// </summary>
        /// <returns> -1(Back) ~ 0(Side) ~ 1(Front)</returns>
        public static float FastGetAngle(this Vector3 forward, Vector3 dir)
        {
            return Vector3.Dot(forward, dir);
        }
    }
}