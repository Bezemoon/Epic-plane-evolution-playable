using System;
using UnityEngine;
using UpgradeSystem;

namespace PlaneBehaviour
{
    public sealed class Plane : MonoBehaviour
    {
#if UNITY_EDITOR
        private const int RampGizmoSegmentCount = 20;
#endif

        [SerializeField] private Transform _planeRoot;
        [SerializeField] private Camera _camera;
        [SerializeField] private Transform _rampStartPoint;
        [SerializeField] private Transform _rampControlPoint;
        [SerializeField] private Transform _rampEndPoint;

        private PlaneConfig _config;
        private PlaneInput _input;
        private PlaneSlingshot _slingshot;
        private PlaneClimb _planeClimb;
        private PlaneFlight _planeFlight;
        private PlaneSlide _planeSlide;
        private PlaneCameraFollow _cameraFollow;
        private PlaneFuel _fuel;
        private Upgrade _planeUpgrade;
        
        private Vector3 _startPosition;

        public event Action Stopped;

        public Transform PlaneRoot => _planeRoot;

        public Camera Camera => _camera;

        public Transform RampStartPoint => _rampStartPoint;

        public Transform RampControlPoint => _rampControlPoint;

        public Transform RampEndPoint => _rampEndPoint;

        public PlaneConfig Config => _config;

        public void Initialize(PlaneConfig config, PlaneInput input, PlaneSlingshot slingshot, PlaneClimb planeClimb, 
            PlaneFlight planeFlight, PlaneSlide planeSlide, PlaneCameraFollow cameraFollow, PlaneFuel fuel, 
            Upgrade planeUpgrade)
        {
            _config = config;
            _input = input;
            _slingshot = slingshot;
            _planeClimb = planeClimb;
            _planeFlight = planeFlight;
            _planeSlide = planeSlide;
            _cameraFollow = cameraFollow;
            _fuel = fuel;
            _planeUpgrade = planeUpgrade;
            
            _startPosition = _planeRoot.position;

            _cameraFollow.Reset();
            _cameraFollow.StopFollowing();

            _slingshot.Shot += OnShot;
            _planeClimb.Finished += OnPlaneClimbFinished;
            _planeFlight.Landed += OnPlaneLanded;
            _planeSlide.Stopped += OnPlaneStopped;
        }
        
        public void Restore()
        {
            _planeSlide.Stop();
            _planeFlight.Stop();
            _planeClimb.Reset();
            _slingshot.Reset();
            _cameraFollow.StopFollowing();
            _cameraFollow.Reset();
        }

        private void Update()
        {
            _input.Tick();

            _slingshot.Tick(Time.deltaTime);
            _planeClimb.Tick(Time.deltaTime);
            _planeFlight.Tick(Time.deltaTime);
            _planeSlide.Tick(Time.deltaTime);
        }

        private void LateUpdate()
        {
            _cameraFollow.Tick(Time.deltaTime);
        }

        private void OnDestroy()
        {
            _slingshot.Shot -= OnShot;
            _planeClimb.Finished -= OnPlaneClimbFinished;
            _planeFlight.Landed -= OnPlaneLanded;
            _planeSlide.Stopped -= OnPlaneStopped;
        }

        private void OnShot(float launchSpeed)
        {
            _planeClimb.Start(launchSpeed);
            _cameraFollow.StartFollowing();
        }

        private void OnPlaneClimbFinished(Vector3 velocity)
        {
            _fuel.Fill(_planeUpgrade.Value);
            _planeFlight.Start(velocity);
        }

        private void OnPlaneLanded(Vector3 velocity)
        {
            _planeSlide.Start(velocity);
        }

        private void OnPlaneStopped()
        {
            Stopped?.Invoke();
        }

#if UNITY_EDITOR
        [Header("Editor")]
        [SerializeField] private PlaneConfig _debugConfig;

        private void OnDrawGizmos()
        {
            if (_planeRoot == null || _camera == null || _debugConfig == null)
            {
                return;
            }

            Vector3 center = Application.isPlaying ? _startPosition : _planeRoot.position;
            PlaneSlingshotConfig slingshotConfig = _debugConfig.SlingshotConfig;

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(center, CalculateWorldRadius(center, slingshotConfig.GrabRadius));

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(center, CalculateWorldRadius(center, slingshotConfig.MinPullRadius));

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(center, CalculateWorldRadius(center, slingshotConfig.MaxPullRadius));

            DrawRampCurve();
        }

        private void DrawRampCurve()
        {
            if (_rampStartPoint == null || _rampControlPoint == null || _rampEndPoint == null)
            {
                return;
            }

            BezierCurve rampCurve = new BezierCurve();
            rampCurve.Set(_rampStartPoint.position, _rampControlPoint.position, _rampEndPoint.position);

            Gizmos.color = Color.cyan;
            Vector3 previousPosition = _rampStartPoint.position;

            for (int segmentIndex = 1; segmentIndex <= RampGizmoSegmentCount; segmentIndex++)
            {
                float distance = rampCurve.Length * segmentIndex / RampGizmoSegmentCount;
                Vector3 position = rampCurve.CalculatePosition(distance);
                Gizmos.DrawLine(previousPosition, position);
                previousPosition = position;
            }
        }

        // Радиус задан долей высоты экрана, а в мире его размер зависит от того, как далеко самолёт от камеры
        private float CalculateWorldRadius(Vector3 center, float screenHeightShare)
        {
            float distanceToCamera = Vector3.Dot(center - _camera.transform.position, _camera.transform.forward);
            float visibleHeight = 2f * distanceToCamera * Mathf.Tan(_camera.fieldOfView * 0.5f * Mathf.Deg2Rad);
            return screenHeightShare * visibleHeight;
        }
#endif
    }
}