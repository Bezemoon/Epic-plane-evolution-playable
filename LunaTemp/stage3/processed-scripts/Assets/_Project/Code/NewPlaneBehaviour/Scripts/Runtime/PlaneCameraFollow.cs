using UnityEngine;

namespace NewPlaneBehaviour
{
    public sealed class PlaneCameraFollow
    {
        private readonly Transform _cameraTransform;
        private readonly Transform _target;
        
        private readonly Vector3 _offset;
        private readonly float _followSharpness;
        private readonly float _lookSharpness;
        private readonly float _lookAheadDistance;

        public bool IsFollowing { get; private set; }

        public PlaneCameraFollow(Transform cameraTransform, Transform target, Vector3 offset, 
            float followSharpness, float lookSharpness,  float lookAheadDistance)
        {
            _cameraTransform = cameraTransform;
            _target = target;
            _offset = offset;
            _followSharpness = followSharpness;
            _lookSharpness = lookSharpness;
            _lookAheadDistance = lookAheadDistance;
        }

        public void StartFollowing()
        {
            IsFollowing = true;
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
 
            Vector3 targetPosition = CalculateTargetPosition();
            float moveProgress = CalculateProgress(_followSharpness, deltaTime);
            _cameraTransform.position = Vector3.Lerp(_cameraTransform.position, targetPosition, moveProgress);
 
            float turnProgress = CalculateProgress(_lookSharpness, deltaTime);
            _cameraTransform.rotation = Quaternion
                .Slerp(_cameraTransform.rotation, CalculateLookRotation(), turnProgress);
            
        }

        public void Reset()
        {
            _cameraTransform.position = _target.position + _offset;
            _cameraTransform.rotation = CalculateLookRotation();
        }
        
        private Vector3 CalculateTargetPosition()
        {
            return _target.position + CalculateFlightDirection() * _offset;
        }

        private Quaternion CalculateLookRotation()
        {
            Vector3 lookPoint = _target.position + Vector3.forward * _lookAheadDistance;
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