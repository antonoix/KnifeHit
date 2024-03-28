using System.Collections.Generic;

namespace Internal.Scripts.Infrastructure.Services.Analytics
{
    public interface IAnalyticsManager : IService
    {
        void SendCustomEvent(string eventName, Dictionary<string, object> parameters);
        void Initialize();
    }
}