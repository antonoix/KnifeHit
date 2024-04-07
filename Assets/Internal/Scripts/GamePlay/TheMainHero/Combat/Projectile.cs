using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using Internal.Scripts.GamePlay.Enemies;
using Internal.Scripts.GamePlay.ShopSystem;
using UnityEngine;

namespace Internal.Scripts.GamePlay.TheMainHero.Combat
{
    public class Projectile : MonoBehaviour
    {
        private const int DEGREES_IN_TURNOVER = 360;
        private const int LIFE_TIME = 5;

        public WeaponType Type;
        [SerializeField] private int damage = 40;
        [SerializeField] private int speedMeterPerSec = 15;
        private Rigidbody _body;
        private bool _collided;

        public event Action<Projectile> OnNeedRelease;

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
            transform.parent = null;
            

            await UniTask.WaitForSeconds(LIFE_TIME);
            OnNeedRelease?.Invoke(this);
        }

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        private void StartRotating(Vector3 destinationPos)
        {
            var distance = Vector3.Distance(transform.position, destinationPos);
            float flyTimeSec = distance / speedMeterPerSec;
            int rotationsCount = Convert.ToInt32(flyTimeSec);
            float degreesPerSec = DEGREES_IN_TURNOVER * rotationsCount / flyTimeSec;

            StartCoroutine(Rotate(degreesPerSec));
        }

        private IEnumerator Rotate(float degreesPerSec)
        {
            while (!_collided)
            {
                _body.MoveRotation(_body.rotation * Quaternion.Euler(degreesPerSec / 50, 0, 0));
                yield return new WaitForSeconds(1f / 50);
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            _body.isKinematic = true;
            GetComponent<Collider>().enabled = false;
            Destroy(GetComponent<Rigidbody>());
            transform.SetParent(other.collider.transform, true);
            TryFindEnemy(other.transform)?.TakeDamage(damage);
            _collided = true;
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
