using UnityEngine;

namespace Scripts.Player
{
    public class PlayerState
    {
        public EPlayerState State;

        public enum EPlayerState
        {
            Idle,
            Walk,
            Run,
            Sprint,
            Dodge,
            Attack,
            Jumping,
            Fall
        }
    }
}
