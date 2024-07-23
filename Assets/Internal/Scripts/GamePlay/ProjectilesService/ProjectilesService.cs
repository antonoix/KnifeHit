using Internal.Scripts.GamePlay.TheMainHero.Combat;
using Internal.Scripts.Infrastructure.Factory;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;

namespace Internal.Scripts.GamePlay.ProjectilesService
{
    public class ProjectilesService : IProjectilesService, IInitializable
    {
        private readonly LevelFactory _levelFactory;
        private IObjectPool<WeaponProjectile> _pool;

        public ProjectilesService(LevelFactory levelFactory)
        {
            _levelFactory = levelFactory;
        }

        public void Initialize()
        {
            _pool = new ObjectPool<WeaponProjectile>(CreateProjectile, OnGetProjectile, OnReleaseProjectile);
        }

        public void ThrowProjectile(Vector3 startPoint, Vector3 destinationPoint)
        {
            var projectile = _pool.Get();
            projectile.Throw(startPoint, destinationPoint).Forget();

            projectile.OnNeedRelease += HandleProjectileRelease;
        }

        private void HandleProjectileRelease(WeaponProjectile weaponProjectile)
        {
            weaponProjectile.OnNeedRelease -= HandleProjectileRelease;
            _pool.Release(weaponProjectile);
        }

        private WeaponProjectile CreateProjectile()
        {
            return _levelFactory.CreateProjectile();
        }

        private void OnGetProjectile(WeaponProjectile weaponProjectile)
        {
            weaponProjectile.SetActive(true);
        }

        private void OnReleaseProjectile(WeaponProjectile weaponProjectile)
        {
            weaponProjectile.SetActive(false);
        }
    }
}