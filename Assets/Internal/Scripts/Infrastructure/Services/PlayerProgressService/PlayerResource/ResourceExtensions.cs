using System.Collections.Generic;

namespace Internal.Scripts.Infrastructure.Services.PlayerProgressService.PlayerResource
{
    public static class ResourceExtensions
    {
        private static Dictionary<ResourceType, string> _resourceTags = new()
        {
            {ResourceType.Coin, "<sprite=0>"},
            {ResourceType.Star, "<sprite=1>"},
        };
        
        public static string GetTextTag(this Resource resource)
        {
            return _resourceTags[resource.Key];
        }
    }
}