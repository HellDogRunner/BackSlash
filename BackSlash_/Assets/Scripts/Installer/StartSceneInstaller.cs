using UnityEngine;
using Zenject;

namespace Scripts.Player
{
    public class StartSceneInstaller : MonoInstaller
    {
        [SerializeField] private UIController controller;

        public override void InstallBindings()
        {
            Container.Bind<UIController>().FromInstance(controller).AsSingle();
        }
    }
}