using Internal.Scripts.Infrastructure.GameStatesMachine.States;

namespace Internal.Scripts.Infrastructure.GameStatesMachine
{
    public interface IGameStatesSwitcher
    {
        void SetState<T>() where T : GameState;
    }
}
