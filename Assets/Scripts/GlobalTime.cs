using System;

namespace Universe
{
    public static class GlobalTime
    {
        public const long TimeOfReset = TimeSpan.TicksPerDay; //1 week

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

        public static float Time => TimeOfInit + UnityEngine.Time.time;

        public static float TotalAge
        {
            get
            {
                return DateTime.UtcNow.Ticks * TimeSpan.TicksPerSecond + UnityEngine.Time.time;
            }
        }

        private static float GetTimeOfInit()
        {
            float currentTime = (DateTime.UtcNow.Ticks % TimeOfReset) / (float)TimeSpan.TicksPerSecond;
            return currentTime - UnityEngine.Time.time;
        }
    }
}
