using Internal.Scripts.Infrastructure.GameStatesMachine.States;

namespace Internal.Scripts.Infrastructure.GameStatesMachine
{
    public interface IGameStatesMachine
    {
        void Enter();
        void SetState<T>() where T : IGameState;
        void RegisterState<T>(IGameState state) where T : IGameState;
        void UnRegisterState<T>() where T : IGameState;
    }
}
