using UnityEngine;

namespace FunS.Utility
{
    public static class MonoBehaviourUtility
    {
        #region Transform
        public static void ResetLocal(this Transform transform)
        {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        }

        public static void ResetGlobal(this Transform transform)
        {
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        }

        public static void LookAtReversal(this Transform transform, Transform lookingAt)
        {
            transform.LookAt(2 * transform.position - lookingAt.position);
        }
        #endregion

        #region Camera
        /// <summary>
        /// Get gazing target angle without Acos.
        /// </summary>
        /// <returns> -1(Back) ~ 0(Side) ~ 1(Front)</returns>
        public static float FastGetGazeAngle(this Transform camera, Transform gazingTarget)
        {
            Vector3 dir = (gazingTarget.position - camera.position).normalized;
            return camera.forward.FastGetAngle(dir);
        }
        #endregion

        #region AudioSource
        public static void Play(this AudioSource source, AudioClip clip)
        {
            source.clip = clip;
            source.Play();
        }

        public static void Play(this AudioSource source, AudioClip clip, float pitch)
        {
            source.clip = clip;
            source.pitch = pitch;
            source.Play();
        }
        #endregion
    }
}

