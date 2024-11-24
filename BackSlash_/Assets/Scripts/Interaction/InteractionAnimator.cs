using DG.Tweening;
using UnityEngine;

public class InteractionAnimator : MonoBehaviour
{
	[SerializeField] private CanvasGroup _talkCG;
	
	[Header("Settings")]
	[SerializeField] private float _showDuration = 0.2f;
	[SerializeField] private float _hideDuration = 0.1f;
	[SerializeField] private float _lookAtDuration = 0.3f;

	private Transform _npcTR;
	private Vector3 _defaultRotation;

	private Tween _talk;
	
	private void Awake()
	{		
		_talkCG.alpha = 0;
		_talkCG.gameObject.SetActive(false);
	}

	public void ShowTalk()
	{
		TryKillTween(_talk);
		
		_talkCG.gameObject.SetActive(true);
		_talk = _talkCG.DOFade(1, _showDuration);
	}

	public void HideTalk()
	{
		TryKillTween(_talk);
		
		_talk = _talkCG.DOFade(0, _hideDuration).
		OnComplete(() => _talkCG.gameObject.SetActive(false));
	}

	public void SetTransform(Transform transform, Vector3 rotation)
	{
		_npcTR = transform;
		_defaultRotation = rotation;
	}

	public void LookAtEachOther(Transform playerTR)
	{
		Vector3 playerPos = new Vector3(playerTR.position.x, _npcTR.position.y, playerTR.position.z);
		Vector3 npcPos = new Vector3(_npcTR.position.x, playerTR.position.y, _npcTR.position.z);

		_npcTR.DOLookAt(playerPos, _lookAtDuration);
		playerTR.DOLookAt(npcPos, _lookAtDuration);
	}

	public void RotateToDefault()
	{
		_npcTR.DORotate(_defaultRotation, _lookAtDuration);
	}
	
	private void TryKillTween(Tween tween)
	{
		if (tween.IsActive()) tween.Kill();
	}
}

// fade анимация с переключением активности объекта
// private void SwitchAlphaAndSetActive(Tween tween, CanvasGroup cg, float alpha = 0)
// {
// 	TryKillTween(tween);
	
// 	if (alpha == 0)
// 	{
// 		tween = cg.DOFade(alpha, _hideDuration).
// 		OnComplete(() => cg.gameObject.SetActive(false));
// 	}
// 	else
// 	{
// 		cg.gameObject.SetActive(true);
// 		tween = cg.DOFade(alpha, _showDuration);
// 	}
// }

// fade анимация без переключения активности объекта
// private void SwitchObjectAlpha(Tween tween, CanvasGroup cg, float alpha, bool isWindow = false) 
// {
// 	float duration;
// 	duration = isWindow ? _hideDuration : _showDuration;
	
// 	TryKillTween(tween);
// 	tween = cg.DOFade(alpha, duration).SetEase(Ease.Flash);
// }