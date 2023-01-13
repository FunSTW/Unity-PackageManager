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

        /// <summary>
        /// Returns the maximum of vector xyz
        /// </summary>
        /// <returns>The maximum value</returns>
        public static float Max(this Vector3 vector)
        {
            return Mathf.Max(Mathf.Max(vector.x, vector.y), vector.z);
        }

        /// <summary>
        /// Returns the minimun of vector xyz
        /// </summary>
        /// <returns>The minimum value</returns>
        public static float Min(this Vector3 vector)
        {
            return Mathf.Min(Mathf.Min(vector.x, vector.y), vector.z);
        }
    }
}