using UnityEngine;
using Zenject;

namespace Gameplay.Staition
{
    public class StationDetectionComponent : ITickable, IInitializable
    {
        [Inject(Id = "player")] private Transform _playerTransform;
        [Inject(Id = "station")] private Transform _stationTransform;

        private float _distance;

        public void Initialize()
        {
            _distance = 2f;
            //Debug.Log("hello from station detection");
        }


        public void Tick()
        {
            //Debug.Log("I'm alive");
            //Debug.Log((_playerTransform.position - _stationTransform.position).magnitude);
            if ((_playerTransform.position - _stationTransform.position).magnitude < _distance)
            {
                Debug.Log("Player at the station");
            }
        }
    }
}