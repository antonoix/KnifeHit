using Internal.Scripts.GamePlay.ProjectilesService;
using Internal.Scripts.GamePlay.TheMainHero;
using Internal.Scripts.Infrastructure.GameStatesMachine.States;
using UnityEngine;
using Zenject;

namespace Internal.Scripts.Infrastructure.Injection
{
    [CreateAssetMenu(fileName = "GameplayServicesInstaller", menuName = "Installers/GameplayServicesInstaller")]
    public class GameplayServicesInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private MainHeroConfig _heroConfig;
        
        public override void InstallBindings()
        {
            Container.BindInstance(_heroConfig);
            Container.BindInterfacesTo<GamePlayState>().AsSingle();
            Container.BindInterfacesTo<ProjectilesService>().AsSingle();
        }
    }
}