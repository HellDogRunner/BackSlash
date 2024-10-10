using Scripts.UI.Dialogue;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class NpcInteractionService : MonoBehaviour
{
    [SerializeField] private DialogueDatabase _botData;

    private void Awake()
    {
        gameObject.tag = "NPC";
    }

    public DialogueDatabase GetDialogueData()
    {
        return _botData;
    }
}