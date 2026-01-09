using UnityEngine;

namespace Scripts.VFX
{
    public class RocketBootsVFX_Test : MonoBehaviour
    {
        [SerializeField] private RocketBootsVFX _vfx;

        private void Awake()
        {
            if (_vfx == null) _vfx = GetComponent<RocketBootsVFX>();
        }

        private void Update()
        {
            // Space держим = парим
            bool active = Input.GetKey(KeyCode.Space);
            _vfx.SetActive(active);

            // интенсивность от времени/просто пример
            _vfx.SetThrottle(active ? 1f : 0f);
        }
    }
}
