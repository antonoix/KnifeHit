using UnityEngine;

namespace Internal.Scripts.GamePlay.ShopSystem
{
    public class ShopContext : MonoBehaviour
    {
        [field: SerializeField] public ShopCamera ShopCamera { get; private set; }
        [field: SerializeField] public ShopItem[] ShopItems { get; private set; }
    }
}