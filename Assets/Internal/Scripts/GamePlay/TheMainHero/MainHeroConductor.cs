using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Internal.Scripts.GamePlay.Enemies;
using Internal.Scripts.GamePlay.HeroRoute;
using Internal.Scripts.UI.GamePlay;
using UnityEngine;

namespace Internal.Scripts.GamePlay.TheMainHero
{
    public class MainHeroConductor
    {
        private readonly MainHero _hero;
        private readonly HeroRouter _router;
        private readonly EnemiesHolder _enemiesHolder;
        private readonly GameplayUIPresenter _gameplayUIPresenter;

        private int _passesPointsCount;
        private CancellationTokenSource _cts;

        public event Action OnLevelPassed;

        public MainHeroConductor(MainHero hero, HeroRouter router, EnemiesHolder enemiesHolder, GameplayUIPresenter gameplayUIPresenter)
        {
            _hero = hero;
            _router = router;
            _enemiesHolder = enemiesHolder;
            _gameplayUIPresenter = gameplayUIPresenter;
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
            while (!_cts.IsCancellationRequested)
            {
                await UniTask.Delay(1000, cancellationToken: _cts.Token);

                if (_router.TryGetNextPoint(out RouterPoint point) &&
                    _enemiesHolder.TryGetEnemiesPack(out EnemiesPack enemiesPack))
                {
                    await _hero.GoToPoint(point, enemiesPack);
                    enemiesPack.Attack(_hero);
                    while (enemiesPack.AliveEnemies.Count > 0 && !_cts.IsCancellationRequested)
                    {
                        var nearestEnemy = enemiesPack.GetNearestEnemy(point.transform.position);
                        foreach (var enemy in enemiesPack.AliveEnemies) 
                            enemy.SetIsNearest(enemy == nearestEnemy);

                        var enemyOnScreen = _hero.HeroCam.WorldToViewportPoint(nearestEnemy.Transform.position);
                        if (enemyOnScreen.x < 0.9f && enemyOnScreen.x > 0.1f && enemyOnScreen.z >= 0)
                        {
                            await UniTask.WaitForSeconds(1);
                        }
                        else
                        {
                            await _hero.RotateToEnemy(nearestEnemy);
                        }
                        
                    }
                    
                    _gameplayUIPresenter.UpdateProgress((float)++_passesPointsCount / _router.TotalPointsCount);
                    
                    Debug.Log("GO");
                }
                else
                {
                    OnLevelPassed?.Invoke();
                    return;
                }
            }
        }

        public void Dispose()
        {
            _cts?.Cancel();
        }
    }
}
