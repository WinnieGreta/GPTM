using Interfaces;
using UnityEngine;

namespace Gameplay.Dish
{
    [CreateAssetMenu(fileName = "New Dish", menuName = "DS/Dish")]
    internal class DishRecipe : ScriptableObject, IDish
    {
        [field:Header("General")]
        [field:SerializeField] public DishType Type { get; private set;}
        [field:SerializeField] public string DishName { get; private set;}
        
        [field:Header("Visual Settings")]
        [field:SerializeField] public Sprite MenuImage { get; private set;}
        [field:SerializeField] public Sprite DishImage { get; private set;}
        
        [field:Header("Cooking Settings")]
        [field:SerializeField] public float CookingTime { get; private set; }
        
        [field:Header("Resources Settings")]
        [field:SerializeField] public int RedCost { get; private set; }
        [field:SerializeField] public int GreenCost { get; private set; }
        [field:SerializeField] public int BlueCost { get; private set; }
        
        
    }
}