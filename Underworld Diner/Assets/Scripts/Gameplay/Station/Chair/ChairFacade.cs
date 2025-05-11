using Gameplay.Dish;
using Interfaces;
using Zenject;

namespace Gameplay.Station.Chair
{
    public class ChairFacade : StationFacade, IChair
    {
        [Inject] private ChairParameters _chairParameters;
        [Inject] private IDish _dirtyDish;
        public bool IsTaken => _occupant != null;
        public bool IsClean => _chairParameters.OccupantDish != _dirtyDish;
        public bool IsFacingRight => (transform.parent.position - transform.position).x > 0;

        private IMonster _occupant;
        public IDish ExpectedDish => _occupant?.ExpectedDish;


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
            _occupant.Serve(dish);

        }

        public void CleanChair()
        {
            if (_chairParameters.OccupantDish == _dirtyDish)
            {
                //_chairParameters.OccupantDish = null;
                //_chairParameters.DishSprite.enabled = false;
                PutDish(null);
            }
        }
        

        public IDish GetDishImEating()
        {
            return _chairParameters.OccupantDish;
        }
        
    }
}