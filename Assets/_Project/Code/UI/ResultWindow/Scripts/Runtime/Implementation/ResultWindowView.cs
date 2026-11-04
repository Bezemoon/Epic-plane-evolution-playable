using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ResultWindow
{
    public class ResultWindowView : MonoBehaviour
    {
        [SerializeField] private RectTransform _title;
        [SerializeField] private TMP_Text _titleText;
        [SerializeField] private RectTransform _distanceRow;
        [SerializeField] private TMP_Text _distanceText;
        [SerializeField] private RectTransform _multiplierRow;
        [SerializeField] private TMP_Text _multiplierText;
        [SerializeField] private RectTransform _totalPanel;
        [SerializeField] private TMP_Text _totalText;
        [SerializeField] private Image[] _stars;
        [SerializeField] private RectTransform _recordStamp;
        [SerializeField] private Button _collectButton;
        [SerializeField] private Button _nextButton;

        [Space]
        [SerializeField] private string _defeatTitle = "FLIGHT REPORT";
        [SerializeField] private string _victoryTitle = "VICTORY!";

        [Space]
        [SerializeField] private Color _starFilledColor = new Color(1f, 0.8f, 0.15f, 1f);
        [SerializeField] private Color _starEmptyColor = new Color(0f, 0f, 0f, 0.35f);
        [SerializeField] private float[] _starDistances = new float[] { 100f, 200f, 300f };

        [Space]
        [SerializeField] private float _countDuration = 0.8f;
        [SerializeField] private float _punchDuration = 0.3f;
        [SerializeField] private float _starInterval = 0.25f;
        [SerializeField] private float _stepPause = 0.15f;
        [SerializeField] private float _pulseAmplitude = 0.06f;
        [SerializeField] private float _pulseSpeed = 5f;

        public RectTransform Title => _title;
        public TMP_Text TitleText => _titleText;
        public RectTransform DistanceRow => _distanceRow;
        public TMP_Text DistanceText => _distanceText;
        public RectTransform MultiplierRow => _multiplierRow;
        public TMP_Text MultiplierText => _multiplierText;
        public RectTransform TotalPanel => _totalPanel;
        public TMP_Text TotalText => _totalText;
        public Image[] Stars => _stars;
        public RectTransform RecordStamp => _recordStamp;
        public Button CollectButton => _collectButton;
        public Button NextButton => _nextButton;

        public string DefeatTitle => _defeatTitle;
        public string VictoryTitle => _victoryTitle;

        public Color StarFilledColor => _starFilledColor;
        public Color StarEmptyColor => _starEmptyColor;
        public float[] StarDistances => _starDistances;

        public float CountDuration => _countDuration;
        public float PunchDuration => _punchDuration;
        public float StarInterval => _starInterval;
        public float StepPause => _stepPause;
        public float PulseAmplitude => _pulseAmplitude;
        public float PulseSpeed => _pulseSpeed;
    }
}
