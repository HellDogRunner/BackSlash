using UnityEngine;
using Zenject;

namespace Scripts.Player
{
    public class StartSceneInstaller : MonoInstaller
    {
        [SerializeField] private UiInputsController _actionsController;

        public override void InstallBindings()
        {
            Container.Bind<UiInputsController>().FromInstance(_actionsController).AsSingle();
        }
    }
}