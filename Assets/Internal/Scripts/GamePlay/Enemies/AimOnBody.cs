using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Internal.Scripts.GamePlay.Enemies
{
    public class AimOnBody : MonoBehaviour
    {
        private const float ANIM_DURATION_SEC = 1.5f;
        private const float MIN_OPACITY = 0.4f;

        [SerializeField] private Image aim;
        private Sequence _animation;

        private void Awake()
        {
            gameObject.SetActive(false);
            StartAnimation();
        }

        public void Activate(bool isActive)
        {
            if (isActive)
                _animation.Play();
            else
                _animation.Pause();
            
            gameObject.SetActive(isActive);
        }

        private void StartAnimation()
        {
            _animation = DOTween.Sequence();
            _animation
                .Join(transform.DOScale(Vector3.one * 1.4f, ANIM_DURATION_SEC))
                .Join(DOTween.To(value => aim.color = new Color(aim.color.r, aim.color.g, aim.color.b, value), MIN_OPACITY, 1, ANIM_DURATION_SEC))
                .Append(transform.DOScale(Vector3.one, ANIM_DURATION_SEC))
                .Join(DOTween.To(value => aim.color = new Color(aim.color.r, aim.color.g, aim.color.b, value), 1, MIN_OPACITY, ANIM_DURATION_SEC))
            .SetLoops(-1);
        }

        private void OnDestroy()
        {
            _animation?.Kill();
        }
    }
}