using Internal.Scripts.GamePlay.Weapons;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Internal.Scripts.GamePlay.LevelsService
{
    [CreateAssetMenu(fileName = "LevelsConfig", menuName = "Configs/LevelsConfig")]
    public class LevelsConfig : ScriptableObject
    {
        [field: SerializeField] public AssetLabelReference LevelLabel { get; private set; }
    }
}