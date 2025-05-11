using Interfaces;
using Interfaces.UI;
using UnityEngine;
using Zenject;

namespace UI.Order
{
    public class OrderIconFactoryInstaller : MonoInstaller
    {
        [SerializeField] private OrderIconFacade _orderIconFacade;
        [SerializeField] private Transform _iconRoot;
        public override void InstallBindings()
        {
            Container.BindMemoryPool<OrderIconFacade, OrderIconPool>()
                .WithInitialSize(5)
                .FromComponentInNewPrefab(_orderIconFacade)
                .UnderTransform(_ => _iconRoot);
            Container.BindFactory<DishType, Transform, IOrderIcon, IOrderIcon.Factory>()
                .FromFactory<OrderIconFactory>();
        }
    }

    internal class OrderIconFactory : IFactory<DishType, Transform, IOrderIcon>
    {
        [Inject] private OrderIconPool _pool;
        public IOrderIcon Create(DishType dish, Transform anchor)
        {
            return _pool.Spawn(dish, anchor);
        }
    }
    
    // alias for readability
    internal class OrderIconPool : MonoPoolableMemoryPool<DishType, Transform, OrderIconFacade>
    {
    }
}