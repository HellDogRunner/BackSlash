using System;
using UnityEngine;

namespace RedMoonGames.Basics
{
    [Serializable]
    public struct Timestamp
    {
        [SerializeField] private long _ticksTimestamp;
        [SerializeField] private bool _isValidTimestamp;

        public long TicksTimestamp
        {
            get => _ticksTimestamp;
            private set
            {
                _ticksTimestamp = value;
            }
        }

        public bool IsValidTimestamp
        {
            get => _isValidTimestamp;
            private set
            {
                _isValidTimestamp = value;
            }
        }


        public Timestamp(long ticks)
        {
            _ticksTimestamp = ticks;
            _isValidTimestamp = _ticksTimestamp > 0;
        }

        public Timestamp(DateTime dateTime)
        {
            _ticksTimestamp = dateTime.Ticks;
            _isValidTimestamp = _ticksTimestamp > 0;
        }

        public static Timestamp Now
        {
            get => new Timestamp(DateTime.UtcNow);
        }

        public static implicit operator DateTime(Timestamp timestamp)
        {
            return timestamp.AsDateTime;
        }

        public DateTime AsDateTime
        {
            get => new DateTime(TicksTimestamp);
        }

        public TimeSpan TimeDelta
        {
            get
            {
                var timeDelta = DateTime.UtcNow - this;
                return timeDelta;
            }
        }
    }
}
