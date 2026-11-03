using System;
using UnityEngine;

namespace NewPlaneBehaviour
{
    public sealed class PlaneFlight
    {
        private readonly Transform _transform;
        private readonly PlaneInput _input;
        
        private readonly float _gravity;
        private readonly float _groundHeight;
        private readonly float _pitchRate;
        private readonly float _drag;
        private readonly float _climbCost;
        private readonly float _noControlSpeed;
        private readonly float _fullControlSpeed;
        private readonly float _maxHeight;
        private readonly float _turnRate;
        private readonly float _maxTurnAngle;
        private readonly float _maxWingAngle;
        private readonly float _wingReactionSpeed;
        private readonly float _maxSideDistance;

        private readonly float _startSidePosition;
        
        private Vector3 _velocity;
        private bool _isRunning;
        private float _wingAngle;

        public event Action Landed;

        public PlaneFlight(Transform transform, PlaneInput input, float gravity, float groundHeight, float pitchRate, 
            float drag, float climbCost, float noControlSpeed, float fullControlSpeed, float maxHeight, float turnRate,
            float maxTurnAngle, float maxWingAngle, float wingReactionSpeed, float maxSideDistance)
        {
            _transform = transform;
            _input = input;
            
            _gravity = gravity;
            _groundHeight = groundHeight;
            _pitchRate = pitchRate;
            _drag = drag;
            _climbCost = climbCost;
            _noControlSpeed = noControlSpeed;
            _fullControlSpeed = fullControlSpeed;
            _maxHeight = maxHeight;
            _turnRate = turnRate;
            _maxTurnAngle = maxTurnAngle;
            _maxWingAngle = maxWingAngle;
            _wingReactionSpeed = wingReactionSpeed;
            _maxSideDistance = maxSideDistance;
            
            _startSidePosition = transform.position.x;
        }

        public void Start([Bridge.Ref] Vector3 velocity)
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
 
            _velocity.y -= _gravity * deltaTime;
            
            SlowDownByAir(deltaTime);
            SlowDownOnClimb(deltaTime);
            TurnNoseWithStick(deltaTime);
            TurnSideWithStick(deltaTime);
            UpdateWingAngle(deltaTime);
 
            Vector3 position = _transform.position + _velocity * deltaTime;
 
            if (position.y > _maxHeight)
            {
                position.y = _maxHeight;
                _velocity.y = Mathf.Min(_velocity.y, 0f);
            }
 
            LimitSideDistance(ref position);
            
            _transform.rotation = Quaternion.LookRotation(_velocity) * Quaternion.Euler(0f, 0f, -_wingAngle);
 
            if (position.y <= _groundHeight)
            {
                position.y = _groundHeight;
                
                _isRunning = false;
                _transform.position = position;
                
                Landed?.Invoke();
                
                return;
            }
 
            _transform.position = position;
        }
        
        private void SlowDownByAir(float deltaTime)
        {
            float speed = _velocity.magnitude;
            SetSpeed(speed - _drag * speed * speed * deltaTime);
        }
        
        private void SlowDownOnClimb(float deltaTime)
        {
            float speed = _velocity.magnitude;
 
            if (_velocity.y <= 0f)
            {
                return;
            }
 
            float climbSteepness  = _velocity.y / speed;
            SetSpeed(speed - (_climbCost - 1f) * _gravity * climbSteepness * deltaTime);
        }
        
        private void TurnNoseWithStick(float deltaTime)
        {
            float stickStrength  = Mathf.InverseLerp(_noControlSpeed, _fullControlSpeed, _velocity.magnitude);
            float turnAngle = -_input.PitchInput * _pitchRate * stickStrength * deltaTime;
 
            _velocity = Quaternion.AngleAxis(turnAngle, Vector3.right) * _velocity;
        }
        
        private void TurnSideWithStick(float deltaTime)
        {
            float angleFromForward = Mathf.Atan2(_velocity.x, _velocity.z) * Mathf.Rad2Deg;
            float turnAngle = _input.TurnInput * _turnRate * deltaTime;
            float newAngleFromForward = Mathf.Clamp(angleFromForward + turnAngle, -_maxTurnAngle, _maxTurnAngle);
 
            _velocity = Quaternion.AngleAxis(newAngleFromForward - angleFromForward, Vector3.up) * _velocity;
        }
        
        private void UpdateWingAngle(float deltaTime)
        {
            float targetWingAngle = _input.TurnInput * _maxWingAngle;
            _wingAngle = Mathf.Lerp(_wingAngle, targetWingAngle, Mathf.Clamp01(_wingReactionSpeed * deltaTime));
        }
        
        private void LimitSideDistance(ref Vector3 position)
        {
            float distanceFromCenter = position.x - _startSidePosition;
 
            if (Mathf.Abs(distanceFromCenter) <= _maxSideDistance)
            {
                return;
            }
 
            position.x = _startSidePosition + Mathf.Sign(distanceFromCenter) * _maxSideDistance;
            _velocity.x = 0f;
        }
        
        private void SetSpeed(float speed)
        {
            _velocity = _velocity.normalized * Mathf.Max(0f, speed);
        }
    }
}