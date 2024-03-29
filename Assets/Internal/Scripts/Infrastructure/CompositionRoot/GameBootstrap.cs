using Internal.Scripts.GamePlay.ShopSystem;
using Internal.Scripts.GamePlay.SpecialEffectsService;
using Internal.Scripts.Infrastructure.GameStatesMachine;
using Internal.Scripts.Infrastructure.Injection;
using Internal.Scripts.Infrastructure.Services.Ads;
using Internal.Scripts.Infrastructure.Services.Analytics;
using Internal.Scripts.Infrastructure.Services.Localization;
using Internal.Scripts.Infrastructure.Services.ProgressService;
using Internal.Scripts.Infrastructure.Services.Sound;
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
    private ShopServiceInjector _shopServiceInjector;
    private AdsManagerInjector _adsManagerInjector;
    private AnalyticsManagerInjector _analyticsManagerInjector;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        
        _specialEffectsInjector = projectDependencies.SpecialEffectsInjector;
        _uiServiceInjector = projectDependencies.UiServiceInjector;
        _playerProgressInjector = projectDependencies.PlayerProgressServiceInjector;
        _localizationServiceInjector = projectDependencies.LocalizationServiceInjector;
        _soundsServiceInjector = projectDependencies.SoundsServiceInjector;
        _shopServiceInjector = projectDependencies.ShopServiceInjector;
        _adsManagerInjector = projectDependencies.AdsManagerInjector;
        _analyticsManagerInjector = projectDependencies.AnalyticsManagerInjector;

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
        _adsManagerInjector.Create();
        _analyticsManagerInjector.Create();

        _uiServiceInjector.Construct(_playerProgressInjector.Service, _localizationServiceInjector.Service, _soundsServiceInjector.Service);
        _uiServiceInjector.Create();
        
        _shopServiceInjector.Construct(_uiServiceInjector.Service, projectDependencies.ShopContext, _playerProgressInjector.Service);
        _shopServiceInjector.Create();
    }

    private void InitializeServices()
    {
        _specialEffectsInjector.Initialize();
        _playerProgressInjector.Initialize();
        _localizationServiceInjector.Initialize();
        _uiServiceInjector.Initialize();
        _soundsServiceInjector.Initialize();
        _shopServiceInjector.Initialize();
        _adsManagerInjector.Initialize();
        _analyticsManagerInjector.Initialize();
    }

    private void OnDestroy()
    {
        //_gameStatesMachine.Dispose();
    }
}
