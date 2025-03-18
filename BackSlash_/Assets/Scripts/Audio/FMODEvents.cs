using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{
    [field: SerializeField] public EventReference LocationMusic { get; private set; }
    [field: Space]
    [field: SerializeField] public EventReference UIButtonClick { get; private set; }
    [field: SerializeField] public EventReference UIButtonHover { get; private set; }
    [field: SerializeField] public EventReference UIWindowOpen { get; private set; }
    [field: SerializeField] public EventReference UITab { get; private set; }
    [field: Space]
    [field: SerializeField] public EventReference FootStep { get; private set; }
    [field: SerializeField] public EventReference Roll { get; private set; }
    [field: Space]
    [field: SerializeField] public EventReference SwordSound { get; private set; }
    //TODO remove weapon equipment ??
    // [field: SerializeField] public EventReference DrawSword { get; private set; }
    // [field: SerializeField] public EventReference SneathSword { get; private set; }
    
    public static FMODEvents instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one FMOD Events instance in the scene.");
        }
        instance = this;
    }
}
