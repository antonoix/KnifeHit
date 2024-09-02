using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using Internal.Scripts.GamePlay.Enemies;
using Internal.Scripts.GamePlay.SpecialEffectsService;
using Internal.Scripts.Infrastructure.Services.PlayerProgressService;
using UnityEngine;
using Zenject;

namespace Internal.Scripts.GamePlay.Weapons
{
    public class WeaponProjectile : MonoBehaviour
    {
        private const int DEGREES_IN_TURNOVER = 360;
        private const int LIFE_TIME = 5;
        private const int METERS_PER_TURNOVER = 9;

        public WeaponType Type;
        
        [SerializeField] private int damage = 40;
        [SerializeField] private int speedMeterPerSec = 15;
        [SerializeField] private Transform modelRoot;

        private ISpecialEffectsService _specialEffectsService;
        private Rigidbody _body;
        private bool _collided;
        private Coroutine _rotatingRoutine;

        public event Action<WeaponProjectile> OnNeedRelease;

        [Inject]
        private void Construct(ISpecialEffectsService specialEffectsService)
        {
            _specialEffectsService = specialEffectsService;
        }
        
        public async UniTaskVoid Throw(Vector3 startPos, Vector3 destinationPos)
        {
            transform.position = startPos;
            transform.LookAt(destinationPos);

            _body = GetComponent<Rigidbody>();
            _body ??= gameObject.AddComponent<Rigidbody>();
            if (_body == null)
                _body = gameObject.AddComponent<Rigidbody>();
            
            _body.isKinematic = false;
            _body.useGravity = false;
            _body.velocity = Vector3.zero;
            _body.AddForce(transform.forward * speedMeterPerSec, ForceMode.VelocityChange);
            StartRotating(destinationPos);
            
            GetComponent<Collider>().enabled = true;

            await UniTask.WaitForSeconds(LIFE_TIME);
            OnNeedRelease?.Invoke(this);
        }

        public void ForceRelease()
        {
            OnNeedRelease?.Invoke(this);
        }

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
            
            if (isActive == false && _rotatingRoutine != null)
            {
                StopCoroutine(_rotatingRoutine);
                _rotatingRoutine = null;
                _collided = false;
            }
        }

        public void SetParent(Transform parent)
        {
            transform.SetParent(parent, true);
        }

        public void SetScale(Vector3 scale)
        {
            transform.localScale = scale;
        }

        private void StartRotating(Vector3 destinationPos)
        {
            var distance = Vector3.Distance(transform.position, destinationPos);
            float flyTimeSec = distance / speedMeterPerSec;
            int rotationsCount = Convert.ToInt32(distance / METERS_PER_TURNOVER);
            float degreesPerSec = DEGREES_IN_TURNOVER * rotationsCount / flyTimeSec;
            
            _rotatingRoutine = StartCoroutine(Rotate(degreesPerSec));
        }

        private IEnumerator Rotate(float degreesPerSec)
        {
            var delayBetweenFrame = 50;
            while (!_collided)
            {
                modelRoot.Rotate(degreesPerSec / delayBetweenFrame, 0, 0);
                yield return new WaitForSeconds(1f / delayBetweenFrame);
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            _body.isKinematic = true;
            GetComponent<Collider>().enabled = false;
            Destroy(GetComponent<Rigidbody>());
            ChangeTransform(other).Forget();
            _collided = true;
            
            var enemy = TryFindEnemy(other.transform);
            if (enemy != null)
            {
                enemy.TakeDamage(damage, other.contacts[0].normal);
                _specialEffectsService.ShowEffect(SpecialEffectType.WeaponEnemyImpact, transform.position, transform.eulerAngles);
            }
            else
            {
                _specialEffectsService.ShowEffect(SpecialEffectType.WeaponEnvImpact, transform.position, transform.eulerAngles);
            }
        }

        private async UniTaskVoid ChangeTransform(Collision other)
        {
            Transform parent = other.transform;
            transform.position = other.contacts[0].point;
            //transform.rotation = Quaternion.Euler(other.contacts[0].normal);
            modelRoot.localRotation = Quaternion.identity;
            await UniTask.Yield(PlayerLoopTiming.Update);
            SetParent(parent);
        }

        private IDamageable TryFindEnemy(Transform gameObj)
        {
            if (gameObj.TryGetComponent(out IDamageable enemy))
            {
                return enemy;
            }
            if (gameObj.parent != null)
                return TryFindEnemy(gameObj.parent);
            
            return null;
        }
    }
}
