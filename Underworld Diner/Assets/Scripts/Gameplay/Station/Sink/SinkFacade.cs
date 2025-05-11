using Interfaces;
using UnityEngine;
using Zenject;

namespace Gameplay.Station.Sink
{
    public class SinkFacade : StationFacade, ISink
    {
        public void WashDirtyPlates()
        {
            Debug.Log("Washing dirty plates!");
        }
    }
}