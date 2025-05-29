using UnityEngine;
using Zenject;

namespace Interfaces.Weapons
{
    public interface IProjectile : IDamageDealer, IDespawnable
    {
        void Shoot(Vector3 shootDirection);

        public class Factory : PlaceholderFactory<Vector3, IProjectile>
        {
            
        }
    }
}