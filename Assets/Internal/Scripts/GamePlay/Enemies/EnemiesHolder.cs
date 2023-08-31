using System.Collections.Generic;
using UnityEngine;

namespace Internal.Scripts.GamePlay.Enemies
{
    public class EnemiesHolder : MonoBehaviour
    {
        [SerializeField] private EnemiesPack[] enemiesPacks;
    
        private Queue<EnemiesPack> _packsQueue;
        private EnemiesPack _currentEnemiesPack;
    
        public void Initialize()
        {
            _packsQueue = new Queue<EnemiesPack>();

            foreach (var pack in enemiesPacks)
            {
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
    }
}
