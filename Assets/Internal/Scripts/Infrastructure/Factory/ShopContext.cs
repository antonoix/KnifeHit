using Internal.Scripts.GamePlay.ShopSystem;
using UnityEngine;

namespace Internal.Scripts.Infrastructure.Factory
{
    public class ShopContext : MonoBehaviour
    {
        [field: SerializeField] public ShopCamera ShopCamera { get; private set; }
    }
}