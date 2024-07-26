using Internal.Scripts.GamePlay.ShopSystem;
using Internal.Scripts.GamePlay.TheMainHero.Combat;
using Internal.Scripts.Infrastructure.GameStatesMachine.States;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Internal.Scripts.Infrastructure.Factory
{
    [CreateAssetMenu(fileName = "ShopInstaller", menuName = "Installers/ShopInstaller")]
    public class ShopInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private ShopConfig shopConfig;
        
        public override void InstallBindings()
        {
            Container.BindInstance(shopConfig);
            Container.BindInterfacesAndSelfTo<ShopFactory>().AsSingle();
            Container.BindInterfacesTo<ShopState>().AsSingle();
            Container.BindInterfacesAndSelfTo<ShopService>().AsSingle();
        }
    }
}