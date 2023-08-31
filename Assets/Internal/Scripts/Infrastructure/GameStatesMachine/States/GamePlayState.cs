using System.Threading.Tasks;
using Internal.Scripts.GamePlay;
using Internal.Scripts.GamePlay.TheMainHero;
using Internal.Scripts.Infrastructure.Constants;
using Internal.Scripts.Infrastructure.GameStatesMachine.Injection.StatesDependencies;
using Internal.Scripts.Infrastructure.Input;
using Internal.Scripts.UI.GamePlay;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Internal.Scripts.Infrastructure.GameStatesMachine.States
{
    public class GamePlayState : GameState
    {
        private readonly LevelContextFactory _levelFactory;
        private readonly InputService _playerInputService;
        private readonly GameplayUIPresenter _uiPresenter;
        private MainHeroConductor _heroConductor;

        public GamePlayState(IGameStatesSwitcher gameStatesSwitcher, IGameStateDepedency gameStateDependency)
            : base(gameStatesSwitcher, gameStateDependency)
        {
            GamePlayStateDependency dependency = gameStateDependency as GamePlayStateDependency;

            _levelFactory = new LevelContextFactory(dependency.MainHero, dependency.LevelContexts[0]);
            _uiPresenter = new GameplayUIPresenter(dependency.GameplayUIPrefab);
            _playerInputService = new InputService();
        }

        public override async void Enter()
        {
            AsyncOperation loadSceneAsync = SceneManager.LoadSceneAsync(ScenesNames.GAMEPLAY_SCENE_NAME);

            while (!loadSceneAsync.isDone)
                await Task.Yield();
        
            InitGameWorld();
        }

        public override void Exit()
        {
            _heroConductor.Dispose();
        }

        private void InitGameWorld()
        {
            var levelContext = _levelFactory.InstantiateLevelContext();
            _playerInputService.Initialize();

            var hero = _levelFactory.InstantiateHero();
            hero.Setup(_playerInputService);
            
            _heroConductor =
                new MainHeroConductor(hero, levelContext.HeroRouter, levelContext.EnemiesHolder);
            
            _heroConductor.StartLevel();
            _heroConductor.OnLevelPassed += HandlePlayerWin;
        }

        private void HandlePlayerWin()
        {
            throw new System.NotImplementedException();
        }
    }
}