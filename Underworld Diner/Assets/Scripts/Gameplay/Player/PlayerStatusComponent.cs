using System.Collections.Generic;
using System.Linq;
using Interfaces;

namespace Gameplay.Player
{
    public class PlayerStatusComponent
    {
        public List<IDish> Hands { get; private set; } = new ();

        public override string ToString()
        {
            string handsString = Hands.Count == 0
                ? "Empty hands"
                : string.Join(", ", Hands.Select(d => d.DishName));
        
            return $"Chef with: {handsString}";
        }
    }
}