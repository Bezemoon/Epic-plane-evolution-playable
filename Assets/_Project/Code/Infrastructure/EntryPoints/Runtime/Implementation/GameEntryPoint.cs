using System.Collections;
using System.Collections.Generic;
using DistanceCounterSystem;
using HUD;
using PlaneBehaviour;
using ResultFlagSystem;
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
        [SerializeField] private float _restartDelay = 2f;
        
        [Space]
        [SerializeField] private ResultFlag _resultFlag;
        [SerializeField] private Vector3 _resultFlagOffset;

        [Space]
        [SerializeField] private StartWindow _startWindow;

        private DistanceCounter _distanceCounter;
        private Wallet _wallet;
        private UpgradeService _upgradeService;
        private StartWindowPresenter _startWindowPresenter;

        private WaitForSeconds _waitForRestart;
        private Coroutine _restartCoroutine;

        private void Awake()
        {
            _wallet = new Wallet();
            
            InitializeUpgrades();
            InitializePlane();

            _startWindowPresenter = new StartWindowPresenter(_startWindow, _wallet, _upgradeService, _plane);
 
            _distanceCounter = new DistanceCounter(_distanceStartPoint.position);
            _waitForRestart = new WaitForSeconds(_restartDelay);
            
            _resultFlag.Hide();
 
            _plane.Stopped += OnPlaneStopped;

            ShowStartWindow();
        }

        private void OnDestroy()
        {
            if (_restartCoroutine != null)
            {
                StopCoroutine(_restartCoroutine);
                
                _restartCoroutine = null;
            }
            
            _startWindowPresenter.Dispose();
            
            _plane.Stopped -= OnPlaneStopped;
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

        private void OnPlaneStopped()
        {
            float distance = _distanceCounter.CalculateDistance(_plane.PlaneRoot.position);

            Upgrade incomeUpgrade= _upgradeService.GetUpgrade(UpgradeType.Income);
            int coins = Mathf.RoundToInt(distance * _coinsPerMeter * incomeUpgrade.Value);
            
            _resultFlag.Place(_plane.PlaneRoot.position + _resultFlagOffset);
 
            _wallet.Add(coins);
            
            _restartCoroutine = StartCoroutine(RunRestartTimer());
 
            Debug.Log("Distance " + distance + " m, coins +" + coins + ", wallet " + _wallet.Amount);
        }
        
        private IEnumerator RunRestartTimer()
        {
            yield return _waitForRestart;
            
            _restartCoroutine = null;
 
            _plane.Restore();
            _resultFlag.Hide();
            ShowStartWindow();
        }
    }
}
