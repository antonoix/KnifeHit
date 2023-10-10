using Internal.Scripts.Infrastructure.GameStatesMachine.Injection.StatesDependencies;

namespace Internal.Scripts.Infrastructure.GameStatesMachine.States
{
    public abstract class GameState
    {
        protected readonly IGameStatesSwitcher _gameStatesSwitcher;
        protected readonly IGameStateDepedency _gameStateDependency;

        protected GameState(IGameStatesSwitcher gameStatesSwitcher, IGameStateDepedency gameStateDependency)
        {
            _gameStatesSwitcher = gameStatesSwitcher;
            _gameStateDependency = gameStateDependency;
        }

        public abstract void Enter();

        public abstract void Exit();
    }
}
