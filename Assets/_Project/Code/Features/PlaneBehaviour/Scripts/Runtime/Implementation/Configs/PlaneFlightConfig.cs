using System;
using UnityEngine;

namespace PlaneBehaviour
{
    [Serializable]
    public class PlaneFlightConfig
    {
        [SerializeField] private float _gravity = 9.81f;
        [SerializeField] private float _groundHeight = 0f;
        [SerializeField] private float _maxHeight = 100f;
        [SerializeField] private float _drag = 0.001f;
        [SerializeField] private float _minClimbCost = 2f;
        [SerializeField] private float _maxClimbCost = 6f;
        [SerializeField] private float _climbCostGrowthHeight = 15f;
        [SerializeField] private float _safeClimbAngle = 30f;
        [SerializeField] private float _verticalClimbCostMultiplier = 6f;
        [SerializeField] private float _noControlSpeed = 6f;
        [SerializeField] private float _fullControlSpeed = 10f;
        [SerializeField] private float _pitchRate = 40f;
        [SerializeField] private float _maxClimbAngle = 75f;
        [SerializeField] private float _maxRotationSpeed = 90f;
        [SerializeField] private float _turnRate = 30f;
        [SerializeField] private float _maxTurnAngle = 60f;
        [SerializeField] private float _maxWingAngle = 25f;
        [SerializeField] private float _wingReactionSpeed = 6f;
        [SerializeField] private float _maxSideDistance = 12f;
        [SerializeField] private float _fuelAcceleration = 10f;
 
        public float Gravity => _gravity;
        public float GroundHeight => _groundHeight;
        public float MaxHeight => _maxHeight;
        public float Drag => _drag;
        public float MinClimbCost => _minClimbCost;
        public float MaxClimbCost => _maxClimbCost;
        public float ClimbCostGrowthHeight => _climbCostGrowthHeight;
        public float SafeClimbAngle => _safeClimbAngle;
        public float VerticalClimbCostMultiplier => _verticalClimbCostMultiplier;
        public float NoControlSpeed => _noControlSpeed;
        public float FullControlSpeed => _fullControlSpeed;
        public float PitchRate => _pitchRate;
        public float MaxClimbAngle => _maxClimbAngle;
        public float MaxRotationSpeed => _maxRotationSpeed;
        public float TurnRate => _turnRate;
        public float MaxTurnAngle => _maxTurnAngle;
        public float MaxWingAngle => _maxWingAngle;
        public float WingReactionSpeed => _wingReactionSpeed;
        public float MaxSideDistance => _maxSideDistance;
        public float FuelAcceleration => _fuelAcceleration;
    }
}