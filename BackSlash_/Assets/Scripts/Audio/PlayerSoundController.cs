using FMOD.Studio;
using Scripts.Player;
using Scripts.Weapon;
using UnityEngine;
using Zenject;

public class PlayerSoundController : MonoBehaviour
{
    private MovementController _movementController;
    private AudioManager _audioManager;
    private InputController _inputController;
    private WeaponController _weaponController;

    private EventInstance _playerFootsteps;
    private EventInstance _swordSlashSound;

    [Inject]
    private void Construct(InputController inputController, AudioManager audioManager, MovementController movementController, WeaponController weaponController)
    {
        _audioManager = audioManager;

        _weaponController = weaponController;
        _weaponController.OnAttack += PlaySwordSound;
        _weaponController.OnDrawWeapon += PlayDrawSwordSound;
        _weaponController.OnSneathWeapon += PlaySneathSwordSound;

        _inputController = inputController;
        _inputController.OnSprintKeyPressed += IsSptrinting;
        _inputController.OnSprintKeyRealesed += IsRunning;

        _movementController = movementController;
        _movementController.IsMovingOnGround += PlayFootstepsSound;
        _movementController.OnLanded += PlayLandingSound;
    }

    private void OnDestroy()
    {
        _weaponController.OnAttack -= PlaySwordSound;
        _weaponController.OnDrawWeapon -= PlayDrawSwordSound;
        _weaponController.OnSneathWeapon -= PlaySneathSwordSound;

        _inputController.OnSprintKeyPressed -= IsSptrinting;
        _inputController.OnSprintKeyRealesed -= IsRunning;

        _movementController.IsMovingOnGround -= PlayFootstepsSound;
        _movementController.OnLanded -= PlayLandingSound;
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
            if (playbackState.Equals(PLAYBACK_STATE.PLAYING) && Time.timeScale == 0)
            {
                _playerFootsteps.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            }
        }
        else
        {
            _playerFootsteps.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }


    private void PlayLandingSound()
    {
        _audioManager.PlayGenericEvent(FMODEvents.instance.PlayerLanded);
    }

    private void PlaySwordSound(int currentAttack)
    {
        _swordSlashSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        _swordSlashSound.setParameterByName("Combo", currentAttack);
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

    private void IsSptrinting()
    {
        ChangeFootStepsFrequency(1);
    }

    private void IsRunning()
    {
        ChangeFootStepsFrequency(0);
    }

    private void ChangeFootStepsFrequency(int parameterValue) 
    {
        _playerFootsteps.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        _playerFootsteps.setParameterByName("RunSprint", parameterValue);
    }

}
