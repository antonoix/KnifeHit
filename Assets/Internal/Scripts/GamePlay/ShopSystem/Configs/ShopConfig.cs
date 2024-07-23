using Internal.Scripts.GamePlay.ShopSystem;
using UnityEngine;

namespace Internal.Scripts.Infrastructure.Factory
{
    [CreateAssetMenu(fileName = "ShopConfig", menuName = "Configs/ShopConfig")]
    public class ShopConfig : ScriptableObject
    {
        [field: SerializeField] public ShopContext ShopContextPrefab { get; private set; }
    }
}