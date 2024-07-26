using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Internal.Scripts.GamePlay.TheMainHero;
using Internal.Scripts.GamePlay.TheMainHero.Combat;
using Internal.Scripts.Infrastructure.AssetManagement;
using Internal.Scripts.Infrastructure.Services.PlayerProgressService;
using UnityEngine;
using Zenject;

namespace Internal.Scripts.Infrastructure.Factory
{
    public class LevelFactory
    {
        private readonly IInstantiator _instantiator;

        public LevelContext CreatedLevel { get; private set; }

        public LevelFactory(IInstantiator instantiator)
        {
            _instantiator = instantiator;
        }

        public LevelContext CreateLevelContext(GameObject levelPrefab)
        {
            CreatedLevel = _instantiator.InstantiatePrefabForComponent<LevelContext>(levelPrefab);
            CreatedLevel.transform.position = Vector3.zero;

            return CreatedLevel;
        }
    }
}
