using UnityEngine;
using Zenject;

public class HUDController : MonoBehaviour
{
    [SerializeField] private CanvasGroup _overlay;
    [SerializeField] private CanvasGroup _dialogueWindow;
    [SerializeField] private CanvasGroup _tradeWindow;

    private GameObject _OpenedWindow;
    private string _npcName;

    private bool _canShowWindow = true;

    private HUDAnimationService _animationService;
    private InteractionSystem _interactionSystem;

    [Inject]
    private void Construct(HUDAnimationService animationService, InteractionSystem interactionSystem)
    {
        _animationService = animationService;
        _interactionSystem = interactionSystem;
    }

    private void Awake()
    {
        _interactionSystem.OnInteract += StartInteraction;
        _interactionSystem.OnEndInteract += EndInteraction;
        _interactionSystem.OnExitTrigger += EndInteraction;

        _overlay.alpha = 0;
        _animationService.AnimateShow(_overlay);
    }

    private void StartInteraction()
    {
        _animationService.AnimateHide(_overlay);
    }

    public void EndInteraction()
    {
        _animationService.AnimateShow(_overlay);
    }

    private void OnDestroy()
    {
        _interactionSystem.OnInteract -= StartInteraction;
        _interactionSystem.OnEndInteract -= EndInteraction;
        _interactionSystem.OnExitTrigger -= EndInteraction;
    }
}