using System.Collections.Generic;
using Interfaces.Player;
using UnityEngine;
using Zenject;

namespace Gameplay.Player
{
    public class PlayerMonoFacade : MonoBehaviour, IPlayer
    {
        [Inject] private PlayerNavigationComponent _navigationComponent;

        public IReadOnlyCollection<ICommand> Commands => _navigationComponent.CommandQueue;
    }
}