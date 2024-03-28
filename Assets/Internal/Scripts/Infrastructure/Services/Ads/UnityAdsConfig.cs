using UnityEngine;

namespace Internal.Scripts.Infrastructure.Services.Ads
{
    [CreateAssetMenu(fileName = "UnityAdsConfig", menuName = "Configs/UnityAdsConfig")]
    public class UnityAdsConfig : ScriptableObject
    {
        [field: SerializeField] public string AndroidGameId { get; private set; }
        [field: SerializeField] public bool IsTestMode { get; private set; }
        [field: SerializeField] public float ShowAdAfterWinChance { get; private set; }
    }
}