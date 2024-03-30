using System;
using System.Linq;
using Internal.Scripts.Infrastructure.Factory;
using Internal.Scripts.Infrastructure.Injection;
using Internal.Scripts.Infrastructure.Services.ProgressService;
using Internal.Scripts.Infrastructure.Services.Sound;
using UnityEngine;
using Zenject;

namespace Internal.Scripts.GamePlay.TheMainHero.Combat
{
    public class CombatComponent : MonoBehaviour
    {
        [SerializeField] private Transform projectileSpawnPoint;
        [SerializeField] private LayerMask enemyHipsMask;

        private Projectile _projectilePrefab;
        private LevelFactoryConfig _levelFactoryConfig;
        private ISoundsService _soundsService;
        private IPlayerProgressService _playerProgressService;
        private Camera _mainCamera;
        private float _lastShootTime;

        private Camera MainCamera => _mainCamera ??= Camera.main;

        [Inject]
        private void Construct(ISoundsService soundsService, LevelFactoryConfig levelFactoryConfig, IPlayerProgressService playerProgressService)
        {
            _soundsService = soundsService;
            _levelFactoryConfig = levelFactoryConfig;
            _playerProgressService = playerProgressService;
        }

        private void Start()
        {
            _projectilePrefab = _levelFactoryConfig.AllProjectiles.FirstOrDefault(x => x.Type == _playerProgressService.GetCurrentSelectedWeapon());
        }

        public void Shoot(Vector2 screenPosition)
        {
            if (Time.time < _lastShootTime + 0.3f)
                return;
            _lastShootTime = Time.time;
            
            var projectile = Instantiate(_projectilePrefab);
            _soundsService.PlaySound(SoundType.Throw);
            projectile.Setup(projectileSpawnPoint.position, CalculateProjectileDestination(screenPosition));
        }

        private Vector3 CalculateProjectileDestination(Vector2 screenPosition)
        {
            Ray ray = MainCamera.ScreenPointToRay(screenPosition);
            Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 2);
            
            Vector3 destination = ray.direction * 500;
            
            if (Physics.Raycast(ray, out RaycastHit hit, 500))
            {
                destination = hit.point;
            }

            return destination;
        }
    }
}
