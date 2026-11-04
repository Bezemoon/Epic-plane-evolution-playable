using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UpgradeSystem;
using Utils;

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
        [SerializeField] private PunchEffect _buttonPunch;
        [SerializeField] private ShakeEffect[] _contentShakes;
        [SerializeField] private FlashEffect _flash;
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
        public Button Button => _button;
        public RectTransform ButtonRect => _buttonRect;

        public void Refresh(Upgrade upgrade, bool isAvailable)
        {
            bool isMax = upgrade.IsMax;

            _levelText.text = (upgrade.PurchaseCount + 1).ToString();
            _priceText.text = upgrade.Price.ToString();

            if (_multiplierText != null)
            {
                _multiplierText.text = "X" + Mathf.Round(upgrade.Value * 10f) / 10f;
                _multiplierText.color = isMax ? _maxColor : _multiplierColor;
            }

            for (int index = 0; index < _stageDots.Length; index++)
            {
                _stageDots[index].color = index < upgrade.Stage ? _stageFilledColor : _stageEmptyColor;
            }

            for (int index = 0; index < _hiddenWhenMax.Length; index++)
            {
                _hiddenWhenMax[index].SetActive(isMax == false);
            }

            _maxLabel.SetActive(isMax);
            _pictureImage.color = isMax ? _maxColor : _pictureColor;
            _buttonImage.sprite = isAvailable ? _buttonAvailableSprite : _buttonUnavailableSprite;
            _buttonImage.color = isAvailable ? Color.white : _buttonUnavailableColor;
        }

        public void PlayPurchased()
        {
            _flash.Play();
        }

        public void PlayPress()
        {
            _buttonPunch.Play();
        }

        public void PlayRejected()
        {
            _buttonPunch.Play();

            for (int index = 0; index < _contentShakes.Length; index++)
            {
                _contentShakes[index].Play();
            }
        }

        public void ResetEffects()
        {
            _buttonPunch.Stop();
            _flash.Stop();

            for (int index = 0; index < _contentShakes.Length; index++)
            {
                _contentShakes[index].Stop();
            }
        }
    }
}