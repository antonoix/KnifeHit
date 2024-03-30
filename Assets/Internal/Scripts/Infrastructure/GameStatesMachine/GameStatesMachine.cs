using System;
using System.Collections.Generic;
using System.Linq;
using Internal.Scripts.Infrastructure.GameStatesMachine.States;
using UnityEngine;

namespace Internal.Scripts.Infrastructure.GameStatesMachine
{
    public class GameStatesMachine : IGameStatesMachine
    {
        private readonly Dictionary<Type, IGameState> _gameStates;
        private IGameState _currentState;

        public GameStatesMachine(IEnumerable<IGameState> states)
        {
            _gameStates = states.ToDictionary(state => state.GetType());
        }

        public void Enter()
        {
            SetState<MenuState>();
        }

        public void SetState<T>() where T : IGameState
        {
            _currentState?.Exit();
            if (_gameStates.TryGetValue(typeof(T), out IGameState newState))
            {
                _currentState = newState;
                _currentState.Enter();
                return;
            }
            Debug.LogError($"Game states dictionary miss {typeof(T)}");
        }

        public void RegisterState<T>(IGameState state) where T : IGameState
        {
            _gameStates.Add(typeof(T), state);
        }

        public void UnRegisterState<T>() where T : IGameState
        {
            _gameStates.Remove(typeof(T));
        }

        public void Dispose()
        {
            _currentState.Exit();
        }
    }
}
