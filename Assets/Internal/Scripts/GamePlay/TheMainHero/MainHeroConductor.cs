using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Internal.Scripts.GamePlay.Enemies;
using Internal.Scripts.Infrastructure.HeroRoute;
using UnityEngine;

namespace Internal.Scripts.GamePlay.TheMainHero
{
    public class MainHeroConductor
    {
        private readonly MainHero _hero;
        private readonly HeroRouter _router;
        private readonly EnemiesHolder _enemiesHolder;

        private CancellationTokenSource _cts;

        public event Action OnLevelPassed;

        public MainHeroConductor(MainHero hero, HeroRouter router, EnemiesHolder enemiesHolder)
        {
            _hero = hero;
            _router = router;
            _enemiesHolder = enemiesHolder;
            _cts = new CancellationTokenSource();
        }

        public void StartLevel()
        {
            _router.Initialize();
            _enemiesHolder.Initialize();
            
            _hero.SetPositionAndRotation(_router.StartPoint.transform);
            
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
                    while (enemiesPack.AliveEnemies.Count > 0)
                    {
                        await _hero.RotateToEnemy(enemiesPack.GetNearestEnemy(point.transform.position));
                    }
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
