using Internal.Scripts.UI;
using Internal.Scripts.UI.Menu;
using UnityEngine;

namespace Internal.Scripts.Infrastructure.GameStatesMachine.Injection.StatesDependencies
{
    [CreateAssetMenu(fileName = "MenuStateDependency", menuName = "Infrastructure/MenuStateDependency")]
    public class MenuStateDependency : ScriptableObject, IGameStateDepedency
    {
        [field: SerializeField] public MenuUIView MenuUIPrefab { get; private set; }
    }
}
