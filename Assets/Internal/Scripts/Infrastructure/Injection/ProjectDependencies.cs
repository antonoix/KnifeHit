using Internal.Scripts.GamePlay.Context;
using Internal.Scripts.GamePlay.ShopSystem;
using Internal.Scripts.GamePlay.SpecialEffectsService;
using Internal.Scripts.Infrastructure.Services.Ads;
using Internal.Scripts.Infrastructure.Services.Analytics;
using Internal.Scripts.Infrastructure.Services.Localization;
using Internal.Scripts.Infrastructure.Services.ProgressService;
using Internal.Scripts.Infrastructure.Services.Sound;
using Internal.Scripts.Infrastructure.Services.UiService;
using UnityEngine;

namespace Internal.Scripts.Infrastructure.Injection
{
    [CreateAssetMenu(fileName = "ProjectDependencies", menuName = "Infrastructure/ProjectDependencies")]
    public class ProjectDependencies : ScriptableObject
    {
        [field: SerializeField] public GameplayEntities GameplayEntities { get; private set; }
        [field: SerializeField] public ShopContext ShopContext { get; private set; }
        [field: SerializeField] public UiServiceInjector UiServiceInjector { get; private set; }
        [field: SerializeField] public SpecialEffectsInjector SpecialEffectsInjector { get; private set; }
        [field: SerializeField] public PlayerProgressServiceInjector PlayerProgressServiceInjector { get; private set; }
        [field: SerializeField] public LocalizationServiceInjector LocalizationServiceInjector { get; private set; }
        [field: SerializeField] public SoundsServiceInjector SoundsServiceInjector { get; private set; }
        [field: SerializeField] public ShopServiceInjector ShopServiceInjector { get; private set; }
        [field: SerializeField] public AdsManagerInjector AdsManagerInjector { get; private set; }
        [field: SerializeField] public AnalyticsManagerInjector AnalyticsManagerInjector { get; private set; }

    }
}
