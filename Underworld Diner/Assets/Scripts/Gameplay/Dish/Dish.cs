using Interfaces;
using UnityEngine;

namespace Gameplay.Dish
{
    [CreateAssetMenu(fileName = "New Dish", menuName = "DS/Dish")]
    public class Dish : ScriptableObject, IDish
    {
        [field:SerializeField] public string DishName { get; private set;}
        [field:SerializeField] public Sprite MenuImage { get; private set;}
        [field:SerializeField] public Sprite DishImage { get; private set;}
    }
}