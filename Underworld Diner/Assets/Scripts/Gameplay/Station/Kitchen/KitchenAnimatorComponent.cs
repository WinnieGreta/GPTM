using UnityEngine;
using Zenject;

namespace Gameplay.Station.Kitchen
{
    public class KitchenAnimatorComponent : IInitializable, ITickable, ILateTickable
    {
        [Inject] private KitchenParameters _kitchenParameters;
        [Inject] private KitchenState _currentKitchen;

        private CookingState _previousState;
        private Animator _cookingAnimator;
        private SpriteRenderer _readyDishRenderer;
        private bool _isCooking;

        public void Initialize()
        {
            _cookingAnimator = _kitchenParameters.CookingAnimator;
            _readyDishRenderer = _kitchenParameters.ReadyDishSprite;
            _previousState = _currentKitchen.State;
            _readyDishRenderer.enabled = false;
        }

        public void Tick()
        {
            if (_previousState != _currentKitchen.State)
            {
                switch (_currentKitchen.State)
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
                _previousState = _currentKitchen.State;
            }
        }

        public void LateTick()
        {
            //_currentState = KitchenState.Cooking;
            //Debug.Log(_currentState.ToString());
            _cookingAnimator.SetBool("isCooking", _isCooking);
        }
    }
}