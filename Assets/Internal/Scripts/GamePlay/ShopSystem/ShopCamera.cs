using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Internal.Scripts.GamePlay.ShopSystem
{
    public class ShopCamera : MonoBehaviour
    {
        [SerializeField] private float focusDuration = 0.4f;
        private Sequence _focusAnim;

        public async UniTask Focus(Transform itemTransform)
        {
            _focusAnim?.Kill();
            _focusAnim = DOTween.Sequence();
            _focusAnim.Append(
                    transform.DOMove(new Vector3(itemTransform.position.x, itemTransform.position.y, transform.position.z), focusDuration));

            await UniTask.WaitForSeconds(_focusAnim.Duration() / 2);
        }
    }
}