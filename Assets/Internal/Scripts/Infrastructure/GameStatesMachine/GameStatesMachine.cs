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
                { typeof(MenuState), new MenuState(this, dependencies.UiServiceInjector.Service) },
                
                { typeof(GamePlayState), new GamePlayState(this, 
                    dependencies.GameplayEntities,
                    dependencies.SpecialEffectsInjector.Service,
                    dependencies.UiServiceInjector.Service,
                    dependencies.PlayerProgressServiceInjector.Service,
                    dependencies.SoundsServiceInjector.Service,
                    dependencies.AdsManagerInjector.Service,
                    dependencies.AnalyticsManagerInjector.Service) },
                
                { typeof(ShopState), new ShopState(this,
                    dependencies.UiServiceInjector.Service,
                    dependencies.ShopServiceInjector.Service,
                    dependencies.AnalyticsManagerInjector.Service)}
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
