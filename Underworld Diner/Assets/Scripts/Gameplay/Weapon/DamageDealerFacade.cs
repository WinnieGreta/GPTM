using System;
using Interfaces;
using Interfaces.Weapons;
using UnityEngine;
using Zenject;

namespace Gameplay.Weapon
{
    public class DamageDealerFacade : MonoBehaviour, IDamageDealer
    {

        private int _hitsLeft;
        
        private int HITABLE_LAYER;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer == HITABLE_LAYER)
            {
                DoDamage(other.GetComponent<IDamagable>());
            }
        }

        public void DoDamage(IDamagable target)
        {
            
            /*if (target.GetDamaged(_weaponParameters.BaseDamage))
            {
                Debug.Log($"{target} got hit by {_weaponParameters.BaseDamage}");
                
            }*/
        }

        public void Despawn()
        {
            Debug.Log("weapon despawned");
            Destroy(gameObject);
        }
    }
}