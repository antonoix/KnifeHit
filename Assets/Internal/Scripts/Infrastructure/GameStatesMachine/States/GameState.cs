namespace Internal.Scripts.Infrastructure.GameStatesMachine.States
{
    public abstract class GameState
    {
        protected readonly IGameStatesSwitcher _gameStatesSwitcher;

        protected GameState(IGameStatesSwitcher gameStatesSwitcher)
        {
            _gameStatesSwitcher = gameStatesSwitcher;
        }

        public abstract void Enter();

        public abstract void Exit();
    }
}
