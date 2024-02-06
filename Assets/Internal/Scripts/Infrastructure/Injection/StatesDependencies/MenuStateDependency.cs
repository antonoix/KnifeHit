using Internal.Scripts.Infrastructure.Services.UiService;
using UnityEngine;

namespace Internal.Scripts.Infrastructure.Injection.StatesDependencies
{
    [CreateAssetMenu(fileName = "MenuStateDependency", menuName = "Infrastructure/MenuStateDependency")]
    public class MenuStateDependency : ScriptableObject
    {
        [field: SerializeField] public UiServiceInjector UiServiceInjector { get; private set; }
    }
}
