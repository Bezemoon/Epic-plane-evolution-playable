using TMPro;
using UnityEngine;

namespace HUD
{
    public class StartWindow : MonoBehaviour
    {
        [SerializeField] private TMP_Text _coinsText;
        [SerializeField] private RectTransform _coinsPanel;
        [SerializeField] private GameObject _hintGroup;
        [SerializeField] private GameObject _upgradesGroup;
        [SerializeField] private GameObject _upgradesTitle;
        [SerializeField] private UpgradeView[] _upgradeViews;
        [SerializeField] private RectTransform[] _pulseTargets;
        [SerializeField] private RectTransform _hand;

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

        public TMP_Text CoinsText => _coinsText;
        public RectTransform CoinsPanel => _coinsPanel;
        public GameObject HintGroup => _hintGroup;
        public GameObject UpgradesGroup => _upgradesGroup;
        public GameObject UpgradesTitle => _upgradesTitle;
        public UpgradeView[] UpgradeViews => _upgradeViews;
        public RectTransform[] PulseTargets => _pulseTargets;
        public RectTransform Hand => _hand;

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
    }
}
