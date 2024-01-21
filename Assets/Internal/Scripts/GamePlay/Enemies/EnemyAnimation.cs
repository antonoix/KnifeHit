using UnityEngine;

namespace Internal.Scripts.GamePlay.Enemies
{
    [RequireComponent(typeof(Animator))]
    public class EnemyAnimation : MonoBehaviour
    {
        private const int MILLISECONDS = 1000;
        
        private static readonly int Walk = Animator.StringToHash("Walk");
        private static readonly int Attack = Animator.StringToHash("Attack");
        private static readonly int StandUpFaceUp = Animator.StringToHash("StandUpFaceUp");
        private static readonly int StandUpFaceDown = Animator.StringToHash("StandUpFaceDown");
        
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void EnableAnimator(bool isEnabled)
            => _animator.enabled = isEnabled;

        public void SetWalk(bool isWalking)
            => _animator.SetBool(Walk, isWalking);

        public void PlayAttack()
            => _animator.SetTrigger(Attack);

        public void PlayStandUp(bool isFaceUp)
        {
            int standUpTrigger = isFaceUp ? StandUpFaceUp : StandUpFaceDown;
            _animator.SetTrigger(standUpTrigger);
        }

        public int GetCurrentAnimationLengthMs()
        {
            return (int)(_animator.GetCurrentAnimatorClipInfo(0)[0].clip.length * MILLISECONDS);
        }
    }
}