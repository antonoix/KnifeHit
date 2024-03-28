using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Internal.Scripts.Infrastructure.Services.ShopSystem
{
    public class ShopCamera : MonoBehaviour
    {
        private Sequence _focusAnim;
        
        public void Focus(Transform itemTransform)
        {
            _focusAnim?.Kill();
            _focusAnim = DOTween.Sequence();
            _focusAnim.Append(
                    transform.DOMove(new Vector3(itemTransform.position.x, itemTransform.position.y, transform.position.z), 0.4f));
        }
    }
}