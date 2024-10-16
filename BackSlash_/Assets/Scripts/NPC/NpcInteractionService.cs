using Scripts.UI.Dialogue;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class NpcInteractionService : MonoBehaviour
{
    [SerializeField] private QuestDatabase _quest;
    [Space]
    [SerializeField] private string _name;
    [Space]
    [SerializeField] private Transform _viewTR;

    private Vector3 _defaultRotation;

    private void Awake()
    {
        _defaultRotation = _viewTR.rotation.eulerAngles;
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