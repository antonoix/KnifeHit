using System.Security.Cryptography;
using Internal.Scripts.GamePlay.Enemies;
using UnityEngine;

namespace Internal.Scripts.GamePlay.TheMainHero.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private int damage = 40;
        [SerializeField] private int speedMeterPerSec = 15;
        private Vector3 _startWorldPos;

        private void Start()
        {
            _startWorldPos = transform.position;
            GetComponent<Rigidbody>().AddForce(transform.forward * speedMeterPerSec, ForceMode.VelocityChange);
        }

        private void OnCollisionEnter(Collision other)
        {
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<Collider>().enabled = false;
            transform.SetParent(other.collider.transform, true);
            //transform.forward = other.GetContact(0).normal;
            //Debug.Break();
            //GetComponent<Rigidbody>().velocity = Vector3.zero;
            Destroy(GetComponent<Rigidbody>());
            TryFindEnemy(other.transform)?.TakeDamage(damage);
        }

        private IDamageable TryFindEnemy(Transform gameObject)
        {
            if (gameObject.TryGetComponent(out IDamageable enemy))
            {
                return enemy;
            }
            if (gameObject.parent != null)
                return TryFindEnemy(gameObject.parent);
            
            return null;
        }
    }
}
