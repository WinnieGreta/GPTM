using UnityEngine;
using Zenject;

namespace Gameplay.Station.Kitchen
{
    public class KitchenAnimatorComponent : IInitializable, ITickable, ILateTickable
    {
        [Inject] private KitchenParameters _kitchenParameters;
        [Inject] private KitchenStatusComponent _statusComponent;

        private CookingState _previousState;
        private Animator _cookingAnimator;
        private SpriteRenderer _readyDishRenderer;
        private bool _isCooking;

        public void Initialize()
        {
            _cookingAnimator = _kitchenParameters.CookingAnimator;
            _readyDishRenderer = _kitchenParameters.ReadyDishSprite;
            _previousState = _statusComponent.State;
            _readyDishRenderer.enabled = false;
        }

        public void Tick()
        {
            if (_previousState != _statusComponent.State)
            {
                switch (_statusComponent.State)
                {
                    case CookingState.Cooking:
                        _isCooking = true;
                        break;
                    case CookingState.Ready:
                        _isCooking = false;
                        _readyDishRenderer.enabled = true;
                        break;
                    case CookingState.Idle:
                        _readyDishRenderer.enabled = false;
                        break;
                }
                _previousState = _statusComponent.State;
            }
        }

        public void LateTick()
        {
            _cookingAnimator.SetBool("isCooking", _isCooking);
        }
    }
}