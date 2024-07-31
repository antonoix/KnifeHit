using System.Collections.Generic;
using UnityEngine;

namespace Internal.Scripts.GamePlay.Weapons
{
    [CreateAssetMenu(fileName = "AllWeaponsConfig", menuName = "Configs/AllWeaponsConfig")]
    public class AllWeaponsConfig : ScriptableObject
    {
        [field: SerializeField] public List<WeaponConfig> AllConfigs;
    }
}