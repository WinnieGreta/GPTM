using Interfaces;
using Zenject;

namespace Gameplay.Station.Chair
{
    public class ChairFacade : StationFacade, IChair
    {
        public bool IsTaken => _occupant != null;
        public bool IsFacingRight => (transform.parent.position - transform.position).x > 0;

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
            _occupant = null;
        }
        

    }
}