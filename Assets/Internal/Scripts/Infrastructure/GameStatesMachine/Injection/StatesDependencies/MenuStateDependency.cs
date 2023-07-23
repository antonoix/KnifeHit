using Internal.Scripts.UI;
using UnityEngine;

namespace Internal.Scripts.Infrastructure.GameStatesMachine.Injection.StatesDependencies
{
    [CreateAssetMenu(fileName = "MenuStateDependency", menuName = "Infrastructure/MenuStateDependency")]
    public class MenuStateDependency : ScriptableObject, IGameStateDepedency
    {
        public MenuUIView MenuUIPrefab;
    }
}
