using System.Collections.Generic;
using Internal.Scripts.Infrastructure.Services.SpecialEffectsService;
using UnityEditor;
using UnityEngine;

namespace Internal.Scripts.GamePlay.Enemies
{
    public class EnemiesHolder : MonoBehaviour
    {
        [SerializeField] private List<EnemiesPack> enemiesPacks;
    
        private Queue<EnemiesPack> _packsQueue;
        private EnemiesPack _currentEnemiesPack;

        public int RewardForEnemies { get; private set; }

        public void Initialize(ISpecialEffectsService specialEffectsService)
        {
            _packsQueue = new Queue<EnemiesPack>();

            foreach (var pack in enemiesPacks)
            {
                foreach (var enemy in pack.AliveEnemies)
                {
                    enemy.Initialize(specialEffectsService);
                    RewardForEnemies += enemy.RewardCoinsCount;
                }
                _packsQueue.Enqueue(pack);
            }
        }

        public bool TryGetEnemiesPack(out EnemiesPack pack)
        {
            pack = null;
            if (_packsQueue.TryDequeue(out _currentEnemiesPack))
            {
                pack = _currentEnemiesPack;
                return true;
            }

            return false;
        }
        
        public void Dispose()
        {
            foreach (var enemiesPack in enemiesPacks) 
                enemiesPack.Dispose();
        }

#if UNITY_EDITOR
        private void Reset()
        {
            foreach (Transform child in transform)
                if (child.TryGetComponent<EnemiesPack>(out var enemy))
                    enemiesPacks.Add(enemy);
            
            foreach (var enemiesPack in enemiesPacks)
            {
                enemiesPack.CollectEnemies();
            }

            GameObject prefabInstance = PrefabUtility.InstantiatePrefab(transform.parent.gameObject) as GameObject;
            PrefabUtility.ApplyPrefabInstance(prefabInstance, InteractionMode.UserAction);
            AssetDatabase.SaveAssets();
            DestroyImmediate(prefabInstance.gameObject);
            
            // PrefabUtility.ApplyPrefabInstance(transform.parent.gameObject, InteractionMode.AutomatedAction);
        }  
#endif
    }
}
