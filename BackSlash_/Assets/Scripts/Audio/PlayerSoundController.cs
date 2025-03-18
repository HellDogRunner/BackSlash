using FMOD.Studio;
using Scripts.Weapon;
using UnityEngine;
using Zenject;

public class PlayerSoundController : MonoBehaviour
{
	[SerializeField] private IKFootPlacement _footIK;

	private AudioController _audioManager;
	private WeaponController _weaponController;
	private ComboSystem _comboSystem;

	private EventInstance _swordSound;

	[Inject]
	private void Construct(AudioController audioManager, WeaponController weaponController, ComboSystem comboSystem)
	{
		_comboSystem = comboSystem;
		_audioManager = audioManager;
		_weaponController = weaponController;
	}

    void OnEnable()
    {
   		// _weaponController.OnDrawWeapon += PlayDrawSwordSound;
		// _weaponController.OnSneathWeapon += PlaySneathSwordSound;
		
		_footIK.OnFeetGrounded += PlayStepSound;
    
        _comboSystem.OnAttackSound += PlaySwordSound;
		_comboSystem.OnComboSound += PlayComboSound;
    }

    void OnDisable()
    {
        // _weaponController.OnDrawWeapon -= PlayDrawSwordSound;
		// _weaponController.OnSneathWeapon -= PlaySneathSwordSound;
    
		_footIK.OnFeetGrounded-= PlayStepSound;
		
		_comboSystem.OnAttackSound -= PlaySwordSound;
		_comboSystem.OnComboSound -= PlayComboSound;
    }

	private void Start()
	{
		_swordSound = _audioManager.CreateEventInstance(FMODEvents.instance.SwordSound, _weaponController.Weapon);
	}

	private void PlayStepSound(Vector3 point)
	{
		_audioManager.PlayGenericEvent(FMODEvents.instance.FootStep, point);
	}

	private void PlayRollSound()
	{
	    _audioManager.PlayGenericEvent(FMODEvents.instance.Roll, transform.position);
	}

	private void PlaySwordSound()
	{
		_swordSound.stop(STOP_MODE.ALLOWFADEOUT);
		_swordSound.setParameterByName("Type", 0);
		_swordSound.start();
	}

	private void PlayComboSound()
	{
		_swordSound.stop(STOP_MODE.ALLOWFADEOUT);
		_swordSound.setParameterByName("Type", 1);
		_swordSound.start();
	}

	//TODO remove weapon equipment ??
	private void PlayDrawSwordSound()
	{
		//_audioManager.PlayGenericEvent(FMODEvents.instance.DrawSword);
	}

	private void PlaySneathSwordSound()
	{
		//_audioManager.PlayGenericEvent(FMODEvents.instance.SneathSword);
	}
}
