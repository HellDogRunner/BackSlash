using Scripts.UI.Dialogue;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class NpcInteractionService : MonoBehaviour
{
    [SerializeField] private QuestDatabase _quest;
    [Space]
    [SerializeField] private string _name;

    private Vector3 _defaultRotation;

    private void Awake()
    {
        _defaultRotation = transform.rotation.eulerAngles;
        gameObject.tag = "NPC";
    }

    public QuestDatabase GetQuestData()
    {
        return _quest;
    }

    public string GetName()
    {
        return _name;
    }

    public Vector3 GetRotation()
    {
        return _defaultRotation;
    }
}