using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UpgradeSystem;
using Utils;

namespace StartWindowUI
{
    public class StartWindow : MonoBehaviour
    {
        [SerializeField] private TMP_Text _coinsText;
        [SerializeField] private GameObject _coinsPanel;
        [SerializeField] private ShakeEffect _coinsShake;
        [SerializeField] private GameObject _upgradesGroup;
        [SerializeField] private GameObject _upgradesTitle;
        [SerializeField] private GameObject _hintGroup;
        [SerializeField] private UpgradeView[] _upgradeViews;
        [SerializeField] private RectTransform[] _pulseTargets;
        [SerializeField] private UpgradeTutorialHand _upgradeHand;
        [SerializeField] private PullTutorialHand _pullHand;
        [SerializeField] private Button _soundButton;
        [SerializeField] private PunchEffect _soundPunch;
        [SerializeField] private Button _downloadButton;
        [SerializeField] private PunchEffect _downloadPunch;
        [SerializeField] private Speedometer _speedometer;
        [SerializeField] private DistanceIndicator _distanceIndicator;

        [Space]
        [SerializeField] private UpgradeType _tutorialUpgrade = UpgradeType.Plane;

        [Space]
        [SerializeField] private float _pulseAmplitude = 0.08f;
        [SerializeField] private float _pulseSpeed = 4f;

        private Coroutine _pulseRoutine;

        public GameObject CoinsPanel => _coinsPanel;
        public GameObject UpgradesGroup => _upgradesGroup;
        public GameObject UpgradesTitle => _upgradesTitle;
        public GameObject HintGroup => _hintGroup;
        public UpgradeView[] UpgradeViews => _upgradeViews;
        public UpgradeType TutorialUpgrade => _tutorialUpgrade;
        public UpgradeTutorialHand UpgradeHand => _upgradeHand;
        public PullTutorialHand PullHand => _pullHand;
        public Button SoundButton => _soundButton;
        public Button DownloadButton => _downloadButton;
        public Speedometer Speedometer => _speedometer;
        public DistanceIndicator DistanceIndicator => _distanceIndicator;

        public void SetCoins(int amount)
        {
            _coinsText.text = amount.ToString();
        }

        public void ShakeCoins()
        {
            _coinsShake.Play();
        }

        public void PunchSoundButton()
        {
            _soundPunch.Play();
        }

        public void PunchDownloadButton()
        {
            _downloadPunch.Play();
        }

        public void StartPulse()
        {
            StopPulse();

            _pulseRoutine = StartCoroutine(Pulse());
        }

        public void SetStartElementsActive(bool isActive)
        {
            for (int index = 0; index < transform.childCount; index++)
            {
                Transform child = transform.GetChild(index);

                if (IsPersistent(child) == false)
                {
                    child.gameObject.SetActive(isActive);
                }
            }
        }

        public void ResetEffects()
        {
            StopPulse();

            _coinsShake.Stop();
            _soundPunch.Stop();
            _downloadPunch.Stop();
            _upgradeHand.Stop();
            _pullHand.Stop();

            for (int index = 0; index < _upgradeViews.Length; index++)
            {
                _upgradeViews[index].ResetEffects();
            }
        }

        private void StopPulse()
        {
            if (_pulseRoutine != null)
            {
                StopCoroutine(_pulseRoutine);
                _pulseRoutine = null;
            }

            SetPulseScale(1f);
        }

        private IEnumerator Pulse()
        {
            float time = 0f;

            while (true)
            {
                float wave = 0.5f + 0.5f * Mathf.Sin(time * _pulseSpeed);
                SetPulseScale(1f + wave * _pulseAmplitude);

                time += Time.deltaTime;
                yield return null;
            }
        }

        private void SetPulseScale(float scale)
        {
            for (int index = 0; index < _pulseTargets.Length; index++)
            {
                _pulseTargets[index].localScale = Vector3.one * scale;
            }
        }

        private bool IsPersistent(Transform element)
        {
            return element == _speedometer.transform
                || element == _distanceIndicator.transform
                || element == _soundButton.transform
                || element == _downloadButton.transform;
        }
    }
}