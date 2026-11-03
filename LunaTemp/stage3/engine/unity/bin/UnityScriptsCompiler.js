if ( TRACE ) { TRACE( JSON.parse( '["NewPlaneBehaviour.BezierCurve#init","NewPlaneBehaviour.BezierCurve#init","NewPlaneBehaviour.BezierCurve#Set","NewPlaneBehaviour.BezierCurve#CalculatePosition","NewPlaneBehaviour.BezierCurve#CalculateDirection","NewPlaneBehaviour.BezierCurve#MeasureLength","NewPlaneBehaviour.BezierCurve#CalculatePoint","NewPlaneBehaviour.BezierCurve#CalculatePositionAtPoint","NewPlaneBehaviour.Plane#init","NewPlaneBehaviour.Plane#Awake","NewPlaneBehaviour.Plane#OnDestroy","NewPlaneBehaviour.Plane#Update","NewPlaneBehaviour.Plane#LateUpdate","NewPlaneBehaviour.Plane#OnShot","NewPlaneBehaviour.Plane#OnPlaneClimbFinished","NewPlaneBehaviour.Plane#OnPlaneLanded","NewPlaneBehaviour.PlaneCameraFollow#init","NewPlaneBehaviour.PlaneCameraFollow#ctor","NewPlaneBehaviour.PlaneCameraFollow#StartFollowing","NewPlaneBehaviour.PlaneCameraFollow#StopFollowing","NewPlaneBehaviour.PlaneCameraFollow#Tick","NewPlaneBehaviour.PlaneCameraFollow#Reset","NewPlaneBehaviour.PlaneCameraFollow#CalculateTargetPosition","NewPlaneBehaviour.PlaneCameraFollow#CalculateLookRotation","NewPlaneBehaviour.PlaneCameraFollow#CalculateFlightDirection","NewPlaneBehaviour.PlaneCameraFollow#CalculateProgress","NewPlaneBehaviour.PlaneClimb#init","NewPlaneBehaviour.PlaneClimb#init","NewPlaneBehaviour.PlaneClimb#ctor","NewPlaneBehaviour.PlaneClimb#Start","NewPlaneBehaviour.PlaneClimb#Tick","NewPlaneBehaviour.PlaneClimb#Reset","NewPlaneBehaviour.PlaneClimb#MoveAlongPath","NewPlaneBehaviour.PlaneClimb#BakePathProgress","NewPlaneBehaviour.PlaneClimb#CalculatePathProgress","NewPlaneBehaviour.PlaneFlight#init","NewPlaneBehaviour.PlaneFlight#ctor","NewPlaneBehaviour.PlaneFlight#Start","NewPlaneBehaviour.PlaneFlight#Stop","NewPlaneBehaviour.PlaneFlight#Tick","NewPlaneBehaviour.PlaneFlight#SlowDownByAir","NewPlaneBehaviour.PlaneFlight#SlowDownOnClimb","NewPlaneBehaviour.PlaneFlight#TurnNoseWithStick","NewPlaneBehaviour.PlaneFlight#TurnSideWithStick","NewPlaneBehaviour.PlaneFlight#UpdateWingAngle","NewPlaneBehaviour.PlaneFlight#LimitSideDistance","NewPlaneBehaviour.PlaneFlight#SetSpeed","NewPlaneBehaviour.PlaneInput#init","NewPlaneBehaviour.PlaneInput#ctor","NewPlaneBehaviour.PlaneInput#Tick","NewPlaneBehaviour.PlaneInput#TickStick","NewPlaneBehaviour.PlaneSlingshot#init","NewPlaneBehaviour.PlaneSlingshot#ctor","NewPlaneBehaviour.PlaneSlingshot#Tick","NewPlaneBehaviour.PlaneSlingshot#Reset","NewPlaneBehaviour.PlaneSlingshot#Release","NewPlaneBehaviour.PlaneSlingshot#UpdatePull","NewPlaneBehaviour.PlaneSlingshot#CanGrab","NewPlaneBehaviour.PlaneSlingshot#FollowPull","NewPlaneBehaviour.PlaneSlingshot#CalculatePlaneScreenPosition"]' ) ); }
/**
 * @version 1.0.9803.37382
 * @copyright anton
 * @compiler Bridge.NET 17.9.42-luna
 */
