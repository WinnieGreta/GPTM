using System.Collections.Generic;
using System.Linq;
using Interfaces;

namespace Gameplay.Player
{
    public class PlayerStatusComponent
    {
        public LinkedList<DishType> Hands { get; private set; } = new ();
        public IStation StationImMovingTo { get; set; }

        public override string ToString()
        {
            string handsString = Hands.Count == 0
                ? "Empty hands"
                : string.Join(", ", Hands.Select(d => d.ToString()));
        
            return $"Chef with: {handsString}";
        }
    }
}