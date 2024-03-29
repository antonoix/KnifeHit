using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Internal.Scripts.GamePlay.Destroyable;
using Internal.Scripts.GamePlay.Enemies;
using Internal.Scripts.GamePlay.HeroRoute;
using Internal.Scripts.GamePlay.SpecialEffectsService;
using Internal.Scripts.GamePlay.TheMainHero.Combat;
using Internal.Scripts.Infrastructure.Input;
using Internal.Scripts.Infrastructure.Services.Sound;
using UnityEngine;
using UnityEngine.AI;

namespace Internal.Scripts.GamePlay.TheMainHero
{
    public class MainHero : MonoBehaviour, IDamageable
    {
        [SerializeField] private NavMeshAgent navAgent;
        [SerializeField] private CombatComponent combat;
        [SerializeField] private new MainHeroCamera camera;
        [SerializeField] private EnvDestroyer destroyer;
        
        private InputService _playerInputService;
        private CancellationTokenSource _cancellation;
        
        public Transform Transform => transform;
        public Camera HeroCam => camera.GetComponent<Camera>();
        public int ShotsCount { get; private set; }
        
        public event Action OnKilled;

        public void Setup(InputService inputService, ISpecialEffectsService specialEffectsService,
            ISoundsService soundsService, Projectile projectile)
        {
            _playerInputService = inputService;
            inputService.OnClicked += Shoot;
            
            destroyer.Construct(specialEffectsService);
            combat.Construct(soundsService, projectile);
        }

        private void OnDestroy()
        {
            _playerInputService.OnClicked -= Shoot;
            
            _cancellation?.Cancel();
        }

        public void TakeDamage(int damage)
        {
            OnKilled?.Invoke();
        }

        public void SetPositionAndRotation(Transform reference)
        {
            navAgent.Warp(reference.position);
            navAgent.updateRotation = false;
            navAgent.enabled = true;
            
            transform.rotation = reference.rotation;
        }

        public async UniTask GoToPoint(RouterPoint point, EnemiesPack enemiesPack)
        {
            _cancellation?.Cancel();
            _cancellation = new CancellationTokenSource();
            navAgent.updateRotation = true;
            navAgent.SetDestination(point.transform.position);

            while (navAgent.remainingDistance < 0.1f)
            {
                await UniTask.WaitForFixedUpdate();
            }

            while (navAgent.remainingDistance > 3f)
            {
                await UniTask.Yield();
            }
            
            navAgent.updateRotation = false;
            
            await RotateToTarget(point.transform.rotation.eulerAngles, 1000);
        }

        public async UniTask RotateToEnemy(Enemy enemy)
        {
            Vector3 nearestEnemyPosition = enemy.transform.position;
            var rotation = Quaternion.LookRotation(nearestEnemyPosition - transform.position).eulerAngles;
            await RotateToTarget(rotation, 700, () => enemy.IsDead);
        }

        public void RotateCameraUp()
        {
            StartCoroutine(camera.SmoothlyRotateUp());
        }

        private void Shoot(Vector2 screenPosition)
        {
            ShotsCount++;
            combat.Shoot(screenPosition);
        }

        private async UniTask RotateToTarget(Vector3 targetRotation, float timeMs, Func<bool> stopCondition = null)
        {
            var cancellation = new CancellationTokenSource();

            var startRotation = transform.rotation.eulerAngles;
            targetRotation = new Vector3(startRotation.x, targetRotation.y, startRotation.z);
            float startY = startRotation.y;
            float targetY = targetRotation.y;
            if (Mathf.Abs(startY - targetY) > 180)
            {
                if (targetY < 180)
                    targetY = 360 + targetY;
                else
                {
                    targetY = targetY - 360;
                }
            }

            targetRotation = new Vector3(targetRotation.x, targetY, targetRotation.z);
            float msPassed = 0;
            int frameDelayMs = 15;

            while (msPassed < timeMs)
            {
                if (stopCondition != null && stopCondition())
                {
                    cancellation.Cancel();
                    return;
                }
                
                var newRotation = Vector3.Lerp(startRotation, targetRotation, msPassed / timeMs);
                transform.rotation = Quaternion.Euler(newRotation);
                await UniTask.Delay(frameDelayMs, cancellationToken: cancellation.Token);
                msPassed += frameDelayMs;
            }
        }
    }
}
