using UnityEngine;

namespace PlaneBehaviour
{
    [CreateAssetMenu(fileName = "PlaneConfig", menuName = "Configs/PlaneBehaviour/Plane Config")]
    public class PlaneConfig : ScriptableObject
    {
        [SerializeField] private PlaneSlingshotConfig _slingshotConfig;
        [SerializeField] private PlaneClimbConfig _planeClimbConfig;
        [SerializeField] private PlaneFlightConfig _planeFlightConfig;
        [SerializeField] private PlaneCameraFollowConfig _planeCameraFollowConfig;
        
        [Space]
        [SerializeField] private float _joystickRadius = 0.15f;
        [SerializeField] private float _startFuel = 3.5f; 
        [SerializeField] private float _slideDeceleration = 20f;
 
        public PlaneSlingshotConfig SlingshotConfig => _slingshotConfig;
        public PlaneClimbConfig PlaneClimbConfig => _planeClimbConfig;
        public PlaneFlightConfig PlaneFlightConfig => _planeFlightConfig;
        public PlaneCameraFollowConfig PlaneCameraFollowConfig => _planeCameraFollowConfig;
        
        public float JoystickRadius => _joystickRadius;
        public float StartFuel => _startFuel;
        public float SlideDeceleration => _slideDeceleration;
    }
}
