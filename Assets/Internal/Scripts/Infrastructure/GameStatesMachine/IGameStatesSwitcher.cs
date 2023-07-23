using Internal.Scripts.Infrastructure.GameStatesMachine.States;

public interface IGameStatesSwitcher
{
    void SetState<T>() where T : GameState;
}