Bridge.assembly("UnityScriptsCompiler", function ($asm, globals) {
    "use strict";

    /*IAmAnEmptyScriptJustToMakeCodelessProjectsCompileProperty start.*/
    Bridge.define("IAmAnEmptyScriptJustToMakeCodelessProjectsCompileProperty", {
        inherits: [UnityEngine.MonoBehaviour]
    });
    /*IAmAnEmptyScriptJustToMakeCodelessProjectsCompileProperty end.*/

    /*NewPlaneBehaviour.BezierCurve start.*/
    Bridge.define("NewPlaneBehaviour.BezierCurve", {
        statics: {
            fields: {
                SampleCount: 0
            },
            ctors: {
                init: function () {
if ( TRACE ) { TRACE( "NewPlaneBehaviour.BezierCurve#init", this ); }

                    this.SampleCount = 16;
                }
            }
        },
        fields: {
            _sampleLengths: null,
            _start: null,
            _control: null,
            _end: null,
            Length: 0
        },
        ctors: {
            init: function () {
if ( TRACE ) { TRACE( "NewPlaneBehaviour.BezierCurve#init", this ); }

                this._start = new UnityEngine.Vector3();
                this._control = new UnityEngine.Vector3();
                this._end = new UnityEngine.Vector3();
                this._sampleLengths = System.Array.init(17, 0, System.Single);
            }
        },
        methods: {
            /*NewPlaneBehaviour.BezierCurve.Set start.*/
            Set: function (start, control, end) {
if ( TRACE ) { TRACE( "NewPlaneBehaviour.BezierCurve#Set", this ); }

                this._start = start.$clone();
                this._control = control.$clone();
                this._end = end.$clone();

                this.MeasureLength();
            },
            /*NewPlaneBehaviour.BezierCurve.Set end.*/

            /*NewPlaneBehaviour.BezierCurve.CalculatePosition start.*/
            CalculatePosition: function (distance) {
if ( TRACE ) { TRACE( "NewPlaneBehaviour.BezierCurve#CalculatePosition", this ); }

                return this.CalculatePositionAtPoint(this.CalculatePoint(distance));
            },
            /*NewPlaneBehaviour.BezierCurve.CalculatePosition end.*/

            /*NewPlaneBehaviour.BezierCurve.CalculateDirection start.*/
            CalculateDirection: function (distance) {
if ( TRACE ) { TRACE( "NewPlaneBehaviour.BezierCurve#CalculateDirection", this ); }

                var point = this.CalculatePoint(distance);
                var direction = (this._control.$clone().sub( this._start )).clone().scale( (1.0 - point) ).add( (this._end.$clone().sub( this._control )).clone().scale( point ) );

                if (direction.lengthSq() <= 0.0) {
                    return (this._end.$clone().sub( this._start )).clone().normalize().$clone();
                }

                return direction.clone().normalize().$clone();
            },
            /*NewPlaneBehaviour.BezierCurve.CalculateDirection end.*/

            /*NewPlaneBehaviour.BezierCurve.MeasureLength start.*/
            MeasureLength: function () {
if ( TRACE ) { TRACE( "NewPlaneBehaviour.BezierCurve#MeasureLength", this ); }

                var previousPosition = this._start.$clone();
                this._sampleLengths[0] = 0.0;

                for (var sampleIndex = 1; sampleIndex <= NewPlaneBehaviour.BezierCurve.SampleCount; sampleIndex = (sampleIndex + 1) | 0) {
                    var position = this.CalculatePositionAtPoint(sampleIndex / NewPlaneBehaviour.BezierCurve.SampleCount);
                    this._sampleLengths[sampleIndex] = this._sampleLengths[((sampleIndex - 1) | 0)] + pc.Vec3.distance( previousPosition, position );
                    previousPosition = position.$clone();
                }

                this.Length = this._sampleLengths[NewPlaneBehaviour.BezierCurve.SampleCount];
            },
            /*NewPlaneBehaviour.BezierCurve.MeasureLength end.*/

            /*NewPlaneBehaviour.BezierCurve.CalculatePoint start.*/
            CalculatePoint: function (distance) {
if ( TRACE ) { TRACE( "NewPlaneBehaviour.BezierCurve#CalculatePoint", this ); }

                for (var sampleIndex = 1; sampleIndex <= NewPlaneBehaviour.BezierCurve.SampleCount; sampleIndex = (sampleIndex + 1) | 0) {
                    if (distance > this._sampleLengths[sampleIndex]) {
                        continue;
                    }

                    var sampleLength = this._sampleLengths[sampleIndex] - this._sampleLengths[((sampleIndex - 1) | 0)];
                    var sampleProgress = sampleLength <= 0.0 ? 0.0 : (distance - this._sampleLengths[((sampleIndex - 1) | 0)]) / sampleLength;
                    return (((sampleIndex - 1) | 0) + sampleProgress) / NewPlaneBehaviour.BezierCurve.SampleCount;
                }

                return 1.0;
            },
            /*NewPlaneBehaviour.BezierCurve.CalculatePoint end.*/

            /*NewPlaneBehaviour.BezierCurve.CalculatePositionAtPoint start.*/
            CalculatePositionAtPoint: function (point) {
if ( TRACE ) { TRACE( "NewPlaneBehaviour.BezierCurve#CalculatePositionAtPoint", this ); }

                var remaining = 1.0 - point;
                return this._start.clone().scale( remaining * remaining ).add( this._control.clone().scale( 2.0 * remaining * point ) ).add( this._end.clone().scale( point * point ) );
            },
            /*NewPlaneBehaviour.BezierCurve.CalculatePositionAtPoint end.*/


        }
    });
    /*NewPlaneBehaviour.BezierCurve end.*/

    /*NewPlaneBehaviour.Plane start.*/
    Bridge.define("NewPlaneBehaviour.Plane", {
        inherits: [UnityEngine.MonoBehaviour],
        fields: {
            _planeRoot: null,
            _cameraTransform: null,
            _camera: null,
            _rampStartPoint: null,
            _rampEndPoint: null,
            _rampControlPoint: null,
            _climbSpeed: 0,
            _climbAngle: 0,
            _climbDuration: 0,
            _noseTurnDuration: 0,
            _climbSpeedCurve: null,
            _gravity: 0,
            _groundHeight: 0,
            _maxHeight: 0,
            _drag: 0,
            _climbCost: 0,
            _noControlSpeed: 0,
            _fullControlSpeed: 0,
            _pitchRate: 0,
            _turnRate: 0,
            _maxTurnAngle: 0,
            _maxWingAngle: 0,
            _wingReactionSpeed: 0,
            _maxSideDistance: 0,
            _cameraOffset: null,
            _cameraFollowSharpness: 0,
            _cameraLookSharpness: 0,
            _cameraLookAheadDistance: 0,
            _joystickRadius: 0,
            _maxPullDistance: 0,
            _grabRadius: 0,
            _maxNoseRaiseAngle: 0,
            _pullReactionSpeed: 0,
            _minPullRadius: 0,
            _maxPullRadius: 0,
            _minLaunchSpeed: 0,
            _maxLaunchSpeed: 0,
            _startPosition: null,
            _planeClimb: null,
            _planeFlight: null,
            _cameraFollow: null,
            _input: null,
            _slingshot: null
        },
        ctors: {
            init: function () {
if ( TRACE ) { TRACE( "NewPlaneBehaviour.Plane#init", this ); }

                this._cameraOffset = new UnityEngine.Vector3();
                this._startPosition = new UnityEngine.Vector3();
                this._climbSpeed = 34.0;
                this._climbAngle = 30.0;
                this._climbDuration = 1.0;
                this._noseTurnDuration = 0.15;
                this._climbSpeedCurve = new pc.AnimationCurve({keyframes: System.Array.init([new pc.Keyframe(0.0, 0.0, 0.0, 2.0), new pc.Keyframe(1.0, 1.0, 0.0, 0.0)], pc.Keyframe)});
                this._gravity = 9.81;
                this._groundHeight = 0.0;
                this._maxHeight = 30.0;
                this._drag = 0.001;
                this._climbCost = 2.0;
                this._noControlSpeed = 10.0;
                this._fullControlSpeed = 14.0;
                this._pitchRate = 40.0;
                this._turnRate = 30.0;
                this._maxTurnAngle = 60.0;
                this._maxWingAngle = 25.0;
                this._wingReactionSpeed = 6.0;
                this._maxSideDistance = 12.0;
                this._cameraOffset = new pc.Vec3( 0.0, 4.0, -8.0 );
                this._cameraFollowSharpness = 8.0;
                this._cameraLookSharpness = 10.0;
                this._cameraLookAheadDistance = 20.0;
                this._joystickRadius = 0.15;
                this._maxPullDistance = 3.0;
                this._grabRadius = 0.12;
                this._maxNoseRaiseAngle = 15.0;
                this._pullReactionSpeed = 15.0;
                this._minPullRadius = 0.03;
                this._maxPullRadius = 0.15;
                this._minLaunchSpeed = 30.0;
                this._maxLaunchSpeed = 38.0;
            }
        },
        methods: {
            /*NewPlaneBehaviour.Plane.Awake start.*/
            Awake: function () {
if ( TRACE ) { TRACE( "NewPlaneBehaviour.Plane#Awake", this ); }

                this._startPosition = this._planeRoot.position.$clone();

                this._planeClimb = new NewPlaneBehaviour.PlaneClimb(this._planeRoot, this._rampStartPoint, this._rampControlPoint, this._rampEndPoint, this._climbSpeedCurve, this._noseTurnDuration);

                this._input = new NewPlaneBehaviour.PlaneInput(this._joystickRadius);

                this._slingshot = new NewPlaneBehaviour.PlaneSlingshot(this._planeRoot, this._input, this._camera, this._grabRadius, this._minPullRadius, this._maxPullRadius, this._maxPullDistance, this._maxNoseRaiseAngle, this._pullReactionSpeed, this._minLaunchSpeed, this._maxLaunchSpeed);

                this._planeFlight = new NewPlaneBehaviour.PlaneFlight(this._planeRoot, this._input, this._gravity, this._groundHeight, this._pitchRate, this._drag, this._climbCost, this._noControlSpeed, this._fullControlSpeed, this._maxHeight, this._turnRate, this._maxTurnAngle, this._maxWingAngle, this._wingReactionSpeed, this._maxSideDistance);

                this._cameraFollow = new NewPlaneBehaviour.PlaneCameraFollow(this._cameraTransform, this._planeRoot, this._cameraOffset.$clone(), this._cameraFollowSharpness, this._cameraLookSharpness, this._cameraLookAheadDistance);

                this._cameraFollow.Reset();
                this._cameraFollow.StopFollowing();

                this._slingshot.addShot(Bridge.fn.cacheBind(this, this.OnShot));
                this._planeClimb.addFinished(Bridge.fn.cacheBind(this, this.OnPlaneClimbFinished));
                this._planeFlight.addLanded(Bridge.fn.cacheBind(this, this.OnPlaneLanded));
            },
            /*NewPlaneBehaviour.Plane.Awake end.*/

            /*NewPlaneBehaviour.Plane.OnDestroy start.*/
            OnDestroy: function () {
if ( TRACE ) { TRACE( "NewPlaneBehaviour.Plane#OnDestroy", this ); }

                this._slingshot.removeShot(Bridge.fn.cacheBind(this, this.OnShot));
                this._planeClimb.removeFinished(Bridge.fn.cacheBind(this, this.OnPlaneClimbFinished));
                this._planeFlight.removeLanded(Bridge.fn.cacheBind(this, this.OnPlaneLanded));
            },
            /*NewPlaneBehaviour.Plane.OnDestroy end.*/

            /*NewPlaneBehaviour.Plane.Update start.*/
            Update: function () {
if ( TRACE ) { TRACE( "NewPlaneBehaviour.Plane#Update", this ); }

                this._input.Tick();

                if (this._input.WasResetButtonPressed) {
                    this._planeFlight.Stop();
                    this._planeClimb.Reset();
                    this._slingshot.Reset();
                    this._cameraFollow.Reset();
                }

                this._slingshot.Tick(UnityEngine.Time.deltaTime);
                this._planeClimb.Tick(UnityEngine.Time.deltaTime);
                this._planeFlight.Tick(UnityEngine.Time.deltaTime);
            },
            /*NewPlaneBehaviour.Plane.Update end.*/

            /*NewPlaneBehaviour.Plane.LateUpdate start.*/
            LateUpdate: function () {
if ( TRACE ) { TRACE( "NewPlaneBehaviour.Plane#LateUpdate", this ); }

                this._cameraFollow.Tick(UnityEngine.Time.deltaTime);
            },
            /*NewPlaneBehaviour.Plane.LateUpdate end.*/

            /*NewPlaneBehaviour.Plane.OnShot start.*/
            OnShot: function (launchSpeed) {
if ( TRACE ) { TRACE( "NewPlaneBehaviour.Plane#OnShot", this ); }

                UnityEngine.Debug.Log$1("Shot at " + System.Single.format(launchSpeed) + " m/s");

                this._planeClimb.Start(launchSpeed);
                this._cameraFollow.StartFollowing();
            },
            /*NewPlaneBehaviour.Plane.OnShot end.*/

            /*NewPlaneBehaviour.Plane.OnPlaneClimbFinished start.*/
            OnPlaneClimbFinished: function (velocity) {
if ( TRACE ) { TRACE( "NewPlaneBehaviour.Plane#OnPlaneClimbFinished", this ); }

                UnityEngine.Debug.Log$1("Climb finished: height " + System.Single.format((this._planeRoot.position.y - this._startPosition.y)) + " m, " + "expected " + System.Single.format(this._planeRoot.position.y) + " m");

                this._planeFlight.Start(velocity);
            },
            /*NewPlaneBehaviour.Plane.OnPlaneClimbFinished end.*/

            /*NewPlaneBehaviour.Plane.OnPlaneLanded start.*/
            OnPlaneLanded: function () {
if ( TRACE ) { TRACE( "NewPlaneBehaviour.Plane#OnPlaneLanded", this ); }

                UnityEngine.Debug.Log$1("Landed at " + System.Single.format((this._planeRoot.position.z - this._startPosition.z)) + " m");
            },
            /*NewPlaneBehaviour.Plane.OnPlaneLanded end.*/


        }
    });
    /*NewPlaneBehaviour.Plane end.*/

    /*NewPlaneBehaviour.PlaneCameraFollow start.*/
    Bridge.define("NewPlaneBehaviour.PlaneCameraFollow", {
        fields: {
            _cameraTransform: null,
            _target: null,
            _offset: null,
            _followSharpness: 0,
            _lookSharpness: 0,
            _lookAheadDistance: 0,
            IsFollowing: false
        },
        ctors: {
            init: function () {
if ( TRACE ) { TRACE( "NewPlaneBehaviour.PlaneCameraFollow#init", this ); }

                this._offset = new UnityEngine.Vector3();
            },
            ctor: function (cameraTransform, target, offset, followSharpness, lookSharpness, lookAheadDistance) {
if ( TRACE ) { TRACE( "NewPlaneBehaviour.PlaneCameraFollow#ctor", this ); }

                this.$initialize();
                this._cameraTransform = cameraTransform;
                this._target = target;
                this._offset = offset.$clone();
                this._followSharpness = followSharpness;
                this._lookSharpness = lookSharpness;
                this._lookAheadDistance = lookAheadDistance;
            }
        },
        methods: {
            /*NewPlaneBehaviour.PlaneCameraFollow.StartFollowing start.*/
            StartFollowing: function () {
if ( TRACE ) { TRACE( "NewPlaneBehaviour.PlaneCameraFollow#StartFollowing", this ); }

                this.IsFollowing = true;
            },
            /*NewPlaneBehaviour.PlaneCameraFollow.StartFollowing end.*/

            /*NewPlaneBehaviour.PlaneCameraFollow.StopFollowing start.*/
            StopFollowing: function () {
if ( TRACE ) { TRACE( "NewPlaneBehaviour.PlaneCameraFollow#StopFollowing", this ); }

                this.IsFollowing = false;
            },
            /*NewPlaneBehaviour.PlaneCameraFollow.StopFollowing end.*/

            /*NewPlaneBehaviour.PlaneCameraFollow.Tick start.*/
            Tick: function (deltaTime) {
if ( TRACE ) { TRACE( "NewPlaneBehaviour.PlaneCameraFollow#Tick", this ); }

                if (this.IsFollowing === false) {
                    return;
                }

                var targetPosition = this.CalculateTargetPosition();
                var moveProgress = this.CalculateProgress(this._followSharpness, deltaTime);
                this._cameraTransform.position = new pc.Vec3().lerp( this._cameraTransform.position, targetPosition, moveProgress );

                var turnProgress = this.CalculateProgress(this._lookSharpness, deltaTime);
                this._cameraTransform.rotation = new pc.Quat().slerpUnclamped( this._cameraTransform.rotation, this.CalculateLookRotation(), pc.math.clamp( turnProgress, 0, 1 ) );

            },
            /*NewPlaneBehaviour.PlaneCameraFollow.Tick end.*/

            /*NewPlaneBehaviour.PlaneCameraFollow.Reset start.*/
            Reset: function () {
if ( TRACE ) { TRACE( "NewPlaneBehaviour.PlaneCameraFollow#Reset", this ); }

                this._cameraTransform.position = this._target.position.$clone().add( this._offset );
                this._cameraTransform.rotation = this.CalculateLookRotation();
            },
            /*NewPlaneBehaviour.PlaneCameraFollow.Reset end.*/

            /*NewPlaneBehaviour.PlaneCameraFollow.CalculateTargetPosition start.*/
            CalculateTargetPosition: function () {
if ( TRACE ) { TRACE( "NewPlaneBehaviour.PlaneCameraFollow#CalculateTargetPosition", this ); }

                return this._target.position.$clone().add( this.CalculateFlightDirection().transformVector( this._offset ) );
            },
            /*NewPlaneBehaviour.PlaneCameraFollow.CalculateTargetPosition end.*/

            /*NewPlaneBehaviour.PlaneCameraFollow.CalculateLookRotation start.*/
            CalculateLookRotation: function () {
if ( TRACE ) { TRACE( "NewPlaneBehaviour.PlaneCameraFollow#CalculateLookRotation", this ); }

                var lookPoint = this._target.position.$clone().add( new pc.Vec3( 0, 0, 1 ).clone().scale( this._lookAheadDistance ) );
                return new pc.Quat().lookRotation( lookPoint.$clone().sub( this._cameraTransform.position ), pc.Vec3.UP );
            },
            /*NewPlaneBehaviour.PlaneCameraFollow.CalculateLookRotation end.*/

            /*NewPlaneBehaviour.PlaneCameraFollow.CalculateFlightDirection start.*/
            CalculateFlightDirection: function () {
if ( TRACE ) { TRACE( "NewPlaneBehaviour.PlaneCameraFollow#CalculateFlightDirection", this ); }

                var forward = this._target.forward.$clone();
                var flatForward = new pc.Vec3( forward.x, 0.0, forward.z );

                if (flatForward.lengthSq() <= 0.0) {
                    return pc.Quat.IDENTITY.clone();
                }

                return new pc.Quat().lookRotation( flatForward, pc.Vec3.UP );
            },
            /*NewPlaneBehaviour.PlaneCameraFollow.CalculateFlightDirection end.*/

            /*NewPlaneBehaviour.PlaneCameraFollow.CalculateProgress start.*/
            CalculateProgress: function (sharpness, deltaTime) {
if ( TRACE ) { TRACE( "NewPlaneBehaviour.PlaneCameraFollow#CalculateProgress", this ); }

                return 1.0 - UnityEngine.Mathf.Exp(-sharpness * deltaTime);
            },
            /*NewPlaneBehaviour.PlaneCameraFollow.CalculateProgress end.*/


        }
    });
    /*NewPlaneBehaviour.PlaneCameraFollow end.*/

    /*NewPlaneBehaviour.PlaneClimb start.*/
    Bridge.define("NewPlaneBehaviour.PlaneClimb", {
        statics: {
            fields: {
                MidPointDistanceRatio: 0,
                PathProgressStepCount: 0
            },
            ctors: {
                init: function () {
if ( TRACE ) { TRACE( "NewPlaneBehaviour.PlaneClimb#init", this ); }

                    this.MidPointDistanceRatio = 0.5;
                    this.PathProgressStepCount = 64;
                }
            }
        },
        fields: {
            _transform: null,
            _rampStartPoint: null,
            _rampControlPoint: null,
            _rampEndPoint: null,
            _speedCurve: null,
            _noseTurnDuration: 0,
            _pathProgressByTime: null,
            _pathToRamp: null,
            _pathOnRamp: null,
            _startPosition: null,
            _directionAfterRamp: null,
            _rotationAtShot: null,
            _pathLength: 0,
            _launchSpeed: 0,
            _climbDuration: 0,
            _elapsedClimbTime: 0,
            _isClimbing: false
        },
        events: {
            Finished: null
        },
        ctors: {
            init: function () {
if ( TRACE ) { TRACE( "NewPlaneBehaviour.PlaneClimb#init", this ); }

                this._startPosition = new UnityEngine.Vector3();
                this._directionAfterRamp = new UnityEngine.Vector3();
                this._rotationAtShot = new UnityEngine.Quaternion();
                this._pathProgressByTime = System.Array.init(65, 0, System.Single);
            },
            ctor: function (transform, rampStartPoint, rampControlPoint, rampEndPoint, speedCurve, noseTurnDuration) {
if ( TRACE ) { TRACE( "NewPlaneBehaviour.PlaneClimb#ctor", this ); }

                this.$initialize();
                this._transform = transform;

                this._rampStartPoint = rampStartPoint;
                this._rampControlPoint = rampControlPoint;
                this._rampEndPoint = rampEndPoint;
                this._speedCurve = speedCurve;
                this._noseTurnDuration = noseTurnDuration;

                this._startPosition = transform.position.$clone();

                this._pathToRamp = new NewPlaneBehaviour.BezierCurve();
                this._pathOnRamp = new NewPlaneBehaviour.BezierCurve();

                this.BakePathProgress();
            }
        },
        methods: {
            /*NewPlaneBehaviour.PlaneClimb.Start start.*/
            Start: function (launchSpeed) {
if ( TRACE ) { TRACE( "NewPlaneBehaviour.PlaneClimb#Start", this ); }

                var planeStartPoint = this._transform.position.$clone();
                var rampStartPosition = this._rampStartPoint.position.$clone();
                var rampDirection = (this._rampControlPoint.position.$clone().sub( rampStartPosition )).clone().normalize().$clone();
                var distanceToRamp = pc.Vec3.distance( planeStartPoint, rampStartPosition );
                var midPointDistance = distanceToRamp * NewPlaneBehaviour.PlaneClimb.MidPointDistanceRatio;
                var planeRampMidPoint = rampStartPosition.$clone().sub( rampDirection.$clone().clone().scale( midPointDistance ) );

                this._pathToRamp.Set(planeStartPoint, planeRampMidPoint, rampStartPosition);
                this._pathOnRamp.Set(rampStartPosition, this._rampControlPoint.position, this._rampEndPoint.position);
                this._directionAfterRamp = this._pathOnRamp.CalculateDirection(this._pathOnRamp.Length);
                this._rotationAtShot = this._transform.rotation.$clone();
                this._launchSpeed = launchSpeed;
                this._pathLength = this._pathToRamp.Length + this._pathOnRamp.Length;
                this._climbDuration = this._pathLength / (launchSpeed * this._pathProgressByTime[NewPlaneBehaviour.PlaneClimb.PathProgressStepCount]);
                this._elapsedClimbTime = 0.0;

                this._isClimbing = true;
            },
            /*NewPlaneBehaviour.PlaneClimb.Start end.*/

            /*NewPlaneBehaviour.PlaneClimb.Tick start.*/
            Tick: function (deltaTime) {
if ( TRACE ) { TRACE( "NewPlaneBehaviour.PlaneClimb#Tick", this ); }

                if (this._isClimbing === false) {
                    return;
                }

                this._elapsedClimbTime = UnityEngine.Mathf.Min(this._elapsedClimbTime + deltaTime, this._climbDuration);

                var timeProgress = this._elapsedClimbTime / this._climbDuration;
                var passedDistance = this._pathLength * this.CalculatePathProgress(timeProgress);

                this.MoveAlongPath(passedDistance);

                if (timeProgress >= 1.0) {
                    this._isClimbing = false;
                    !Bridge.staticEquals(this.Finished, null) ? this.Finished(this._directionAfterRamp.$clone().clone().scale( this._launchSpeed )) : null;
                }
            },
            /*NewPlaneBehaviour.PlaneClimb.Tick end.*/

            /*NewPlaneBehaviour.PlaneClimb.Reset start.*/
            Reset: function () {
if ( TRACE ) { TRACE( "NewPlaneBehaviour.PlaneClimb#Reset", this ); }

                this._isClimbing = false;
                this._elapsedClimbTime = 0.0;
                this._transform.position = this._startPosition.$clone();
                this._transform.rotation = pc.Quat.IDENTITY.clone();
            },
            /*NewPlaneBehaviour.PlaneClimb.Reset end.*/

            /*NewPlaneBehaviour.PlaneClimb.MoveAlongPath start.*/
            MoveAlongPath: function (passedDistance) {
if ( TRACE ) { TRACE( "NewPlaneBehaviour.PlaneClimb#MoveAlongPath", this ); }

                var direction = new UnityEngine.Vector3();

                if (passedDistance <= this._pathToRamp.Length) {
                    this._transform.position = this._pathToRamp.CalculatePosition(passedDistance);
                    direction = this._pathToRamp.CalculateDirection(passedDistance);
                } else {
                    var distanceOnRamp = passedDistance - this._pathToRamp.Length;
                    this._transform.position = this._pathOnRamp.CalculatePosition(distanceOnRamp);
                    direction = this._pathOnRamp.CalculateDirection(distanceOnRamp);
                }

                var noseTurnProgress = pc.math.smoothstep(0.0, 1.0, this._elapsedClimbTime / this._noseTurnDuration);
                this._transform.rotation = new pc.Quat().slerpUnclamped( this._rotationAtShot, new pc.Quat().lookRotation( direction, pc.Vec3.UP ), pc.math.clamp( noseTurnProgress, 0, 1 ) );
            },
            /*NewPlaneBehaviour.PlaneClimb.MoveAlongPath end.*/

            /*NewPlaneBehaviour.PlaneClimb.BakePathProgress start.*/
            BakePathProgress: function () {
if ( TRACE ) { TRACE( "NewPlaneBehaviour.PlaneClimb#BakePathProgress", this ); }

                this._pathProgressByTime[0] = 0.0;

                for (var stepIndex = 1; stepIndex <= NewPlaneBehaviour.PlaneClimb.PathProgressStepCount; stepIndex = (stepIndex + 1) | 0) {
                    var previousSpeed = this._speedCurve.value((((stepIndex - 1) | 0)) / NewPlaneBehaviour.PlaneClimb.PathProgressStepCount);
                    var currentSpeed = this._speedCurve.value(stepIndex / NewPlaneBehaviour.PlaneClimb.PathProgressStepCount);

                    var averageSpeed = (previousSpeed + currentSpeed) * 0.5;
                    var stepProgress = averageSpeed / NewPlaneBehaviour.PlaneClimb.PathProgressStepCount;

                    this._pathProgressByTime[stepIndex] = this._pathProgressByTime[((stepIndex - 1) | 0)] + stepProgress;
                }
            },
            /*NewPlaneBehaviour.PlaneClimb.BakePathProgress end.*/

            /*NewPlaneBehaviour.PlaneClimb.CalculatePathProgress start.*/
            CalculatePathProgress: function (timeProgress) {
if ( TRACE ) { TRACE( "NewPlaneBehaviour.PlaneClimb#CalculatePathProgress", this ); }

                var stepPosition = timeProgress * NewPlaneBehaviour.PlaneClimb.PathProgressStepCount;
                var stepIndex = UnityEngine.Mathf.Min(Bridge.Int.clip32(stepPosition), 63);
                var positionBetweenSteps = stepPosition - stepIndex;
                var currentStepProgress = this._pathProgressByTime[stepIndex];
                var nextStepProgress = this._pathProgressByTime[((stepIndex + 1) | 0)];
                var bakedProgress = pc.math.lerp(currentStepProgress, nextStepProgress, positionBetweenSteps);
                return bakedProgress / this._pathProgressByTime[NewPlaneBehaviour.PlaneClimb.PathProgressStepCount];
            },
            /*NewPlaneBehaviour.PlaneClimb.CalculatePathProgress end.*/


        }
    });
    /*NewPlaneBehaviour.PlaneClimb end.*/

    /*NewPlaneBehaviour.PlaneFlight start.*/
    Bridge.define("NewPlaneBehaviour.PlaneFlight", {
        fields: {
            _transform: null,
            _input: null,
            _gravity: 0,
            _groundHeight: 0,
            _pitchRate: 0,
            _drag: 0,
            _climbCost: 0,
            _noControlSpeed: 0,
            _fullControlSpeed: 0,
            _maxHeight: 0,
            _turnRate: 0,
            _maxTurnAngle: 0,
            _maxWingAngle: 0,
            _wingReactionSpeed: 0,
            _maxSideDistance: 0,
            _startSidePosition: 0,
            _velocity: null,
            _isRunning: false,
            _wingAngle: 0
        },
        events: {
            Landed: null
        },
        ctors: {
            init: function () {
if ( TRACE ) { TRACE( "NewPlaneBehaviour.PlaneFlight#init", this ); }

                this._velocity = new UnityEngine.Vector3();
            },
            ctor: function (transform, input, gravity, groundHeight, pitchRate, drag, climbCost, noControlSpeed, fullControlSpeed, maxHeight, turnRate, maxTurnAngle, maxWingAngle, wingReactionSpeed, maxSideDistance) {
if ( TRACE ) { TRACE( "NewPlaneBehaviour.PlaneFlight#ctor", this ); }

                this.$initialize();
                this._transform = transform;
                this._input = input;

                this._gravity = gravity;
                this._groundHeight = groundHeight;
                this._pitchRate = pitchRate;
                this._drag = drag;
                this._climbCost = climbCost;
                this._noControlSpeed = noControlSpeed;
                this._fullControlSpeed = fullControlSpeed;
                this._maxHeight = maxHeight;
                this._turnRate = turnRate;
                this._maxTurnAngle = maxTurnAngle;
                this._maxWingAngle = maxWingAngle;
                this._wingReactionSpeed = wingReactionSpeed;
                this._maxSideDistance = maxSideDistance;

                this._startSidePosition = transform.position.x;
            }
        },
        methods: {
            /*NewPlaneBehaviour.PlaneFlight.Start start.*/
            Start: function (velocity) {
if ( TRACE ) { TRACE( "NewPlaneBehaviour.PlaneFlight#Start", this ); }

                this._velocity = velocity.$clone();
                this._wingAngle = 0.0;
                this._isRunning = true;
            },
            /*NewPlaneBehaviour.PlaneFlight.Start end.*/

            /*NewPlaneBehaviour.PlaneFlight.Stop start.*/
            Stop: function () {
if ( TRACE ) { TRACE( "NewPlaneBehaviour.PlaneFlight#Stop", this ); }

                this._isRunning = false;
            },
            /*NewPlaneBehaviour.PlaneFlight.Stop end.*/

            /*NewPlaneBehaviour.PlaneFlight.Tick start.*/
            Tick: function (deltaTime) {
if ( TRACE ) { TRACE( "NewPlaneBehaviour.PlaneFlight#Tick", this ); }

                if (this._isRunning === false) {
                    return;
                }

                this._velocity.y -= this._gravity * deltaTime;

                this.SlowDownByAir(deltaTime);
                this.SlowDownOnClimb(deltaTime);
                this.TurnNoseWithStick(deltaTime);
                this.TurnSideWithStick(deltaTime);
                this.UpdateWingAngle(deltaTime);

                var position = { v : this._transform.position.$clone().add( this._velocity.$clone().clone().scale( deltaTime ) ) };

                if (position.v.y > this._maxHeight) {
                    position.v.y = this._maxHeight;
                    this._velocity.y = UnityEngine.Mathf.Min(this._velocity.y, 0.0);
                }

                this.LimitSideDistance(position);

                this._transform.rotation = new pc.Quat().lookRotation( this._velocity, pc.Vec3.UP ).clone().mul( new pc.Quat().setFromEulerAngles_Unity( 0.0, 0.0, -this._wingAngle ) );

                if (position.v.y <= this._groundHeight) {
                    position.v.y = this._groundHeight;

                    this._isRunning = false;
                    this._transform.position = position.v.$clone();

                    !Bridge.staticEquals(this.Landed, null) ? this.Landed() : null;

                    return;
                }

                this._transform.position = position.v.$clone();
            },
            /*NewPlaneBehaviour.PlaneFlight.Tick end.*/

            /*NewPlaneBehaviour.PlaneFlight.SlowDownByAir start.*/
            SlowDownByAir: function (deltaTime) {
if ( TRACE ) { TRACE( "NewPlaneBehaviour.PlaneFlight#SlowDownByAir", this ); }

                var speed = this._velocity.length();
                this.SetSpeed(speed - this._drag * speed * speed * deltaTime);
            },
            /*NewPlaneBehaviour.PlaneFlight.SlowDownByAir end.*/

            /*NewPlaneBehaviour.PlaneFlight.SlowDownOnClimb start.*/
            SlowDownOnClimb: function (deltaTime) {
if ( TRACE ) { TRACE( "NewPlaneBehaviour.PlaneFlight#SlowDownOnClimb", this ); }

                var speed = this._velocity.length();

                if (this._velocity.y <= 0.0) {
                    return;
                }

                var climbSteepness = this._velocity.y / speed;
                this.SetSpeed(speed - (this._climbCost - 1.0) * this._gravity * climbSteepness * deltaTime);
            },
            /*NewPlaneBehaviour.PlaneFlight.SlowDownOnClimb end.*/

            /*NewPlaneBehaviour.PlaneFlight.TurnNoseWithStick start.*/
            TurnNoseWithStick: function (deltaTime) {
if ( TRACE ) { TRACE( "NewPlaneBehaviour.PlaneFlight#TurnNoseWithStick", this ); }

                var stickStrength = pc.math.inverseLerp(this._noControlSpeed, this._fullControlSpeed, this._velocity.length());
                var turnAngle = -this._input.PitchInput * this._pitchRate * stickStrength * deltaTime;

                this._velocity = new pc.Quat().setFromAxisAngle( pc.Vec3.RIGHT.clone(), turnAngle ).transformVector( this._velocity );
            },
            /*NewPlaneBehaviour.PlaneFlight.TurnNoseWithStick end.*/

            /*NewPlaneBehaviour.PlaneFlight.TurnSideWithStick start.*/
            TurnSideWithStick: function (deltaTime) {
if ( TRACE ) { TRACE( "NewPlaneBehaviour.PlaneFlight#TurnSideWithStick", this ); }

                var angleFromForward = Math.atan2(this._velocity.x, this._velocity.z) * UnityEngine.Mathf.Rad2Deg;
                var turnAngle = this._input.TurnInput * this._turnRate * deltaTime;
                var newAngleFromForward = Math.max(-this._maxTurnAngle, Math.min(angleFromForward + turnAngle, this._maxTurnAngle));

                this._velocity = new pc.Quat().setFromAxisAngle( pc.Vec3.UP.clone(), newAngleFromForward - angleFromForward ).transformVector( this._velocity );
            },
            /*NewPlaneBehaviour.PlaneFlight.TurnSideWithStick end.*/

            /*NewPlaneBehaviour.PlaneFlight.UpdateWingAngle start.*/
            UpdateWingAngle: function (deltaTime) {
if ( TRACE ) { TRACE( "NewPlaneBehaviour.PlaneFlight#UpdateWingAngle", this ); }

                var targetWingAngle = this._input.TurnInput * this._maxWingAngle;
                this._wingAngle = pc.math.lerp(this._wingAngle, targetWingAngle, Math.max(0, Math.min(1, this._wingReactionSpeed * deltaTime)));
            },
            /*NewPlaneBehaviour.PlaneFlight.UpdateWingAngle end.*/

            /*NewPlaneBehaviour.PlaneFlight.LimitSideDistance start.*/
            LimitSideDistance: function (position) {
if ( TRACE ) { TRACE( "NewPlaneBehaviour.PlaneFlight#LimitSideDistance", this ); }

                var distanceFromCenter = position.v.x - this._startSidePosition;

                if (Math.abs(distanceFromCenter) <= this._maxSideDistance) {
                    return;
                }

                position.v.x = this._startSidePosition + (distanceFromCenter === 0 ? 1 : Math.sign(distanceFromCenter)) * this._maxSideDistance;
                this._velocity.x = 0.0;
            },
            /*NewPlaneBehaviour.PlaneFlight.LimitSideDistance end.*/

            /*NewPlaneBehaviour.PlaneFlight.SetSpeed start.*/
            SetSpeed: function (speed) {
if ( TRACE ) { TRACE( "NewPlaneBehaviour.PlaneFlight#SetSpeed", this ); }

                this._velocity = this._velocity.clone().normalize().$clone().clone().scale( UnityEngine.Mathf.Max(0.0, speed) );
            },
            /*NewPlaneBehaviour.PlaneFlight.SetSpeed end.*/


        }
    });
    /*NewPlaneBehaviour.PlaneFlight end.*/

    /*NewPlaneBehaviour.PlaneInput start.*/
    Bridge.define("NewPlaneBehaviour.PlaneInput", {
        fields: {
            _joystickRadius: 0,
            _pressPosition: null,
            IsPressed: false,
            PitchInput: 0,
            TurnInput: 0,
            DragStarted: false,
            DragEnded: false,
            WasResetButtonPressed: false,
            PointerPosition: null
        },
        ctors: {
            init: function () {
if ( TRACE ) { TRACE( "NewPlaneBehaviour.PlaneInput#init", this ); }

                this._pressPosition = new UnityEngine.Vector2();
                this.PointerPosition = new UnityEngine.Vector2();
            },
            ctor: function (joystickRadius) {
if ( TRACE ) { TRACE( "NewPlaneBehaviour.PlaneInput#ctor", this ); }

                this.$initialize();
                this._joystickRadius = joystickRadius;
            }
        },
        methods: {
            /*NewPlaneBehaviour.PlaneInput.Tick start.*/
            Tick: function () {
if ( TRACE ) { TRACE( "NewPlaneBehaviour.PlaneInput#Tick", this ); }

                this.DragStarted = UnityEngine.Input.GetMouseButtonDown(0);
                this.DragEnded = UnityEngine.Input.GetMouseButtonUp(0);

                this.PointerPosition = UnityEngine.Vector2.FromVector3(UnityEngine.Input.mousePosition.$clone());

                this.WasResetButtonPressed = UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.R);

                this.TickStick();
            },
            /*NewPlaneBehaviour.PlaneInput.Tick end.*/

            /*NewPlaneBehaviour.PlaneInput.TickStick start.*/
            TickStick: function () {
if ( TRACE ) { TRACE( "NewPlaneBehaviour.PlaneInput#TickStick", this ); }

                if (UnityEngine.Input.GetMouseButtonDown(0)) {
                    this.IsPressed = true;
                    this._pressPosition = UnityEngine.Vector2.FromVector3(UnityEngine.Input.mousePosition.$clone());
                }

                if (UnityEngine.Input.GetMouseButtonUp(0)) {
                    this.IsPressed = false;
                }

                if (this.IsPressed === false) {
                    this.PitchInput = 0.0;
                    this.TurnInput = 0.0;
                    return;
                }

                var fullDeflection = this._joystickRadius * UnityEngine.Screen.height;
                var drag = UnityEngine.Vector2.FromVector3(UnityEngine.Input.mousePosition).sub( this._pressPosition );

                this.PitchInput = Math.max(-1.0, Math.min(-drag.y / fullDeflection, 1.0));
                this.TurnInput = Math.max(-1.0, Math.min(drag.x / fullDeflection, 1.0));
            },
            /*NewPlaneBehaviour.PlaneInput.TickStick end.*/


        }
    });
    /*NewPlaneBehaviour.PlaneInput end.*/

    /*NewPlaneBehaviour.PlaneSlingshot start.*/
    Bridge.define("NewPlaneBehaviour.PlaneSlingshot", {
        fields: {
            _transform: null,
            _input: null,
            _camera: null,
            _grabRadius: 0,
            _minPullRadius: 0,
            _maxPullRadius: 0,
            _maxPullDistance: 0,
            _maxNoseRaiseAngle: 0,
            _pullReactionSpeed: 0,
            _minLaunchSpeed: 0,
            _maxLaunchSpeed: 0,
            _startPosition: null,
            _isActive: false,
            _minPullAmount: 0,
            _pullDirection: null,
            IsPulling: false,
            PullAmount: 0
        },
        events: {
            Shot: null
        },
        ctors: {
            init: function () {
if ( TRACE ) { TRACE( "NewPlaneBehaviour.PlaneSlingshot#init", this ); }

                this._startPosition = new UnityEngine.Vector3();
                this._pullDirection = new UnityEngine.Vector3();
            },
            ctor: function (transform, input, camera, grabRadius, minPullRadius, maxPullRadius, maxPullDistance, maxNoseRaiseAngle, pullReactionSpeed, minLaunchSpeed, maxLaunchSpeed) {
if ( TRACE ) { TRACE( "NewPlaneBehaviour.PlaneSlingshot#ctor", this ); }

                this.$initialize();
                this._transform = transform;
                this._input = input;
                this._camera = camera;

                this._grabRadius = grabRadius;
                this._minPullRadius = minPullRadius;
                this._maxPullRadius = maxPullRadius;
                this._maxPullDistance = maxPullDistance;
                this._maxNoseRaiseAngle = maxNoseRaiseAngle;
                this._pullReactionSpeed = pullReactionSpeed;

                this._minLaunchSpeed = minLaunchSpeed;
                this._maxLaunchSpeed = maxLaunchSpeed;

                this._startPosition = transform.position.$clone();
                this._minPullAmount = minPullRadius / maxPullRadius;

                this._isActive = true;
            }
        },
        methods: {
            /*NewPlaneBehaviour.PlaneSlingshot.Tick start.*/
            Tick: function (deltaTime) {
if ( TRACE ) { TRACE( "NewPlaneBehaviour.PlaneSlingshot#Tick", this ); }

                if (this._isActive === false) {
                    return;
                }

                if (this._input.DragStarted && this.CanGrab()) {
                    this.IsPulling = true;
                }

                if (this.IsPulling && this._input.DragEnded) {
                    this.Release();
                }

                if (this._isActive === false) {
                    return;
                }

                if (this.IsPulling) {
                    this.UpdatePull();
                }

                this.FollowPull(deltaTime);
            },
            /*NewPlaneBehaviour.PlaneSlingshot.Tick end.*/

            /*NewPlaneBehaviour.PlaneSlingshot.Reset start.*/
            Reset: function () {
if ( TRACE ) { TRACE( "NewPlaneBehaviour.PlaneSlingshot#Reset", this ); }

                this._isActive = true;

                this.IsPulling = false;
                this.PullAmount = 0.0;

                this._transform.position = this._startPosition.$clone();
                this._transform.rotation = pc.Quat.IDENTITY.clone();
            },
            /*NewPlaneBehaviour.PlaneSlingshot.Reset end.*/

            /*NewPlaneBehaviour.PlaneSlingshot.Release start.*/
            Release: function () {
if ( TRACE ) { TRACE( "NewPlaneBehaviour.PlaneSlingshot#Release", this ); }

                this.IsPulling = false;

                if (this.PullAmount < this._minPullAmount) {
                    this.PullAmount = 0.0;
                    return;
                }

                this._isActive = false;
                !Bridge.staticEquals(this.Shot, null) ? this.Shot(pc.math.lerp(this._minLaunchSpeed, this._maxLaunchSpeed, this.PullAmount)) : null;
            },
            /*NewPlaneBehaviour.PlaneSlingshot.Release end.*/

            /*NewPlaneBehaviour.PlaneSlingshot.UpdatePull start.*/
            UpdatePull: function () {
if ( TRACE ) { TRACE( "NewPlaneBehaviour.PlaneSlingshot#UpdatePull", this ); }

                var pointerOffset = this._input.PointerPosition.$clone().sub( this.CalculatePlaneScreenPosition() );
                var direction = pointerOffset.clone().normalize().$clone();

                this.PullAmount = Math.max(0, Math.min(1, pointerOffset.length() / (this._maxPullRadius * UnityEngine.Screen.height)));
                this._pullDirection = new pc.Vec3( direction.x, 0.0, UnityEngine.Mathf.Min(direction.y, 0.0) );
            },
            /*NewPlaneBehaviour.PlaneSlingshot.UpdatePull end.*/

            /*NewPlaneBehaviour.PlaneSlingshot.CanGrab start.*/
            CanGrab: function () {
if ( TRACE ) { TRACE( "NewPlaneBehaviour.PlaneSlingshot#CanGrab", this ); }

                var pressOffset = this._input.PointerPosition.$clone().sub( this.CalculatePlaneScreenPosition() );
                return pressOffset.length() <= this._grabRadius * UnityEngine.Screen.height;
            },
            /*NewPlaneBehaviour.PlaneSlingshot.CanGrab end.*/

            /*NewPlaneBehaviour.PlaneSlingshot.FollowPull start.*/
            FollowPull: function (deltaTime) {
if ( TRACE ) { TRACE( "NewPlaneBehaviour.PlaneSlingshot#FollowPull", this ); }

                var targetPosition = this._startPosition.$clone().add( this._pullDirection.$clone().clone().scale( (this.PullAmount * this._maxPullDistance) ) );
                var targetRotation = new pc.Quat().setFromEulerAngles_Unity( -this.PullAmount * this._maxNoseRaiseAngle, 0.0, 0.0 );
                var moveProgress = 1.0 - UnityEngine.Mathf.Exp(-this._pullReactionSpeed * deltaTime);

                this._transform.position = new pc.Vec3().lerp( this._transform.position, targetPosition, moveProgress );
                this._transform.rotation = new pc.Quat().slerpUnclamped( this._transform.rotation, targetRotation, pc.math.clamp( moveProgress, 0, 1 ) );
            },
            /*NewPlaneBehaviour.PlaneSlingshot.FollowPull end.*/

            /*NewPlaneBehaviour.PlaneSlingshot.CalculatePlaneScreenPosition start.*/
            CalculatePlaneScreenPosition: function () {
if ( TRACE ) { TRACE( "NewPlaneBehaviour.PlaneSlingshot#CalculatePlaneScreenPosition", this ); }

                var screenPosition = this._camera.WorldToScreenPoint(this._startPosition);
                return new pc.Vec2( screenPosition.x, screenPosition.y );
            },
            /*NewPlaneBehaviour.PlaneSlingshot.CalculatePlaneScreenPosition end.*/


        }
    });
    /*NewPlaneBehaviour.PlaneSlingshot end.*/

    if ( MODULE_reflection ) {
    var $m = Bridge.setMetadata,
        $n = ["System","UnityEngine","NewPlaneBehaviour"];

    /*IAmAnEmptyScriptJustToMakeCodelessProjectsCompileProperty start.*/
    $m("IAmAnEmptyScriptJustToMakeCodelessProjectsCompileProperty", function () { return {"att":1048577,"a":2,"m":[{"a":2,"isSynthetic":true,"n":".ctor","t":1,"sn":"ctor"}]}; }, $n);
    /*IAmAnEmptyScriptJustToMakeCodelessProjectsCompileProperty end.*/

    /*NewPlaneBehaviour.BezierCurve start.*/
    $m("NewPlaneBehaviour.BezierCurve", function () { return {"att":1048577,"a":2,"m":[{"a":2,"isSynthetic":true,"n":".ctor","t":1,"sn":"ctor"},{"a":2,"n":"CalculateDirection","t":8,"pi":[{"n":"distance","pt":$n[0].Single,"ps":0}],"sn":"CalculateDirection","rt":$n[1].Vector3,"p":[$n[0].Single]},{"a":1,"n":"CalculatePoint","t":8,"pi":[{"n":"distance","pt":$n[0].Single,"ps":0}],"sn":"CalculatePoint","rt":$n[0].Single,"p":[$n[0].Single],"box":function ($v) { return Bridge.box($v, System.Single, System.Single.format, System.Single.getHashCode);}},{"a":2,"n":"CalculatePosition","t":8,"pi":[{"n":"distance","pt":$n[0].Single,"ps":0}],"sn":"CalculatePosition","rt":$n[1].Vector3,"p":[$n[0].Single]},{"a":1,"n":"CalculatePositionAtPoint","t":8,"pi":[{"n":"point","pt":$n[0].Single,"ps":0}],"sn":"CalculatePositionAtPoint","rt":$n[1].Vector3,"p":[$n[0].Single]},{"a":1,"n":"MeasureLength","t":8,"sn":"MeasureLength","rt":$n[0].Void},{"a":2,"n":"Set","t":8,"pi":[{"n":"start","pt":$n[1].Vector3,"ps":0},{"n":"control","pt":$n[1].Vector3,"ps":1},{"n":"end","pt":$n[1].Vector3,"ps":2}],"sn":"Set","rt":$n[0].Void,"p":[$n[1].Vector3,$n[1].Vector3,$n[1].Vector3]},{"a":2,"n":"Length","t":16,"rt":$n[0].Single,"g":{"a":2,"n":"get_Length","t":8,"rt":$n[0].Single,"fg":"Length","box":function ($v) { return Bridge.box($v, System.Single, System.Single.format, System.Single.getHashCode);}},"s":{"a":1,"n":"set_Length","t":8,"p":[$n[0].Single],"rt":$n[0].Void,"fs":"Length"},"fn":"Length"},{"a":1,"n":"SampleCount","is":true,"t":4,"rt":$n[0].Int32,"sn":"SampleCount","box":function ($v) { return Bridge.box($v, System.Int32);}},{"a":1,"n":"_control","t":4,"rt":$n[1].Vector3,"sn":"_control"},{"a":1,"n":"_end","t":4,"rt":$n[1].Vector3,"sn":"_end"},{"a":1,"n":"_sampleLengths","t":4,"rt":$n[0].Array.type(System.Single),"sn":"_sampleLengths","ro":true},{"a":1,"n":"_start","t":4,"rt":$n[1].Vector3,"sn":"_start"},{"a":1,"backing":true,"n":"<Length>k__BackingField","t":4,"rt":$n[0].Single,"sn":"Length","box":function ($v) { return Bridge.box($v, System.Single, System.Single.format, System.Single.getHashCode);}}]}; }, $n);
    /*NewPlaneBehaviour.BezierCurve end.*/

    /*NewPlaneBehaviour.Plane start.*/
    $m("NewPlaneBehaviour.Plane", function () { return {"att":1048577,"a":2,"m":[{"a":2,"isSynthetic":true,"n":".ctor","t":1,"sn":"ctor"},{"a":1,"n":"Awake","t":8,"sn":"Awake","rt":$n[0].Void},{"a":1,"n":"LateUpdate","t":8,"sn":"LateUpdate","rt":$n[0].Void},{"a":1,"n":"OnDestroy","t":8,"sn":"OnDestroy","rt":$n[0].Void},{"a":1,"n":"OnPlaneClimbFinished","t":8,"pi":[{"n":"velocity","pt":$n[1].Vector3,"ps":0}],"sn":"OnPlaneClimbFinished","rt":$n[0].Void,"p":[$n[1].Vector3]},{"a":1,"n":"OnPlaneLanded","t":8,"sn":"OnPlaneLanded","rt":$n[0].Void},{"a":1,"n":"OnShot","t":8,"pi":[{"n":"launchSpeed","pt":$n[0].Single,"ps":0}],"sn":"OnShot","rt":$n[0].Void,"p":[$n[0].Single]},{"a":1,"n":"Update","t":8,"sn":"Update","rt":$n[0].Void},{"at":[new UnityEngine.SerializeFieldAttribute()],"a":1,"n":"_camera","t":4,"rt":$n[1].Camera,"sn":"_camera"},{"a":1,"n":"_cameraFollow","t":4,"rt":$n[2].PlaneCameraFollow,"sn":"_cameraFollow"},{"at":[new UnityEngine.SerializeFieldAttribute()],"a":1,"n":"_cameraFollowSharpness","t":4,"rt":$n[0].Single,"sn":"_cameraFollowSharpness","box":function ($v) { return Bridge.box($v, System.Single, System.Single.format, System.Single.getHashCode);}},{"at":[new UnityEngine.SerializeFieldAttribute()],"a":1,"n":"_cameraLookAheadDistance","t":4,"rt":$n[0].Single,"sn":"_cameraLookAheadDistance","box":function ($v) { return Bridge.box($v, System.Single, System.Single.format, System.Single.getHashCode);}},{"at":[new UnityEngine.SerializeFieldAttribute()],"a":1,"n":"_cameraLookSharpness","t":4,"rt":$n[0].Single,"sn":"_cameraLookSharpness","box":function ($v) { return Bridge.box($v, System.Single, System.Single.format, System.Single.getHashCode);}},{"at":[new UnityEngine.HeaderAttribute("Camera"),new UnityEngine.SerializeFieldAttribute()],"a":1,"n":"_cameraOffset","t":4,"rt":$n[1].Vector3,"sn":"_cameraOffset"},{"at":[new UnityEngine.SerializeFieldAttribute()],"a":1,"n":"_cameraTransform","t":4,"rt":$n[1].Transform,"sn":"_cameraTransform"},{"at":[new UnityEngine.SerializeFieldAttribute()],"a":1,"n":"_climbAngle","t":4,"rt":$n[0].Single,"sn":"_climbAngle","box":function ($v) { return Bridge.box($v, System.Single, System.Single.format, System.Single.getHashCode);}},{"at":[new UnityEngine.SerializeFieldAttribute()],"a":1,"n":"_climbCost","t":4,"rt":$n[0].Single,"sn":"_climbCost","box":function ($v) { return Bridge.box($v, System.Single, System.Single.format, System.Single.getHashCode);}},{"at":[new UnityEngine.SerializeFieldAttribute()],"a":1,"n":"_climbDuration","t":4,"rt":$n[0].Single,"sn":"_climbDuration","box":function ($v) { return Bridge.box($v, System.Single, System.Single.format, System.Single.getHashCode);}},{"at":[new UnityEngine.HeaderAttribute("Climb"),new UnityEngine.SerializeFieldAttribute()],"a":1,"n":"_climbSpeed","t":4,"rt":$n[0].Single,"sn":"_climbSpeed","box":function ($v) { return Bridge.box($v, System.Single, System.Single.format, System.Single.getHashCode);}},{"at":[new UnityEngine.SerializeFieldAttribute()],"a":1,"n":"_climbSpeedCurve","t":4,"rt":pc.AnimationCurve,"sn":"_climbSpeedCurve"},{"at":[new UnityEngine.SerializeFieldAttribute()],"a":1,"n":"_drag","t":4,"rt":$n[0].Single,"sn":"_drag","box":function ($v) { return Bridge.box($v, System.Single, System.Single.format, System.Single.getHashCode);}},{"at":[new UnityEngine.SerializeFieldAttribute()],"a":1,"n":"_fullControlSpeed","t":4,"rt":$n[0].Single,"sn":"_fullControlSpeed","box":function ($v) { return Bridge.box($v, System.Single, System.Single.format, System.Single.getHashCode);}},{"at":[new UnityEngine.SerializeFieldAttribute()],"a":1,"n":"_grabRadius","t":4,"rt":$n[0].Single,"sn":"_grabRadius","box":function ($v) { return Bridge.box($v, System.Single, System.Single.format, System.Single.getHashCode);}},{"at":[new UnityEngine.HeaderAttribute("Flight"),new UnityEngine.SerializeFieldAttribute()],"a":1,"n":"_gravity","t":4,"rt":$n[0].Single,"sn":"_gravity","box":function ($v) { return Bridge.box($v, System.Single, System.Single.format, System.Single.getHashCode);}},{"at":[new UnityEngine.SerializeFieldAttribute()],"a":1,"n":"_groundHeight","t":4,"rt":$n[0].Single,"sn":"_groundHeight","box":function ($v) { return Bridge.box($v, System.Single, System.Single.format, System.Single.getHashCode);}},{"a":1,"n":"_input","t":4,"rt":$n[2].PlaneInput,"sn":"_input"},{"at":[new UnityEngine.HeaderAttribute("Input"),new UnityEngine.SerializeFieldAttribute()],"a":1,"n":"_joystickRadius","t":4,"rt":$n[0].Single,"sn":"_joystickRadius","box":function ($v) { return Bridge.box($v, System.Single, System.Single.format, System.Single.getHashCode);}},{"at":[new UnityEngine.SerializeFieldAttribute()],"a":1,"n":"_maxHeight","t":4,"rt":$n[0].Single,"sn":"_maxHeight","box":function ($v) { return Bridge.box($v, System.Single, System.Single.format, System.Single.getHashCode);}},{"at":[new UnityEngine.SerializeFieldAttribute()],"a":1,"n":"_maxLaunchSpeed","t":4,"rt":$n[0].Single,"sn":"_maxLaunchSpeed","box":function ($v) { return Bridge.box($v, System.Single, System.Single.format, System.Single.getHashCode);}},{"at":[new UnityEngine.SerializeFieldAttribute()],"a":1,"n":"_maxNoseRaiseAngle","t":4,"rt":$n[0].Single,"sn":"_maxNoseRaiseAngle","box":function ($v) { return Bridge.box($v, System.Single, System.Single.format, System.Single.getHashCode);}},{"at":[new UnityEngine.HeaderAttribute("Slingshot"),new UnityEngine.SerializeFieldAttribute()],"a":1,"n":"_maxPullDistance","t":4,"rt":$n[0].Single,"sn":"_maxPullDistance","box":function ($v) { return Bridge.box($v, System.Single, System.Single.format, System.Single.getHashCode);}},{"at":[new UnityEngine.SerializeFieldAttribute()],"a":1,"n":"_maxPullRadius","t":4,"rt":$n[0].Single,"sn":"_maxPullRadius","box":function ($v) { return Bridge.box($v, System.Single, System.Single.format, System.Single.getHashCode);}},{"at":[new UnityEngine.SerializeFieldAttribute()],"a":1,"n":"_maxSideDistance","t":4,"rt":$n[0].Single,"sn":"_maxSideDistance","box":function ($v) { return Bridge.box($v, System.Single, System.Single.format, System.Single.getHashCode);}},{"at":[new UnityEngine.SerializeFieldAttribute()],"a":1,"n":"_maxTurnAngle","t":4,"rt":$n[0].Single,"sn":"_maxTurnAngle","box":function ($v) { return Bridge.box($v, System.Single, System.Single.format, System.Single.getHashCode);}},{"at":[new UnityEngine.SerializeFieldAttribute()],"a":1,"n":"_maxWingAngle","t":4,"rt":$n[0].Single,"sn":"_maxWingAngle","box":function ($v) { return Bridge.box($v, System.Single, System.Single.format, System.Single.getHashCode);}},{"at":[new UnityEngine.SpaceAttribute.ctor(),new UnityEngine.SerializeFieldAttribute()],"a":1,"n":"_minLaunchSpeed","t":4,"rt":$n[0].Single,"sn":"_minLaunchSpeed","box":function ($v) { return Bridge.box($v, System.Single, System.Single.format, System.Single.getHashCode);}},{"at":[new UnityEngine.SerializeFieldAttribute()],"a":1,"n":"_minPullRadius","t":4,"rt":$n[0].Single,"sn":"_minPullRadius","box":function ($v) { return Bridge.box($v, System.Single, System.Single.format, System.Single.getHashCode);}},{"at":[new UnityEngine.SerializeFieldAttribute()],"a":1,"n":"_noControlSpeed","t":4,"rt":$n[0].Single,"sn":"_noControlSpeed","box":function ($v) { return Bridge.box($v, System.Single, System.Single.format, System.Single.getHashCode);}},{"at":[new UnityEngine.SerializeFieldAttribute()],"a":1,"n":"_noseTurnDuration","t":4,"rt":$n[0].Single,"sn":"_noseTurnDuration","box":function ($v) { return Bridge.box($v, System.Single, System.Single.format, System.Single.getHashCode);}},{"at":[new UnityEngine.SpaceAttribute.ctor(),new UnityEngine.SerializeFieldAttribute()],"a":1,"n":"_pitchRate","t":4,"rt":$n[0].Single,"sn":"_pitchRate","box":function ($v) { return Bridge.box($v, System.Single, System.Single.format, System.Single.getHashCode);}},{"a":1,"n":"_planeClimb","t":4,"rt":$n[2].PlaneClimb,"sn":"_planeClimb"},{"a":1,"n":"_planeFlight","t":4,"rt":$n[2].PlaneFlight,"sn":"_planeFlight"},{"at":[new UnityEngine.SerializeFieldAttribute()],"a":1,"n":"_planeRoot","t":4,"rt":$n[1].Transform,"sn":"_planeRoot"},{"at":[new UnityEngine.SerializeFieldAttribute()],"a":1,"n":"_pullReactionSpeed","t":4,"rt":$n[0].Single,"sn":"_pullReactionSpeed","box":function ($v) { return Bridge.box($v, System.Single, System.Single.format, System.Single.getHashCode);}},{"at":[new UnityEngine.SerializeFieldAttribute()],"a":1,"n":"_rampControlPoint","t":4,"rt":$n[1].Transform,"sn":"_rampControlPoint"},{"at":[new UnityEngine.SerializeFieldAttribute()],"a":1,"n":"_rampEndPoint","t":4,"rt":$n[1].Transform,"sn":"_rampEndPoint"},{"at":[new UnityEngine.HeaderAttribute("Ramp Points"),new UnityEngine.SerializeFieldAttribute()],"a":1,"n":"_rampStartPoint","t":4,"rt":$n[1].Transform,"sn":"_rampStartPoint"},{"a":1,"n":"_slingshot","t":4,"rt":$n[2].PlaneSlingshot,"sn":"_slingshot"},{"a":1,"n":"_startPosition","t":4,"rt":$n[1].Vector3,"sn":"_startPosition"},{"at":[new UnityEngine.SerializeFieldAttribute()],"a":1,"n":"_turnRate","t":4,"rt":$n[0].Single,"sn":"_turnRate","box":function ($v) { return Bridge.box($v, System.Single, System.Single.format, System.Single.getHashCode);}},{"at":[new UnityEngine.SerializeFieldAttribute()],"a":1,"n":"_wingReactionSpeed","t":4,"rt":$n[0].Single,"sn":"_wingReactionSpeed","box":function ($v) { return Bridge.box($v, System.Single, System.Single.format, System.Single.getHashCode);}}]}; }, $n);
    /*NewPlaneBehaviour.Plane end.*/

    /*NewPlaneBehaviour.PlaneCameraFollow start.*/
    $m("NewPlaneBehaviour.PlaneCameraFollow", function () { return {"att":1048833,"a":2,"m":[{"a":2,"n":".ctor","t":1,"p":[$n[1].Transform,$n[1].Transform,$n[1].Vector3,$n[0].Single,$n[0].Single,$n[0].Single],"pi":[{"n":"cameraTransform","pt":$n[1].Transform,"ps":0},{"n":"target","pt":$n[1].Transform,"ps":1},{"n":"offset","pt":$n[1].Vector3,"ps":2},{"n":"followSharpness","pt":$n[0].Single,"ps":3},{"n":"lookSharpness","pt":$n[0].Single,"ps":4},{"n":"lookAheadDistance","pt":$n[0].Single,"ps":5}],"sn":"ctor"},{"a":1,"n":"CalculateFlightDirection","t":8,"sn":"CalculateFlightDirection","rt":$n[1].Quaternion},{"a":1,"n":"CalculateLookRotation","t":8,"sn":"CalculateLookRotation","rt":$n[1].Quaternion},{"a":1,"n":"CalculateProgress","t":8,"pi":[{"n":"sharpness","pt":$n[0].Single,"ps":0},{"n":"deltaTime","pt":$n[0].Single,"ps":1}],"sn":"CalculateProgress","rt":$n[0].Single,"p":[$n[0].Single,$n[0].Single],"box":function ($v) { return Bridge.box($v, System.Single, System.Single.format, System.Single.getHashCode);}},{"a":1,"n":"CalculateTargetPosition","t":8,"sn":"CalculateTargetPosition","rt":$n[1].Vector3},{"a":2,"n":"Reset","t":8,"sn":"Reset","rt":$n[0].Void},{"a":2,"n":"StartFollowing","t":8,"sn":"StartFollowing","rt":$n[0].Void},{"a":2,"n":"StopFollowing","t":8,"sn":"StopFollowing","rt":$n[0].Void},{"a":2,"n":"Tick","t":8,"pi":[{"n":"deltaTime","pt":$n[0].Single,"ps":0}],"sn":"Tick","rt":$n[0].Void,"p":[$n[0].Single]},{"a":2,"n":"IsFollowing","t":16,"rt":$n[0].Boolean,"g":{"a":2,"n":"get_IsFollowing","t":8,"rt":$n[0].Boolean,"fg":"IsFollowing","box":function ($v) { return Bridge.box($v, System.Boolean, System.Boolean.toString);}},"s":{"a":1,"n":"set_IsFollowing","t":8,"p":[$n[0].Boolean],"rt":$n[0].Void,"fs":"IsFollowing"},"fn":"IsFollowing"},{"a":1,"n":"_cameraTransform","t":4,"rt":$n[1].Transform,"sn":"_cameraTransform","ro":true},{"a":1,"n":"_followSharpness","t":4,"rt":$n[0].Single,"sn":"_followSharpness","ro":true,"box":function ($v) { return Bridge.box($v, System.Single, System.Single.format, System.Single.getHashCode);}},{"a":1,"n":"_lookAheadDistance","t":4,"rt":$n[0].Single,"sn":"_lookAheadDistance","ro":true,"box":function ($v) { return Bridge.box($v, System.Single, System.Single.format, System.Single.getHashCode);}},{"a":1,"n":"_lookSharpness","t":4,"rt":$n[0].Single,"sn":"_lookSharpness","ro":true,"box":function ($v) { return Bridge.box($v, System.Single, System.Single.format, System.Single.getHashCode);}},{"a":1,"n":"_offset","t":4,"rt":$n[1].Vector3,"sn":"_offset","ro":true},{"a":1,"n":"_target","t":4,"rt":$n[1].Transform,"sn":"_target","ro":true},{"a":1,"backing":true,"n":"<IsFollowing>k__BackingField","t":4,"rt":$n[0].Boolean,"sn":"IsFollowing","box":function ($v) { return Bridge.box($v, System.Boolean, System.Boolean.toString);}}]}; }, $n);
    /*NewPlaneBehaviour.PlaneCameraFollow end.*/

    /*NewPlaneBehaviour.PlaneClimb start.*/
    $m("NewPlaneBehaviour.PlaneClimb", function () { return {"att":1048577,"a":2,"m":[{"a":2,"n":".ctor","t":1,"p":[$n[1].Transform,$n[1].Transform,$n[1].Transform,$n[1].Transform,pc.AnimationCurve,$n[0].Single],"pi":[{"n":"transform","pt":$n[1].Transform,"ps":0},{"n":"rampStartPoint","pt":$n[1].Transform,"ps":1},{"n":"rampControlPoint","pt":$n[1].Transform,"ps":2},{"n":"rampEndPoint","pt":$n[1].Transform,"ps":3},{"n":"speedCurve","pt":pc.AnimationCurve,"ps":4},{"n":"noseTurnDuration","pt":$n[0].Single,"ps":5}],"sn":"ctor"},{"a":1,"n":"BakePathProgress","t":8,"sn":"BakePathProgress","rt":$n[0].Void},{"a":1,"n":"CalculatePathProgress","t":8,"pi":[{"n":"timeProgress","pt":$n[0].Single,"ps":0}],"sn":"CalculatePathProgress","rt":$n[0].Single,"p":[$n[0].Single],"box":function ($v) { return Bridge.box($v, System.Single, System.Single.format, System.Single.getHashCode);}},{"a":1,"n":"MoveAlongPath","t":8,"pi":[{"n":"passedDistance","pt":$n[0].Single,"ps":0}],"sn":"MoveAlongPath","rt":$n[0].Void,"p":[$n[0].Single]},{"a":2,"n":"Reset","t":8,"sn":"Reset","rt":$n[0].Void},{"a":2,"n":"Start","t":8,"pi":[{"n":"launchSpeed","pt":$n[0].Single,"ps":0}],"sn":"Start","rt":$n[0].Void,"p":[$n[0].Single]},{"a":2,"n":"Tick","t":8,"pi":[{"n":"deltaTime","pt":$n[0].Single,"ps":0}],"sn":"Tick","rt":$n[0].Void,"p":[$n[0].Single]},{"a":1,"n":"MidPointDistanceRatio","is":true,"t":4,"rt":$n[0].Single,"sn":"MidPointDistanceRatio","box":function ($v) { return Bridge.box($v, System.Single, System.Single.format, System.Single.getHashCode);}},{"a":1,"n":"PathProgressStepCount","is":true,"t":4,"rt":$n[0].Int32,"sn":"PathProgressStepCount","box":function ($v) { return Bridge.box($v, System.Int32);}},{"a":1,"n":"_climbDuration","t":4,"rt":$n[0].Single,"sn":"_climbDuration","box":function ($v) { return Bridge.box($v, System.Single, System.Single.format, System.Single.getHashCode);}},{"a":1,"n":"_directionAfterRamp","t":4,"rt":$n[1].Vector3,"sn":"_directionAfterRamp"},{"a":1,"n":"_elapsedClimbTime","t":4,"rt":$n[0].Single,"sn":"_elapsedClimbTime","box":function ($v) { return Bridge.box($v, System.Single, System.Single.format, System.Single.getHashCode);}},{"a":1,"n":"_isClimbing","t":4,"rt":$n[0].Boolean,"sn":"_isClimbing","box":function ($v) { return Bridge.box($v, System.Boolean, System.Boolean.toString);}},{"a":1,"n":"_launchSpeed","t":4,"rt":$n[0].Single,"sn":"_launchSpeed","box":function ($v) { return Bridge.box($v, System.Single, System.Single.format, System.Single.getHashCode);}},{"a":1,"n":"_noseTurnDuration","t":4,"rt":$n[0].Single,"sn":"_noseTurnDuration","ro":true,"box":function ($v) { return Bridge.box($v, System.Single, System.Single.format, System.Single.getHashCode);}},{"a":1,"n":"_pathLength","t":4,"rt":$n[0].Single,"sn":"_pathLength","box":function ($v) { return Bridge.box($v, System.Single, System.Single.format, System.Single.getHashCode);}},{"a":1,"n":"_pathOnRamp","t":4,"rt":$n[2].BezierCurve,"sn":"_pathOnRamp","ro":true},{"a":1,"n":"_pathProgressByTime","t":4,"rt":$n[0].Array.type(System.Single),"sn":"_pathProgressByTime","ro":true},{"a":1,"n":"_pathToRamp","t":4,"rt":$n[2].BezierCurve,"sn":"_pathToRamp","ro":true},{"a":1,"n":"_rampControlPoint","t":4,"rt":$n[1].Transform,"sn":"_rampControlPoint","ro":true},{"a":1,"n":"_rampEndPoint","t":4,"rt":$n[1].Transform,"sn":"_rampEndPoint","ro":true},{"a":1,"n":"_rampStartPoint","t":4,"rt":$n[1].Transform,"sn":"_rampStartPoint","ro":true},{"a":1,"n":"_rotationAtShot","t":4,"rt":$n[1].Quaternion,"sn":"_rotationAtShot"},{"a":1,"n":"_speedCurve","t":4,"rt":pc.AnimationCurve,"sn":"_speedCurve","ro":true},{"a":1,"n":"_startPosition","t":4,"rt":$n[1].Vector3,"sn":"_startPosition","ro":true},{"a":1,"n":"_transform","t":4,"rt":$n[1].Transform,"sn":"_transform","ro":true},{"a":2,"n":"Finished","t":2,"ad":{"a":2,"n":"add_Finished","t":8,"pi":[{"n":"value","pt":Function,"ps":0}],"sn":"addFinished","rt":$n[0].Void,"p":[Function]},"r":{"a":2,"n":"remove_Finished","t":8,"pi":[{"n":"value","pt":Function,"ps":0}],"sn":"removeFinished","rt":$n[0].Void,"p":[Function]}}]}; }, $n);
    /*NewPlaneBehaviour.PlaneClimb end.*/

    /*NewPlaneBehaviour.PlaneFlight start.*/
    $m("NewPlaneBehaviour.PlaneFlight", function () { return {"att":1048833,"a":2,"m":[{"a":2,"n":".ctor","t":1,"p":[$n[1].Transform,$n[2].PlaneInput,$n[0].Single,$n[0].Single,$n[0].Single,$n[0].Single,$n[0].Single,$n[0].Single,$n[0].Single,$n[0].Single,$n[0].Single,$n[0].Single,$n[0].Single,$n[0].Single,$n[0].Single],"pi":[{"n":"transform","pt":$n[1].Transform,"ps":0},{"n":"input","pt":$n[2].PlaneInput,"ps":1},{"n":"gravity","pt":$n[0].Single,"ps":2},{"n":"groundHeight","pt":$n[0].Single,"ps":3},{"n":"pitchRate","pt":$n[0].Single,"ps":4},{"n":"drag","pt":$n[0].Single,"ps":5},{"n":"climbCost","pt":$n[0].Single,"ps":6},{"n":"noControlSpeed","pt":$n[0].Single,"ps":7},{"n":"fullControlSpeed","pt":$n[0].Single,"ps":8},{"n":"maxHeight","pt":$n[0].Single,"ps":9},{"n":"turnRate","pt":$n[0].Single,"ps":10},{"n":"maxTurnAngle","pt":$n[0].Single,"ps":11},{"n":"maxWingAngle","pt":$n[0].Single,"ps":12},{"n":"wingReactionSpeed","pt":$n[0].Single,"ps":13},{"n":"maxSideDistance","pt":$n[0].Single,"ps":14}],"sn":"ctor"},{"a":1,"n":"LimitSideDistance","t":8,"pi":[{"n":"position","ref":true,"pt":$n[1].Vector3,"ps":0}],"sn":"LimitSideDistance","rt":$n[0].Void,"p":[$n[1].Vector3]},{"a":1,"n":"SetSpeed","t":8,"pi":[{"n":"speed","pt":$n[0].Single,"ps":0}],"sn":"SetSpeed","rt":$n[0].Void,"p":[$n[0].Single]},{"a":1,"n":"SlowDownByAir","t":8,"pi":[{"n":"deltaTime","pt":$n[0].Single,"ps":0}],"sn":"SlowDownByAir","rt":$n[0].Void,"p":[$n[0].Single]},{"a":1,"n":"SlowDownOnClimb","t":8,"pi":[{"n":"deltaTime","pt":$n[0].Single,"ps":0}],"sn":"SlowDownOnClimb","rt":$n[0].Void,"p":[$n[0].Single]},{"a":2,"n":"Start","t":8,"pi":[{"n":"velocity","pt":$n[1].Vector3,"ps":0}],"sn":"Start","rt":$n[0].Void,"p":[$n[1].Vector3]},{"a":2,"n":"Stop","t":8,"sn":"Stop","rt":$n[0].Void},{"a":2,"n":"Tick","t":8,"pi":[{"n":"deltaTime","pt":$n[0].Single,"ps":0}],"sn":"Tick","rt":$n[0].Void,"p":[$n[0].Single]},{"a":1,"n":"TurnNoseWithStick","t":8,"pi":[{"n":"deltaTime","pt":$n[0].Single,"ps":0}],"sn":"TurnNoseWithStick","rt":$n[0].Void,"p":[$n[0].Single]},{"a":1,"n":"TurnSideWithStick","t":8,"pi":[{"n":"deltaTime","pt":$n[0].Single,"ps":0}],"sn":"TurnSideWithStick","rt":$n[0].Void,"p":[$n[0].Single]},{"a":1,"n":"UpdateWingAngle","t":8,"pi":[{"n":"deltaTime","pt":$n[0].Single,"ps":0}],"sn":"UpdateWingAngle","rt":$n[0].Void,"p":[$n[0].Single]},{"a":1,"n":"_climbCost","t":4,"rt":$n[0].Single,"sn":"_climbCost","ro":true,"box":function ($v) { return Bridge.box($v, System.Single, System.Single.format, System.Single.getHashCode);}},{"a":1,"n":"_drag","t":4,"rt":$n[0].Single,"sn":"_drag","ro":true,"box":function ($v) { return Bridge.box($v, System.Single, System.Single.format, System.Single.getHashCode);}},{"a":1,"n":"_fullControlSpeed","t":4,"rt":$n[0].Single,"sn":"_fullControlSpeed","ro":true,"box":function ($v) { return Bridge.box($v, System.Single, System.Single.format, System.Single.getHashCode);}},{"a":1,"n":"_gravity","t":4,"rt":$n[0].Single,"sn":"_gravity","ro":true,"box":function ($v) { return Bridge.box($v, System.Single, System.Single.format, System.Single.getHashCode);}},{"a":1,"n":"_groundHeight","t":4,"rt":$n[0].Single,"sn":"_groundHeight","ro":true,"box":function ($v) { return Bridge.box($v, System.Single, System.Single.format, System.Single.getHashCode);}},{"a":1,"n":"_input","t":4,"rt":$n[2].PlaneInput,"sn":"_input","ro":true},{"a":1,"n":"_isRunning","t":4,"rt":$n[0].Boolean,"sn":"_isRunning","box":function ($v) { return Bridge.box($v, System.Boolean, System.Boolean.toString);}},{"a":1,"n":"_maxHeight","t":4,"rt":$n[0].Single,"sn":"_maxHeight","ro":true,"box":function ($v) { return Bridge.box($v, System.Single, System.Single.format, System.Single.getHashCode);}},{"a":1,"n":"_maxSideDistance","t":4,"rt":$n[0].Single,"sn":"_maxSideDistance","ro":true,"box":function ($v) { return Bridge.box($v, System.Single, System.Single.format, System.Single.getHashCode);}},{"a":1,"n":"_maxTurnAngle","t":4,"rt":$n[0].Single,"sn":"_maxTurnAngle","ro":true,"box":function ($v) { return Bridge.box($v, System.Single, System.Single.format, System.Single.getHashCode);}},{"a":1,"n":"_maxWingAngle","t":4,"rt":$n[0].Single,"sn":"_maxWingAngle","ro":true,"box":function ($v) { return Bridge.box($v, System.Single, System.Single.format, System.Single.getHashCode);}},{"a":1,"n":"_noControlSpeed","t":4,"rt":$n[0].Single,"sn":"_noControlSpeed","ro":true,"box":function ($v) { return Bridge.box($v, System.Single, System.Single.format, System.Single.getHashCode);}},{"a":1,"n":"_pitchRate","t":4,"rt":$n[0].Single,"sn":"_pitchRate","ro":true,"box":function ($v) { return Bridge.box($v, System.Single, System.Single.format, System.Single.getHashCode);}},{"a":1,"n":"_startSidePosition","t":4,"rt":$n[0].Single,"sn":"_startSidePosition","ro":true,"box":function ($v) { return Bridge.box($v, System.Single, System.Single.format, System.Single.getHashCode);}},{"a":1,"n":"_transform","t":4,"rt":$n[1].Transform,"sn":"_transform","ro":true},{"a":1,"n":"_turnRate","t":4,"rt":$n[0].Single,"sn":"_turnRate","ro":true,"box":function ($v) { return Bridge.box($v, System.Single, System.Single.format, System.Single.getHashCode);}},{"a":1,"n":"_velocity","t":4,"rt":$n[1].Vector3,"sn":"_velocity"},{"a":1,"n":"_wingAngle","t":4,"rt":$n[0].Single,"sn":"_wingAngle","box":function ($v) { return Bridge.box($v, System.Single, System.Single.format, System.Single.getHashCode);}},{"a":1,"n":"_wingReactionSpeed","t":4,"rt":$n[0].Single,"sn":"_wingReactionSpeed","ro":true,"box":function ($v) { return Bridge.box($v, System.Single, System.Single.format, System.Single.getHashCode);}},{"a":2,"n":"Landed","t":2,"ad":{"a":2,"n":"add_Landed","t":8,"pi":[{"n":"value","pt":Function,"ps":0}],"sn":"addLanded","rt":$n[0].Void,"p":[Function]},"r":{"a":2,"n":"remove_Landed","t":8,"pi":[{"n":"value","pt":Function,"ps":0}],"sn":"removeLanded","rt":$n[0].Void,"p":[Function]}}]}; }, $n);
    /*NewPlaneBehaviour.PlaneFlight end.*/

    /*NewPlaneBehaviour.PlaneInput start.*/
    $m("NewPlaneBehaviour.PlaneInput", function () { return {"att":1048833,"a":2,"m":[{"a":2,"n":".ctor","t":1,"p":[$n[0].Single],"pi":[{"n":"joystickRadius","pt":$n[0].Single,"ps":0}],"sn":"ctor"},{"a":2,"n":"Tick","t":8,"sn":"Tick","rt":$n[0].Void},{"a":1,"n":"TickStick","t":8,"sn":"TickStick","rt":$n[0].Void},{"a":2,"n":"DragEnded","t":16,"rt":$n[0].Boolean,"g":{"a":2,"n":"get_DragEnded","t":8,"rt":$n[0].Boolean,"fg":"DragEnded","box":function ($v) { return Bridge.box($v, System.Boolean, System.Boolean.toString);}},"s":{"a":1,"n":"set_DragEnded","t":8,"p":[$n[0].Boolean],"rt":$n[0].Void,"fs":"DragEnded"},"fn":"DragEnded"},{"a":2,"n":"DragStarted","t":16,"rt":$n[0].Boolean,"g":{"a":2,"n":"get_DragStarted","t":8,"rt":$n[0].Boolean,"fg":"DragStarted","box":function ($v) { return Bridge.box($v, System.Boolean, System.Boolean.toString);}},"s":{"a":1,"n":"set_DragStarted","t":8,"p":[$n[0].Boolean],"rt":$n[0].Void,"fs":"DragStarted"},"fn":"DragStarted"},{"a":2,"n":"IsPressed","t":16,"rt":$n[0].Boolean,"g":{"a":2,"n":"get_IsPressed","t":8,"rt":$n[0].Boolean,"fg":"IsPressed","box":function ($v) { return Bridge.box($v, System.Boolean, System.Boolean.toString);}},"s":{"a":1,"n":"set_IsPressed","t":8,"p":[$n[0].Boolean],"rt":$n[0].Void,"fs":"IsPressed"},"fn":"IsPressed"},{"a":2,"n":"PitchInput","t":16,"rt":$n[0].Single,"g":{"a":2,"n":"get_PitchInput","t":8,"rt":$n[0].Single,"fg":"PitchInput","box":function ($v) { return Bridge.box($v, System.Single, System.Single.format, System.Single.getHashCode);}},"s":{"a":1,"n":"set_PitchInput","t":8,"p":[$n[0].Single],"rt":$n[0].Void,"fs":"PitchInput"},"fn":"PitchInput"},{"a":2,"n":"PointerPosition","t":16,"rt":$n[1].Vector2,"g":{"a":2,"n":"get_PointerPosition","t":8,"rt":$n[1].Vector2,"fg":"PointerPosition"},"s":{"a":1,"n":"set_PointerPosition","t":8,"p":[$n[1].Vector2],"rt":$n[0].Void,"fs":"PointerPosition"},"fn":"PointerPosition"},{"a":2,"n":"TurnInput","t":16,"rt":$n[0].Single,"g":{"a":2,"n":"get_TurnInput","t":8,"rt":$n[0].Single,"fg":"TurnInput","box":function ($v) { return Bridge.box($v, System.Single, System.Single.format, System.Single.getHashCode);}},"s":{"a":1,"n":"set_TurnInput","t":8,"p":[$n[0].Single],"rt":$n[0].Void,"fs":"TurnInput"},"fn":"TurnInput"},{"a":2,"n":"WasResetButtonPressed","t":16,"rt":$n[0].Boolean,"g":{"a":2,"n":"get_WasResetButtonPressed","t":8,"rt":$n[0].Boolean,"fg":"WasResetButtonPressed","box":function ($v) { return Bridge.box($v, System.Boolean, System.Boolean.toString);}},"s":{"a":1,"n":"set_WasResetButtonPressed","t":8,"p":[$n[0].Boolean],"rt":$n[0].Void,"fs":"WasResetButtonPressed"},"fn":"WasResetButtonPressed"},{"a":1,"n":"_joystickRadius","t":4,"rt":$n[0].Single,"sn":"_joystickRadius","ro":true,"box":function ($v) { return Bridge.box($v, System.Single, System.Single.format, System.Single.getHashCode);}},{"a":1,"n":"_pressPosition","t":4,"rt":$n[1].Vector2,"sn":"_pressPosition"},{"a":1,"backing":true,"n":"<DragEnded>k__BackingField","t":4,"rt":$n[0].Boolean,"sn":"DragEnded","box":function ($v) { return Bridge.box($v, System.Boolean, System.Boolean.toString);}},{"a":1,"backing":true,"n":"<DragStarted>k__BackingField","t":4,"rt":$n[0].Boolean,"sn":"DragStarted","box":function ($v) { return Bridge.box($v, System.Boolean, System.Boolean.toString);}},{"a":1,"backing":true,"n":"<IsPressed>k__BackingField","t":4,"rt":$n[0].Boolean,"sn":"IsPressed","box":function ($v) { return Bridge.box($v, System.Boolean, System.Boolean.toString);}},{"a":1,"backing":true,"n":"<PitchInput>k__BackingField","t":4,"rt":$n[0].Single,"sn":"PitchInput","box":function ($v) { return Bridge.box($v, System.Single, System.Single.format, System.Single.getHashCode);}},{"a":1,"backing":true,"n":"<PointerPosition>k__BackingField","t":4,"rt":$n[1].Vector2,"sn":"PointerPosition"},{"a":1,"backing":true,"n":"<TurnInput>k__BackingField","t":4,"rt":$n[0].Single,"sn":"TurnInput","box":function ($v) { return Bridge.box($v, System.Single, System.Single.format, System.Single.getHashCode);}},{"a":1,"backing":true,"n":"<WasResetButtonPressed>k__BackingField","t":4,"rt":$n[0].Boolean,"sn":"WasResetButtonPressed","box":function ($v) { return Bridge.box($v, System.Boolean, System.Boolean.toString);}}]}; }, $n);
    /*NewPlaneBehaviour.PlaneInput end.*/

    /*NewPlaneBehaviour.PlaneSlingshot start.*/
    $m("NewPlaneBehaviour.PlaneSlingshot", function () { return {"att":1048577,"a":2,"m":[{"a":2,"n":".ctor","t":1,"p":[$n[1].Transform,$n[2].PlaneInput,$n[1].Camera,$n[0].Single,$n[0].Single,$n[0].Single,$n[0].Single,$n[0].Single,$n[0].Single,$n[0].Single,$n[0].Single],"pi":[{"n":"transform","pt":$n[1].Transform,"ps":0},{"n":"input","pt":$n[2].PlaneInput,"ps":1},{"n":"camera","pt":$n[1].Camera,"ps":2},{"n":"grabRadius","pt":$n[0].Single,"ps":3},{"n":"minPullRadius","pt":$n[0].Single,"ps":4},{"n":"maxPullRadius","pt":$n[0].Single,"ps":5},{"n":"maxPullDistance","pt":$n[0].Single,"ps":6},{"n":"maxNoseRaiseAngle","pt":$n[0].Single,"ps":7},{"n":"pullReactionSpeed","pt":$n[0].Single,"ps":8},{"n":"minLaunchSpeed","pt":$n[0].Single,"ps":9},{"n":"maxLaunchSpeed","pt":$n[0].Single,"ps":10}],"sn":"ctor"},{"a":1,"n":"CalculatePlaneScreenPosition","t":8,"sn":"CalculatePlaneScreenPosition","rt":$n[1].Vector2},{"a":1,"n":"CanGrab","t":8,"sn":"CanGrab","rt":$n[0].Boolean,"box":function ($v) { return Bridge.box($v, System.Boolean, System.Boolean.toString);}},{"a":1,"n":"FollowPull","t":8,"pi":[{"n":"deltaTime","pt":$n[0].Single,"ps":0}],"sn":"FollowPull","rt":$n[0].Void,"p":[$n[0].Single]},{"a":1,"n":"Release","t":8,"sn":"Release","rt":$n[0].Void},{"a":2,"n":"Reset","t":8,"sn":"Reset","rt":$n[0].Void},{"a":2,"n":"Tick","t":8,"pi":[{"n":"deltaTime","pt":$n[0].Single,"ps":0}],"sn":"Tick","rt":$n[0].Void,"p":[$n[0].Single]},{"a":1,"n":"UpdatePull","t":8,"sn":"UpdatePull","rt":$n[0].Void},{"a":2,"n":"IsPulling","t":16,"rt":$n[0].Boolean,"g":{"a":2,"n":"get_IsPulling","t":8,"rt":$n[0].Boolean,"fg":"IsPulling","box":function ($v) { return Bridge.box($v, System.Boolean, System.Boolean.toString);}},"s":{"a":1,"n":"set_IsPulling","t":8,"p":[$n[0].Boolean],"rt":$n[0].Void,"fs":"IsPulling"},"fn":"IsPulling"},{"a":2,"n":"PullAmount","t":16,"rt":$n[0].Single,"g":{"a":2,"n":"get_PullAmount","t":8,"rt":$n[0].Single,"fg":"PullAmount","box":function ($v) { return Bridge.box($v, System.Single, System.Single.format, System.Single.getHashCode);}},"s":{"a":1,"n":"set_PullAmount","t":8,"p":[$n[0].Single],"rt":$n[0].Void,"fs":"PullAmount"},"fn":"PullAmount"},{"a":1,"n":"_camera","t":4,"rt":$n[1].Camera,"sn":"_camera","ro":true},{"a":1,"n":"_grabRadius","t":4,"rt":$n[0].Single,"sn":"_grabRadius","ro":true,"box":function ($v) { return Bridge.box($v, System.Single, System.Single.format, System.Single.getHashCode);}},{"a":1,"n":"_input","t":4,"rt":$n[2].PlaneInput,"sn":"_input","ro":true},{"a":1,"n":"_isActive","t":4,"rt":$n[0].Boolean,"sn":"_isActive","box":function ($v) { return Bridge.box($v, System.Boolean, System.Boolean.toString);}},{"a":1,"n":"_maxLaunchSpeed","t":4,"rt":$n[0].Single,"sn":"_maxLaunchSpeed","ro":true,"box":function ($v) { return Bridge.box($v, System.Single, System.Single.format, System.Single.getHashCode);}},{"a":1,"n":"_maxNoseRaiseAngle","t":4,"rt":$n[0].Single,"sn":"_maxNoseRaiseAngle","ro":true,"box":function ($v) { return Bridge.box($v, System.Single, System.Single.format, System.Single.getHashCode);}},{"a":1,"n":"_maxPullDistance","t":4,"rt":$n[0].Single,"sn":"_maxPullDistance","ro":true,"box":function ($v) { return Bridge.box($v, System.Single, System.Single.format, System.Single.getHashCode);}},{"a":1,"n":"_maxPullRadius","t":4,"rt":$n[0].Single,"sn":"_maxPullRadius","ro":true,"box":function ($v) { return Bridge.box($v, System.Single, System.Single.format, System.Single.getHashCode);}},{"a":1,"n":"_minLaunchSpeed","t":4,"rt":$n[0].Single,"sn":"_minLaunchSpeed","ro":true,"box":function ($v) { return Bridge.box($v, System.Single, System.Single.format, System.Single.getHashCode);}},{"a":1,"n":"_minPullAmount","t":4,"rt":$n[0].Single,"sn":"_minPullAmount","box":function ($v) { return Bridge.box($v, System.Single, System.Single.format, System.Single.getHashCode);}},{"a":1,"n":"_minPullRadius","t":4,"rt":$n[0].Single,"sn":"_minPullRadius","ro":true,"box":function ($v) { return Bridge.box($v, System.Single, System.Single.format, System.Single.getHashCode);}},{"a":1,"n":"_pullDirection","t":4,"rt":$n[1].Vector3,"sn":"_pullDirection"},{"a":1,"n":"_pullReactionSpeed","t":4,"rt":$n[0].Single,"sn":"_pullReactionSpeed","ro":true,"box":function ($v) { return Bridge.box($v, System.Single, System.Single.format, System.Single.getHashCode);}},{"a":1,"n":"_startPosition","t":4,"rt":$n[1].Vector3,"sn":"_startPosition","ro":true},{"a":1,"n":"_transform","t":4,"rt":$n[1].Transform,"sn":"_transform","ro":true},{"a":2,"n":"Shot","t":2,"ad":{"a":2,"n":"add_Shot","t":8,"pi":[{"n":"value","pt":Function,"ps":0}],"sn":"addShot","rt":$n[0].Void,"p":[Function]},"r":{"a":2,"n":"remove_Shot","t":8,"pi":[{"n":"value","pt":Function,"ps":0}],"sn":"removeShot","rt":$n[0].Void,"p":[Function]}},{"a":1,"backing":true,"n":"<IsPulling>k__BackingField","t":4,"rt":$n[0].Boolean,"sn":"IsPulling","box":function ($v) { return Bridge.box($v, System.Boolean, System.Boolean.toString);}},{"a":1,"backing":true,"n":"<PullAmount>k__BackingField","t":4,"rt":$n[0].Single,"sn":"PullAmount","box":function ($v) { return Bridge.box($v, System.Single, System.Single.format, System.Single.getHashCode);}}]}; }, $n);
    /*NewPlaneBehaviour.PlaneSlingshot end.*/

    }});
