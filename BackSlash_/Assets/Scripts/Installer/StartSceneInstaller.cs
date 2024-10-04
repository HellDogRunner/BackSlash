using UnityEngine;
using Zenject;

namespace Scripts.Player
{
    public class StartSceneInstaller : MonoInstaller
    {
        [SerializeField] private UIPauseInputs _pauseInputs;
        [SerializeField] private UIMenuInputs _menuInputs;

        public override void InstallBindings()
        {
            Container.Bind<UIPauseInputs>().FromInstance(_pauseInputs).AsSingle();
            Container.Bind<UIMenuInputs>().FromInstance(_menuInputs).AsSingle();
        }
    }
}