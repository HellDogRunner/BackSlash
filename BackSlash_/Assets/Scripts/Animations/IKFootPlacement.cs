using Scripts.Animations;
using Scripts.Player;
using UnityEngine;
using Zenject;

public class IKFootPlacement : MonoBehaviour
{
	private Vector3 rightFootPosition, leftFootPosition, rightFootIkPosition, leftFootIkPosition;
	private float lastPevisPositionY, lastRightFootPositionY, lastLeftFootPositionY;
	[SerializeField] [Range(0, 1)] private float ikWeight;
	
	[SerializeField] private Animator _animator;
	[SerializeField] private LayerMask _layer;
	
	[Header("Settings")]
	[SerializeField] private float _toGroundHeight;
	[SerializeField] private float _raycastDistance;
	[Space]
	[SerializeField] private float _weightSpeed;
	[SerializeField] private float _footSpeed;
	[SerializeField] private float _pelvisSpeed;
	[Space]
	[SerializeField] private float _footOffset;
	[SerializeField] private float _stayPelvisOffset;
	[SerializeField] private float _movePelvisOffset;
	[SerializeField] private float _maxFootAngle;
	
	private MovementController _movement;
	private PlayerAnimationController _playerAnimator;
	
	[Inject]
	private void Construct(MovementController movement, PlayerAnimationController playerAnimator)
	{
		_movement = movement;
		_playerAnimator = playerAnimator;
	}
	
	private void Update()
	{
		AdjustFeetTarget(ref rightFootPosition, HumanBodyBones.RightFoot);
		AdjustFeetTarget(ref leftFootPosition, HumanBodyBones.LeftFoot);
		
		FeetPositionSolver(rightFootPosition, ref rightFootIkPosition);
		FeetPositionSolver(leftFootPosition, ref leftFootIkPosition);	
	}
	
	private void OnAnimatorIK()
	{
		MovePelvisHeight();
		SetIkWeight();
		
		if (!_movement.Air)
		{
			MoveFeetToPoint(AvatarIKGoal.LeftFoot, leftFootIkPosition, ref lastLeftFootPositionY);
			MoveFeetToPoint(AvatarIKGoal.RightFoot, rightFootIkPosition, ref lastRightFootPositionY);
		}
	}	
	
	private void SetIkWeight()
	{
		float requiredWeight = _movement.Air ? 0 : 1;

		ikWeight =  Mathf.Lerp(ikWeight, requiredWeight, _weightSpeed * Time.deltaTime);
		
		_animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, ikWeight);
		_animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, ikWeight);
		_animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, ikWeight);
		_animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, ikWeight);
	}
	
	private void MoveFeetToPoint(AvatarIKGoal foot, Vector3 positionHolder, ref float lastFootPositionY)
	{
		if (positionHolder != Vector3.zero)
		{
			Vector3 ikPosition = _animator.GetIKPosition(foot);
			ikPosition = transform.InverseTransformPoint(ikPosition);
			positionHolder = transform.InverseTransformPoint(positionHolder);
			
			float yVariable = Mathf.Lerp(lastFootPositionY, positionHolder.y, _footSpeed * Time.deltaTime);
			ikPosition.y += yVariable;
			lastFootPositionY = yVariable;
			
			ikPosition = transform.TransformPoint(ikPosition);
			
			Physics.Raycast(ikPosition + Vector3.up * _toGroundHeight, Vector3.down, out var hit, _raycastDistance, _layer);
			
			var angle = Vector3.Angle(Vector3.up, hit.normal);
			if (angle > _maxFootAngle) angle = _maxFootAngle;
			var rotation = Quaternion.AngleAxis(angle, Vector3.Cross(Vector3.up, hit.normal));
			
			_animator.SetIKRotation(foot, rotation * _animator.GetIKRotation(foot));
			_animator.SetIKPosition(foot, ikPosition);
		}
	}
	
	private void FeetPositionSolver(Vector3 footPosition, ref Vector3 ikPosition)
	{
		Debug.DrawLine(footPosition, footPosition + Vector3.down * (_raycastDistance + _toGroundHeight), Color.red);
		
		if (Physics.Raycast(footPosition, Vector3.down, out var feetHit, _raycastDistance + _toGroundHeight, _layer))
		{
			ikPosition = footPosition;
			ikPosition.y = feetHit.point.y + _footOffset;
		}
		else
		{
			ikPosition = Vector3.zero;
		}
	}
	
	private void AdjustFeetTarget(ref Vector3 feetPosition, HumanBodyBones foot)
	{
		feetPosition = _animator.GetBoneTransform(foot).position;
		feetPosition.y += _footOffset + _toGroundHeight;
	}
	
	private void MovePelvisHeight()
	{
		if (_movement.Air || lastPevisPositionY == 0)
		{
			lastPevisPositionY = _animator.bodyPosition.y;	
			return;
		}
		
		Vector3 newPelvisPosition = _animator.bodyPosition + Vector3.up * GetPelvisOffset();
		
		newPelvisPosition.y = Mathf.Lerp(lastPevisPositionY, newPelvisPosition.y, _pelvisSpeed * Time.deltaTime);	
		_animator.bodyPosition = newPelvisPosition;
		lastPevisPositionY = _animator.bodyPosition.y;
	}
	
	private float GetPelvisOffset()
	{
		if (rightFootIkPosition == Vector3.zero || leftFootIkPosition == Vector3.zero)
		{
			return 0;
		}
		
		float offset = Mathf.Min(leftFootIkPosition.y - transform.position.y, rightFootIkPosition.y - transform.position.y);
		float correctOffset = _playerAnimator.GetMove() ? _movePelvisOffset : _stayPelvisOffset;
		return Mathf.Max(offset, correctOffset);
	}
}
