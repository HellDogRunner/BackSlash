using UnityEngine;
using Zenject;

namespace Scripts.Player
{
    public class StartSceneInstaller : MonoInstaller
    {
        [SerializeField] private UIActionsController _actionsController;

        public override void InstallBindings()
        {
            Container.Bind<UIActionsController>().FromInstance(_actionsController).AsSingle();
        }
    }
}