using UnityEngine;

namespace Internal.Scripts.GamePlay.TheMainHero.Combat
{
    public class CombatComponent : MonoBehaviour
    {
        [SerializeField] private Projectile projectilePrefab;
        [SerializeField] private Transform projectileSpawnPoint;

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
            
            Vector3 destination = ray.direction * 5000;
            
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                destination = hit.point;
            }

            return destination;
        }
    }
}
