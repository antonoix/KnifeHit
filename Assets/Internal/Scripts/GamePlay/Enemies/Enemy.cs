using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Internal.Scripts.GamePlay.Destroyable;
using Internal.Scripts.Infrastructure.Services.SpecialEffectsService;
using UnityEngine;

namespace Internal.Scripts.GamePlay.Enemies
{
    public class Enemy : MonoBehaviour, IDamageable
    {
        [SerializeField] private EnemyAnimation animation;
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
        private Rigidbody Rigidbody => _rigidbody ??= GetComponent<Rigidbody>();
        private List<Rigidbody> Rigidbodies => _rigidbodies ??= GetComponentsInChildren<Rigidbody>().ToList();

        public void Initialize(ISpecialEffectsService specialEffectsService)
        {
            _specialEffectsService = specialEffectsService;
            envDestroyer.Construct(_specialEffectsService);
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
            PlayDamageEffect();
            SetIsNearest(false);
        }

        public void SetAim(IDamageable aim)
        {
            _currentAim = aim;
            //_cancellation?.Cancel();
            _cancellation = new CancellationTokenSource();
            GoToAim();
        }

        public void SetIsNearest(bool isNearest)
        {
            aimOnBody.Activate(!_isGetDamageAnimationPlaying && isNearest);
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
                    animation.SetWalk(false);
                    await UniTask.Delay(1000, cancellationToken: _cancellation.Token);
                    _currentAim.TakeDamage(0);
                    //_cancellation.Cancel();
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
            
            _specialEffectsService.ShowEffect(SpecialEffectType.EnemyResurrection, transform.position + Vector3.up * 0.5f);
            
            await UniTask.Delay((int)(config.StandingUpTimeAfterDamagedSec * 2200));
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
