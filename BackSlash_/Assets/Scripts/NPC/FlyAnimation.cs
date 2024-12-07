using DG.Tweening;
using UnityEngine;

public class FlyAnimation : MonoBehaviour
{
	[SerializeField] private Transform model;
	
	[Header("Settings")]
	[SerializeField] private float duration;
	[SerializeField] private float amplitude;

	private Sequence fly;
	
	private void Start()
	{
		Animate();
	}
	
	private void Animate()
	{
		var posY = model.transform.position.y;
		model.transform.position += new Vector3(0, amplitude, 0);

		fly = DOTween.Sequence()
			.Append(model.DOMoveY(posY - amplitude, duration).SetEase(Ease.InOutSine))
			.Append(model.DOMoveY(posY + amplitude, duration).SetEase(Ease.InOutSine))
			.SetLoops(-1);
	}
	
	private void OnDestroy() 
	{
		fly.Kill();
	}
}
