using UnityEngine;
using Zenject;

namespace Internal.Scripts.Infrastructure.Services.Sound
{
    [CreateAssetMenu(fileName = "SoundsServiceInstaller", menuName = "Installers/SoundsServiceInstaller")]
    public class SoundsServiceInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private SoundsConfig config;
        
        public override void InstallBindings()
        {
            Container.BindInstance(config);
            Container.BindInterfacesAndSelfTo<SoundsService>().AsSingle();
        }
    }
}