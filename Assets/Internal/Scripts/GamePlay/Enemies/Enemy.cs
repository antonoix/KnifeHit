using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Internal.Scripts.GamePlay.Enemies
{
    public class Enemy : MonoBehaviour, IDamageable
    {
        [SerializeField] private Animator animator;
        [SerializeField] private Transform hips;
        [SerializeField] private EnemyConfig config;
        private int _health = 100;
        private bool _isGetDamageAnimationPlaying;
        private Rigidbody _rigidbody;
        private List<Rigidbody> _rigidbodies;
        private IDamageable _currentAim;
        private CancellationTokenSource _cancellation;
        
        public bool IsDead { get; private set; }

        private Rigidbody Rigidbody => _rigidbody ??= GetComponent<Rigidbody>();
        private List<Rigidbody> Rigidbodies => _rigidbodies ??= GetComponentsInChildren<Rigidbody>().ToList();

        public Transform Transform => transform;

        public async void TakeDamage(int damage)
        {
            _health -= 20;

            if (_health <= 0)
            {
                IsDead = true;
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
            animator.SetBool("Walk", true);
            while (!_cancellation.IsCancellationRequested)
            {
                await UniTask.Delay(50, cancellationToken: _cancellation.Token);
                if (_isGetDamageAnimationPlaying) continue;
                
                transform.LookAt(_currentAim.Transform);
                Rigidbody.velocity = transform.forward * config.SpeedMeterPerSec;
                if (Vector3.Distance(transform.position, _currentAim.Transform.position) < config.AttackDistance)
                {
                    animator.SetTrigger("Attack");
                    Rigidbody.velocity = Vector3.zero;
                    _cancellation.Cancel();
                }
            }
        }

        private async UniTask PlayDamageEffect()
        {
            if (_isGetDamageAnimationPlaying) return;
            foreach (var body in Rigidbodies)
            {
                body.velocity = Vector3.zero;
            }
            _isGetDamageAnimationPlaying = true;
            animator.enabled = false;
            await UniTask.Delay((int)(config.DisableTimeAfterDamagedSec * 1000));

            if (!IsDead)
                animator.enabled = true;
            animator.SetTrigger("StandUp");
            await UniTask.Delay((int)(config.StandingUpTimeAfterDamagedSec * 1000));
            _isGetDamageAnimationPlaying = false;
        }

        private void OnDestroy()
        {
            _cancellation?.Cancel();
        }
    }
}
