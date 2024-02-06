using Internal.Scripts.Infrastructure.GameStatesMachine.Injection.StatesDependencies;
using Internal.Scripts.Infrastructure.Injection.StatesDependencies;
using Internal.Scripts.Infrastructure.Services.SpecialEffectsService;
using Internal.Scripts.Infrastructure.Services.UiService;

namespace Internal.Scripts.Infrastructure.GameStatesMachine.States
{
    public class BootstrapState : GameState
    {
        private readonly SpecialEffectsInjector _specialEffectsInjector;
        private readonly UiServiceInjector _uiServiceInjector;

        public BootstrapState(IGameStatesSwitcher gameStatesSwitcher, BootstrapStateDependency dependency) : base(gameStatesSwitcher)
        {
            _specialEffectsInjector = dependency.SpecialEffectsInjector;
            _uiServiceInjector = dependency.UiServiceInjector;
        }

        public override void Enter()
        {
            InitializeServices();
        }

        private void InitializeServices()
        {
            _uiServiceInjector.Initialize();
            _specialEffectsInjector.Initialize();
            
            _gameStatesSwitcher.SetState<MenuState>();
        }

        public override void Exit()
        {
            
        }
    }
}