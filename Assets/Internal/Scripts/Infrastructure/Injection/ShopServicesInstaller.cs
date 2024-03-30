using Internal.Scripts.GamePlay.ShopSystem;
using Internal.Scripts.Infrastructure.GameStatesMachine.States;
using UnityEngine;
using Zenject;

namespace Internal.Scripts.Infrastructure.Injection
{
    [CreateAssetMenu(fileName = "ShopServicesInstaller", menuName = "Installers/ShopServicesInstaller")]
    public class ShopServicesInstaller : ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<ShopState>().AsSingle();
            Container.BindInterfacesAndSelfTo<ShopService>().AsSingle();
        }
    }
}