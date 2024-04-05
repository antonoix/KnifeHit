using UnityEngine;
using Zenject;

namespace Internal.Scripts.Infrastructure.Factory
{
    [CreateAssetMenu(fileName = "LevelFactoryInstaller", menuName = "Installers/LevelFactoryInstaller")]
    public class LevelFactoryInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private LevelFactoryConfig config;
        
        public override void InstallBindings()
        {
            Container.BindInstance(config);
            Container.BindInterfacesAndSelfTo<LevelFactory>().AsSingle();
        }
    }
}