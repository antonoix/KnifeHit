using System;
using System.Threading;
using System.Threading.Tasks;
using Internal.Scripts.GamePlay.Enemies;
using Internal.Scripts.GamePlay.TheMainHero.Combat;
using Internal.Scripts.Infrastructure.HeroRoute;
using Internal.Scripts.Infrastructure.Input;
using UnityEngine;
using UnityEngine.AI;

namespace Internal.Scripts.GamePlay.TheMainHero
{
    public class MainHero : MonoBehaviour, IDamageable
    {
        [SerializeField] private NavMeshAgent navAgent;
        [SerializeField] private CombatComponent combat;
        
        private InputService _playerInputService;
        private CancellationTokenSource _cancellation;

        public Transform Transform => transform;

        public void Setup(InputService inputService)
        {
            navAgent.updateRotation = false;
            _playerInputService = inputService;
            inputService.OnClicked += Shoot;
        }

        private void OnDestroy()
        {
            _playerInputService.OnClicked -= Shoot;
            
            _cancellation?.Cancel();
        }

        public void TakeDamage(int damage)
        {
            throw new NotImplementedException();
        }

        public void SetPositionAndRotation(Transform reference)
        {
            transform.position = reference.position;
            transform.rotation = reference.rotation;
        }

        public async Task GoToPoint(RouterPoint point, EnemiesPack enemiesPack)
        {
            _cancellation?.Cancel();
            _cancellation = new CancellationTokenSource();
            navAgent.SetDestination(point.transform.position);
            await RotateToTarget(point.transform.rotation.eulerAngles, 1500);
            while (navAgent.remainingDistance > 0.5f)
            {
                await Task.Yield();
            }
        }

        private void Shoot(Vector2 screenPosition)
        {
            combat.Shoot(screenPosition);
        }

        private async Task RotateToTarget(Vector3 targetRotation, float timeMs, Func<bool> stopCondition = null)
        {
            var cancellation = new CancellationTokenSource();

            var startRotation = transform.rotation.eulerAngles;
            targetRotation = new Vector3(startRotation.x, targetRotation.y, startRotation.z);
            float msPassed = 0;
            int frameDelayMs = 30;

            while (msPassed < timeMs)
            {
                if (stopCondition != null && stopCondition())
                {
                    cancellation.Cancel();
                    return;
                }
                
                var newRotation = Vector3.Lerp(startRotation, targetRotation, msPassed / timeMs);
                transform.rotation = Quaternion.Euler(newRotation);
                await Task.Delay(frameDelayMs, cancellation.Token);
                msPassed += frameDelayMs;
            }
        }

        public async Task RotateToEnemy(Enemy enemy)
        {
            Vector3 nearestEnemyPosition = enemy.transform.position;
            var rotation = Quaternion.LookRotation(nearestEnemyPosition - transform.position).eulerAngles;
            await RotateToTarget(rotation, 2000, () => enemy.IsDead);
        }
    }
}
