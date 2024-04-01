using System;
using System.Collections.Generic;
using UnityEditor;

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
            ResourcePack.Add(settings.StartResources);
            PlayerState = new PlayerState(settings);
        }
    }
}