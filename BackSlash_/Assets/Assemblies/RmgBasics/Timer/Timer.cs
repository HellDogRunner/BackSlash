using System;

namespace RedMoonGames.Basics.Timer
{
    public class Timer
    {
        private TimerData _data = new TimerData();

        public event Action TimerFinished;
        public event Action TimerUpdated;

        public TimerData Data => _data;

        public void Load(TimerData data) 
        {
            _data = data;
        }

        public void Init(float initTime)
        {
            Reset();
            _data.InitValue = initTime;
        }

        public void DeInit() 
        {
            _data.InitValue = null;
            Reset();
        }

        public void Reset() 
        {
            _data.Seconds = 0;
            _data.IsTimerFinished = false;
        }

        public void SetFinished()
        {
            if (!_data.IsInitialized || _data.IsTimerFinished)
            {
                return;
            }

            _data.Seconds = _data.InitValue;
            _data.IsTimerFinished = true;
            TimerFinished?.Invoke();
        }

        public void Update(float deltaTime)
        {
            if(!_data.IsInitialized)
            {
                return;
            }
             
            _data.Seconds += deltaTime;

            if (!_data.IsTimerFinished && _data.Seconds >= _data.InitValue)
            {
                SetFinished();
                return;
            }

            if (!_data.IsTimerFinished)
            {
                TimerUpdated?.Invoke();
            }
        }
    }
}
