using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Internal.Scripts.Infrastructure.GameStatesMachine.Injection.StatesDependencies;
using Internal.Scripts.Infrastructure.Services;
using Internal.Scripts.Infrastructure.Services.SpecialEffectsService;
using UnityEngine;

namespace Internal.Scripts.GamePlay.Enemies
{
    public class Enemy : MonoBehaviour, IDamageable
    {
        [SerializeField] private EnemyAnimation animation;
        [SerializeField] private Transform hips;
        [SerializeField] private EnemyConfig config;
        [SerializeField] private Rigidbody rootBody;
        [SerializeField] private List<Rigidbody> rigidbodies;
        private ISpecialEffectsService _specialEffectsService;
        private int _health = 100;
        private bool _isGetDamageAnimationPlaying;
        private Rigidbody _rigidbody;
        private List<Rigidbody> _rigidbodies;
        private IDamageable _currentAim;
        private CancellationTokenSource _cancellation;

        public bool IsDead { get; private set; }

        public Transform Transform => transform;
        private Rigidbody Rigidbody => _rigidbody ??= GetComponent<Rigidbody>();
        private List<Rigidbody> Rigidbodies => _rigidbodies ??= GetComponentsInChildren<Rigidbody>().ToList();

        public void Initialize(ISpecialEffectsService specialEffectsService)
        {
            _specialEffectsService = specialEffectsService;
        }

        public async void TakeDamage(int damage)
        {
            _health -= 20;

            if (_health <= 0)
            {
                IsDead = true;
                animation.EnableAnimator(false);
                _cancellation?.Cancel();
            }
            PlayDamageEffect();
        }

        public void SetAim(IDamageable aim)
        {
            _currentAim = aim;
            _cancellation?.Cancel();
            _cancellation = new CancellationTokenSource();
            GoToAim();
        }

        private async UniTask GoToAim()
        {
            while (!_cancellation.IsCancellationRequested)
            {
                await UniTask.Delay(25, cancellationToken: _cancellation.Token);
                if (_isGetDamageAnimationPlaying) continue;

                animation.SetWalk(true);
                
                var lerpAim = Vector3.Lerp(transform.position + transform.forward, _currentAim.Transform.position, 0.03f);
                transform.LookAt(lerpAim);
                rootBody.velocity = transform.forward * config.SpeedMeterPerSec;
                if (Vector3.Distance(transform.position, _currentAim.Transform.position) < config.AttackDistance)
                {
                    animation.PlayAttack();
                    rootBody.velocity = Vector3.zero;
                    _cancellation.Cancel();
                    await UniTask.Delay(animation.GetCurrentAnimationLengthMs());
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
            await UniTask.Delay((int)(config.DisableTimeAfterDamagedSec * 1000));

            bool isFaceUp = Vector3.Angle(hips.transform.forward, Vector3.up) < 90 ? true : false;
            
            Transform hipsParent = hips.parent;
            hips.SetParent(null);
            // hips.transform.position = new Vector3(hips.transform.position.x, 0, hips.transform.position.z);
            transform.position = new Vector3(hips.position.x, transform.position.y, hips.transform.position.z);
            // transform.forward = isFaceUp ? hips.up * -1 : hips.up;
            hips.SetParent(hipsParent, true);

            if (!IsDead)
                animation.EnableAnimator(true);

            animation.PlayStandUp(isFaceUp);
            
            _specialEffectsService.ShowEffect(SpecialEffectType.EnemyResurrection, transform.position + Vector3.up * 0.5f);
            
            await UniTask.Delay((int)(config.StandingUpTimeAfterDamagedSec * 2200));
            _isGetDamageAnimationPlaying = false;
        }

        private void OnDestroy()
        {
            _cancellation?.Cancel();
        }
        
        
        [ContextMenu("CollectHipsColliders")]
        private void CollectHipsColliders()
        {
            rigidbodies = hips.GetComponentsInChildren<Rigidbody>().ToList();
        }
    }
}
