using Zenject;
using UnityEngine;

public class LocationInstaller : MonoInstaller
{
    [SerializeField] private LocationView locationView;

    public override void InstallBindings()
    {
        Container.Bind<LocationGenerationModel>().AsSingle();
        Container.Bind<LocationController>().AsTransient();
        Container.BindInstance(locationView).AsSingle();
    }
}
