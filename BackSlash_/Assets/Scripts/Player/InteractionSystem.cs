using Scripts.Player;
using Scripts.UI.Dialogue;
using System;
using UnityEngine;
using Zenject;

public class InteractionSystem : MonoBehaviour
{
    private bool _canInteract;

    private UIActionsController _uiActions;
    private InteractionAnimation _interaction;

    public event Action<QuestDatabase> SetData;
    public event Action OnInteract;
    public event Action OnExitTrigger;
    public event Action<string> OnEnterTrigger;

    [Inject]
    private void Construct(InteractionAnimation interaction, UIActionsController uIActions)
    {
        _interaction = interaction;
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
            var npc = other.GetComponent<NpcInteractionService>();
            var dialogueData = npc.GetQuestData();

            if (dialogueData != null)
            {
                _interaction.SetNPCTransform(npc.transform, npc.GetRotation());
                OnEnterTrigger?.Invoke(npc.GetName());
                SetData?.Invoke(dialogueData);
                _canInteract = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "NPC")
        {
            _canInteract = false;

            _interaction.RotateToDefault();
            OnExitTrigger?.Invoke();
        }
    }

    private void Interact()
    {
        if (_canInteract)
        {
            _interaction.LookAtEachOther(gameObject.transform);
            OnInteract?.Invoke();
        }
    }

    private void OnDestroy()
    {
        _uiActions.OnEnterKeyPressed -= Interact;
    }
}