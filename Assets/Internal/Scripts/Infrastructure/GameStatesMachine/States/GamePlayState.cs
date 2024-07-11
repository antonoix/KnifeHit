using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Internal.Scripts.GamePlay.Enemies;
using Internal.Scripts.GamePlay.SpecialEffectsService;
using Internal.Scripts.GamePlay.TheMainHero;
using Internal.Scripts.Infrastructure.Constants;
using Internal.Scripts.Infrastructure.Factory;
using Internal.Scripts.Infrastructure.SaveLoad;
using Internal.Scripts.Infrastructure.Services.Ads;
using Internal.Scripts.Infrastructure.Services.Analytics;
using Internal.Scripts.Infrastructure.Services.PlayerProgressService;
using Internal.Scripts.Infrastructure.Services.PlayerProgressService.PlayerResource;
using Internal.Scripts.Infrastructure.Services.UiService;
using Internal.Scripts.UI.GamePlay;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using Random = UnityEngine.Random;

namespace Internal.Scripts.Infrastructure.GameStatesMachine.States
{
    public class GamePlayState : IGameState, IInitializable, ILateDisposable
    {
        private const float ONE_STAR = 0.33f;
        
        private readonly LevelFactory _levelFactory;
        private readonly LevelFactoryConfig _levelFactoryConfig;
        private readonly IGameStatesMachine _gameStatesMachine;
        private readonly IUiService _uiService;
        private readonly IPersistentProgressService _playerProgressService;
        private readonly ISaveLoadService _saveLoadService;
        private readonly IAdsService _adsService;
        private readonly IAnalyticsService _analyticsService;
        private readonly ISpecialEffectsService _specialEffectsService;
        private readonly MainHeroConfig _heroConfig;
        
        private MainHeroConductor _heroConductor;
        private GameplayUIPresenter _gameplayUiPresenter;
        private MainHero _hero;
        private EnemiesHolder _enemiesHolder;

        public GamePlayState(IGameStatesMachine gameStatesMachine,
            LevelFactory levelFactory,
            LevelFactoryConfig levelFactoryConfig,
            IUiService uiService,
            IPersistentProgressService playerProgressService,
            ISaveLoadService saveLoadService,
            IAdsService adsService,
            IAnalyticsService analyticsService,
            ISpecialEffectsService specialEffectsService,
            MainHeroConfig heroConfig)
        {
            _gameStatesMachine = gameStatesMachine;
            _levelFactory = levelFactory;
            _levelFactoryConfig = levelFactoryConfig;
            _uiService = uiService;
            _playerProgressService = playerProgressService;
            _saveLoadService = saveLoadService;
            _adsService = adsService;
            _analyticsService = analyticsService;
            _specialEffectsService = specialEffectsService;
            _heroConfig = heroConfig;
        }

        public void Initialize()
        {
            _gameStatesMachine.RegisterState<GamePlayState>(this);
        }

        public void LateDispose()
        {
            _gameStatesMachine.UnRegisterState<GamePlayState>();
        }

        public void Enter()
        {
            _gameplayUiPresenter = (GameplayUIPresenter) _uiService.GetPresenter<GameplayUIPresenter>();
            
            _gameplayUiPresenter.Show();
            _gameplayUiPresenter.OnMenuBtnClick += HandleMenuBtnClick;
            _gameplayUiPresenter.OnNextBtnClick += HandleNextBtnClick;
            _gameplayUiPresenter.OnRestartBtnClick += HandleRestartBtnClick;

            Work();
        }

        public void Exit()
        {
            _heroConductor.Dispose();

            _gameplayUiPresenter.OnMenuBtnClick -= HandleMenuBtnClick;
            _gameplayUiPresenter.OnNextBtnClick -= HandleNextBtnClick;
            _gameplayUiPresenter.OnRestartBtnClick -= HandleRestartBtnClick;
            
            _gameplayUiPresenter.Hide();
        }

