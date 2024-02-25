using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Internal.Scripts.GamePlay.Enemies
{
    public class EnemiesPack : MonoBehaviour
    {
        [SerializeField] private List<Enemy> enemies;

        public List<Enemy> AliveEnemies => enemies.FindAll(enemy => !enemy.IsDead);

        public bool IsPassed
        {
            get { return enemies.All(enemy => enemy.IsDead); }
        }

        public Enemy GetNearestEnemy(Vector3 position)
        {
            var minDistance = float.MaxValue;
            Enemy neaerestEnemy = null;

            foreach (var enemy in AliveEnemies)
            {
                var sqrMagnitude = Vector3.SqrMagnitude(enemy.transform.position - position);
                if (sqrMagnitude < minDistance)
                {
                    minDistance = sqrMagnitude;
                    neaerestEnemy = enemy;
                }
            }

            return neaerestEnemy;
        }

        public void Attack(IDamageable aim)
        {
            foreach (var enemy in enemies)
            {
                enemy.SetAim(aim);
            }
        }

        public void CollectEnemies()
        {
            enemies.Clear();

            foreach (Transform child in transform)
                if (child.TryGetComponent<Enemy>(out var enemy))
                    enemies.Add(enemy);
        }

        public void Dispose()
        {
            foreach (var enemy in enemies)
            {
                Destroy(enemy.gameObject);
            }
        }
    }
}