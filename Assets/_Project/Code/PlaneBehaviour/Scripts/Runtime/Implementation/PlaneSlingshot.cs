using System;
using UnityEngine;
using UpgradeSystem;

namespace PlaneBehaviour
{
    public class PlaneSlingshot
    {
        private readonly PlaneSlingshotConfig _config;
        private readonly Transform _transform;
        private readonly PlaneInput _input;
        private readonly Camera _camera;
        private readonly Upgrade _upgrade;

        private readonly Vector3 _startPosition;
        private readonly float _minPullAmount;

        private bool _isActive;
        private Vector3 _pullDirection;

        public bool IsPulling { get; private set; }
        public float PullAmount { get; private set; }
        
        public event Action<float> Shot;

        public PlaneSlingshot(PlaneSlingshotConfig config, Transform transform, PlaneInput input, Camera camera, 
            Upgrade upgrade)
        {
            _config = config;
            _transform = transform;
            _input = input;
            _camera = camera;
            _upgrade = upgrade;

            _startPosition = transform.position;
            _minPullAmount = _config.MinPullRadius / _config.MaxPullRadius;
            
            _isActive = true;
        }

        public void Tick(float deltaTime)
        {
            if (_isActive == false)
            {
                return;
            }
 
            if (_input.DragStarted && CanGrab())
            {
                IsPulling = true;
            }
 
            if (IsPulling && _input.DragEnded)
            {
                Release();
            }
 
            if (_isActive == false)
            {
                return;
            }
 
            if (IsPulling)
            {
                UpdatePull();
            }
 
            FollowPull(deltaTime);
        }

        public void Reset()
        {
            _isActive = true;
            
            IsPulling = false;
            PullAmount = 0f;
            
            _transform.position = _startPosition;
            _transform.rotation = Quaternion.identity;
        }
        
        private void Release()
        {
            IsPulling = false;
 
            if (PullAmount < _minPullAmount)
            {
                PullAmount = 0f;
                return;
            }
 
            _isActive = false;
            Shot?.Invoke(Mathf.Lerp(_config.MaxLaunchSpeed, _upgrade.Value, PullAmount));
        }
        
        private void UpdatePull()
        {
            Vector2 pointerOffset = _input.PointerPosition - CalculatePlaneScreenPosition();
            Vector2 direction = pointerOffset.normalized;
 
            PullAmount = Mathf.Clamp01(pointerOffset.magnitude / (_config.MaxPullRadius * Screen.height));
            _pullDirection = new Vector3(direction.x, 0f, Mathf.Min(direction.y, 0f));
        }

        private bool CanGrab()
        {
            Vector2 pressOffset = _input.PointerPosition - CalculatePlaneScreenPosition();
            return pressOffset.magnitude <= _config.GrabRadius * Screen.height;
        }
        
        private void FollowPull(float deltaTime)
        {
            Vector3 targetPosition = _startPosition + _pullDirection * (PullAmount * _config.MaxPullDistance);
            Quaternion targetRotation = Quaternion.Euler(-PullAmount * _config.MaxNoseRaiseAngle, 0f, 0f);
            float moveProgress = 1f - Mathf.Exp(-_config.PullReactionSpeed * deltaTime);
 
            _transform.position = Vector3.Lerp(_transform.position, targetPosition, moveProgress);
            _transform.rotation = Quaternion.Slerp(_transform.rotation, targetRotation, moveProgress);
        }

        private Vector2 CalculatePlaneScreenPosition()
        {
            Vector3 screenPosition = _camera.WorldToScreenPoint(_startPosition);
            return new Vector2(screenPosition.x, screenPosition.y);
        }
    }
}