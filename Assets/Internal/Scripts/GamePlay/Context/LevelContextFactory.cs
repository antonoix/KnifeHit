using Internal.Scripts.GamePlay.TheMainHero;
using Internal.Scripts.Infrastructure.GameStatesMachine.Injection.StatesDependencies;
using Internal.Scripts.Infrastructure.HeroRoute;
using UnityEngine;

namespace Internal.Scripts.GamePlay
{
    public class LevelContextFactory
    {
        private readonly MainHero _heroPrefab;
        private readonly LevelContext _levelContextPrefab;

        public LevelContextFactory(MainHero heroPrefab, LevelContext levelContext)
        {
            _heroPrefab = heroPrefab;
            _levelContextPrefab = levelContext;
        }
        
        public LevelContext InstantiateLevelContext()
        {
            var levelContext = Object.Instantiate(_levelContextPrefab);
            levelContext.transform.position = Vector3.zero;

            return levelContext;
        }
    
        public MainHero InstantiateHero()
        {
            return Object.Instantiate(_heroPrefab);
        }
    }
}
