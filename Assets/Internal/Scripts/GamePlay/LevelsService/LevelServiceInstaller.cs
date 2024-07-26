using Internal.Scripts.GamePlay.LevelsService;
using UnityEngine;
using Zenject;

namespace Internal.Scripts.Infrastructure.Factory
{
    [CreateAssetMenu(fileName = "LevelServiceInstaller", menuName = "Installers/LevelServiceInstaller")]
    public class LevelServiceInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private LevelsConfig config;
        
        public override void InstallBindings()
        {
            Container.BindInstance(config);
            Container.BindInterfacesAndSelfTo<LevelsService>().AsSingle();
            Container.BindInterfacesAndSelfTo<LevelFactory>().AsSingle();
        }
    }
}