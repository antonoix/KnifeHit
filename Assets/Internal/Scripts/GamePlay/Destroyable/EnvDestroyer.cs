using Internal.Scripts.GamePlay.SpecialEffectsService;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Internal.Scripts.GamePlay.Destroyable
{
    [RequireComponent(typeof(Collider), typeof(Rigidbody))]
    public class EnvDestroyer : MonoBehaviour
    {
        private readonly Vector3 _explosionYOffset = Vector3.down * 0.65f;
        private ISpecialEffectsService _specialEffects;
        private Rigidbody _body;
        private Transform _parent;

        private void Awake()
        {
            _body = GetComponent<Rigidbody>();
            _parent = transform.parent;
            transform.parent = null;
        }

        public void Construct(ISpecialEffectsService specialEffectsService)
        {
            _specialEffects = specialEffectsService;
        }

        private void FixedUpdate()
        {
            _body.MovePosition(_parent.position);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent<EnvObject>(out var envObject))
            {
                if (envObject.WasExploded)
                    return;
                
                float force = Random.Range(400, 800f);
                var distanceFromEnvObject = (transform.position - collision.transform.position) * 0.5f;
                envObject.Explode(collision.transform.position + distanceFromEnvObject + _explosionYOffset, force).Forget();

                _specialEffects?.ShowEffect(SpecialEffectType.CloudyExplosion, collision.contacts[0].point);
            }
        }
    }
}
