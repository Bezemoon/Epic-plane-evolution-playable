using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace StartWindowUI
{
    public class StartWindow : MonoBehaviour
    {
        [SerializeField] private TMP_Text _coinsText;
        [SerializeField] private RectTransform _coinsPanel;
        [SerializeField] private GameObject _upgradesGroup;
        [SerializeField] private GameObject _upgradesTitle;
        [SerializeField] private GameObject _hintGroup;
        [SerializeField] private UpgradeView[] _upgradeViews;
        [SerializeField] private RectTransform[] _pulseTargets;
        [SerializeField] private RectTransform _hand;
        [SerializeField] private RectTransform _pullHand;
        [SerializeField] private Button _soundButton;
        [SerializeField] private Button _downloadButton;
        [SerializeField] private Speedometer _speedometer;
        [SerializeField] private DistanceIndicator _distanceIndicator;

        [Space]
        [SerializeField] private float _shakeDuration = 0.35f;
        [SerializeField] private float _shakeAmplitude = 14f;
        [SerializeField] private float _shakeSpeed = 60f;
        [SerializeField] private float _flashDuration = 0.25f;
        [SerializeField] private float _flashAlpha = 0.85f;
        [SerializeField] private float _punchDuration = 0.25f;
        [SerializeField] private float _punchStartScale = 0.8f;
        [SerializeField] private float _pulseAmplitude = 0.08f;
        [SerializeField] private float _pulseSpeed = 6f;
        [SerializeField] private float _handMoveDuration = 0.6f;
        [SerializeField] private float _handClickDuration = 0.25f;
        [SerializeField] private float _handClickScale = 0.8f;
        [SerializeField] private float _handPauseDuration = 0.35f;
        [SerializeField] private float _pullHandDistance = 300f;
        [SerializeField] private float _pullHandMoveDuration = 0.9f;
        [SerializeField] private float _pullHandFadeDuration = 0.3f;

        public TMP_Text CoinsText => _coinsText;
        public RectTransform CoinsPanel => _coinsPanel;
        public GameObject UpgradesGroup => _upgradesGroup;
        public GameObject UpgradesTitle => _upgradesTitle;
        public GameObject HintGroup => _hintGroup;
        public UpgradeView[] UpgradeViews => _upgradeViews;
        public RectTransform[] PulseTargets => _pulseTargets;
        public RectTransform Hand => _hand;
        public RectTransform PullHand => _pullHand;
        public Button SoundButton => _soundButton;
        public Button DownloadButton => _downloadButton;
        public Speedometer Speedometer => _speedometer;
        public DistanceIndicator DistanceIndicator => _distanceIndicator;

        public float ShakeDuration => _shakeDuration;
        public float ShakeAmplitude => _shakeAmplitude;
        public float ShakeSpeed => _shakeSpeed;
        public float FlashDuration => _flashDuration;
        public float FlashAlpha => _flashAlpha;
        public float PunchDuration => _punchDuration;
        public float PunchStartScale => _punchStartScale;
        public float PulseAmplitude => _pulseAmplitude;
        public float PulseSpeed => _pulseSpeed;
        public float HandMoveDuration => _handMoveDuration;
        public float HandClickDuration => _handClickDuration;
        public float HandClickScale => _handClickScale;
        public float HandPauseDuration => _handPauseDuration;
        public float PullHandDistance => _pullHandDistance;
        public float PullHandMoveDuration => _pullHandMoveDuration;
        public float PullHandFadeDuration => _pullHandFadeDuration;
    }
}
