using System;
using UnityEngine;

namespace Universe
{
    public static class GlobalTime
    {
        public const long TimeOfReset = TimeSpan.TicksPerDay; //1 day

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
                if (sinTimeLast == UnityEngine.Time.frameCount)
                    return _sinTime;
                _sinTime = Mathf.Sin(UnityEngine.Time.time);
                sinTimeLast = (byte)UnityEngine.Time.frameCount;
                return _sinTime;
            }
        }

        public static float Time => TimeOfInit + UnityEngine.Time.time;

        public static double TotalAge
        {
            get
            {
                return DateTime.UtcNow.Ticks * TimeSpan.TicksPerSecond;
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
    }
}
