using Scripts.Player;
using UnityEngine;
using Zenject;

public class IKFootPlacement : MonoBehaviour
{
	[SerializeField] private Animator _animator;
	[SerializeField] private CharacterController _characterController;
	[Space]
	[SerializeField] private LayerMask _layer;
	[Space]
	[Header("Settings")]
	[SerializeField] private float _rayLength;
	[SerializeField] private float _distanceToGround;
	[SerializeField] private float _speed;
	[SerializeField] private float _maxFootAngle;
	[SerializeField] private Vector3 _standCollider;
	
	private Vector3 _moveCollider;
	
	private InputController _playerInputs;
	
	[Inject]
	private void Construct(InputController playerInputs)
	{
		_playerInputs = playerInputs;
	}
	
	private void Awake()
	{
		_moveCollider = _characterController.center;
	}
	
	private void Update()
	{
		var direction = _playerInputs.MoveDirection;
		var vector = direction == Vector2.zero ? _standCollider : _moveCollider;
		_characterController.center = Vector3.Lerp(_characterController.center, vector, _speed * Time.deltaTime);
	}
	
	private void OnAnimatorIK()
	{
		SetFoot(AvatarIKGoal.LeftFoot);				
		SetFoot(AvatarIKGoal.RightFoot);
	}	
	
	private void SetFoot(AvatarIKGoal foot)
	{
		RaycastHit hit;
		var weight = _playerInputs.MoveDirection == Vector2.zero ? 1 : 0;
		var ray = new Ray(_animator.GetIKPosition(foot) + Vector3.up * 0.5f, Vector3.down);

		_animator.SetIKPositionWeight(foot, weight);
		_animator.SetIKRotationWeight(foot, weight);
		
		
		if (Physics.Raycast(ray, out hit, _rayLength, _layer))
		{
			var position = hit.point;
			position.y += _distanceToGround;
			_animator.SetIKPosition(foot, position);
			
			var angle = Vector3.Angle(Vector3.up, hit.normal);
			if (angle > _maxFootAngle) angle = _maxFootAngle;
			var rotation = Quaternion.AngleAxis(angle, Vector3.Cross(Vector3.up, hit.normal));
			_animator.SetIKRotation(foot, rotation * _animator.GetIKRotation(foot));
		}
	}
}
