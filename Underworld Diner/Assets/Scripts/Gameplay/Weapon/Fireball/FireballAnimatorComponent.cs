using UnityEngine;
using Zenject;

namespace Gameplay.Weapon.Fireball
{
    public class FireballAnimatorComponent
    {
        [Inject] private Animator _animator;

        public void ExplodeOnImpact()
        {
            _animator.SetTrigger("hasHit");
        }

        public void FinalExplosion()
        {
            _animator.SetBool("isExploading", true);
        }

        private bool FinishedAnimation(string animationName)
        {
            var stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.normalizedTime >= 1 && stateInfo.IsName(animationName))
            {
                return true;
            }

            return false;
        }

        public bool FinishedFinalExplosion()
        {
            return FinishedAnimation("FireballDeath");
        }

        public float FinalExplosionDeceleration()
        {
            var stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("FireballDeath"))
            {
                return 1 - stateInfo.normalizedTime;
            }
            return 1;
        }
        
        
    }
}