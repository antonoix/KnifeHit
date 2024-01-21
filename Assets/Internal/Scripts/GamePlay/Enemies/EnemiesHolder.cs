using System;
using System.Collections.Generic;
using Internal.Scripts.Infrastructure.GameStatesMachine.Injection.StatesDependencies;
using UnityEngine;

namespace Internal.Scripts.GamePlay.Enemies
{
    public class EnemiesHolder : MonoBehaviour
    {
        [SerializeField] private EnemiesPack[] enemiesPacks;
    
        private Queue<EnemiesPack> _packsQueue;
        private EnemiesPack _currentEnemiesPack;

        public event Action OnEnemyAttackedHero;
    
        public void Initialize(ISpecialEffectsService specialEffectsService)
        {
            _packsQueue = new Queue<EnemiesPack>();

            foreach (var pack in enemiesPacks)
            {
                foreach (var enemy in pack.AliveEnemies)
                {
                    enemy.Initialize(specialEffectsService);
                    enemy.OnAttackedHero += HandleEnemyAttackHero;
                }
                _packsQueue.Enqueue(pack);
            }
        }

        private void HandleEnemyAttackHero()
        {
            OnEnemyAttackedHero?.Invoke();
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
    }
}
