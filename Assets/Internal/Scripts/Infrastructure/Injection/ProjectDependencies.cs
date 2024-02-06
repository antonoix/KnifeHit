using Internal.Scripts.Infrastructure.Injection.StatesDependencies;
using UnityEngine;

namespace Internal.Scripts.Infrastructure.Injection
{
    [CreateAssetMenu(fileName = "ProjectDependencies", menuName = "Infrastructure/ProjectDependencies")]
    public class ProjectDependencies : ScriptableObject
    {
        [field: SerializeField] public MenuStateDependency MenuStateDependency { get; private set; }
        [field: SerializeField] public BootstrapStateDependency BootstrapStateDependency { get; private set; }
        [field: SerializeField] public GamePlayStateDependency GameplayStateDependency { get; private set; }

    }
}
