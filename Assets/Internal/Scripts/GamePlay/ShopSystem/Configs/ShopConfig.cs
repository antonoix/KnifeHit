using UnityEngine;

namespace Internal.Scripts.GamePlay.ShopSystem.Configs
{
    [CreateAssetMenu(fileName = "ShopConfig", menuName = "Configs/ShopConfig")]
    public class ShopConfig : ScriptableObject
    {
        [field: SerializeField] public ShopContext ShopContextPrefab { get; private set; }
    }
}