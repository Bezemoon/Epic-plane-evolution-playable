using System;
using UnityEngine;

namespace PlaneBehaviour
{
    [Serializable]
    public class PlaneSlingshotConfig
    {
        [SerializeField] private float _grabRadius = 0.12f;
        [SerializeField] private float _maxPullDistance = 3f;
        [SerializeField] private float _maxNoseRaiseAngle = 15f;
        [SerializeField] private float _pullReactionSpeed = 15f;

        [SerializeField] private float _minPullRadius = 0.03f;
        [SerializeField] private float _maxPullRadius = 0.15f;
        
        [SerializeField] private float _minLaunchSpeed = 30f;
        [SerializeField] private float _maxLaunchSpeed = 38f;
 
        public float GrabRadius => _grabRadius;
        public float MaxPullDistance => _maxPullDistance;
        public float MaxNoseRaiseAngle => _maxNoseRaiseAngle;
        public float PullReactionSpeed => _pullReactionSpeed;
        
        public float MinPullRadius => _minPullRadius;
        public float MaxPullRadius => _maxPullRadius;
        
        public float MinLaunchSpeed => _minLaunchSpeed;
        public float MaxLaunchSpeed => _maxLaunchSpeed;
    }
}