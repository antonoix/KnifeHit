using Internal.Scripts.GamePlay.ShopSystem.Configs;
using Internal.Scripts.Infrastructure.GameStatesMachine.States;
using UnityEngine;
using Zenject;

namespace Internal.Scripts.GamePlay.ShopSystem
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