using DG.Tweening;
using Internal.Scripts.GamePlay.TheMainHero.Combat;
using Internal.Scripts.Infrastructure.Services.PlayerProgressService;
using Internal.Scripts.Infrastructure.Services.PlayerProgressService.PlayerResource;
using UnityEngine;

namespace Internal.Scripts.GamePlay.ShopSystem
{
    public class ShopItem : MonoBehaviour
    {
        private Sequence _scaleEffectAnim;
        
        public WeaponConfig CurrentWeapon { get; private set; }

        public void Setup(WeaponConfig config)
            => CurrentWeapon = config;

        public void PlayScaleEffect()
        {
            _scaleEffectAnim?.Kill();

            _scaleEffectAnim = DOTween.Sequence();
            _scaleEffectAnim.Append(transform.DOScale(Vector3.one * 0.9f, 0.4f));
            _scaleEffectAnim.Append(transform.DOScale(Vector3.one, 0.4f));
            _scaleEffectAnim.SetEase(Ease.OutBack);
        }
    }
}