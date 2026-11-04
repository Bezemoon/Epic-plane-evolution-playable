using System.Collections;
using System.Collections.Generic;
using DistanceCounterSystem;
using FlightControlPanel;
using PlaneBehaviour;
using ResultFlagSystem;
using ResultWindow;
using StartWindowUI;
using UnityEngine;
using UpgradeSystem;
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
        
        [Space]
        [SerializeField] private float _finishDistance = 300f;
        [SerializeField] private Transform _finishLine;
        
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
        private StartWindowPresenter _startWindowPresenter;
        private ResultWindowPresenter _resultWindowPresenter;
        private FlightControlPanelPresenter _flightStickPresenter;

        private WaitForSeconds _waitForResult;
        private Coroutine _resultCoroutine;
        private int _pendingCoins;
        private bool _isFinished;

        private void Awake()
        {
            _wallet = new Wallet();
            
            InitializeUpgrades();
            InitializePlane();

            _distanceCounter = new DistanceCounter(_distanceStartPoint.position);
            _startWindowPresenter = new StartWindowPresenter(_startWindow, _wallet, _upgradeService, _plane, 
                _distanceCounter);
 
            _flightStickPresenter = new FlightControlPanelPresenter(_flightStick, _plane);
            _resultWindowPresenter = new ResultWindowPresenter(_resultWindow);
            _resultWindowPresenter.NextClicked += OnNextClicked;
 
            _waitForResult = new WaitForSeconds(_resultDelay);
            
            _resultFlag.Hide();
            PlaceFinishLine();
 
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
            
            _resultWindowPresenter.NextClicked -= OnNextClicked;
            _resultWindowPresenter.Dispose();
            _flightStickPresenter.Dispose();
            _startWindowPresenter.Dispose();
            
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
            ShowResult(_finishDistance, true);
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
            _pendingCoins = Mathf.RoundToInt(distanceCoins * incomeUpgrade.Value);
 
            _resultCoroutine = StartCoroutine(RunResultTimer(roundedDistance, incomeUpgrade.Value, _pendingCoins, 
                isVictory));
        }
        
        private IEnumerator RunResultTimer(int distance, float multiplier, int total, bool isVictory)
        {
            yield return _waitForResult;
            
            _resultCoroutine = null;
 
            _resultWindowPresenter.Show(distance, multiplier, total, isVictory);
        }

        private void OnNextClicked()
        {
            _wallet.Add(_pendingCoins);
            _pendingCoins = 0;
            _isFinished = false;
 
            _resultWindowPresenter.Hide();
            _plane.Restore();
            _resultFlag.Hide();
            ShowStartWindow();
        }
    }
}
