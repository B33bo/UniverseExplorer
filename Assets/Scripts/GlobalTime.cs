using System;
using UnityEngine;

namespace Universe
{
    public static class GlobalTime
    {
        public const long TimeOfReset = TimeSpan.TicksPerDay;

        private static float? _TimeOfInit;
        public static float TimeOfInit
        {
            get
            {
                if (!_TimeOfInit.HasValue)
                    _TimeOfInit = GetTimeOfInit();

                return _TimeOfInit.Value;
            }
        }

        private static float _sinTime = 0;
        private static byte sinTimeLast = 0;
        public static float SinTime
        {
            get
            {
                if (sinTimeLast == (byte)UnityEngine.Time.frameCount)
                    return _sinTime;
                _sinTime = Mathf.Sin(UnityEngine.Time.time);
                sinTimeLast = (byte)UnityEngine.Time.frameCount;
                return _sinTime;
            }
        }

        private static byte rotationTimeLast = 0;
        private static Quaternion _rotation;

        private static float timeSinceImportantUpdate;
        public delegate void ImportantUpdate(float dt);
        public static event ImportantUpdate OnImportantUpdate;

        public static Quaternion Rotation
        {
            get
            {
                if (rotationTimeLast == (byte)UnityEngine.Time.frameCount)
                    return _rotation;
                _rotation = Quaternion.Euler(0, 0, Time);
                rotationTimeLast = (byte)UnityEngine.Time.frameCount;
                return _rotation;
            }
        }
        public static bool IsImportantFrame => UnityEngine.Time.frameCount % 4 == 0;

        public static float Time => TimeOfInit + UnityEngine.Time.time;

        public static double TotalAge
        {
            get
            {
                return DateTime.UtcNow.Ticks / (double)TimeSpan.TicksPerSecond;
            }
        }

        private static float GetTimeOfInit()
        {
            float currentTime = (DateTime.UtcNow.Ticks % TimeOfReset) / (float)TimeSpan.TicksPerSecond;
            return currentTime - UnityEngine.Time.time;
        }

        public static void SetTime(float time)
        {
            _TimeOfInit = time - UnityEngine.Time.time;
        }

        public static void MaybeInvokeImportantUpdate()
        {
            timeSinceImportantUpdate += UnityEngine.Time.deltaTime;
            if (!IsImportantFrame)
                return;
            OnImportantUpdate?.Invoke(timeSinceImportantUpdate);
            timeSinceImportantUpdate = 0;
        }
    }
}
