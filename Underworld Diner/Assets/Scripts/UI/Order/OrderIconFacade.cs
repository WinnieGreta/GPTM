using Interfaces;
using Interfaces.UI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Order
{
    public class OrderIconFacade : MonoBehaviour, IOrderIcon, IPoolable<DishType, Transform>, IDespawnable
    {
        [Inject] private OrderIconPool _orderIconPool;
        [Inject] private Transform _containerImageTransform;
        [Inject] private Image _dishImage;
        [Inject] private IRecipeBook _recipeBook;


        [SerializeField] private Vector3 _orderIconOffset;
        
        public void OnDespawned()
        {
            
        }

        public void OnSpawned(DishType dish, Transform anchor)
        {
            _containerImageTransform.position = anchor.position + _orderIconOffset;
            _dishImage.sprite = _recipeBook[dish].MenuImage;
        }

        public void Despawn()
        {
            _orderIconPool.Despawn(this);
        }
    }
}