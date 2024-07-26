using System.Collections.Generic;
using Unity.Services.Analytics;
using Unity.Services.Core;
using Zenject;

namespace Internal.Scripts.Infrastructure.Services.Analytics
{
    public class CustomUnityAnalyticsService : IAnalyticsService, IInitializable
    {
        public async void Initialize()
        {
            await UnityServices.InitializeAsync();
            AnalyticsService.Instance.StartDataCollection();
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