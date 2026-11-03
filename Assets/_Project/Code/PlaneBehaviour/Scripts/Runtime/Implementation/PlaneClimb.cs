using System;
using UnityEngine;

namespace PlaneBehaviour
{
    public class PlaneClimb
    {
        private const float MidPointDistanceRatio = 0.5f;
        private const int PathProgressStepCount = 64;

        private readonly PlaneClimbConfig _config;
        private readonly Transform _transform;
        private readonly Transform _rampStartPoint;
        private readonly Transform _rampControlPoint;
        private readonly Transform _rampEndPoint;
        
        private readonly float[] _pathProgressByTime = new float[PathProgressStepCount + 1];
        private readonly BezierCurve _pathToRamp;
        private readonly BezierCurve _pathOnRamp;

        private readonly Vector3 _startPosition;

        private Vector3 _directionAfterRamp;
        private Quaternion _rotationAtShot;
        private float _pathLength;
        private float _launchSpeed;
        private float _climbDuration;
        private float _elapsedClimbTime;
        private bool _isClimbing;


        public event Action<Vector3> Finished;

        public PlaneClimb(PlaneClimbConfig config, Transform transform, Transform rampStartPoint, 
            Transform rampControlPoint, Transform rampEndPoint)
        {
            _config = config;
            _transform = transform;
            
            _rampStartPoint = rampStartPoint;
            _rampControlPoint = rampControlPoint;
            _rampEndPoint = rampEndPoint;
            
            _startPosition = transform.position;

            _pathToRamp = new BezierCurve();
            _pathOnRamp = new BezierCurve();
            
            BakePathProgress();
        }
        
        public void Start(float launchSpeed)
        {
            Vector3 planeStartPoint = _transform.position;
            Vector3 rampStartPosition = _rampStartPoint.position;
            Vector3 rampDirection = (_rampControlPoint.position - rampStartPosition).normalized;
            float distanceToRamp = Vector3.Distance(planeStartPoint, rampStartPosition);
            float midPointDistance = distanceToRamp * MidPointDistanceRatio;
            Vector3 planeRampMidPoint = rampStartPosition - rampDirection * midPointDistance;
 
            _pathToRamp.Set(planeStartPoint, planeRampMidPoint, rampStartPosition);
            _pathOnRamp.Set(rampStartPosition, _rampControlPoint.position, _rampEndPoint.position);
            _directionAfterRamp = _pathOnRamp.CalculateDirection(_pathOnRamp.Length);
            _rotationAtShot = _transform.rotation;
            _launchSpeed = launchSpeed;
            _pathLength = _pathToRamp.Length + _pathOnRamp.Length;
            _climbDuration = _pathLength / (launchSpeed * _pathProgressByTime[PathProgressStepCount]);
            _elapsedClimbTime = 0f;
            
            _isClimbing = true;
        }
        
        public void Tick(float deltaTime)
        {
            if (_isClimbing == false)
            {
                return;
            }
 
            _elapsedClimbTime = Mathf.Min(_elapsedClimbTime + deltaTime, _climbDuration);
 
            float timeProgress = _elapsedClimbTime / _climbDuration;
            float passedDistance = _pathLength * CalculatePathProgress(timeProgress);
 
            MoveAlongPath(passedDistance);
 
            if (timeProgress >= 1f)
            {
                _isClimbing = false;
                Finished?.Invoke(_directionAfterRamp * _launchSpeed);
            }
        }

        public void Reset()
        {
            _isClimbing = false;
            _elapsedClimbTime = 0f;
            _transform.position = _startPosition;
            _transform.rotation = Quaternion.identity;
        }
        
        private void MoveAlongPath(float passedDistance)
        {
            Vector3 direction;
 
            if (passedDistance <= _pathToRamp.Length)
            {
                _transform.position = _pathToRamp.CalculatePosition(passedDistance);
                direction = _pathToRamp.CalculateDirection(passedDistance);
            }
            else
            {
                float distanceOnRamp = passedDistance - _pathToRamp.Length;
                _transform.position = _pathOnRamp.CalculatePosition(distanceOnRamp);
                direction = _pathOnRamp.CalculateDirection(distanceOnRamp);
            }
 
            float noseTurnProgress = Mathf.SmoothStep(0f, 1f, _elapsedClimbTime / _config.NoseTurnDuration);
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            
            _transform.rotation = Quaternion.Slerp(_rotationAtShot, targetRotation, noseTurnProgress);
        }
        
        private void BakePathProgress()
        {
            _pathProgressByTime[0] = 0f;
 
            for (int stepIndex = 1; stepIndex <= PathProgressStepCount; stepIndex++)
            {
                float previousSpeed = _config.ClimbSpeedCurve
                    .Evaluate((float)(stepIndex - 1) / PathProgressStepCount);
                
                float currentSpeed = _config.ClimbSpeedCurve
                    .Evaluate((float)stepIndex / PathProgressStepCount);
                
                float averageSpeed = (previousSpeed + currentSpeed) * 0.5f;
                float stepProgress = averageSpeed / PathProgressStepCount;
 
                _pathProgressByTime[stepIndex] = _pathProgressByTime[stepIndex - 1] + stepProgress;
            }
        }

        private float CalculatePathProgress(float timeProgress)
        {
            float stepPosition = timeProgress * PathProgressStepCount;
            int stepIndex = Mathf.Min((int)stepPosition, PathProgressStepCount - 1);
            float positionBetweenSteps = stepPosition - stepIndex;
            float currentStepProgress = _pathProgressByTime[stepIndex];
            float nextStepProgress = _pathProgressByTime[stepIndex + 1];
            float bakedProgress = Mathf.Lerp(currentStepProgress, nextStepProgress, positionBetweenSteps);
            return bakedProgress / _pathProgressByTime[PathProgressStepCount];
        }
    }
}
