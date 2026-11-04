using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UpgradeSystem;

namespace StartWindowUI
{
    public class UpgradeView : MonoBehaviour
    {
        [SerializeField] private UpgradeType _type;
        [SerializeField] private TMP_Text _levelText;
        [SerializeField] private TMP_Text _priceText;
        [SerializeField] private TMP_Text _multiplierText;
        [SerializeField] private Image[] _stageDots;
        [SerializeField] private Image _pictureImage;
        [SerializeField] private Button _button;
        [SerializeField] private Image _buttonImage;
        [SerializeField] private RectTransform _buttonRect;
        [SerializeField] private RectTransform[] _buttonContent;
        [SerializeField] private Image _flashImage;
        [SerializeField] private GameObject[] _hiddenWhenMax;
        [SerializeField] private GameObject _maxLabel;

        [Space]
        [SerializeField] private Color _pictureColor = Color.white;
        [SerializeField] private Color _stageFilledColor = Color.white;
        [SerializeField] private Color _stageEmptyColor = Color.gray;
        [SerializeField] private Color _multiplierColor = Color.yellow;
        [SerializeField] private Color _maxColor = Color.gray;
        [SerializeField] private Sprite _buttonAvailableSprite;
        [SerializeField] private Sprite _buttonUnavailableSprite;
        [SerializeField] private Color _buttonUnavailableColor = Color.gray;

        public UpgradeType Type => _type;
        public TMP_Text LevelText => _levelText;
        public TMP_Text PriceText => _priceText;
        public TMP_Text MultiplierText => _multiplierText;
        public Image[] StageDots => _stageDots;
        public Image PictureImage => _pictureImage;
        public Button Button => _button;
        public Image ButtonImage => _buttonImage;
        public RectTransform ButtonRect => _buttonRect;
        public RectTransform[] ButtonContent => _buttonContent;
        public Image FlashImage => _flashImage;
        public GameObject[] HiddenWhenMax => _hiddenWhenMax;
        public GameObject MaxLabel => _maxLabel;

        public Color PictureColor => _pictureColor;
        public Color StageFilledColor => _stageFilledColor;
        public Color StageEmptyColor => _stageEmptyColor;
        public Color MultiplierColor => _multiplierColor;
        public Color MaxColor => _maxColor;
        public Sprite ButtonAvailableSprite => _buttonAvailableSprite;
        public Sprite ButtonUnavailableSprite => _buttonUnavailableSprite;
        public Color ButtonUnavailableColor => _buttonUnavailableColor;
    }
}
