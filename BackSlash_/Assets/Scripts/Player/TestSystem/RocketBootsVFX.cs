using UnityEngine;

namespace Scripts.VFX
{
    public class RocketBootsVFX : MonoBehaviour
    {
        [System.Serializable]
        public class BootVFX
        {
            public ParticleSystem jetLoop;
            public ParticleSystem jetBurst;
            public ParticleSystem dustLand;

            [HideInInspector] public ParticleSystem.EmissionModule loopEmission;
            [HideInInspector] public bool inited;
        }

        [Header("Boots")]
        [SerializeField] private BootVFX _left;
        [SerializeField] private BootVFX _right;

        [Header("Logic")]
        [SerializeField] private bool _playDustOnStop = true;
        [SerializeField] private bool _playDustOnLand = true;

        [Header("Loop Intensity")]
        [SerializeField] private float _minRate = 80f;
        [SerializeField] private float _maxRate = 260f;
        [SerializeField] private float _smooth = 12f;

        private bool _active;
        private float _rate01; // 0..1

        private void Awake()
        {
            InitBoot(_left);
            InitBoot(_right);

            StopAll();
        }

        private void InitBoot(BootVFX b)
        {
            if (b == null || b.jetLoop == null) return;
            b.loopEmission = b.jetLoop.emission;
            b.inited = true;
        }

        /// <summary>
        /// Включить/выключить режим парения.
        /// </summary>
        public void SetActive(bool value)
        {
            if (value == _active) return;
            _active = value;

            if (_active)
            {
                PlayStart(_left);
                PlayStart(_right);
            }
            else
            {
                StopLoop(_left);
                StopLoop(_right);

                if (_playDustOnStop)
                {
                    PlayLand(_left);
                    PlayLand(_right);
                }
            }
        }

        /// <summary>
        /// Интенсивность 0..1 (например от скорости/высоты/времени удержания)
        /// </summary>
        public void SetThrottle(float value01)
        {
            _rate01 = Mathf.Clamp01(value01);
        }

        /// <summary>Вызывай когда реально приземлился.</summary>
        public void OnLanded()
        {
            if (!_playDustOnLand) return;

            if (_active)
            {
                // если приземлился с активными ботинками — легкая пыль тоже норм
                PlayLand(_left);
                PlayLand(_right);
            }
            else
            {
                PlayLand(_left);
                PlayLand(_right);
            }
        }

        private void Update()
        {
            // плавно меняем rate у струи
            UpdateEmission(_left);
            UpdateEmission(_right);
        }

        private void UpdateEmission(BootVFX b)
        {
            if (!b.inited || b.jetLoop == null) return;

            float targetRate = Mathf.Lerp(_minRate, _maxRate, _rate01);
            var rate = b.loopEmission.rateOverTime;
            float current = rate.constant;

            float newRate = Mathf.Lerp(current, targetRate, Time.deltaTime * _smooth);
            rate.constant = newRate;
            b.loopEmission.rateOverTime = rate;
        }

        private void PlayStart(BootVFX b)
        {
            if (b.jetBurst != null) b.jetBurst.Play(true);
            if (b.jetLoop != null) b.jetLoop.Play(true);
        }

        private void StopLoop(BootVFX b)
        {
            if (b.jetLoop != null) b.jetLoop.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }

        private void PlayLand(BootVFX b)
        {
            if (b.dustLand != null) b.dustLand.Play(true);
        }

        private void StopAll()
        {
            StopPS(_left.jetLoop);
            StopPS(_left.jetBurst);
            StopPS(_left.dustLand);
            StopPS(_right.jetLoop);
            StopPS(_right.jetBurst);
            StopPS(_right.dustLand);
        }

        private void StopPS(ParticleSystem ps)
        {
            if (ps == null) return;
            ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }
}
