using Internal.Scripts.GamePlay.TheMainHero;
using UnityEngine;
using Zenject;

namespace Internal.Scripts.Infrastructure.Factory
{
    public class LevelFactory
    {
        private readonly IInstantiator _instantiator;
        private readonly LevelFactoryConfig _levelFactoryConfig;
        
        public LevelContext CreatedLevel { get; private set; }

        public LevelFactory(IInstantiator instantiator, LevelFactoryConfig levelFactoryConfig)
        {
            _instantiator = instantiator;
            _levelFactoryConfig = levelFactoryConfig;
        }

        public LevelContext CreateLevelContext(int levelIndex)
        {
            CreatedLevel = _instantiator.InstantiatePrefabForComponent<LevelContext>(_levelFactoryConfig.LevelContexts[levelIndex]);
            CreatedLevel.transform.position = Vector3.zero;

            return CreatedLevel;
        }
    
        public MainHero InstantiateHero()
        {
            var hero = _instantiator.InstantiatePrefabForComponent<MainHero>(_levelFactoryConfig.MainHero);

            return hero;
        }
    }
}
