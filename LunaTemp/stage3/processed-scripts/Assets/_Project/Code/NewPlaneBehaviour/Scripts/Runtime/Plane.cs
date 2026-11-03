using UnityEngine;

namespace NewPlaneBehaviour
{
    public class Plane : MonoBehaviour
    {
#if UNITY_EDITOR
        private const int RampGizmoSegmentCount = 20;
#endif

        [SerializeField] private Transform _planeRoot;
        [SerializeField] private Transform _cameraTransform;
        [SerializeField] private Camera _camera;

        [Header("Ramp Points")] [SerializeField]
        private Transform _rampStartPoint;

        [SerializeField] private Transform _rampEndPoint;
        [SerializeField] private Transform _rampControlPoint;

        [Header("Climb")] [SerializeField] private float _climbSpeed = 34f;
        [SerializeField] private float _climbAngle = 30f;
        [SerializeField] private float _climbDuration = 1f;
        [SerializeField] private float _noseTurnDuration = 0.15f;
        [SerializeField] private AnimationCurve _climbSpeedCurve = new AnimationCurve(
            new Keyframe[]
            {
                new Keyframe(0f, 0f, 0f, 2f),
                new Keyframe(1f, 1f, 0f, 0f)
            });

        [Header("Flight")] [SerializeField] private float _gravity = 9.81f;
        [SerializeField] private float _groundHeight = 0f;
        [SerializeField] private float _maxHeight = 30f;
        [SerializeField] private float _drag = 0.001f;
        [SerializeField] private float _climbCost = 2f;
        [SerializeField] private float _noControlSpeed = 10f;
        [SerializeField] private float _fullControlSpeed = 14f;

        [Space] [SerializeField] private float _pitchRate = 40f;
        [SerializeField] private float _turnRate = 30f;
        [SerializeField] private float _maxTurnAngle = 60f;
        [SerializeField] private float _maxWingAngle = 25f;
        [SerializeField] private float _wingReactionSpeed = 6f;
        [SerializeField] private float _maxSideDistance = 12f;

        [Header("Camera")] [SerializeField] private Vector3 _cameraOffset = new Vector3(0f, 4f, -8f);
        [SerializeField] private float _cameraFollowSharpness = 8f;
        [SerializeField] private float _cameraLookSharpness = 10f;
        [SerializeField] private float _cameraLookAheadDistance = 20f;

        [Header("Input")] [SerializeField] private float _joystickRadius = 0.15f;

        [Header("Slingshot")] [SerializeField] private float _maxPullDistance = 3f;
        [SerializeField] private float _grabRadius = 0.12f;
        [SerializeField] private float _maxNoseRaiseAngle = 15f;
        [SerializeField] private float _pullReactionSpeed = 15f;
        [SerializeField] private float _minPullRadius = 0.03f;
        [SerializeField] private float _maxPullRadius = 0.15f;

        [Space] [SerializeField] private float _minLaunchSpeed = 30f;
        [SerializeField] private float _maxLaunchSpeed = 38f;

        private Vector3 _startPosition;

        private PlaneClimb _planeClimb;
        private PlaneFlight _planeFlight;
        private PlaneCameraFollow _cameraFollow;
        private PlaneInput _input;
        private PlaneSlingshot _slingshot;

        private void Awake()
        {
            _startPosition = _planeRoot.position;

            _planeClimb = new PlaneClimb(_planeRoot, _rampStartPoint, _rampControlPoint,
                _rampEndPoint, _climbSpeedCurve, _noseTurnDuration);

            _input = new PlaneInput(_joystickRadius);

            _slingshot = new PlaneSlingshot(_planeRoot, _input, _camera, _grabRadius, _minPullRadius, _maxPullRadius,
                _maxPullDistance, _maxNoseRaiseAngle, _pullReactionSpeed, _minLaunchSpeed, _maxLaunchSpeed);

            _planeFlight = new PlaneFlight(_planeRoot, _input, _gravity, _groundHeight, _pitchRate, _drag,
                _climbCost, _noControlSpeed, _fullControlSpeed, _maxHeight, _turnRate, _maxTurnAngle,
                _maxWingAngle, _wingReactionSpeed, _maxSideDistance);

            _cameraFollow = new PlaneCameraFollow(_cameraTransform, _planeRoot, _cameraOffset,
                _cameraFollowSharpness, _cameraLookSharpness, _cameraLookAheadDistance);

            _cameraFollow.Reset();
            _cameraFollow.StopFollowing();

            _slingshot.Shot += OnShot;
            _planeClimb.Finished += OnPlaneClimbFinished;
            _planeFlight.Landed += OnPlaneLanded;
        }

        private void OnDestroy()
        {
            _slingshot.Shot -= OnShot;
            _planeClimb.Finished -= OnPlaneClimbFinished;
            _planeFlight.Landed -= OnPlaneLanded;
        }

        private void Update()
        {
            _input.Tick();

            if (_input.WasResetButtonPressed)
            {
                _planeFlight.Stop();
                _planeClimb.Reset();
                _slingshot.Reset();
                _cameraFollow.Reset();
            }

            _slingshot.Tick(Time.deltaTime);
            _planeClimb.Tick(Time.deltaTime);
            _planeFlight.Tick(Time.deltaTime);
        }

        private void LateUpdate()
        {
            _cameraFollow.Tick(Time.deltaTime);
        }

        private void OnShot(float launchSpeed)
        {
            Debug.Log("Shot at " + launchSpeed + " m/s");

            _planeClimb.Start(launchSpeed);
            _cameraFollow.StartFollowing();
        }

        private void OnPlaneClimbFinished([Bridge.Ref] Vector3 velocity)
        {
            Debug.Log("Climb finished: height " + (_planeRoot.position.y - _startPosition.y) + " m, " +
                      "expected " + _planeRoot.position.y + " m");

            _planeFlight.Start(velocity);
        }

        private void OnPlaneLanded()
        {
            Debug.Log("Landed at " + (_planeRoot.position.z - _startPosition.z) + " m");
        }

#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            if (_planeRoot == null || _camera == null)
            {
                return;
            }

            Vector3 center = Application.isPlaying ? _startPosition : _planeRoot.position;

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(center, CalculateWorldRadius(center, _grabRadius));

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(center, CalculateWorldRadius(center, _minPullRadius));

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(center, CalculateWorldRadius(center, _maxPullRadius));

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

        private float CalculateWorldRadius(Vector3 center, float screenHeightShare)
        {
            float distanceToCamera = Vector3.Dot(center - _camera.transform.position, _camera.transform.forward);
            float visibleHeight = 2f * distanceToCamera * Mathf.Tan(_camera.fieldOfView * 0.5f * Mathf.Deg2Rad);
            return screenHeightShare * visibleHeight;
        }

#endif
    }
}