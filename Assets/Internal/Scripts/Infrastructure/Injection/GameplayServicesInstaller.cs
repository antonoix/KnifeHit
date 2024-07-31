using Internal.Scripts.GamePlay.TheMainHero;
using Internal.Scripts.GamePlay.Weapons.ProjectilesService;
using Internal.Scripts.Infrastructure.GameStatesMachine.States;
using UnityEngine;
using Zenject;

namespace Internal.Scripts.Infrastructure.Injection
{
    [CreateAssetMenu(fileName = "GameplayServicesInstaller", menuName = "Installers/GameplayServicesInstaller")]
    public class GameplayServicesInstaller : ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<GamePlayState>().AsSingle();
            Container.BindInterfacesAndSelfTo<ProjectilesFactory>().AsSingle();
            Container.BindInterfacesTo<ProjectilesService>().AsSingle();
        }
    }
}