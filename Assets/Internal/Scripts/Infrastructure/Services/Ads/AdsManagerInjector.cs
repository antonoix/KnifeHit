using Internal.Scripts.Infrastructure.Injection;
using UnityEngine;

namespace Internal.Scripts.Infrastructure.Services.Ads
{
    [CreateAssetMenu(fileName = "AdsManagerInjector", menuName = "Injectors/AdsManagerInjector")]
    public class AdsManagerInjector : ServiceInjector<IAdsManager>
    {
        [SerializeField] private UnityAdsConfig config;
        
        public override IAdsManager Create()
        {
            _service = new CustomUnityAdsManager(config);
            return _service;
        }

        public override void Initialize()
        {
            base.Initialize();
            
            _service.Initialize();
        }
    }
}