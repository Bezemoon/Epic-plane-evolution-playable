using System.Collections;
using System.Collections.Generic;
using DistanceCounterSystem;
using FlightControlPanel;
using LunaIntegration;
using PlaneBehaviour;
using ResultFlagSystem;
using ResultWindow;
using StartWindowUI;
using UnityEngine;
using UpgradeSystem;
using Utils;
using WalletSystem;
using Plane = PlaneBehaviour.Plane;

namespace EntryPoints
{
    public class GameEntryPoint : MonoBehaviour
    {
        [SerializeField] private Plane _plane;
        [SerializeField] private PlaneConfig _planeConfig;
        
        [Space]
        [SerializeField] private UpgradeConfig[] _upgradeConfigs;

        [Space] 
        [SerializeField] private Transform _distanceStartPoint;
        
        [Space]
        [SerializeField] private float _coinsPerMeter = 1f;
        [SerializeField] private float _resultDelay = 1f;
        [SerializeField] private int _runsToPlay = 3;
        
        [Space]
        [SerializeField] private float _finishDistance = 300f;
        [SerializeField] private Transform _finishLine;
        [SerializeField] private ParticleEffect _finishConfetti;
        
        [Space]
        [SerializeField] private ResultFlag _resultFlag;
        [SerializeField] private Vector3 _resultFlagOffset;

        [Space]
        [SerializeField] private StartWindow _startWindow;
        [SerializeField] private ResultWindowView _resultWindow;
        [SerializeField] private FlightControlPanelView _flightStick;

        private DistanceCounter _distanceCounter;
        private Wallet _wallet;
        private UpgradeService _upgradeService;
        private LunaPlayable _lunaPlayable;
        private StartWindowPresenter _startWindowPresenter;
        private ResultWindowPresenter _resultWindowPresenter;
        private FlightControlPanelPresenter _flightStickPresenter;

        private WaitForSeconds _waitForResult;
        private Coroutine _resultCoroutine;
        private bool _isFinished;
        private bool _isFirstRunSkipped;

        private void Awake()
        {
            _wallet = new Wallet();

            _lunaPlayable = new LunaPlayable();
            
            InitializeUpgrades();
            InitializePlane();

            _distanceCounter = new DistanceCounter(_distanceStartPoint.position);
            _startWindowPresenter = new StartWindowPresenter(_startWindow, _wallet, _upgradeService, _plane, 
                _distanceCounter, _lunaPlayable);
 
            _flightStickPresenter = new FlightControlPanelPresenter(_flightStick, _plane);
            _resultWindowPresenter = new ResultWindowPresenter(_resultWindow, _wallet, _plane, _resultFlag, 
                _lunaPlayable, _runsToPlay - 1);
 
            _waitForResult = new WaitForSeconds(_resultDelay);
            
            _resultFlag.Hide();
            PlaceFinishLine();
 
            _plane.Launched += OnPlaneLaunched;
            _plane.Stopped += OnPlaneStopped;
            _plane.PositionChanged += OnPlanePositionChanged;

            ShowStartWindow();
        }

        private void OnDestroy()
        {
            if (_resultCoroutine != null)
            {
                StopCoroutine(_resultCoroutine);
                
                _resultCoroutine = null;
            }
            
            _resultWindowPresenter.Dispose();
            _flightStickPresenter.Dispose();
            _startWindowPresenter.Dispose();
            
            _plane.Launched -= OnPlaneLaunched;
            _plane.Stopped -= OnPlaneStopped;
            _plane.PositionChanged -= OnPlanePositionChanged;
        }
        
        private void InitializeUpgrades()
        {
            Dictionary<UpgradeType, Upgrade> upgrades = new Dictionary<UpgradeType, Upgrade>();
 
            for (int index = 0; index < _upgradeConfigs.Length; index++)
            {
                UpgradeConfig upgradeConfig = _upgradeConfigs[index];
                upgrades.Add(upgradeConfig.Type, new Upgrade(upgradeConfig));
            }
 
            _upgradeService = new UpgradeService(_wallet, upgrades);
        }

        private void InitializePlane()
        {
            Transform planeRoot = _plane.PlaneRoot;
 
            var planeInput = new PlaneInput(_planeConfig.JoystickRadius);
            var planeFuel = new PlaneFuel();
            var planeFlight = new PlaneFlight(_planeConfig.PlaneFlightConfig, planeRoot, planeInput, planeFuel);
            var planeSlide = new PlaneSlide(planeRoot, _planeConfig.SlideDeceleration);
            var planeSlingshot = new PlaneSlingshot(_planeConfig.SlingshotConfig, planeRoot, planeInput, _plane.Camera, 
                _upgradeService.GetUpgrade(UpgradeType.Slingshot));
            
            var planeClimb = new PlaneClimb(_planeConfig.PlaneClimbConfig, planeRoot, _plane.RampStartPoint, 
                _plane.RampControlPoint, _plane.RampEndPoint);
            
            var cameraFollow = new PlaneCameraFollow(_planeConfig.PlaneCameraFollowConfig, 
                _plane.Camera.transform, planeRoot);
 
            _plane.Initialize(_planeConfig, planeInput, planeSlingshot, planeClimb, planeFlight, planeSlide, 
                cameraFollow, planeFuel, _upgradeService.GetUpgrade(UpgradeType.Plane));
        }

        private void ShowStartWindow()
        {
            _startWindowPresenter.Show();
        }

        private void PlaceFinishLine()
        {
            Vector3 position = _finishLine.position;
            _finishLine.position = new Vector3(position.x, position.y, _distanceStartPoint.position.z + _finishDistance);
        }

        private void OnPlanePositionChanged(Vector3 position)
        {
            if (_isFinished)
            {
                return;
            }

            float distance = _distanceCounter.CalculateDistance(position);

            if (distance < _finishDistance)
            {
                return;
            }

            _isFinished = true;
            _finishConfetti.Play();
            ShowResult(_finishDistance, true);
        }

        private void OnPlaneLaunched()
        {
            _isFinished = false;
        }

        private void OnPlaneStopped()
        {
            if (_isFinished)
            {
                return;
            }

            float distance = _distanceCounter.CalculateDistance(_plane.PlaneRoot.position);

            _resultFlag.Place(_plane.PlaneRoot.position + _resultFlagOffset);
            ShowResult(distance, false);
        }

        private void ShowResult(float distance, bool isVictory)
        {
            Upgrade incomeUpgrade = _upgradeService.GetUpgrade(UpgradeType.Income);
            int roundedDistance = Mathf.RoundToInt(distance);
            int distanceCoins = Mathf.RoundToInt(distance * _coinsPerMeter);
            int totalCoins = Mathf.RoundToInt(distanceCoins * incomeUpgrade.Value);
 
            _resultCoroutine = StartCoroutine(RunResultTimer(roundedDistance, incomeUpgrade.Value, totalCoins, 
                isVictory));
        }
        
        private IEnumerator RunResultTimer(int distance, float multiplier, int total, bool isVictory)
        {
            yield return _waitForResult;
            
            _resultCoroutine = null;
 
            if (_isFirstRunSkipped == false && isVictory == false)
            {
                _isFirstRunSkipped = true;
                _wallet.Add(total);
                _resultFlag.Hide();
                _plane.Restore();

                yield break;
            }
 
            _resultWindowPresenter.Show(distance, multiplier, total, isVictory);
        }
    }
}