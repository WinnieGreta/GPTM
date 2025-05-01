using Interfaces;
using UnityEngine;
using Zenject;

namespace Gameplay.Staition
{
    public class StationFacade : MonoBehaviour, IStation
    {
        [Inject] private Transform _stationTransform;
        public void Ping()
        {
            Debug.Log("You clicked on a station " + _stationTransform.position);
        }

        public void ProcessClick(Vector2 playerPosition)
        {
            Debug.Log("Distance to player " + ((Vector2)_stationTransform.position - playerPosition).magnitude);
        }
    }
}