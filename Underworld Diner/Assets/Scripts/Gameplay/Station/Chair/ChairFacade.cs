using Gameplay.Dish;
using Interfaces;
using Zenject;

namespace Gameplay.Station.Chair
{
    public class ChairFacade : StationFacade, IChair
    {
        [Inject] private ChairParameters _chairParameters;
        [Inject] private DishRecipe _dirtyDish;
        public bool IsTaken => _occupant != null;
        public bool IsClean => (DishRecipe)_chairParameters.OccupantDish != _dirtyDish;
        public bool IsFacingRight => (transform.parent.position - transform.position).x > 0;

        public IDish ExpectedDish { get; private set; }

        private IMonster _occupant;

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
            OrderDish(null);
            _occupant = null;
        }
        
        private void LeaveChairDirty()
        {
            if (_chairParameters.OccupantDish != null)
            {
                PutDish(_dirtyDish);
            }
        }

        public void PutDish(IDish dish)
        {
            if (dish == null)
            {
                _chairParameters.DishSprite.enabled = false;
                return;
                
            }
            _chairParameters.DishSprite.sprite = dish.DishImage;
            _chairParameters.OccupantDish = dish;
            ExpectedDish = null;

        }

        public void CleanChair()
        {
            if ((DishRecipe)_chairParameters.OccupantDish == _dirtyDish)
            {
                //_chairParameters.OccupantDish = null;
                //_chairParameters.DishSprite.enabled = false;
                PutDish(null);
            }
        }

        public void OrderDish(IDish dish)
        {
            ExpectedDish = dish;
        }

        public IDish GetDishImEating()
        {
            return _chairParameters.OccupantDish;
        }
        
    }
}