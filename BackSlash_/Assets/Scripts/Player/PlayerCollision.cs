using UnityEngine;

namespace Scripts.Player
{
    public class PlayerCollision : MonoBehaviour
    {
        [SerializeField] private int countSurfaces;
        public bool IsGrounded;

        private void OnTriggerEnter(Collider other)
        {
            IsGroundedUpate(other, 1);
        }

        private void OnTriggerExit(Collider other)
        {
            IsGroundedUpate(other, -1);
        }

        private void IsGroundedUpate(Collider other, int value)
        {
            if (other.gameObject.tag == ("Ground"))
            {
                countSurfaces += value;
                if (countSurfaces > 0)
                {
                    IsGrounded = true;
                }
                else IsGrounded = false;

            }
        }
    }
}
