using Scripts.UI.Dialogue;
using UnityEngine;
using Zenject;

public class QuestTarget : MonoBehaviour
{
    [SerializeField] private QuestDatabase _quest;
    [Space]
    [SerializeField] HealthController _healthController;

    private QuestSystem _questSystem;

    [Inject]
    private void Construct(QuestSystem questSystem)
    {
        _questSystem = questSystem;
    }

    private void Awake()
    {
        _healthController.OnDeath += CompleteQuest;
    }

    private void CompleteQuest()
    {
        _questSystem.ChangeQuestState(_quest, "Reward");
    }

    private void OnDestroy()
    {
        _healthController.OnDeath -= CompleteQuest;
    }
}