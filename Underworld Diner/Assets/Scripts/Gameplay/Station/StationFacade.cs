﻿using System.Collections.Generic;
using Interfaces;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Gameplay.Station
{
    public class StationFacade : MonoBehaviour, IStation
    {
        [Inject] private StationAnchorParameters _anchorParameters;

        private List<Transform> _anchors;
            
        public Vector2 GetClosestAnchorPosition(NavMeshAgent agent)
        {
            return GetClosestAnchorInternal(agent).position;
        }

        public virtual LinkedList<DishType> PlayerStationInteraction(LinkedList<DishType> playerHands)
        {
            return playerHands;
        }

        // general use function to get the length of a navmesh path
        private float CalculateNavMeshPathLength(Vector2 agentPosition, Transform stationTransform)
        {
            NavMeshPath path = new NavMeshPath();
            NavMesh.CalculatePath(agentPosition, stationTransform.position, NavMesh.AllAreas, path);
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
        private Transform GetClosestAnchorInternal(NavMeshAgent agent)
        {
            if (agent.CompareTag("Player"))
            {
                _anchors = _anchorParameters.PlayerAnchors;
            }
            else
            {
                _anchors = _anchorParameters.MonsterAnchors;
            }
            
            Transform result = _anchors[0];
            float previousMinLength = CalculateNavMeshPathLength(agent.transform.position, result);
            float newMinLength;
            
            for (int i = 1; i < _anchors.Count; i++)
            {
                newMinLength = CalculateNavMeshPathLength(agent.transform.position, _anchors[i]);
                if (newMinLength < previousMinLength)
                {
                    result = _anchors[i];
                    previousMinLength = newMinLength;
                }
            }

            return result;
        }
    }
}