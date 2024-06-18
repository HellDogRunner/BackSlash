using Scripts.Player;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Scripts.UI
{
    public class GameplayUI : MonoBehaviour
    { 
        [Header("Game Objects")]
        [SerializeField] private GameObject _player;
        [SerializeField] private Image _dodgeTransition;

        [Header("Scripts")]
        [SerializeField] private PLayerHealthBar _healthBarView;

        private MovementController _movementController;
        private ImageTransition _imageTransition;

        [Inject]
        private void Construct(MovementController movementController)
        {
            _movementController = movementController;
        }

        private void Awake()
        {
            _imageTransition = _dodgeTransition.GetComponent<ImageTransition>();
        }

        private void Start()
        {
            _movementController.OnDogde += DodgeCooldown;
        }

        private void DodgeCooldown()
        {
            _imageTransition.StartCooldown();
        }

        private void OnDestroy()
        {
            _movementController.OnDogde -= DodgeCooldown;
        }
    }
}