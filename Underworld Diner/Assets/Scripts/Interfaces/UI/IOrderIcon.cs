using UnityEngine;
using Zenject;

namespace Interfaces.UI
{
    public interface IOrderIcon
    {
        public class Factory : PlaceholderFactory<IDish, Transform, IOrderIcon>
        {
            
        }
    }
}