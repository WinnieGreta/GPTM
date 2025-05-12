using System.Collections.Generic;
using UnityEngine;

namespace Interfaces
{
    public enum DishType
    {
        None,
        DirtyPlate,
        ChocolateCake,
        Burger,
        Pudding
    }
    
    public interface IDish
    {
        DishType Type { get; }
        string DishName { get; }
        Sprite MenuImage { get; }
        Sprite DishImage { get; }
        float CookingTime { get; }
        
        int RedCost { get; }
        int GreenCost { get; }
        int BlueCost { get; }
    }

    public interface IRecipeBook : IDictionary<DishType, IDish>
    {
        
    }
}