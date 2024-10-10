using Scripts.Player;
using Scripts.UI.Dialogue;
using System;
using UnityEngine;
using Zenject;

public class InteractionSystem : MonoBehaviour
{
    private bool _canInteract;

    private InputController _gameInputs;

    public event Action<DialogueDatabase> SetData;
    public event Action OnInteract;
    public event Action OnEndInteract;
    public event Action OnExitTrigger;
    public event Action OnEnterTrigger;

    [Inject]
    private void Construct(InputController gameInputs)
    {
        _gameInputs = gameInputs;
    }

    private void Awake()
    {
        _gameInputs.OnInteractionKeyPressed += Interact;
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

    public void EndInteraction()
    {
        OnEndInteract?.Invoke();
    }

    private void OnDestroy()
    {
        _gameInputs.OnInteractionKeyPressed -= Interact;
    }
}