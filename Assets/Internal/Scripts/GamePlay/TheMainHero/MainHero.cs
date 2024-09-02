using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Internal.Scripts.GamePlay.Enemies;
using Internal.Scripts.GamePlay.HeroRoute;
using Internal.Scripts.GamePlay.LevelsService;
using Internal.Scripts.GamePlay.TheMainHero.Combat;
using Internal.Scripts.Infrastructure.Services.Input;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Internal.Scripts.GamePlay.TheMainHero
{
    public class MainHero : MonoBehaviour, IDamageable
    {
        private const float THRESHOLD = 0.1f;
        private const float DISTANCE_TO_START_ROTATE = 3f;
        private const int MS_IN_SEC = 1000;

        [field: SerializeField] public Transform[] VisualEffectsPoints { get; private set; }
        [SerializeField] private NavMeshAgent navAgent;
        [SerializeField] private CombatComponent combat;
        [SerializeField] private new MainHeroCamera camera;

        private IInputService _playerInputService;
        private MainHeroConfig _config;
        private CancellationTokenSource _cancellation;
        
        public Transform Transform => transform;
        public Camera HeroCam => camera.GetComponent<Camera>();
        public int ShotsCount { get; private set; }
        
        public event Action OnKilled;

        [Inject]
        private void Construct(IInputService inputService, MainHeroConfig config)
        {
            _playerInputService = inputService;
            _config = config;
        }

        private void Start()
        {
            _playerInputService.OnClicked += Shoot;
        }

        private void OnDestroy()
        {
            _playerInputService.OnClicked -= Shoot;
            
            _cancellation?.Cancel();
        }

        public void TakeDamage(int damage, Vector3 dir)
        {
            OnKilled?.Invoke();
        }

        public void SetupNavMeshAgent(LevelContext level)
        {
            navAgent.agentTypeID = level.AgentTypeId;
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

            while (navAgent.remainingDistance < THRESHOLD)
            {
                await UniTask.WaitForFixedUpdate();
            }

            while (navAgent.remainingDistance > DISTANCE_TO_START_ROTATE)
            {
                await UniTask.Yield();
            }
            
            navAgent.updateRotation = false;
            
            await RotateToTarget(point.transform.rotation.eulerAngles, _config.RotationDurationSec * MS_IN_SEC);
        }

        public async UniTask RotateToEnemy(Enemy enemy)
        {
            Vector3 nearestEnemyPosition = enemy.transform.position;
            var rotation = Quaternion.LookRotation(nearestEnemyPosition - transform.position).eulerAngles;
            await RotateToTarget(rotation, _config.RotationDurationSec * MS_IN_SEC, () => enemy.IsDead);
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

            float targetY = GetTargetYRotation(startRotation.y, targetRotation.y);

            targetRotation = new Vector3(startRotation.x, targetY, startRotation.z);
            
            float msPassed = 0;
            int frameDelayMs = 15;

            while (msPassed < timeMs)
            {
                if (stopCondition != null && stopCondition())
                {
                    cancellation.Cancel();
                    return;
                }

                await UniTask.Delay(frameDelayMs, cancellationToken: cancellation.Token);
                var newRotation = Vector3.Lerp(startRotation, targetRotation, msPassed / timeMs);
                transform.rotation = Quaternion.Euler(newRotation);
                msPassed += frameDelayMs;
            }
        }

        private float GetTargetYRotation(float startY, float targetY)
        {
            if (Mathf.Abs(startY - targetY) > 180)
            {
                if (targetY < 180)
                    targetY = 360 + targetY;
                else
                {
                    targetY -= 360;
                }
            }

            return targetY;
        }
    }
}
