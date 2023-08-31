using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Internal.Scripts.GamePlay.Enemies
{
    public class Enemy : MonoBehaviour, IDamageable
    {
        [SerializeField] private Animator animator;
        [SerializeField] private Transform hips;
        private int _health = 100;
        private bool _isGetDamageAnimationPlaying;
        private Rigidbody _rigidbody;
        private IDamageable _currentAim;
        private CancellationTokenSource _cancellation;
        
        public bool IsDead { get; private set; }

        private Rigidbody Rigidbody => _rigidbody ??= GetComponent<Rigidbody>();

        public Transform Transform => transform;

        public async void TakeDamage(int damage)
        {
            _health -= 20;

            if (_health <= 0)
            {
                IsDead = true;
                _cancellation?.Cancel();
            }

            
            StartCoroutine(TakeDamage());
        }

        public void SetAim(IDamageable aim)
        {
            _currentAim = aim;
            _cancellation?.Cancel();
            _cancellation = new CancellationTokenSource();
            GoToAim();
        }

        private async Task GoToAim()
        {
            animator.SetTrigger("Walk");
            while (!_cancellation.IsCancellationRequested)
            {
                await Task.Delay(50, _cancellation.Token);
                if (!_isGetDamageAnimationPlaying)
                {
                    transform.LookAt(_currentAim.Transform);
                    Rigidbody.velocity = transform.forward;
                    if (Vector3.Distance(transform.position, _currentAim.Transform.position) < 1)
                    {
                        Rigidbody.velocity = Vector3.zero;
                        _cancellation.Cancel();
                    }
                }
                else
                {
                    Rigidbody.velocity = Vector3.zero;;
                }
            }
        }

        private IEnumerator TakeDamage()
        {
            if (_isGetDamageAnimationPlaying) yield break;
            
            _isGetDamageAnimationPlaying = true;
            animator.enabled = false;
            animator.SetTrigger("StandUp");
            yield return new WaitForSeconds(3);

            Transform hipsParent = hips.parent;
            hips.SetParent(null);
            hips.transform.position = new Vector3(hips.transform.position.x, 0, hips.transform.position.z);
            transform.position = new Vector3(hips.position.x, transform.position.y, hips.transform.position.z);
            transform.rotation = Quaternion.Euler(0, hips.transform.rotation.eulerAngles.y, 0);
            hips.SetParent(hipsParent);

            animator.SetTrigger("Walk");
            if (!IsDead)
                animator.enabled = true;
            yield return new WaitForSeconds(1);
            _isGetDamageAnimationPlaying = false;
        }

        private void OnDestroy()
        {
            _cancellation?.Cancel();
        }
    }
}
