using System.Collections.Generic;
using Unity.Services.Analytics;
using Unity.Services.Core;

namespace Internal.Scripts.Infrastructure.Services.Analytics
{
    public class CustomUnityAnalyticsManager : IAnalyticsManager
    {
        public void Initialize()
        {
            UnityServices.InitializeAsync();
        }
        
        public void SendCustomEvent(string eventName, Dictionary<string, object> parameters)
        {
            CustomEvent customEvent = new CustomEvent(eventName);
            foreach (var (key, value) in parameters)
            {
                customEvent.Add(key, value);
            }
            AnalyticsService.Instance.RecordEvent(customEvent);
        }
    }
}