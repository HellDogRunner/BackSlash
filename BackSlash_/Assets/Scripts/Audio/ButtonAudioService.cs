using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ButtonAudioService : MonoBehaviour
{
	[SerializeField] private bool _soundOnHover = true;
	[SerializeField] private bool _soundOnClick;

	private Button _button;
	private AudioController _audioManager;

	[Inject]
	private void Construct(AudioController audioManager)
	{
		_audioManager = audioManager;
	}
	
	private void Awake()
	{
		_button = GetComponent<Button>();
		
		_button.onClick.AsObservable().Subscribe(_ =>
		{
			OnClick();
		}).AddTo(this);

		_button.OnSelectAsObservable().Subscribe(_ =>
		{
			OnHover();
		}).AddTo(this);


		_button.OnPointerEnterAsObservable().Subscribe(_ =>
		{
			//OnHover();
		}).AddTo(this);
	}
	
	private void OnHover()
	{
		if (_soundOnHover)
		{
			_audioManager.PlayGenericEvent(FMODEvents.instance.UIHoverEvent);
		}
	}

	private void OnClick()
	{
		if (_soundOnClick)
		{
			_audioManager.PlayGenericEvent(FMODEvents.instance.UIButtonClickEvent);
		}
	}
}
