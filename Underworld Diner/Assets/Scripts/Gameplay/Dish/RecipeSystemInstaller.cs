using System.Collections.Generic;
using System.Linq;
using Interfaces;
using UnityEngine;
using Zenject;

namespace Gameplay.Dish
{
    // shorthand for recipes holder
    internal class RecipeBook : Dictionary<DishType, IDish>, IRecipeBook
    {
        public RecipeBook(Dictionary<DishType, IDish> dictionary) : base(dictionary)
        {
            
        }
    }
    
    public class RecipeSystemInstaller : MonoInstaller
    {
        [SerializeField] private List<DishRecipe> _recipes;

        public override void InstallBindings()
        {
            var dictionary = _recipes.ToDictionary(x => x.Type, x => (IDish)x);
            Container.BindInterfacesTo<RecipeBook>().FromInstance(new RecipeBook(dictionary)).AsSingle();
        }
    }
}