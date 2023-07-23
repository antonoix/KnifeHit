using Internal.Scripts.Infrastructure.GameStatesMachine.Injection.StatesDependencies;
using UnityEngine;

namespace Internal.Scripts.Infrastructure.GameStatesMachine.Injection
{
    [CreateAssetMenu(fileName = "GameStatesMachineDependencies", menuName = "Infrastructure/GameStatesMachineDependencies")]
    public class GameStatesMachineDependencies : ScriptableObject
    {
        [SerializeField] private MenuStateDependency menuStateDependency;
        [SerializeField] private GamePlayStateDependency gamePlayStateDependencyDependency;
 
        public IGameStateDepedency MainMenuDependency => menuStateDependency as IGameStateDepedency;
    
        public IGameStateDepedency GamePlayDependency => gamePlayStateDependencyDependency as IGameStateDepedency;
    }
}
