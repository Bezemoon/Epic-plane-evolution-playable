using System;
using UnityEngine;

namespace PlaneBehaviour
{
    [Serializable]
    public sealed class PlaneCameraFollowConfig
    {
        [SerializeField] private Vector3 _cameraOffset = new Vector3(0f, 6f, -15f);
        [SerializeField] private float _cameraLookAheadDistance = 20f;
        [SerializeField] private float _cameraFollowSharpness = 8f;
        [SerializeField] private float _cameraLookSharpness = 10f;
        [SerializeField] private float _startCameraPitch = 30f;
 
        public Vector3 CameraOffset => _cameraOffset;
        public float CameraLookAheadDistance => _cameraLookAheadDistance;
        public float CameraFollowSharpness => _cameraFollowSharpness;
        public float CameraLookSharpness => _cameraLookSharpness;
        public float StartCameraPitch => _startCameraPitch;
    }
}