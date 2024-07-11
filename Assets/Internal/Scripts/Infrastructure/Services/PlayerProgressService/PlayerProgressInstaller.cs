using System;
using Internal.Scripts.Infrastructure.Services.PlayerProgressService.PlayerResource;
using UnityEngine;
using Zenject;

namespace Internal.Scripts.Infrastructure.Services.PlayerProgressService
{
    [CreateAssetMenu(menuName = "Installers/PlayerProgressInstaller", fileName = nameof(PlayerProgressInstaller))]
    public class PlayerProgressInstaller : ScriptableObjectInstaller<PlayerProgressInstaller>
    {
        [SerializeField] private Settings settings;
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<PersistentProgressService>().AsSingle();
            Container.BindInstance(settings).WhenInjectedInto<PersistentProgressService>();
        }
    }
    
    [Serializable]
    public class Settings
    {
        public WeaponType DefaultWeaponType;
        public Resource[] StartResources;
    }
}