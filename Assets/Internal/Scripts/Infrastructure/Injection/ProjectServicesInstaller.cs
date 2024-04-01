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
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<CustomUnityAnalyticsService>().AsSingle().NonLazy();
            Container.BindInterfacesTo<JsonLocalizationService>().AsSingle();
            Container.BindInterfacesTo<SaveLoadService>().AsSingle();
            Container.BindInterfacesTo<InputService>().AsSingle();
        }
    }
}