using System;
using UnityEngine;

namespace PlaneBehaviour
{
    [Serializable]
    public class PlaneClimbConfig
    {
        [SerializeField] private AnimationCurve _climbSpeedCurve = new AnimationCurve(
            new Keyframe[]
            {
                new Keyframe(0f, 0f, 0f, 2f),
                new Keyframe(1f, 1f, 0f, 0f)
            });
 
        [SerializeField] private float _noseTurnDuration = 0.15f;
 
        public AnimationCurve ClimbSpeedCurve => _climbSpeedCurve;
        public float NoseTurnDuration => _noseTurnDuration;
    }
}