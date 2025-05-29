using UnityEngine;
using Zenject;

namespace Interfaces.UI
{
    public interface IPatienceMeter : IDespawnable
    {
        void UpdatePatienceMeter(float patience, Transform anchor);
        float UpdateHeartAmount(float patience, Transform anchor, float newHeartAmount);
        public class Factory : PlaceholderFactory<Transform, int, IPatienceMeter>
        {
            
        }
    }
}