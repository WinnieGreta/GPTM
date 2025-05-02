using Interfaces;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Gameplay.Staition
{
    public class StationFacade : MonoBehaviour, IStation
    {
        [Inject] private StationAnchorParameters _anchorParameters;

        public Vector2 GetClosestAnchorPosition(Vector2 playerPosition)
        {
            //Debug.Log("Distance to player " + ((Vector2)_stationTransform.position - playerPosition).magnitude);
            //Debug.Log("Navmesh Distance to player " + CalculateNavMeshPathLength(playerPosition, _stationTransform));
            //Debug.Log("Closest anchor position " + GetClosestAnchorInternal(playerPosition).position);
            return GetClosestAnchorInternal(playerPosition).position;
        }

        // general use function to get the length of a navmesh path
        // TODO make more abstract and move to some module (e.g. Tools) for the maximum DRYness
        private float CalculateNavMeshPathLength(Vector2 playerPosition, Transform stationTransform)
        {
            NavMeshPath path = new NavMeshPath();
            NavMesh.CalculatePath(playerPosition, stationTransform.position, NavMesh.AllAreas, path);
            float length = 0f;
            if ((path.status != NavMeshPathStatus.PathInvalid) && (path.corners.Length > 1))
            {
                for (int i = 1; i < path.corners.Length; i++)
                {
                    length += Vector3.Distance(path.corners[i - 1], path.corners[i]);
                }
            }

            return length;
        }
        
        // find the anchor that would require player to travel the shortest length on navmesh from their current position
        // we opted to use for loop (not LINQ) because the number of player movement anchors for any given station would be small (up to ~5)
        private Transform GetClosestAnchorInternal(Vector2 playerPosition)
        {
            Transform result = _anchorParameters.Anchors[0];
            float previousMinLength = CalculateNavMeshPathLength(playerPosition, result);
            float newMinLength;
            
            for (int i = 1; i < _anchorParameters.Anchors.Count; i++)
            {
                newMinLength = CalculateNavMeshPathLength(playerPosition, _anchorParameters.Anchors[i]);
                if (newMinLength < previousMinLength)
                {
                    result = _anchorParameters.Anchors[i];
                    previousMinLength = newMinLength;
                }
            }

            return result;
        }
    }
}