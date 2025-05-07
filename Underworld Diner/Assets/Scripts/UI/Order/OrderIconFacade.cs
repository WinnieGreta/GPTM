using Interfaces;
using Interfaces.UI;
using UnityEngine;
using Zenject;

namespace UI.Order
{
    public class OrderIconFacade : MonoBehaviour, IOrderIcon, IPoolable<IDish, Transform>, IDespawnable
    {
        [Inject] private OrderIconPool _orderIconPool;
        
        public void OnDespawned()
        {
            
        }

        public void OnSpawned(IDish dish, Transform p2)
        {
            
        }

        public void Despawn()
        {
            _orderIconPool.Despawn(this);
        }
    }
}