using UnityEngine;
using Unity.Cinemachine;

namespace Scripts.Combat
{
    public class CinemachineOrbitalAimAssist : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] private CinemachineCamera _cmCam;          // твоя Cinemachine Camera
        [SerializeField] private SlasherTargeting _targeting;       // твой таргетинг (CurrentTarget)
        [SerializeField] private Transform _playerRoot;             // обычно Player transform (ось, вокруг которой крутим)

        [Header("Assist")]
        [SerializeField] private bool _onlyWhileAttacking = true;
        [SerializeField] private float _maxDistance = 10f;
        [SerializeField] private float _yawSpeed = 360f;            // градусов/сек (довольно быстро для слэшера)
        [SerializeField] private float _tiltSpeed = 240f;           // если включишь tilt assist
        [SerializeField] private bool _assistTilt = false;
        [SerializeField] private float _tiltMin = -10f;             // должны совпадать с range в OrbitalFollow
        [SerializeField] private float _tiltMax = 45f;

        private CinemachineOrbitalFollow _orbital;
        private CinemachinePanTilt _panTilt;
        private bool _attacking;

        public void SetAttacking(bool value) => _attacking = value;

        private void Awake()
        {
            if (_cmCam == null) _cmCam = GetComponent<CinemachineCamera>();
            if (_playerRoot == null) _playerRoot = transform;

            if (_cmCam != null)
            {
                _orbital = _cmCam.GetComponent<CinemachineOrbitalFollow>();
                _panTilt = _cmCam.GetComponent<CinemachinePanTilt>();
            }
        }

        private void LateUpdate()
        {
            if (_cmCam == null || _orbital == null || _targeting == null) return;
            if (_onlyWhileAttacking && !_attacking) return;

            var t = _targeting.CurrentTarget;
            if (t == null) return;

            float dist = Vector3.Distance(_playerRoot.position, t.position);
            if (dist > _maxDistance) return;

            // хотим повернуть камеру так, чтобы "вперёд игрока" смотрело на цель.
            Vector3 to = t.position - _playerRoot.position;
            to.y = 0f;
            if (to.sqrMagnitude < 0.0001f) return;

            // yaw относительно мира (можно и относительно игрока — ниже вариант тоже)
            float desiredYawWorld = Mathf.Atan2(to.x, to.z) * Mathf.Rad2Deg;

            // OrbitalFollow.HorizontalAxis.Value обычно в диапазоне -180..180 (wrap)
            float currentYaw = _orbital.HorizontalAxis.Value;
            float newYaw = MoveAngle(currentYaw, desiredYawWorld, _yawSpeed * Time.deltaTime);
            _orbital.HorizontalAxis.Value = newYaw;

            if (_assistTilt)
            {
                // простой tilt: смотрим на высоту цели (не обязательно)
                Vector3 to3 = t.position - _cmCam.transform.position;
                float desiredTilt = Mathf.Clamp(Mathf.Asin(to3.normalized.y) * Mathf.Rad2Deg, _tiltMin, _tiltMax);

                float currentTilt = _orbital.VerticalAxis.Value;
                float newTilt = Mathf.MoveTowards(currentTilt, desiredTilt, _tiltSpeed * Time.deltaTime);
                _orbital.VerticalAxis.Value = newTilt;
            }
        }

        private static float MoveAngle(float current, float target, float maxDelta)
        {
            float delta = Mathf.DeltaAngle(current, target);
            if (Mathf.Abs(delta) <= maxDelta) return target;
            return current + Mathf.Sign(delta) * maxDelta;
        }
    }
}
