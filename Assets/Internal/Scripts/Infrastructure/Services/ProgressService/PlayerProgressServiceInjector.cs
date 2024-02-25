using Internal.Scripts.Infrastructure.Injection;
using UnityEngine;

namespace Internal.Scripts.Infrastructure.Services.ProgressService
{
    [CreateAssetMenu(fileName = "PlayerProgressServiceInjector", menuName = "Injectors/PlayerProgressServiceInjector")]
    public class PlayerProgressServiceInjector : ServiceInjector<IPlayerProgressService>
    {
        public override IPlayerProgressService Create()
        {
            _service = new PlayerPrefsProgressService();
            return _service;
        }
    }
}