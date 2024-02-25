using Internal.Scripts.Infrastructure.GameStatesMachine;
using Internal.Scripts.Infrastructure.Injection;
using Internal.Scripts.Infrastructure.Services.Localization;
using Internal.Scripts.Infrastructure.Services.ProgressService;
using Internal.Scripts.Infrastructure.Services.Sound;
using Internal.Scripts.Infrastructure.Services.SpecialEffectsService;
using Internal.Scripts.Infrastructure.Services.UiService;
using UnityEngine;

public class GameBootstrap : MonoBehaviour
{
    [SerializeField] private ProjectDependencies projectDependencies;
    
    private GameStatesMachine _gameStatesMachine;
    
    private SpecialEffectsInjector _specialEffectsInjector;
    private UiServiceInjector _uiServiceInjector;
    private PlayerProgressServiceInjector _playerProgressInjector;
    private LocalizationServiceInjector _localizationServiceInjector;
    private SoundsServiceInjector _soundsServiceInjector;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        
        _specialEffectsInjector = projectDependencies.SpecialEffectsInjector;
        _uiServiceInjector = projectDependencies.UiServiceInjector;
        _playerProgressInjector = projectDependencies.PlayerProgressServiceInjector;
        _localizationServiceInjector = projectDependencies.LocalizationServiceInjector;
        _soundsServiceInjector = projectDependencies.SoundsServiceInjector;

        ResolveDependencies();
        InitializeServices();

        _gameStatesMachine = new GameStatesMachine(projectDependencies);
        _gameStatesMachine.Enter();
    }

    private void ResolveDependencies()
    {
        _specialEffectsInjector.Create();
        _localizationServiceInjector.Create();
        _playerProgressInjector.Create();
        _soundsServiceInjector.Create();

        _uiServiceInjector.Construct(_playerProgressInjector.Service, _localizationServiceInjector.Service, _soundsServiceInjector.Service);
        _uiServiceInjector.Create();
    }

    private void InitializeServices()
    {
        _specialEffectsInjector.Initialize();
        _playerProgressInjector.Initialize();
        _localizationServiceInjector.Initialize();
        _uiServiceInjector.Initialize();
        _soundsServiceInjector.Initialize();
    }

    private void OnDestroy()
    {
        //_gameStatesMachine.Dispose();
    }
}
