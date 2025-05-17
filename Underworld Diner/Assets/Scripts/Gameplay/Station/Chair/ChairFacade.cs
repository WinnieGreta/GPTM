using Interfaces;
using Zenject;

namespace Gameplay.Station.Chair
{
    public class ChairFacade : StationFacade, IChair
    {
        [Inject] private ChairParameters _chairParameters;
        [Inject] private IRecipeBook _recipeBook;

        
        public bool IsTaken => _occupant != null;
        public bool IsClean => _occupantDish != DishType.DirtyPlate;
        public bool IsFacingRight => (transform.parent.position - transform.position).x > 0;

        private IMonster _occupant;
        public DishType ExpectedDish => _occupant?.ExpectedDish ?? DishType.None;

        private DishType _occupantDish;

        [Inject]
        private void OnInject(IChairManager chairManager)
        {
            chairManager.Register(this);
        }
        
        public void TakeChair(IMonster occupant)
        {
            _occupant = occupant;
        }

        public void FreeChair()
        {
            LeaveChairDirty();
            _occupant = null;
        }
        
        private void LeaveChairDirty()
        {
            if (_occupantDish != DishType.None)
            {
                PutDish(DishType.DirtyPlate);
            }
        }

        public void PutDish(DishType dish)
        {
            if (dish == DishType.None)
            {
                _chairParameters.DishSprite.enabled = false;
                // TODO move to monster
                _occupantDish = DishType.None;
                return;
                
            }
            _chairParameters.DishSprite.sprite = _recipeBook[dish].DishImage;
            _occupantDish = dish;
            _chairParameters.DishSprite.enabled = true;
            _occupant.Serve(dish);

        }
    }
}