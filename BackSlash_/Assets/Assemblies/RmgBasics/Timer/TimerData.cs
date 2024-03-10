using System;
using UnityEngine;

namespace RedMoonGames.Basics.Timer
{
    [Serializable]
    public struct TimerData
    {
        public NullableFloat InitValue;
        public float Seconds;
        public bool IsTimerFinished;

        public bool IsInitialized
        {
            get => (float?)InitValue != null;
        }

        public float ClampedSeconds 
        {
            get 
            {
                var clampedSeconds = Math.Max(0, Seconds);
                clampedSeconds = Math.Min(InitValue, clampedSeconds);

                return clampedSeconds;
            }
        }

        public float TimeRemaining
        {
            get => InitValue - ClampedSeconds;
        }

        public float FillAmount
        {
            get
            {
                if (!IsInitialized || InitValue == 0f)
                {
                    return 0f;
                }

                return Mathf.Clamp01(ClampedSeconds / InitValue);
            }
        }
    }
}
