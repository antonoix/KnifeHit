using UnityEngine;
using Zenject;

namespace Internal.Scripts.Infrastructure.Factory
{
    [CreateAssetMenu(fileName = "ShopFactoryInstaller", menuName = "Installers/ShopFactoryInstaller")]
    public class ShopFactoryInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private ShopFactoryConfig config;
        
        public override void InstallBindings()
        {
            Container.BindInstance(config);
            Container.BindInterfacesAndSelfTo<ShopFactory>().AsSingle();
        }
    }
}