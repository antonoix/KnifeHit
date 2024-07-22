using Internal.Scripts.GamePlay.ProjectilesService;
using Internal.Scripts.Infrastructure.Services.Sound;
using UnityEngine;
using Zenject;

namespace Internal.Scripts.GamePlay.TheMainHero.Combat
{
    public class CombatComponent : MonoBehaviour
    {
        private const int MAX_SHOOT_DISTANCE = 100;
        
        [SerializeField] private Transform projectileSpawnPoint;
        [SerializeField] private LayerMask enemyHipsMask;
        
        private MainHeroConfig _config;
        private ISoundsService _soundsService;
        private IProjectilesService _projectilesService;
        private Camera _mainCamera;
        private float _lastShootTime;

        private Camera MainCamera => _mainCamera ??= Camera.main;

        [Inject]
        private void Construct(ISoundsService soundsService, IProjectilesService projectilesService, MainHeroConfig config)
        {
            _soundsService = soundsService;
            _projectilesService = projectilesService;
            _config = config;
        }

        public void Shoot(Vector2 screenPosition)
        {
            if (Time.time < _lastShootTime + _config.ShootDelaySec)
                return;
            _lastShootTime = Time.time;
            
            _soundsService.PlaySound(SoundType.Throw);
            _projectilesService.ThrowProjectile(projectileSpawnPoint.position, CalculateProjectileDestination(screenPosition));
        }

        private Vector3 CalculateProjectileDestination(Vector2 screenPosition)
        {
            Ray ray = MainCamera.ScreenPointToRay(screenPosition);

            Vector3 destination = ray.GetPoint(MAX_SHOOT_DISTANCE);
            
            if (Physics.Raycast(ray, out RaycastHit hit, MAX_SHOOT_DISTANCE, _config.AimLayers))
            {
                destination = hit.point;
            }
            
            // Debug.DrawRay(ray.origin, ray.direction * 20, Color.red);
            // EditorApplication.isPaused = true;

            return destination;
        }
    }
}
