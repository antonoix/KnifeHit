namespace Internal.Scripts.Infrastructure.GameStatesMachine.States
{
    public interface IGameState
    {
        void Enter();
        void Exit();
    }
}
