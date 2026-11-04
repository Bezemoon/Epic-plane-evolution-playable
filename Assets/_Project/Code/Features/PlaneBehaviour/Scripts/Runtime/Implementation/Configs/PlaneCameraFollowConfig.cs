using System;
using UnityEngine;

namespace PlaneBehaviour
{
    [Serializable]
    public sealed class PlaneCameraFollowConfig
    {
        [SerializeField] private Vector3 _cameraOffset = new Vector3(0f, 6f, -15f);
        [SerializeField] private float _cameraLookAheadDistance = 20f;
        [SerializeField] private float _cameraFollowSmoothTime = 0.35f;
        [SerializeField] private float _cameraFollowDelay = 0.2f;
        [SerializeField] private float _cameraLookBlendDuration = 0.5f;
        [SerializeField] private float _cameraLookSharpness = 10f;
        [SerializeField] private float _startCameraPitch = 30f;
 
        public Vector3 CameraOffset => _cameraOffset;
        public float CameraLookAheadDistance => _cameraLookAheadDistance;
        public float CameraFollowSmoothTime => _cameraFollowSmoothTime;
        public float CameraFollowDelay => _cameraFollowDelay;
        public float CameraLookBlendDuration => _cameraLookBlendDuration;
        public float CameraLookSharpness => _cameraLookSharpness;
        public float StartCameraPitch => _startCameraPitch;
    }
}