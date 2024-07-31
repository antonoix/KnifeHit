using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;

namespace Internal.Scripts.GamePlay.Weapons.ProjectilesService
{
    public class ProjectilesService : IProjectilesService, IInitializable
    {
        private readonly ProjectilesFactory _projectilesFactory;
        private IObjectPool<WeaponProjectile> _pool;

        public ProjectilesService(ProjectilesFactory projectilesFactory)
        {
            _projectilesFactory = projectilesFactory;
        }

        public void Initialize()
        {
            _pool = new ObjectPool<WeaponProjectile>(CreateProjectile, OnGetProjectile, OnReleaseProjectile);
            _projectilesFactory.Initialize().Forget();
        }

        public async UniTaskVoid ThrowProjectile(Vector3 startPoint, Vector3 destinationPoint)
        {
            await UniTask.WaitUntil(() => _projectilesFactory.IsInitialized);
            
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
            return _projectilesFactory.CreateProjectile();
        }

        private void OnGetProjectile(WeaponProjectile weaponProjectile)
        {
            weaponProjectile.SetParent(null);
            weaponProjectile.SetActive(true);
        }

        private void OnReleaseProjectile(WeaponProjectile weaponProjectile)
        {
            weaponProjectile.SetParent(null);
            weaponProjectile.SetActive(false);
        }
    }
}