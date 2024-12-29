using UnityEngine;
using Zenject;

public class LocationGeneratorEditor : MonoBehaviour
{
    [SerializeField] Transform _player;

    private LocationController locationController;
    [Inject]
    private void Construct(LocationController controller)
    {
        locationController = controller;
    }

    private void Start()
    {
        Generate();
        var startpos = locationController.GetRandomPlatformPosition();
        _player.position = startpos;
    }

    [ContextMenu("Generate Location")]
    public void Generate()
    {
        locationController.GenerateLocation();
    }
}
