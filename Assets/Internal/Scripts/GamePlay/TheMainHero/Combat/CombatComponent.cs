using UnityEngine;

namespace Internal.Scripts.GamePlay.TheMainHero.Combat
{
    public class CombatComponent : MonoBehaviour
    {
        [SerializeField] private Projectile projectilePrefab;
        [SerializeField] private Transform projectileSpawnPoint;
        [SerializeField] private LayerMask enemyHipsMask;

        private Camera _mainCamera;

        private Camera MainCamera => _mainCamera ??= Camera.main;

        public void Shoot(Vector2 screenPosition)
        {
            var projectile = Instantiate(projectilePrefab);
            projectile.transform.position = projectileSpawnPoint.position;
            projectile.transform.LookAt(CalculateProjectileDestination(screenPosition));
        }
        
        private Vector3 CalculateProjectileDestination(Vector2 screenPosition)
        {
            Ray ray = MainCamera.ScreenPointToRay(screenPosition);
            Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 2);
            
            Vector3 destination = ray.direction * 5000;
            
            if (Physics.Raycast(ray, out RaycastHit hit, 500, enemyHipsMask))
            {
                Debug.Log(hit.collider.gameObject.name);
                destination = hit.point;
            }

            return destination;
        }
    }
}
