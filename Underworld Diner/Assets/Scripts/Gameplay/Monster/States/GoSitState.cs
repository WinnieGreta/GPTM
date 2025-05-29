using Gameplay.Monster.Abstract;
using Interfaces;
using Zenject;

namespace Gameplay.Monster.States
{
    public class GoSitState : BaseMonsterState
    {
        [Inject] private INavigationComponent _navigation;
        [Inject] private IAiComponent _aiComponent;
        [Inject] private IChairManager _chairManager;
        
        private bool _isDestinationSet;

        public override void OnTick()
        {
            if (!_isDestinationSet)
            {
                _isDestinationSet = true;
                if (!FindFreeChair())
                {
                    _aiComponent.ChangeState(MonsterState.Leave);
                }
            }

            if (_navigation.HasReachedDestination())
            {
                _aiComponent.ChangeState(MonsterState.Sit);
            }
        }

        private bool FindFreeChair()
        {
            foreach (var chair in _chairManager.Chairs)
            {
                if (!chair.IsTaken && chair.IsClean)
                {
                    _aiComponent.TakeChairByMonster(chair);
                    _navigation.ProcessStationMovement(chair);
                    //Debug.Log("Found free chair");
                    return true;
                }
            }
            //Debug.Log("Couldn't find free chair");
            return false;
        }
    }
}