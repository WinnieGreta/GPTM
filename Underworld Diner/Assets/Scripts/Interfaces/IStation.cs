using UnityEngine;

namespace Interfaces
{
    public interface IStation
    {
        public void Ping();
        public void ProcessClick(Vector2 playerPosition);
    }
}