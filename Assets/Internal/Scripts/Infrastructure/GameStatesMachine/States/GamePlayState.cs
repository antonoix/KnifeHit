using System.Threading.Tasks;
using Internal.Scripts.Infrastructure.Constants;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Internal.Scripts.Infrastructure.GameStatesMachine.States
{
    public class GamePlayState : GameState
    {
        public GamePlayState(IGameStatesSwitcher gameStatesSwitcher, IGameStateDepedency gameStateDependency)
            : base(gameStatesSwitcher, gameStateDependency)
        {
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
            throw new System.NotImplementedException();
        }

        private void InitGameWorld()
        {
        
        }
    }
}
