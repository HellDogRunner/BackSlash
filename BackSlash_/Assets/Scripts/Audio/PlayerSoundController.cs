using FMOD.Studio;
using Scripts.Player;
using Scripts.Weapon;
using UnityEngine;
using Zenject;

public class PlayerSoundController : MonoBehaviour
{
	private MovementController _movementController;
	private AudioController _audioManager;
	private InputController _inputController;
	private WeaponController _weaponController;
	private ComboSystem _comboSystem;

	private EventInstance _playerFootsteps;
	private EventInstance _swordSlashSound;

	[Inject]
	private void Construct(InputController inputController, AudioController audioManager, MovementController movementController,
		WeaponController weaponController, ComboSystem comboSystem)
	{
		_audioManager = audioManager;

		_weaponController = weaponController;
		_weaponController.OnDrawWeapon += PlayDrawSwordSound;
		_weaponController.OnSneathWeapon += PlaySneathSwordSound;

		_inputController = inputController;
		_inputController.OnSprintKeyPressed += IsSptrinting;

		_movementController = movementController;
		_movementController.PlaySteps += PlayFootstepsSound;
		_movementController.InAir += PlayLandingSound;

		_comboSystem = comboSystem;
		_comboSystem.OnAttackSound += PlaySwordSound;
		_comboSystem.OnComboSound += PlayComboSound;
	}

	private void OnDestroy()
	{
		_weaponController.OnDrawWeapon -= PlayDrawSwordSound;
		_weaponController.OnSneathWeapon -= PlaySneathSwordSound;

		_inputController.OnSprintKeyPressed -= IsSptrinting;

		_movementController.PlaySteps -= PlayFootstepsSound;
		_movementController.InAir -= PlayLandingSound;

		_comboSystem.OnAttackSound -= PlaySwordSound;
		_comboSystem.OnComboSound -= PlayComboSound;
	}

	private void Start()
	{
		_playerFootsteps = _audioManager.CreateEventInstance(FMODEvents.instance.PlayerFootSteps);
		_swordSlashSound = _audioManager.CreateEventInstance(FMODEvents.instance.SlashSword);
	}

	private void PlayFootstepsSound(bool isPlaying)
	{
		if (isPlaying)
		{
			PLAYBACK_STATE playbackState;
			_playerFootsteps.getPlaybackState(out playbackState);
			if (playbackState.Equals(PLAYBACK_STATE.STOPPED))
			{
				_playerFootsteps.start();
			}
			if (Time.timeScale == 0)
			{
				_playerFootsteps.stop(STOP_MODE.IMMEDIATE);
			}
		}
		else
		{
			_playerFootsteps.stop(STOP_MODE.ALLOWFADEOUT);
		}
	}


	private void PlayLandingSound(bool inAir)
	{
		if (!inAir) _audioManager.PlayGenericEvent(FMODEvents.instance.PlayerLanded);
	}

	private void PlaySwordSound()
	{
		_swordSlashSound.stop(STOP_MODE.ALLOWFADEOUT);
		_swordSlashSound.setParameterByName("Combo", 1);
		_swordSlashSound.start();
	}

	private void PlayComboSound()
	{
		_swordSlashSound.stop(STOP_MODE.ALLOWFADEOUT);
		_swordSlashSound.setParameterByName("Combo", 2);
		_swordSlashSound.start();
	}

	private void PlayDrawSwordSound()
	{
		_audioManager.PlayGenericEvent(FMODEvents.instance.DrawSword);
	}

	private void PlaySneathSwordSound()
	{
		_audioManager.PlayGenericEvent(FMODEvents.instance.SneathSword);
	}

	private void IsSptrinting(bool isPressed)
	{
		if (isPressed)
		{
			ChangeFootStepsFrequency(1);

		}
		else 
		{ 
			ChangeFootStepsFrequency(0);
		}
	}

	private void ChangeFootStepsFrequency(int parameterValue)
	{
		_playerFootsteps.stop(STOP_MODE.ALLOWFADEOUT);
		_playerFootsteps.setParameterByName("RunSprint", parameterValue);
	}

}
