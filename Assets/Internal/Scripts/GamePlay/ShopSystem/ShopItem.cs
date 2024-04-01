using Internal.Scripts.Infrastructure.ResourceService;
using UnityEngine;

namespace Internal.Scripts.GamePlay.ShopSystem
{
    public class ShopItem : MonoBehaviour
    {
        [field: SerializeField] public Resource ResourceCost { get; private set; }
        [field: SerializeField] public WeaponType Type { get; private set; }
    }
}