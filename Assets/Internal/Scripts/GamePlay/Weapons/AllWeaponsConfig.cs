using System.Collections.Generic;
using UnityEngine;

namespace Internal.Scripts.GamePlay.TheMainHero.Combat
{
    [CreateAssetMenu(fileName = "AllWeaponsConfig", menuName = "Configs/AllWeaponsConfig")]
    public class AllWeaponsConfig : ScriptableObject
    {
        [field: SerializeField] public List<WeaponConfig> AllConfigs;
    }
}