using UnityEngine;
using Zenject;

namespace Internal.Scripts.Infrastructure.Services.Ads
{
    [CreateAssetMenu(fileName = "AdsServiceInstaller", menuName = "Installers/AdsServiceInstaller")]
    public class AdsServiceInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private UnityAdsConfig config;
        
        public override void InstallBindings()
        {
            Container.BindInstance(config);
            Container.BindInterfacesAndSelfTo<CustomYaGamesAdsService>().AsSingle().NonLazy();
        }
    }
}