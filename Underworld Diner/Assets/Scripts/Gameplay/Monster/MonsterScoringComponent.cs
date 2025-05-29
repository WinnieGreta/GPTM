using Gameplay.Monster.Abstract;
using Interfaces;
using Signals;
using Zenject;

namespace Gameplay.Monster
{
    internal class MonsterScoringComponent : IScoringComponent
    {
        [Inject] private MonsterServiceSettings _serviceSettings;
        [Inject] private IRecipeBook _recipeBook;
        [Inject] private SignalBus _signalBus;
        [Inject] private MonsterStatusComponent _statusComponent;

        public void ScoreFood()
        {
            float score = 0;
            foreach (var dish in _statusComponent.FullOrder)
            {
                if (dish == DishType.None)
                {
                    continue;
                }
                score += _recipeBook[dish].CookingTime;
            }
            _signalBus.Fire(new OnMonsterScoredSignal() { Score = score * _serviceSettings.EatingDowntime * _statusComponent.Patience});
        }
    }
}