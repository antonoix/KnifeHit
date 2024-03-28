using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Internal.Scripts.GamePlay.Context;
using Internal.Scripts.GamePlay.Enemies;
using Internal.Scripts.GamePlay.TheMainHero;
using Internal.Scripts.Infrastructure.Constants;
using Internal.Scripts.Infrastructure.Injection;
using Internal.Scripts.Infrastructure.Input;
using Internal.Scripts.Infrastructure.Services.Analytics;
using Internal.Scripts.Infrastructure.Services.ProgressService;
using Internal.Scripts.Infrastructure.Services.Sound;
using Internal.Scripts.Infrastructure.Services.SpecialEffectsService;
using Internal.Scripts.Infrastructure.Services.UiService;
using Internal.Scripts.UI.GamePlay;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Internal.Scripts.Infrastructure.GameStatesMachine.States
{
    public class GamePlayState : GameState
    {
        private readonly LevelContextFactory _levelFactory;
        private readonly InputService _playerInputService;
        private readonly GameplayEntities _gameEntities;
        private readonly ISpecialEffectsService _specialEffects;
        private readonly IUiService _uiService;
        private readonly IPlayerProgressService _playerProgressService;
        private readonly ISoundsService _soundsService;
        private readonly IAdsManager _adsManager;
        private readonly IAnalyticsManager _analyticsManager;
        private GameplayUIPresenter _gameplayUiPresenter;
        private MainHeroConductor _heroConductor;
        private MainHero _hero;
        private EnemiesHolder _enemiesHolder;


        public GamePlayState(IGameStatesSwitcher gameStatesSwitcher, GameplayEntities gameEntities,
            ISpecialEffectsService specialEffects, IUiService uiService,
            IPlayerProgressService playerProgressService,
            ISoundsService soundsService,
            IAdsManager adsManager,
            IAnalyticsManager analyticsManager)
            : base(gameStatesSwitcher)
        {
            _playerInputService = new InputService();
            _gameEntities = gameEntities;
            _specialEffects = specialEffects;
            _soundsService = soundsService;
            _adsManager = adsManager;
            _analyticsManager = analyticsManager;

            _levelFactory = new LevelContextFactory(gameEntities, _playerInputService,
                _specialEffects, _soundsService, playerProgressService);
            _uiService = uiService;
            _playerProgressService = playerProgressService;
        }

        public override void Enter()
        {
            _gameplayUiPresenter = (GameplayUIPresenter) _uiService.GetPresenter<GameplayUIPresenter>();
            
            _gameplayUiPresenter.Show();
            _gameplayUiPresenter.OnMenuBtnClick += HandleMenuBtnClick;
            _gameplayUiPresenter.OnNextBtnClick += HandleNextBtnClick;
            _gameplayUiPresenter.OnRestartBtnClick += HandleRestartBtnClick;

            Work();
        }

        private async UniTaskVoid Work()
        {
            AsyncOperation loadSceneAsync = SceneManager.LoadSceneAsync(ScenesNames.GAMEPLAY_SCENE_NAME);

            while (!loadSceneAsync.isDone)
                await UniTask.Yield();

            InitGameWorld();
            _adsManager.LoadAd();
            _analyticsManager.SendCustomEvent("Levels", new Dictionary<string, object>(){{"Level", GetLevelIndex()}});
        }

        public override void Exit()
        {
            _heroConductor.Dispose();
            //_specialEffects.Dispose();
            
            _gameplayUiPresenter.OnMenuBtnClick -= HandleMenuBtnClick;
            _gameplayUiPresenter.OnNextBtnClick -= HandleNextBtnClick;
            _gameplayUiPresenter.OnRestartBtnClick -= HandleRestartBtnClick;
            
            _gameplayUiPresenter.Hide();
        }

        private void InitGameWorld()
        {
            var levelContext = _levelFactory.InstantiateLevelContext(GetLevelIndex());
            _enemiesHolder = levelContext.EnemiesHolder;
            _enemiesHolder.Initialize(_specialEffects);

            _playerInputService.Initialize();

            _hero = _levelFactory.InstantiateHero();

            _hero.OnKilled += HandlePlayerLose;

            _heroConductor =
                new MainHeroConductor(_hero, levelContext.HeroRouter, levelContext.EnemiesHolder, _gameplayUiPresenter);

            _heroConductor.StartLevel();
            _heroConductor.OnLevelPassed += HandlePlayerWin;
        }

        private int GetLevelIndex()
        {
            var index = _playerProgressService.GetPassedLevelsCount();
            if (index >= 0 && index < _gameEntities.LevelContexts.Length)
            {
                return index;
            }

            return Random.Range(0, _gameEntities.LevelContexts.Length);
        }

        private void HandlePlayerWin()
        {
            _gameplayUiPresenter.ShowWinPanel(_enemiesHolder.RewardForEnemies);
            _playerProgressService.IncreasePassedLevel();
            _playerProgressService.AddCoins(_enemiesHolder.RewardForEnemies);
            _adsManager.TryShowAdAfterWin();
            
            _analyticsManager.SendCustomEvent("GameplayResult", new Dictionary<string, object>(){{"Win", true}});
        }

        private void HandlePlayerLose()
        {
            _heroConductor.Dispose();
            _enemiesHolder.Dispose();
            
            _hero.RotateCameraUp();
            _gameplayUiPresenter.ShowLosePanel();
            _adsManager.ShowAd();
            
            _analyticsManager.SendCustomEvent("GameplayResult", new Dictionary<string, object>(){{"Win", false}});
        }

        private void HandleMenuBtnClick()
        {
            _gameStatesSwitcher.SetState<MenuState>();
        }

        private void HandleNextBtnClick()
        {
            _gameStatesSwitcher.SetState<GamePlayState>();
        }

        private void HandleRestartBtnClick()
        {
            _gameStatesSwitcher.SetState<GamePlayState>();
        }
    }
}
