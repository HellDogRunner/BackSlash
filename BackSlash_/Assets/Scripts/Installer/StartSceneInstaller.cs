using UnityEngine;
using Zenject;

namespace Scripts.Player
{
    public class StartSceneInstaller : MonoInstaller
    {
        [SerializeField] private UIPauseInputs controller;

        public override void InstallBindings()
        {
            Container.Bind<UIPauseInputs>().FromInstance(controller).AsSingle();
        }
    }
}