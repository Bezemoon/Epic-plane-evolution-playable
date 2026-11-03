using System;
using UnityEngine;

namespace PlaneBehaviour
{
    public sealed class PlaneFlight
    {
        private readonly PlaneFlightConfig _config;
        private readonly Transform _transform;
        private readonly PlaneInput _input;
        private readonly PlaneFuel _fuel;

        private readonly float _startSidePosition;
        
        private Vector3 _velocity;
        private bool _isRunning;
        private float _wingAngle;

        public event Action<Vector3> Landed;

        public PlaneFlight(PlaneFlightConfig config, Transform transform, PlaneInput input, PlaneFuel fuel)
        {
            _config = config;
            _transform = transform;
            _input = input;
            _fuel = fuel;

            _startSidePosition = transform.position.x;
        }

        public void Start(Vector3 velocity)
        {
            _velocity = velocity;
            _wingAngle = 0f;
            _isRunning = true;
        }

        public void Stop()
        {
            _isRunning = false;
        }

        public void Tick(float deltaTime)
        {
            if (_isRunning == false)
            {
                return;
            }
 
            _velocity.y -= _config.Gravity * deltaTime;
            
            SlowDownByAir(deltaTime);
            SlowDownOnClimb(deltaTime);
            AccelerateWithFuel(deltaTime);
            TurnNoseWithStick(deltaTime);
            TurnSideWithStick(deltaTime);
            UpdateWingAngle(deltaTime);
 
            Vector3 position = _transform.position + _velocity * deltaTime;
 
            if (position.y > _config.MaxHeight)
            {
                position.y = _config.MaxHeight;
                _velocity.y = Mathf.Min(_velocity.y, 0f);
            }
 
            LimitSideDistance(ref position);
            
            _transform.rotation = Quaternion.LookRotation(_velocity) * Quaternion.Euler(0f, 0f, -_wingAngle);
 
            if (position.y <= _config.GroundHeight)
            {
                position.y = _config.GroundHeight;
                
                _isRunning = false;
                _transform.position = position;
                
                Landed?.Invoke(_velocity);
                
                return;
            }
 
            _transform.position = position;
        }
        
        private void SlowDownByAir(float deltaTime)
        {
            float speed = _velocity.magnitude;
            SetSpeed(speed - _config.Drag * speed * speed * deltaTime);
        }
        
        private void SlowDownOnClimb(float deltaTime)
        {
            float speed = _velocity.magnitude;
 
            if (_velocity.y <= 0f)
            {
                return;
            }
 
            float climbSteepness = _velocity.y / speed;
            float climbCost = CalculateClimbCost(_transform.position.y) * CalculateClimbCostMultiplier(climbSteepness);
            SetSpeed(speed - (climbCost - 1f) * _config.Gravity * climbSteepness * deltaTime);
        }
        
        private float CalculateClimbCost(float height)
        {
            float growthProgress = Mathf.InverseLerp(_config.ClimbCostGrowthHeight, _config.MaxHeight, height);
            return Mathf.Lerp(_config.MinClimbCost, _config.MaxClimbCost, growthProgress);
        }
        
        private float CalculateClimbCostMultiplier(float climbSteepness)
        {
            float climbAngle = Mathf.Asin(Mathf.Clamp01(climbSteepness)) * Mathf.Rad2Deg;
            float steepProgress = Mathf.InverseLerp(_config.SafeClimbAngle, 90f, climbAngle);

            return Mathf.Lerp(1f, _config.VerticalClimbCostMultiplier, steepProgress);
        }
        
        private void AccelerateWithFuel(float deltaTime)
        {
            if (_fuel.IsEmpty)
            {
                return;
            }

            SetSpeed(_velocity.magnitude + _config.FuelAcceleration * deltaTime);
            _fuel.Reduce(deltaTime);
        }
        
        private void TurnNoseWithStick(float deltaTime)
        {
            float stickStrength  = Mathf.InverseLerp(_config.NoControlSpeed, _config.FullControlSpeed, _velocity.magnitude);
            float turnAngle = -_input.PitchInput * _config.PitchRate * stickStrength * deltaTime;
 
            _velocity = Quaternion.AngleAxis(turnAngle, Vector3.right) * _velocity;
        }
        
        private void TurnSideWithStick(float deltaTime)
        {
            float angleFromForward = Mathf.Atan2(_velocity.x, _velocity.z) * Mathf.Rad2Deg;
            float turnAngle = _input.TurnInput * _config.TurnRate * deltaTime;
            float newAngleFromForward = Mathf.Clamp(angleFromForward + turnAngle, 
                -_config.MaxTurnAngle, _config.MaxTurnAngle);
 
            _velocity = Quaternion.AngleAxis(newAngleFromForward - angleFromForward, Vector3.up) * _velocity;
        }
        
        private void UpdateWingAngle(float deltaTime)
        {
            float targetWingAngle = _input.TurnInput * _config.MaxWingAngle;
            _wingAngle = Mathf.Lerp(_wingAngle, targetWingAngle, Mathf.Clamp01(_config.WingReactionSpeed * deltaTime));
        }
        
        private void LimitSideDistance(ref Vector3 position)
        {
            float distanceFromCenter = position.x - _startSidePosition;
 
            if (Mathf.Abs(distanceFromCenter) <= _config.MaxSideDistance)
            {
                return;
            }
 
            position.x = _startSidePosition + Mathf.Sign(distanceFromCenter) * _config.MaxSideDistance;
            _velocity.x = 0f;
        }
        
        private void SetSpeed(float speed)
        {
            _velocity = _velocity.normalized * Mathf.Max(0f, speed);
        }
    }
}