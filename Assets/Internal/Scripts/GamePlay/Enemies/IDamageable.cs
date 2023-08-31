using UnityEngine;

namespace Internal.Scripts.GamePlay.Enemies
{
    public interface IDamageable
    {
        Transform Transform { get; }
        void TakeDamage(int damage);
        
    }
}
