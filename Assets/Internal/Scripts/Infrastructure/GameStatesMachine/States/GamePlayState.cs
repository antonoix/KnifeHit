using Cysharp.Threading.Tasks;
using Internal.Scripts.GamePlay;
using Internal.Scripts.GamePlay.TheMainHero;
using Internal.Scripts.Infrastructure.Constants;
using Internal.Scripts.Infrastructure.Injection.StatesDependencies;
using Internal.Scripts.Infrastructure.Input;
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
        private readonly ISpecialEffectsService _specialEffects;
        private readonly IUiService _uiService;
        private GameplayUIPresenter _gameplayUiPresenter;
        private MainHeroConductor _heroConductor;
        private MainHero _hero;


        public GamePlayState(IGameStatesSwitcher gameStatesSwitcher, GamePlayStateDependency gameStateDependency)
            : base(gameStatesSwitcher)
        {
            _levelFactory = new LevelContextFactory(gameStateDependency.MainHero, gameStateDependency.LevelContexts[0]);
            _specialEffects = gameStateDependency.SpecialEffectsInjector.Service;
            _uiService = gameStateDependency.UiServiceInjector.Service;

            _playerInputService = new InputService();
        }

        public override async void Enter()
        {
            _gameplayUiPresenter = (GameplayUIPresenter) _uiService.GetPresenter<GameplayUIPresenter>();
            
            _gameplayUiPresenter.Show();
            _gameplayUiPresenter.OnMenuBtnClick += HandleMenuBtnClick;
            _gameplayUiPresenter.OnNextBtnClick += HandleNextBtnClick;
            _gameplayUiPresenter.OnRestartBtnClick += HandleRestartBtnClick;

            AsyncOperation loadSceneAsync = SceneManager.LoadSceneAsync(ScenesNames.GAMEPLAY_SCENE_NAME);

            while (!loadSceneAsync.isDone)
                await UniTask.Yield();
        
            InitGameWorld();
        }

        public override void Exit()
        {
            _heroConductor.Dispose();
            
            _gameplayUiPresenter.OnMenuBtnClick -= HandleMenuBtnClick;
            _gameplayUiPresenter.OnNextBtnClick -= HandleNextBtnClick;
            _gameplayUiPresenter.OnRestartBtnClick -= HandleRestartBtnClick;
            
            _gameplayUiPresenter.Hide();
        }

        private void InitGameWorld()
        {
            var levelContext = _levelFactory.InstantiateLevelContext();
            levelContext.EnemiesHolder.Initialize(_specialEffects);

            _playerInputService.Initialize();

            _hero = _levelFactory.InstantiateHero();
            _hero.Setup(_playerInputService);
            _hero.OnKilled += HandlePlayerLose;
            
            _heroConductor =
                new MainHeroConductor(_hero, levelContext.HeroRouter, levelContext.EnemiesHolder);
            
            _heroConductor.StartLevel();
            _heroConductor.OnLevelPassed += HandlePlayerWin;
        }

        private void HandlePlayerWin()
        {
            _gameplayUiPresenter.ShowWinPanel();
        }

        private void HandlePlayerLose()
        {
            _hero.RotateCameraUp();
            _gameplayUiPresenter.ShowLosePanel();
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
