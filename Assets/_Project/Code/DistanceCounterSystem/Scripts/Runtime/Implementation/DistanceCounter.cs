using UnityEngine;

namespace DistanceCounterSystem
{
    public class DistanceCounter
    {
        private readonly Vector3 _startPosition;
 
        public DistanceCounter(Vector3 startPosition)
        {
            _startPosition = startPosition;
        }
 
        public float CalculateDistance(Vector3 position)
        {
            return Mathf.Max(0f, position.z - _startPosition.z);
        }
    }
}
