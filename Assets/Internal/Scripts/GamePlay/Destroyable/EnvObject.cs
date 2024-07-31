using System.Threading;
using Cysharp.Threading.Tasks;
using Internal.Scripts.GamePlay.TheMainHero.Combat;
using Internal.Scripts.GamePlay.Weapons;
using UnityEngine;

namespace Internal.Scripts.GamePlay.Destroyable
{
    [RequireComponent(typeof(Collider), typeof(Rigidbody))]
    public class EnvObject : MonoBehaviour
    {
        [SerializeField] private new Renderer renderer;
        private const int DURATION_EXPLOSION = 2;

        private readonly CancellationTokenSource _cancellation = new();
        private Rigidbody _body;
        
        public bool WasExploded { get; private set; }

        private void Awake()
        {
            _body = GetComponent<Rigidbody>();
        }

        private void OnDestroy()
        {
            _cancellation?.Cancel();
            
            var projectiles = transform.GetComponentsInChildren<WeaponProjectile>();
            foreach (var projectile in projectiles)
            {
                projectile.ForceRelease();
            }
        }

        public async UniTaskVoid Explode(Vector3 explosionPos, float force)
        {
            if (WasExploded) return;

            WasExploded = true;

            _body.AddExplosionForce(force, explosionPos, 4, 2);
            var newMaterial = ChangeMaterialToTransparent();

            int timeSec = 0;
            while (timeSec++ < DURATION_EXPLOSION)
            {
                var newColor = new Color(newMaterial.color.r, newMaterial.color.g, newMaterial.color.b, newMaterial.color.a - 0.2f);
                newMaterial.color = newColor;
                await UniTask.WaitForSeconds(Random.Range(0.8f, 1.2f), cancellationToken: _cancellation.Token);
            }
            
            Destroy(newMaterial);
            Destroy(gameObject);
        }

        private Material ChangeMaterialToTransparent()
        {
            Material newMaterial = new Material(renderer.material);
            var newColor = newMaterial.color;
            newMaterial.color = new Color(newColor.r, newColor.g, newColor.b, 0.8f);
            renderer.material = newMaterial;
            return newMaterial;
        }
    }
}
