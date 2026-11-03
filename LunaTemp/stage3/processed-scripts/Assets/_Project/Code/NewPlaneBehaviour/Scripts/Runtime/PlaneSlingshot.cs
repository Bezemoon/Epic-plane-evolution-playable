using System;
using UnityEngine;

namespace NewPlaneBehaviour
{
    public class PlaneSlingshot
    {
        private readonly Transform _transform;
        private readonly PlaneInput _input;
        private readonly Camera _camera;

        private readonly float _grabRadius;
        private readonly float _minPullRadius;
        private readonly float _maxPullRadius;
        private readonly float _maxPullDistance;
        private readonly float _maxNoseRaiseAngle;
        private readonly float _pullReactionSpeed;
        private readonly float _minLaunchSpeed;
        private readonly float _maxLaunchSpeed;

        private readonly Vector3 _startPosition;
        
        private bool _isActive;
        private float _minPullAmount;
        private Vector3 _pullDirection;

        public bool IsPulling { get; private set; }
        public float PullAmount { get; private set; }
        
        public event Action<float> Shot;

        public PlaneSlingshot(Transform transform, PlaneInput input, Camera camera, float grabRadius, 
            float minPullRadius, float maxPullRadius, float maxPullDistance, float maxNoseRaiseAngle, 
            float pullReactionSpeed, float minLaunchSpeed, float maxLaunchSpeed)
        {
            _transform = transform;
            _input = input;
            _camera = camera;
            
            _grabRadius = grabRadius;
            _minPullRadius = minPullRadius;
            _maxPullRadius = maxPullRadius;
            _maxPullDistance = maxPullDistance;
            _maxNoseRaiseAngle = maxNoseRaiseAngle;
            _pullReactionSpeed = pullReactionSpeed;
            
            _minLaunchSpeed = minLaunchSpeed;
            _maxLaunchSpeed = maxLaunchSpeed;

            _startPosition = transform.position;
            _minPullAmount = minPullRadius / maxPullRadius;
            
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
            Shot?.Invoke(Mathf.Lerp(_minLaunchSpeed, _maxLaunchSpeed, PullAmount));
        }
        
        private void UpdatePull()
        {
            Vector2 pointerOffset = _input.PointerPosition - CalculatePlaneScreenPosition();
            Vector2 direction = pointerOffset.normalized;
 
            PullAmount = Mathf.Clamp01(pointerOffset.magnitude / (_maxPullRadius * Screen.height));
            _pullDirection = new Vector3(direction.x, 0f, Mathf.Min(direction.y, 0f));
        }

        private bool CanGrab()
        {
            Vector2 pressOffset = _input.PointerPosition - CalculatePlaneScreenPosition();
            return pressOffset.magnitude <= _grabRadius * Screen.height;
        }
        
        private void FollowPull(float deltaTime)
        {
            Vector3 targetPosition = _startPosition + _pullDirection * (PullAmount * _maxPullDistance);
            Quaternion targetRotation = Quaternion.Euler(-PullAmount * _maxNoseRaiseAngle, 0f, 0f);
            float moveProgress = 1f - Mathf.Exp(-_pullReactionSpeed * deltaTime);
 
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