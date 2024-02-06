using Internal.Scripts.UI.GamePlay;
using Internal.Scripts.UI.Menu;
using UnityEngine;

namespace Internal.Scripts.Infrastructure.Services.UiService
{
    [CreateAssetMenu(fileName = "UiConfig", menuName = "Configs/UiConfig")]
    public class UiConfig : ScriptableObject
    {
        [field: SerializeField] public GameplayUIView GameplayUIPrefab { get; private set; }
        [field: SerializeField] public MenuUIView MenuUIPrefab { get; private set; }
    }
}