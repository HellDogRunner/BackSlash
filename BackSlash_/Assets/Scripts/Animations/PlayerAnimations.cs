using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private Animator _animator;
    public GameObject _player;
    private CharacterControl _characterControl;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _characterControl = _player.GetComponent<CharacterControl>();
    }

    private void Update()
    {
        _animator.SetBool("IsWalk", _characterControl.GetVectorCount() > 0);
        if (Input.GetKey(KeyCode.LeftControl))
            _animator.SetBool("SwordRun", true);
        else
            _animator.SetBool("SwordRun", false);
    }
}
