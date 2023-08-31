using System;
using System.Collections.Generic;
using Internal.Scripts.Infrastructure.GameStatesMachine.Injection;
using Internal.Scripts.Infrastructure.GameStatesMachine.States;
using UnityEngine;

namespace Internal.Scripts.Infrastructure.GameStatesMachine
{
    public class GameStatesMachine : IGameStatesSwitcher
    {
        private readonly Dictionary<Type, GameState> _gameStates;
        private GameState _currentState;

        public GameStatesMachine(GameStatesMachineDependencies dependencies)
        {
            _gameStates = new Dictionary<Type, GameState>()
            {
                { typeof(MenuState), new MenuState(this, dependencies.MainMenuDependency) },
                { typeof(GamePlayState), new GamePlayState(this, dependencies.GamePlayDependency) }
            };
        }

        public void Enter()
        {
            SetState<MenuState>();
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