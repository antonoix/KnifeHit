using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Internal.Scripts.GamePlay.Destroyable;
using Internal.Scripts.GamePlay.SpecialEffectsService;
using UnityEngine;
using Zenject;

namespace Internal.Scripts.GamePlay.Enemies
{
    public class Enemy : MonoBehaviour, IDamageable
    {
        [SerializeField] private new EnemyAnimation animation;
        [SerializeField] private AimOnBody aimOnBody;
        [SerializeField] private EnvDestroyer envDestroyer;
        [SerializeField] private Transform hips;
        [SerializeField] private EnemyConfig config;
        [SerializeField] private Rigidbody rootBody;
        [SerializeField] private List<Rigidbody> rigidbodies;
        private ISpecialEffectsService _specialEffectsService;
        private int _health = 1;
        private bool _isGetDamageAnimationPlaying;
        private Rigidbody _rigidbody;
        private List<Rigidbody> _rigidbodies;
        private IDamageable _currentAim;
        private CancellationTokenSource _cancellation = new CancellationTokenSource();

        public bool IsDead { get; private set; }

        public Transform Transform => transform;
        public int RewardCoinsCount => config.RewardCoinsCount;
        private Rigidbody Rigidbody => _rigidbody ??= GetComponent<Rigidbody>();
        private List<Rigidbody> Rigidbodies => _rigidbodies ??= GetComponentsInChildren<Rigidbody>().ToList();

        [Inject]
        private void Construct(ISpecialEffectsService specialEffectsService)
        {
            _specialEffectsService = specialEffectsService;
        }

        private void Start()
        {
            _health = config.Health;
        }

        public void TakeDamage(int damage)
        {
            _health -= 1;

            if (_health <= 0)
            {
                IsDead = true;
                animation.EnableAnimator(false);
                _cancellation?.Cancel();
            }
            PlayDamageEffect().Forget();
            SetIsNearest(false);
        }

        public void SetAim(IDamageable aim)
        {
            _currentAim = aim;
            //_cancellation?.Cancel();
            _cancellation = new CancellationTokenSource();
            GoToAim().Forget();
        }

        public void SetIsNearest(bool isNearest)
        {
            aimOnBody.Activate(!_isGetDamageAnimationPlaying && isNearest);
        }

        private async UniTask GoToAim()
        {
            while (!_cancellation.IsCancellationRequested)
            {
                await UniTask.Yield(PlayerLoopTiming.FixedUpdate, cancellationToken: _cancellation.Token);
                if (_isGetDamageAnimationPlaying) continue;

                animation.SetWalk(true);
                
                // var lerpAim = Vector3.Lerp(transform.position + transform.forward, _currentAim.Transform.position, 0.03f);
                // transform.LookAt(lerpAim);
                
                
                Vector3 relativePos = _currentAim.Transform.position - transform.position;

                // the second argument, upwards, defaults to Vector3.up
                Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
                var lerpRot = Quaternion.Lerp(transform.rotation, rotation, 0.3f);
                rootBody.MoveRotation(lerpRot);
                
                rootBody.velocity = transform.forward * config.SpeedMeterPerSec;
                if (Vector3.Distance(transform.position, _currentAim.Transform.position) < config.AttackDistance)
                {
                    animation.PlayAttack();
                    rootBody.velocity = Vector3.zero;
                    animation.SetWalk(false);
                    await UniTask.Delay(1000, cancellationToken: _cancellation.Token);
                    _currentAim.TakeDamage(0);
                }
            }
        }

        private async UniTask PlayDamageEffect()
        {
            if (_isGetDamageAnimationPlaying) return;
            animation.SetWalk(false);
            await UniTask.WaitForEndOfFrame(this);
            foreach (var body in Rigidbodies)
            {
                body.isKinematic = false;
                body.velocity = Vector3.zero;
            }
            _isGetDamageAnimationPlaying = true;
            animation.EnableAnimator(false);
            await UniTask.Delay((int)(config.DisableTimeAfterDamagedSec * 1000), cancellationToken: _cancellation.Token);

            bool isFaceUp = Vector3.Angle(hips.transform.forward, Vector3.up) < 90 ? true : false;
            
            Transform hipsParent = hips.parent;
            hips.SetParent(null);
            transform.position = new Vector3(hips.position.x, transform.position.y, hips.transform.position.z);
            hips.SetParent(hipsParent, true);

            if (!IsDead)
                animation.EnableAnimator(true);

            animation.PlayStandUp(isFaceUp);
            
            _specialEffectsService.ShowEffect(SpecialEffectType.EnemyResurrection, transform.position + Vector3.up * 0.5f).Forget();
            
            await UniTask.Delay((int)(config.StandingUpTimeAfterDamagedSec * 1000));
            _isGetDamageAnimationPlaying = false;
        }

        private void OnDestroy()
        {
            _cancellation?.Cancel();
            Destroy(envDestroyer);
        }


        [ContextMenu("CollectHipsColliders")]
        private void CollectHipsColliders()
        {
            rigidbodies = hips.GetComponentsInChildren<Rigidbody>().ToList();
        }
    }
}
