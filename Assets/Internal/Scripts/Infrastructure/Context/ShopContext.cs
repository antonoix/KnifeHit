using Internal.Scripts.Infrastructure.Services.ShopSystem;
using UnityEngine;

namespace Internal.Scripts.GamePlay.Context
{
    public class ShopContext : MonoBehaviour
    {
        [field: SerializeField] public ShopCamera ShopCamera { get; private set; }
        [field: SerializeField] public ShopItem[] ShopItems { get; private set; }
    }
}