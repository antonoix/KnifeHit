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
                animator.enabled = false;
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
                await UniTask.Delay(25, cancellationToken: _cancellation.Token);
                if (_isGetDamageAnimationPlaying) continue;

                var lerpAim = Vector3.Lerp(transform.position + transform.forward, _currentAim.Transform.position, 0.03f);
                transform.LookAt(lerpAim);
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

            bool isFaceUp = Vector3.Angle(hips.transform.forward, Vector3.up) < 90 ? true : false;
            
            Transform hipsParent = hips.parent;
            hips.SetParent(null);
            hips.transform.position = new Vector3(hips.transform.position.x, 0, hips.transform.position.z);
            transform.position = new Vector3(hips.position.x, transform.position.y, hips.transform.position.z);
            transform.forward = isFaceUp ? hips.up * -1 : hips.up;
            hips.SetParent(hipsParent, false);

            if (!IsDead)
                animator.enabled = true;

            string standUpTrigger = isFaceUp ? "StandUpFaceUp" : "StandUpFaceDown";
            animator.SetTrigger(standUpTrigger);
            await UniTask.Delay((int)(config.StandingUpTimeAfterDamagedSec * 2200));
            _isGetDamageAnimationPlaying = false;
        }

        private void OnDestroy()
        {
            _cancellation?.Cancel();
        }
    }
}
