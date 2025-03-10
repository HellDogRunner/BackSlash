using FMOD.Studio;
using Scripts.Player;
using Scripts.Animations;
using Scripts.Weapon;
using UnityEngine;
using Zenject;
using System.Collections;

public class PlayerSoundController : MonoBehaviour
{
	[SerializeField] private IKFootPlacement _footIK;

	private MovementController _movementController;
	private PlayerAnimationController _playerAnimator;
	private AudioController _audioManager;
	private WeaponController _weaponController;
	private ComboSystem _comboSystem;

	private EventInstance _playerFootsteps;
	private EventInstance _swordSlashSound;

	[Inject]
	private void Construct(AudioController audioManager, MovementController movementController,
		WeaponController weaponController, ComboSystem comboSystem, PlayerAnimationController playerAnimator)
	{
		_comboSystem = comboSystem;
		_audioManager = audioManager;
		_playerAnimator = playerAnimator;
		_weaponController = weaponController;
		_movementController = movementController;
	}

    void OnEnable()
    {
		_footIK.OnFeetGrounded += PlayStepSound;
    
   		_weaponController.OnDrawWeapon += PlayDrawSwordSound;
		_weaponController.OnSneathWeapon += PlaySneathSwordSound;
		
        _comboSystem.OnAttackSound += PlaySwordSound;
		_comboSystem.OnComboSound += PlayComboSound;
    }

    void OnDisable()
    {
		_footIK.OnFeetGrounded-= PlayStepSound;
    
        _weaponController.OnDrawWeapon -= PlayDrawSwordSound;
		_weaponController.OnSneathWeapon -= PlaySneathSwordSound;
		
		_comboSystem.OnAttackSound -= PlaySwordSound;
		_comboSystem.OnComboSound -= PlayComboSound;
    }

	private void Start()
	{
		//_playerFootsteps = _audioManager.CreateEventInstance(FMODEvents.instance.PlayerFootSteps);
		//_swordSlashSound = _audioManager.CreateEventInstance(FMODEvents.instance.SlashSword);
	}

	private void PlayStepSound(Vector3 point)
	{
		_audioManager.PlayGenericEvent(FMODEvents.instance.FootStep, point);
	}

	private void PlaySwordSound()
	{
		// _swordSlashSound.stop(STOP_MODE.ALLOWFADEOUT);
		// _swordSlashSound.setParameterByName("Combo", 1);
		// _swordSlashSound.start();
		_audioManager.PlayGenericEvent(FMODEvents.instance.WeaponAttack);
	}

	private void PlayComboSound()
	{
		// _swordSlashSound.stop(STOP_MODE.ALLOWFADEOUT);
		// _swordSlashSound.setParameterByName("Combo", 2);
		// _swordSlashSound.start();
		_audioManager.PlayGenericEvent(FMODEvents.instance.WeaponCombo);
	}

	private void PlayDrawSwordSound()
	{
		//TODO remove weapon equipment ??
		//_audioManager.PlayGenericEvent(FMODEvents.instance.DrawSword);
	}

	private void PlaySneathSwordSound()
	{
		//_audioManager.PlayGenericEvent(FMODEvents.instance.SneathSword);
	}
}
