using UnityEngine;
using Zenject;

namespace Internal.Scripts.GamePlay.SpecialEffectsService
{
    [CreateAssetMenu(fileName = "SpecialEffectsServiceInstaller", menuName = "Installers/SpecialEffectsServiceInstaller")]
    public class SpecialEffectsServiceInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private SpecialEffectsConfig config;
        
        public override void InstallBindings()
        {
            Container.BindInstance(config);
            Container.BindInterfacesTo<SpecialEffectsService>().AsSingle();
        }
    }
}