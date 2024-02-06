using System;
using System.Collections.Generic;
using Internal.Scripts.Infrastructure.GameStatesMachine.States;
using Internal.Scripts.Infrastructure.Injection;
using UnityEngine;

namespace Internal.Scripts.Infrastructure.GameStatesMachine
{
    public class GameStatesMachine : IGameStatesSwitcher
    {
        private readonly Dictionary<Type, GameState> _gameStates;
        private GameState _currentState;

        public GameStatesMachine(ProjectDependencies dependencies)
        {
            _gameStates = new Dictionary<Type, GameState>()
            {
                { typeof(BootstrapState), new BootstrapState(this, dependencies.BootstrapStateDependency) },
                { typeof(MenuState), new MenuState(this, dependencies.MenuStateDependency) },
                { typeof(GamePlayState), new GamePlayState(this, dependencies.GameplayStateDependency) }
            };
        }

        public void Enter()
        {
            SetState<BootstrapState>();
        }

        public void SetState<T>() where T : GameState
        {
            _currentState?.Exit();
            if (_gameStates.TryGetValue(typeof(T), out GameState newState))
            {
                _currentState = newState;
                _currentState.Enter();
                return;
            }
            Debug.LogError($"Game states dictionary miss {typeof(T)}");
        }

        public void Dispose()
        {
            _currentState.Exit();
        }
    }
}
