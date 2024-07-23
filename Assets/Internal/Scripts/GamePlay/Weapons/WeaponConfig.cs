using Internal.Scripts.Infrastructure.Services.PlayerProgressService;
using Internal.Scripts.Infrastructure.Services.PlayerProgressService.PlayerResource;
using UnityEngine;

namespace Internal.Scripts.GamePlay.TheMainHero.Combat
{
    [CreateAssetMenu(fileName = "WeaponConfig", menuName = "Configs/WeaponConfig")]
    public class WeaponConfig : ScriptableObject
    {
        [field: SerializeField] public Resource ResourceCost { get; private set; }
        [field: SerializeField] public WeaponType Type { get; private set; }
    }
}