using UnityEngine;

public class IKFootPlacement : MonoBehaviour
{
	private Vector3 rightFootPosition, leftFootPosition, rightFootIkPosition, leftFootIkPosition;
	private float lastPevisPositionY, lastRightFootPositionY, lastLeftFootPositionY;
	
	[SerializeField] private Animator _animator;
	[SerializeField] private LayerMask _layer;
	[Header("Settings")]
	[SerializeField] [Range(0, 1)] private float _leftWeight, _rightWeight;
	[SerializeField] private float _toGroundHeight;
	[SerializeField] private float _raycastDistance;
	[Space]
	[SerializeField] private float _weightSpeed;
	[SerializeField] private float _footSpeed;
	[SerializeField] private float _pelvisSpeed;
	[Space]
	[SerializeField] private float _pelvisOffset;
	[SerializeField] private float _maxFootOffset;
	[SerializeField] private float _maxFootAngle;
	[Space]
	[SerializeField] private float _minIkDistance;
	[SerializeField] private float _maxIkDistance;
	
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

		SetIkWeight(AvatarIKGoal.LeftFoot, ref _leftWeight);
		MoveFeetToPoint(AvatarIKGoal.LeftFoot, leftFootIkPosition, ref lastLeftFootPositionY);

		SetIkWeight(AvatarIKGoal.RightFoot, ref _rightWeight);
		MoveFeetToPoint(AvatarIKGoal.RightFoot, rightFootIkPosition, ref lastRightFootPositionY);
	}	
	
	private void SetIkWeight(AvatarIKGoal foot, ref float footWeight)
	{
		Physics.Raycast(_animator.GetIKPosition(foot) + Vector3.up * 0.15f, Vector3.down, out var hit, _maxIkDistance, _layer);
		float weight;
		
		if (hit.distance == 0) weight = 0;
		else if (hit.distance < _minIkDistance) weight = 1;
		else weight = 1 - (hit.distance - _minIkDistance) / (_maxIkDistance - _minIkDistance);
		
		footWeight = Mathf.Lerp(footWeight, weight, _weightSpeed);
		
		_animator.SetIKPositionWeight(foot, footWeight);
		_animator.SetIKRotationWeight(foot, footWeight);
	}
	
	private void MoveFeetToPoint(AvatarIKGoal foot, Vector3 positionHolder, ref float lastFootPositionY)
	{
		Vector3 IkPosition = _animator.GetIKPosition(foot);

		if (positionHolder != Vector3.zero)
		{
			IkPosition = transform.InverseTransformPoint(IkPosition);
			positionHolder = transform.InverseTransformPoint(positionHolder);
			
			float yVariable = Mathf.Lerp(lastFootPositionY, positionHolder.y, _footSpeed);
			IkPosition.y += yVariable;
			lastFootPositionY = yVariable;
			
			IkPosition = transform.TransformPoint(IkPosition);
			
			Physics.Raycast(IkPosition, Vector3.down, out var hit, _toGroundHeight, _layer);
			
			var angle = Vector3.Angle(Vector3.up, hit.normal);
			if (angle > _maxFootAngle) angle = _maxFootAngle;
			var rotation = Quaternion.AngleAxis(angle, Vector3.Cross(Vector3.up, hit.normal));
			
			_animator.SetIKRotation(foot, rotation * _animator.GetIKRotation(foot));
		}
		
		_animator.SetIKPosition(foot, IkPosition);
	}
	
	private void MovePelvisHeight()
	{
		if (rightFootIkPosition == Vector3.zero || leftFootIkPosition == Vector3.zero || lastPevisPositionY == 0)
		{
			lastPevisPositionY = _animator.bodyPosition.y;
			return;
		}
		
		float lOffsetPosition = leftFootIkPosition.y - transform.position.y;
		float rOffsetPosition = rightFootIkPosition.y - transform.position.y;
		float totalOffset = Mathf.Min(lOffsetPosition, rOffsetPosition);
		totalOffset = totalOffset > _maxFootOffset ? totalOffset : _maxFootOffset;
		Vector3 newPelvisPosition = _animator.bodyPosition + Vector3.up * totalOffset;
		
		newPelvisPosition.y = Mathf.Lerp(lastPevisPositionY, newPelvisPosition.y, _pelvisSpeed);	
		_animator.bodyPosition = newPelvisPosition;
		lastPevisPositionY = _animator.bodyPosition.y;
	}
	
	private void FeetPositionSolver(Vector3 footPosition, ref Vector3 IkPosition)
	{
		RaycastHit feetHit;
		// TODO clear debug
		Debug.DrawLine(footPosition, footPosition + Vector3.down * (_raycastDistance + _toGroundHeight), Color.red);

		if (Physics.Raycast(footPosition, Vector3.down, out feetHit, _raycastDistance + _toGroundHeight, _layer))
		{
			IkPosition = footPosition;
			IkPosition.y = feetHit.point.y + _pelvisOffset;
			return;
		}
		
		IkPosition = Vector3.zero;
	}
	
	private void AdjustFeetTarget(ref Vector3 feetPosition, HumanBodyBones foot)
	{
		feetPosition = _animator.GetBoneTransform(foot).position;
		feetPosition.y = transform.position.y + _toGroundHeight;
	}
}
