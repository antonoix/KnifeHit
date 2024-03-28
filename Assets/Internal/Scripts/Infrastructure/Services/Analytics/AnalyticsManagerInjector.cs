using Internal.Scripts.Infrastructure.Injection;
using UnityEngine;

namespace Internal.Scripts.Infrastructure.Services.Analytics
{
    [CreateAssetMenu(fileName = "AnalyticsManagerInjector", menuName = "Injectors/AnalyticsManagerInjector")]
    public class AnalyticsManagerInjector : ServiceInjector<IAnalyticsManager>
    {
        public override IAnalyticsManager Create()
        {
            _service = new CustomUnityAnalyticsManager();
            return _service;
        }

        public override void Initialize()
        {
            base.Initialize();
            
            _service.Initialize();
        }
    }
}