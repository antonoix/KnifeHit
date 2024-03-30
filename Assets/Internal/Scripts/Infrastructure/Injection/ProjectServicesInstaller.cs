using Internal.Scripts.Infrastructure.Input;
using Internal.Scripts.Infrastructure.Services.Analytics;
using Internal.Scripts.Infrastructure.Services.Localization;
using Internal.Scripts.Infrastructure.Services.ProgressService;
using UnityEngine;
using Zenject;

namespace Internal.Scripts.Infrastructure.Injection
{
    [CreateAssetMenu(fileName = "ProjectServicesInstaller", menuName = "Installers/ProjectServicesInstaller")]
    public class ProjectServicesInstaller : ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<CustomUnityAnalyticsService>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<JsonLocalizationService>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerPrefsProgressService>().AsSingle();
            Container.BindInterfacesTo<InputService>().AsSingle();
        }
    }
}