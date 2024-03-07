using UnityEngine;

namespace Scripts.Player
{
    public class PlayerState : MonoBehaviour
    {
        public EPlayerState _state;

        public enum EPlayerState
        {
            Idle,
            Walk,
            Run,
            Sprint,
            Dodge,
            Target,
            Jump,
            InAir,
            Fall,
        }
    }
}
