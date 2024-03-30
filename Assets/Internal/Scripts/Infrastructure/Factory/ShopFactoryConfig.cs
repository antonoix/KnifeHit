using UnityEngine;

namespace Internal.Scripts.Infrastructure.Factory
{
    [CreateAssetMenu(fileName = "ShopFactoryConfig", menuName = "Configs/ShopFactoryConfig")]
    public class ShopFactoryConfig : ScriptableObject
    {
        [field: SerializeField] public ShopContext ShopContextPrefab { get; private set; }
    }
}