using System;
using UnityEngine;

namespace PlaneBehaviour
{
    public class PlaneSlide
    {
        private readonly Transform _transform;
        private readonly float _deceleration;
        
        private Vector3 _velocity;
        private bool _isSliding;

        public event Action Stopped;

        public PlaneSlide(Transform transform, float deceleration)
        {
            _transform = transform;
            _deceleration = deceleration;
        }

        public void Start(Vector3 landingVelocity)
        {
            _velocity = new Vector3(landingVelocity.x, 0f, landingVelocity.z);
            _isSliding = true;

            if (_velocity.sqrMagnitude > 0f)
            {
                _transform.rotation = Quaternion.LookRotation(_velocity);
            }
        }

        public void Stop()
        {
            _isSliding = false;
        }

        public void Tick(float deltaTime)
        {
            if (_isSliding == false)
            {
                return;
            }

            float speed = Mathf.Max(0f, _velocity.magnitude - _deceleration * deltaTime);

            _velocity = _velocity.normalized * speed;
            _transform.position += _velocity * deltaTime;

            if (speed <= 0f)
            {
                _isSliding = false;
                Stopped?.Invoke();
            }
        }
    }
}
