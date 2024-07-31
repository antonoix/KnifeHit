using UnityEngine;
using Zenject;

namespace Internal.Scripts.GamePlay.LevelsService
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
            CreatedLevel.transform.parent = null;
            CreatedLevel.transform.position = Vector3.zero;

            return CreatedLevel;
        }

        public void DestroyCurrentLevel()
        {
            GameObject.Destroy(CreatedLevel.gameObject);
            CreatedLevel = null;
        }
    }
}
