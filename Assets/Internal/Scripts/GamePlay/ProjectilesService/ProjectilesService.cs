using System.Linq;
using Cysharp.Threading.Tasks;
using Internal.Scripts.GamePlay.TheMainHero.Combat;
using Internal.Scripts.Infrastructure.Factory;
using Internal.Scripts.Infrastructure.PlayerProgressService;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;

namespace Internal.Scripts.GamePlay.ProjectilesService
{
    public class ProjectilesService : IProjectilesService, IInitializable
    {
        private readonly LevelFactory _levelFactory;
        private IObjectPool<Projectile> _pool;

        public ProjectilesService(LevelFactory levelFactory)
        {
            _levelFactory = levelFactory;
        }

        public void Initialize()
        {
            _pool = new ObjectPool<Projectile>(CreateProjectile, OnGetProjectile, OnReleaseProjectile);
        }

        public void ThrowProjectile(Vector3 startPoint, Vector3 destinationPoint)
        {
            var projectile = _pool.Get();
            projectile.Throw(startPoint, destinationPoint).Forget();

            projectile.OnNeedRelease += HandleProjectileRelease;
        }

        private void HandleProjectileRelease(Projectile projectile)
        {
            projectile.OnNeedRelease -= HandleProjectileRelease;
            _pool.Release(projectile);
        }

        private Projectile CreateProjectile()
        {
            return _levelFactory.CreateProjectile();
        }

        private void OnGetProjectile(Projectile projectile)
        {
            projectile.SetActive(true);
        }

        private void OnReleaseProjectile(Projectile projectile)
        {
            projectile.SetActive(false);
        }
    }
}