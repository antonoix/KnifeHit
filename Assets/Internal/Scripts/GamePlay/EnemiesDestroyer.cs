using System;
using Internal.Scripts.GamePlay.Enemies;
using UnityEngine;

namespace Internal.Scripts.GamePlay
{
    public class EnemiesDestroyer : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            TryFindEnemy(other.transform)?.TakeDamage(Int32.MaxValue);
        }
        
        private IDamageable TryFindEnemy(Transform gameObj)
        {
            if (gameObj.TryGetComponent(out IDamageable enemy))
            {
                return enemy;
            }
            if (gameObj.parent != null)
                return TryFindEnemy(gameObj.parent);
            
            return null;
        }
    }
}