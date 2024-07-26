﻿using Internal.Scripts.GamePlay.LevelsService;
using Internal.Scripts.GamePlay.TheMainHero.Combat;
using Internal.Scripts.Infrastructure.AssetManagement;
using Internal.Scripts.Infrastructure.Input;
using Internal.Scripts.Infrastructure.SaveLoad;
using Internal.Scripts.Infrastructure.Services.Analytics;
using Internal.Scripts.Infrastructure.Services.Localization;
using UnityEngine;
using Zenject;

namespace Internal.Scripts.Infrastructure.Injection
{
    [CreateAssetMenu(fileName = "ProjectServicesInstaller", menuName = "Installers/ProjectServicesInstaller")]
    public class ProjectServicesInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private AllWeaponsConfig _allWeaponsConfig;
        
        public override void InstallBindings()
        {
            Container.BindInstance(_allWeaponsConfig).AsSingle();
            Container.BindInterfacesTo<CustomUnityAnalyticsService>().AsSingle().NonLazy();
            Container.BindInterfacesTo<JsonLocalizationService>().AsSingle();
            Container.BindInterfacesTo<SaveLoadService>().AsSingle();
            Container.BindInterfacesTo<InputService>().AsSingle();
            Container.BindInterfacesTo<AddressablesAssetsProvider>().AsSingle();
        }
    }
}