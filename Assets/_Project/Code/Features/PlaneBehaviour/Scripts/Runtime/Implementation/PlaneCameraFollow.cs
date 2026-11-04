using UnityEngine;

namespace PlaneBehaviour
{
    public class PlaneCameraFollow
    {
        private readonly PlaneCameraFollowConfig _config;
        private readonly Transform _cameraTransform;
        private readonly Transform _target;

        private Vector3 _moveVelocity;
        private float _followTime;

        public bool IsFollowing { get; private set; }

        public PlaneCameraFollow(PlaneCameraFollowConfig config, Transform cameraTransform, Transform target)
        {
            _config = config;
            _cameraTransform = cameraTransform;
            _target = target;
        }

        public void StartFollowing()
        {
            IsFollowing = true;
            _followTime = 0f;
            _moveVelocity = Vector3.zero;
        }
 
        public void StopFollowing()
        {
            IsFollowing = false;
        }
        
        public void Tick(float deltaTime)
        {
            if (IsFollowing == false)
            {
                return;
            }
 
            _followTime += deltaTime;

            if (_followTime < _config.CameraFollowDelay)
            {
                return;
            }

            float blendProgress = Mathf.Clamp01((_followTime - _config.CameraFollowDelay) / _config.CameraLookBlendDuration);

            Vector3 targetPosition = CalculateTargetPosition();
            _cameraTransform.position = Vector3.SmoothDamp(_cameraTransform.position, targetPosition, 
                ref _moveVelocity, _config.CameraFollowSmoothTime, Mathf.Infinity, deltaTime);

            float turnSharpness = _config.CameraLookSharpness * Mathf.SmoothStep(0f, 1f, blendProgress);
            float turnProgress = CalculateProgress(turnSharpness, deltaTime);
            _cameraTransform.rotation = Quaternion
                .Slerp(_cameraTransform.rotation, CalculateLookRotation(), turnProgress);
        }

        public void Reset()
        {
            _cameraTransform.position = _target.position + _config.CameraOffset;
            _cameraTransform.rotation = CalculateFlightDirection() * Quaternion.Euler(_config.StartCameraPitch, 0f, 0f);
        }
        
        private Vector3 CalculateTargetPosition()
        {
            return _target.position + CalculateFlightDirection() * _config.CameraOffset;
        }

        private Quaternion CalculateLookRotation()
        {
            Vector3 lookPoint = _target.position + Vector3.forward * _config.CameraLookAheadDistance;
            return Quaternion.LookRotation(lookPoint - _cameraTransform.position);
        }

        private Quaternion CalculateFlightDirection()
        {
            Vector3 forward = _target.forward;
            Vector3 flatForward = new Vector3(forward.x, 0f, forward.z);
 
            if (flatForward.sqrMagnitude <= 0f)
            {
                return Quaternion.identity;
            }
 
            return Quaternion.LookRotation(flatForward);
        }

        private float CalculateProgress(float sharpness, float deltaTime)
        {
            return 1f - Mathf.Exp(-sharpness * deltaTime);
        }
    }
}