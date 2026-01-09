using UnityEngine;

namespace Scripts.Combat
{
    public class SlasherTargeting : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform _camera;

        [Header("Targeting")]
        [SerializeField] private LayerMask _targetMask;
        [SerializeField] private float _maxAimDistance = 18f;
        [SerializeField] private float _aimAssistRadius = 1.0f; // “прощение” вокруг луча
        [SerializeField] private float _maxLockDistanceFromPlayer = 6.5f; // дистанция, где магнит/доведение работает

        public Transform CurrentTarget { get; private set; }
        public float CurrentTargetDistance { get; private set; }

        private void Awake()
        {
            if (_camera == null && Camera.main != null) _camera = Camera.main.transform;
        }

        private void Update()
        {
            UpdateTarget();

            if (CurrentTarget != null)
                Debug.DrawLine(_camera.position, CurrentTarget.position, Color.green);
            else
                Debug.DrawRay(_camera.position, _camera.forward * 5f, Color.red);
        }


        private void UpdateTarget()
        {
            CurrentTarget = null;
            CurrentTargetDistance = 0f;

            if (_camera == null) return;

            Ray ray = new Ray(_camera.position, _camera.forward);

            // 1) Пытаемся попасть лучом
            if (Physics.Raycast(ray, out var hit, _maxAimDistance, _targetMask, QueryTriggerInteraction.Ignore))
            {
                Transform t = hit.collider.attachedRigidbody ? hit.collider.attachedRigidbody.transform : hit.collider.transform;
                SetTargetIfValid(t);
                return;
            }

            // 2) Если луч не попал — делаем "сферу" по лучу, выбираем ближайшего к оси прицеливания
            Vector3 probeCenter = _camera.position + _camera.forward * Mathf.Min(_maxAimDistance, 10f);
            Collider[] cols = Physics.OverlapSphere(probeCenter, _aimAssistRadius, _targetMask, QueryTriggerInteraction.Ignore);

            if (cols == null || cols.Length == 0) return;

            float bestScore = float.NegativeInfinity;
            Transform best = null;

            for (int i = 0; i < cols.Length; i++)
            {
                Transform t = cols[i].attachedRigidbody ? cols[i].attachedRigidbody.transform : cols[i].transform;

                // скоринг: ближе к центру экрана + ближе к игроку
                Vector3 to = (t.position - _camera.position);
                float distCam = to.magnitude;
                Vector3 dir = to / Mathf.Max(0.001f, distCam);
                float dot = Vector3.Dot(_camera.forward, dir); // 1 = идеально по центру

                float distPlayer = Vector3.Distance(transform.position, t.position);
                float score = dot * 2.0f - distPlayer * 0.08f;

                if (score > bestScore)
                {
                    bestScore = score;
                    best = t;
                }
            }

            if (best != null) SetTargetIfValid(best);
        }

        private void SetTargetIfValid(Transform t)
        {
            float d = Vector3.Distance(transform.position, t.position);
            CurrentTargetDistance = d;

            // цель можно подсвечивать всегда, но магнит будем делать только вблизи
            CurrentTarget = t;
        }

        public bool CanMagnetToTarget(out Transform t)
        {
            t = CurrentTarget;
            if (t == null) return false;

            float d = Vector3.Distance(transform.position, t.position);
            return d <= _maxLockDistanceFromPlayer;
        }
    }
}
