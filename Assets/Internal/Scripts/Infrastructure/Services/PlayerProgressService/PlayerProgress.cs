using System;
using Internal.Scripts.Infrastructure.Services.PlayerProgressService.PlayerResource;

namespace Internal.Scripts.Infrastructure.PlayerProgressService
{
    [Serializable]
    public class PlayerProgress
    {
        public PlayerState PlayerState;
        public ResourcePack ResourcePack;

        // need for SaveSystem
        public PlayerProgress() {}
        
        public PlayerProgress(Settings settings)
        {
            ResourcePack = new ResourcePack();
            foreach (var resource in settings.StartResources)
            {
                ResourcePack.Add(resource);
            }
            ResourcePack.Add(settings.StartResources);
            PlayerState = new PlayerState(settings);
        }
    }
}