        private void Work()
        {
            InitGameWorld().Forget();
            _adsService.LoadAd();
            _analyticsService.SendCustomEvent("Levels", new Dictionary<string, object>(){{"Level", GetLevelIndex()}});
        }

        private async UniTaskVoid InitGameWorld()
        {
            var levelContext = await _levelFactory.CreateLevelContext(GetLevelIndex());
            _enemiesHolder = levelContext.EnemiesHolder;

            _hero = _levelFactory.InstantiateHero();
            _hero.SetupNavMeshAgent(_levelFactory.CreatedLevel);
            _hero.OnKilled += HandlePlayerLose;

            _heroConductor = new MainHeroConductor(_hero, levelContext.HeroRouter,
                levelContext.EnemiesHolder, _gameplayUiPresenter, _heroConfig);

            _heroConductor.StartLevel();
            _heroConductor.OnLevelPassed += HandlePlayerWin;
        }

        private int GetLevelIndex()
        {
            var index = _playerProgressService.PlayerProgress.PlayerState.LastCompletedLevelIndex;
            if (index >= 0 && index < _levelFactoryConfig.LevelContexts.Length)
            {
                return index;
            }

            return Random.Range(0, _levelFactoryConfig.LevelContexts.Length);
        }

        private void HandlePlayerWin()
        {
            float resultPercent = _enemiesHolder.EnemiesCount * 3.5f / _hero.ShotsCount;
            int starsCount = (int)Math.Round(resultPercent / ONE_STAR);
            
            GameplayResult result = new(_enemiesHolder.RewardForEnemies, starsCount, _hero.ShotsCount);
            _gameplayUiPresenter.ShowWinPanel(result);

            foreach (var point in _hero.VisualEffectsPoints)
            {
                _specialEffectsService.ShowEffect(SpecialEffectType.CoinsFountain, point.position);
            }

            UpdateProgress(starsCount);

            _analyticsService.SendCustomEvent("GameplayResult", new Dictionary<string, object>(){{"Win", true}});
        }

        private void UpdateProgress(int starsCount)
        {
            _playerProgressService.PlayerProgress.PlayerState.IncreasePassedLevel();
            _playerProgressService.PlayerProgress.ResourcePack[ResourceType.Coin].Value += _enemiesHolder.RewardForEnemies;
            _playerProgressService.PlayerProgress.ResourcePack[ResourceType.Star].Value += starsCount;
            _saveLoadService.SaveProgress();
        }

        private async void HandlePlayerLose()
        {
            _heroConductor.Dispose();
            _enemiesHolder.Dispose();
            
            _hero.RotateCameraUp();
            _gameplayUiPresenter.ShowLosePanel();
            await UniTask.WaitForSeconds(1.2f);

            _analyticsService.SendCustomEvent("GameplayResult", new Dictionary<string, object>(){{"Lose", false}});
        }

        private async void HandleMenuBtnClick()
        {
            AsyncOperation loadSceneAsync = SceneManager.LoadSceneAsync(ScenesNames.MENU_SCENE_NAME);

            while (!loadSceneAsync.isDone)
                await UniTask.Yield();
            
            _gameStatesMachine.SetState<MenuState>();
        }

        private async void HandleNextBtnClick()
        {
            AsyncOperation loadSceneAsync = SceneManager.LoadSceneAsync(ScenesNames.GAMEPLAY_SCENE_NAME);

            while (!loadSceneAsync.isDone)
                await UniTask.Yield();
            
            _gameStatesMachine.SetState<GamePlayState>();
            
            _adsService.ShowAd();
        }

        private async void HandleRestartBtnClick()
        {
            AsyncOperation loadSceneAsync = SceneManager.LoadSceneAsync(ScenesNames.GAMEPLAY_SCENE_NAME);

            while (!loadSceneAsync.isDone)
                await UniTask.Yield();
            
            _gameStatesMachine.SetState<GamePlayState>();
            
            _adsService.ShowAd();
        }
    }
}
