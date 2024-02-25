using Internal.Scripts.Infrastructure.Services.Localization;
using Internal.Scripts.Infrastructure.Services.ProgressService;
using Internal.Scripts.Infrastructure.Services.Sound;
using Internal.Scripts.Infrastructure.Services.SpecialEffectsService;
using Internal.Scripts.Infrastructure.Services.UiService;
using UnityEngine;

namespace Internal.Scripts.Infrastructure.Injection
{
    [CreateAssetMenu(fileName = "ProjectDependencies", menuName = "Infrastructure/ProjectDependencies")]
    public class ProjectDependencies : ScriptableObject
    {
        [field: SerializeField] public GameplayEntities GameplayEntities { get; private set; }
        [field: SerializeField] public UiServiceInjector UiServiceInjector { get; private set; }
        [field: SerializeField] public SpecialEffectsInjector SpecialEffectsInjector { get; private set; }
        [field: SerializeField] public PlayerProgressServiceInjector PlayerProgressServiceInjector { get; private set; }
        [field: SerializeField] public LocalizationServiceInjector LocalizationServiceInjector { get; private set; }
        [field: SerializeField] public SoundsServiceInjector SoundsServiceInjector { get; private set; }

    }
}
