using UnityEngine;

namespace Interfaces
{
    public interface IDish
    {
        string DishName { get; }
        Sprite MenuImage { get; }
        Sprite DishImage { get; }
    }
}