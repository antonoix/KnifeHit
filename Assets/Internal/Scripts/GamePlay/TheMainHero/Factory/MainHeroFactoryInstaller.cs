using UnityEngine;
using Zenject;

namespace Internal.Scripts.GamePlay.TheMainHero.Factory
{
    [CreateAssetMenu(fileName = "MainHeroFactoryInstaller", menuName = "Installers/MainHeroFactoryInstaller")]
    public class MainHeroFactoryInstaller : ScriptableObjectInstaller
    {
        [field: SerializeField] private MainHeroConfig _heroConfig;
        
        public override void InstallBindings()
        {
            Container.BindInstance(_heroConfig).AsSingle();
            Container.BindInterfacesAndSelfTo<MainHeroFactory>().AsSingle();
        }
    }
}