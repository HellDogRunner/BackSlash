using Scripts.Player;
using Scripts.UI.Dialogue;
using System;
using UnityEngine;
using Zenject;

public class InteractionSystem : MonoBehaviour
{
    private bool _canInteract;

    private UIActionsController _uiActions;

    public event Action<DialogueDatabase> SetData;
    public event Action OnInteract;
    public event Action OnExitTrigger;
    public event Action OnEnterTrigger;

    [Inject]
    private void Construct(UIActionsController uIActions)
    {
        _uiActions = uIActions;
    }

    private void Awake()
    {
        _uiActions.OnEnterKeyPressed += Interact;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "NPC")
        {
            OnEnterTrigger?.Invoke();

            var npcInteraction = other.GetComponent<NpcInteractionService>();
            var dialogueData = npcInteraction.GetDialogueData();

            SetData?.Invoke(dialogueData);

            _canInteract = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "NPC")
        {
            _canInteract = false;
            OnExitTrigger?.Invoke();
        }
    }

    private void Interact()
    {
        if (_canInteract)
        {
            OnInteract?.Invoke();
        }
    }

    private void OnDestroy()
    {
        _uiActions.OnEnterKeyPressed -= Interact;
    }
}