using Scripts.Player;
using Scripts.UI.Dialogue;
using System;
using UnityEngine;
using Zenject;

public class InteractionSystem : MonoBehaviour
{
    private Transform _npcTR;
    private bool _canInteract;
    private float _maxDistance;

    private UIActionsController _uiActions;
    private InteractionAnimation _interactionAnimation;

    public event Action<QuestDatabase> SetData;
    public event Action OnInteract;
    public event Action OnExitTrigger;
    public event Action<string> OnEnterTrigger;

    [Inject]
    private void Construct(InteractionAnimation interaction, UIActionsController uIActions)
    {
        _interactionAnimation = interaction;
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
                _interactionAnimation.SetNPCTransform(npc.transform, npc.GetRotation());

                _npcTR = npc.transform;
                _maxDistance = other.GetComponent<SphereCollider>().radius + 0.2f;

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

            _interactionAnimation.RotateToDefault();

            OnExitTrigger?.Invoke();
        }
    }

    private void Interact()
    {
        if (_canInteract && GetCanInteract(transform))
        {
            _interactionAnimation.LookAtEachOther(gameObject.transform);
            OnInteract?.Invoke();
        }
    }

    private bool GetCanInteract(Transform playerTR)
    {
        float distance = (playerTR.position - _npcTR.position).magnitude;

        return distance < _maxDistance;
    }

    private void OnDestroy()
    {
        _uiActions.OnEnterKeyPressed -= Interact;
    }
}