using Internal.Scripts.Infrastructure.Services.SpecialEffectsService;
using Internal.Scripts.Infrastructure.Services.UiService;
using UnityEngine;

namespace Internal.Scripts.Infrastructure.Injection.StatesDependencies
{
    [CreateAssetMenu(fileName = "BootstrapStateDependency", menuName = "Infrastructure/BootstrapStateDependency")]
    public class BootstrapStateDependency : ScriptableObject
    {
        [field: SerializeField] public UiServiceInjector UiServiceInjector { get; private set; }
        [field: SerializeField] public SpecialEffectsInjector SpecialEffectsInjector { get; private set; }
    }
}