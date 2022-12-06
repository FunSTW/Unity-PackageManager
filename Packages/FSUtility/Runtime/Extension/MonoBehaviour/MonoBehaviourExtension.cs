using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FunS.Utility
{
    public static class MonoBehaviourExtension
    {
        #region Transform
        public static void LocalReset(this Transform transform)
        {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        }

        public static void LookAtReversal(this Transform transform, Transform lookingAt)
        {
            transform.LookAt(2 * transform.position - lookingAt.position);
        }
        #endregion

        #region Camera
        /// <summary>
        /// Get Gaze Angle(without Acos)
        /// </summary>
        /// <returns>1 Face, -1 Back</returns>
        public static float FastGetGazeAngle(this Transform camera, Transform lookingAt)
        {
            Vector3 targetDir = (lookingAt.position - camera.position).normalized;
            return MathUtility.FastGetGazeAngle(camera.forward, targetDir);
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

