using System.Security.Cryptography;
using Internal.Scripts.GamePlay.Enemies;
using UnityEngine;

namespace Internal.Scripts.GamePlay.TheMainHero.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private int damage = 40;
        [SerializeField] private int speedMeterPerSec = 15;

        private void Start()
        {
            GetComponent<Rigidbody>().AddForce(transform.forward * speedMeterPerSec, ForceMode.VelocityChange);
        }

        private void OnCollisionEnter(Collision other)
        {
            TryFindEnemy(other.transform)?.TakeDamage(damage);
            transform.SetParent(other.transform);
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            Destroy(GetComponent<Rigidbody>());
            Destroy(GetComponent<Collider>());
            transform.position = other.contacts[0].point;
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
