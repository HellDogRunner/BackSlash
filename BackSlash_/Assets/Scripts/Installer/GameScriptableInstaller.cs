using RedMoonGames.Window;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Scripts.Installer
{
    [CreateAssetMenu(fileName = "GameScriptableInstaller", menuName = "[RMG] Scriptable/Installer/GameScriptableInstaller")]
    public class GameScriptableInstaller : ScriptableObjectInstaller<GameScriptableInstaller>
    {
        [SerializeField] private List<ScriptableObject> scriptableObjects;

        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);

            ForeachScriptableObject(scriptableObject =>
            {
                Container.Bind(scriptableObject.GetType()).FromScriptableObject(scriptableObject).AsSingle();
            });
        }

        private void ForeachScriptableObject(Action<ScriptableObject> action)
        {
            scriptableObjects.ForEach(action);
        }
    }
}
