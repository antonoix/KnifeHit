using UnityEngine;

namespace Internal.Scripts.GamePlay.ShopSystem
{
    public class ShopItem : MonoBehaviour
    {
        [field: SerializeField] public ShopItemType Type { get; private set; }
        [field: SerializeField] public int Cost { get; private set; }
    }
}