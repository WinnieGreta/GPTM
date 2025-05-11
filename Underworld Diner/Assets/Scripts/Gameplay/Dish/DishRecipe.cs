using Interfaces;
using UnityEngine;

namespace Gameplay.Dish
{
    [CreateAssetMenu(fileName = "New Dish", menuName = "DS/Dish")]
    internal class DishRecipe : ScriptableObject, IDish
    {
        [field:SerializeField] public DishType Type { get; private set;}
        [field:SerializeField] public string DishName { get; private set;}
        [field:SerializeField] public Sprite MenuImage { get; private set;}
        [field:SerializeField] public Sprite DishImage { get; private set;}
        
        [field:SerializeField] public float CookingTime { get; private set; }
    }
}