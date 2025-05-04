using Interfaces;
using UnityEngine;
using Zenject;

namespace Gameplay.Monster
{
    public class MonsterFacade : MonoBehaviour, IMonster, IPoolable<Transform>
    {
        public void OnDespawned()
        {
            
        }

        public void OnSpawned(Transform transform)
        {
            gameObject.transform.position = transform.position;
        }
    }
}