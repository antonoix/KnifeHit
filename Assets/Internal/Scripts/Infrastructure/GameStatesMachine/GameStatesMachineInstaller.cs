using Internal.Scripts.Infrastructure.GameStatesMachine.States;
using UnityEngine;
using Zenject;

namespace Internal.Scripts.Infrastructure.GameStatesMachine
{
    [CreateAssetMenu(fileName = "GameStatesMachineInstaller", menuName = "Installers/GameStatesMachineInstaller")]
    public class GameStatesMachineInstaller : ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<GameStatesMachine>().AsSingle();
        }
    }
}