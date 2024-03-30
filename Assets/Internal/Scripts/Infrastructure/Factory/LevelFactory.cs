using Internal.Scripts.GamePlay.TheMainHero;
using UnityEngine;
using Zenject;

namespace Internal.Scripts.Infrastructure.Factory
{
    public class LevelFactory
    {
        private readonly IInstantiator _instantiator;
        private readonly LevelFactoryConfig _levelFactoryConfig;

        public LevelFactory(IInstantiator instantiator, LevelFactoryConfig levelFactoryConfig)
        {
            _instantiator = instantiator;
            _levelFactoryConfig = levelFactoryConfig;
        }

        public LevelContext CreateLevelContext(int levelIndex)
        {
            var levelContext = _instantiator.InstantiatePrefabForComponent<LevelContext>(_levelFactoryConfig.LevelContexts[levelIndex]);
            levelContext.transform.position = Vector3.zero;

            return levelContext;
        }
    
        public MainHero InstantiateHero()
        {
            var hero = _instantiator.InstantiatePrefabForComponent<MainHero>(_levelFactoryConfig.MainHero);

            return hero;
        }
    }
}
