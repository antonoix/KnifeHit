using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Internal.Scripts.GamePlay.Enemies;
using Internal.Scripts.GamePlay.HeroRoute;
using Internal.Scripts.UI.GamePlay;

namespace Internal.Scripts.GamePlay.TheMainHero
{
    public class MainHeroConductor
    {
        private const float MAX_ENEMY_SCREEN_POS = 0.75f;
        private const float MIN_ENEMY_SCREEN_POS = 0.25f;
        
        private readonly MainHero _hero;
        private readonly HeroRouter _router;
        private readonly EnemiesHolder _enemiesHolder;
        private readonly GameplayUIPresenter _gameplayUIPresenter;
        private readonly MainHeroConfig _config;

        private int _passesPointsCount;
        private CancellationTokenSource _cts;

        public event Action OnLevelPassed;

        public MainHeroConductor(MainHero hero, HeroRouter router,
            EnemiesHolder enemiesHolder, GameplayUIPresenter gameplayUIPresenter, MainHeroConfig config)
        {
            _hero = hero;
            _router = router;
            _enemiesHolder = enemiesHolder;
            _gameplayUIPresenter = gameplayUIPresenter;
            _config = config;
            _cts = new CancellationTokenSource();
        }

        public void StartLevel()
        {
            _router.Initialize();

            _hero.SetPositionAndRotation(_router.StartPoint.transform);
            
            _gameplayUIPresenter.UpdateProgress((float)_passesPointsCount / _router.TotalPointsCount);
            
            GoThroughPoints();
        }

        private async void GoThroughPoints()
        {
            while (_router.TryGetNextPoint(out RouterPoint point) &&
                   _enemiesHolder.TryGetEnemiesPack(out EnemiesPack enemiesPack))
            {
                await UniTask.WaitForSeconds(1, cancellationToken: _cts.Token).SuppressCancellationThrow();
                
                await _hero.GoToPoint(point, enemiesPack);
                enemiesPack.Attack(_hero);
                await WaitUntilPointPassed(enemiesPack, point);

                _gameplayUIPresenter.UpdateProgress((float)++_passesPointsCount / _router.TotalPointsCount);
            }
            
            OnLevelPassed?.Invoke();
        }

        private async Task WaitUntilPointPassed(EnemiesPack enemiesPack, RouterPoint point)
        {
            while (enemiesPack.AliveEnemies.Count > 0 && !_cts.IsCancellationRequested)
            {
                var nearestEnemy = enemiesPack.GetNearestEnemy(point.transform.position);
                foreach (var enemy in enemiesPack.AliveEnemies)
                    enemy.SetIsNearest(enemy == nearestEnemy);

                var enemyOnScreen = _hero.HeroCam.WorldToViewportPoint(nearestEnemy.Transform.position);
                var IsEnemyOnScreen = enemyOnScreen.x < MAX_ENEMY_SCREEN_POS && enemyOnScreen.x > MIN_ENEMY_SCREEN_POS && enemyOnScreen.z >= 0;
                if (IsEnemyOnScreen)
                {
                    await UniTask.WaitForSeconds(_config.TimeBetweenRotationCheck);
                }
                else
                {
                    await _hero.RotateToEnemy(nearestEnemy);
                }
            }
        }

        public void Dispose()
        {
            _cts?.Cancel();
        }
    }
}
