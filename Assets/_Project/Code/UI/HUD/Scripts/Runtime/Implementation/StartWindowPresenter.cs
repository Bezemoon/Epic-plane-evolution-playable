using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UpgradeSystem;
using WalletSystem;

namespace HUD
{
    public sealed class StartWindowPresenter : IDisposable
    {
        private readonly StartWindow _window;
        private readonly Wallet _wallet;
        private readonly UpgradeService _upgradeService;

        private readonly Dictionary<RectTransform, float> _startPositionsX = new Dictionary<RectTransform, float>();
        private readonly Dictionary<RectTransform, Coroutine> _shakes = new Dictionary<RectTransform, Coroutine>();
        private readonly Dictionary<RectTransform, Coroutine> _punches = new Dictionary<RectTransform, Coroutine>();
        private readonly Dictionary<Image, Coroutine> _flashes = new Dictionary<Image, Coroutine>();

        private Coroutine _handRoutine;
        private bool _hasUpgraded;

        public StartWindowPresenter(StartWindow window, Wallet wallet, UpgradeService upgradeService)
        {
            _window = window;
            _wallet = wallet;
            _upgradeService = upgradeService;

            _wallet.Changed += OnWalletChanged;
            _upgradeService.Bought += OnUpgradeBought;

            for (int index = 0; index < _window.UpgradeViews.Length; index++)
            {
                UpgradeView view = _window.UpgradeViews[index];
                view.Button.onClick.AddListener(() => OnUpgradeClicked(view));

                for (int contentIndex = 0; contentIndex < view.ButtonContent.Length; contentIndex++)
                {
                    RectTransform content = view.ButtonContent[contentIndex];
                    _startPositionsX[content] = content.anchoredPosition.x;
                }
            }

            _startPositionsX[_window.CoinsPanel] = _window.CoinsPanel.anchoredPosition.x;

            Refresh();
        }
        
        public void Show(bool showUpgrades)
        {
            StopEffects();

            _window.gameObject.SetActive(true);
            _window.HintGroup.SetActive(showUpgrades == false);
            _window.UpgradesGroup.SetActive(showUpgrades);

            Refresh();

            _window.StartCoroutine(Pulse());
            
            bool showHand = showUpgrades && _hasUpgraded == false;
            _window.Hand.gameObject.SetActive(showHand);
            _window.UpgradesTitle.SetActive(_hasUpgraded == false);

            if (showHand)
            {
                _handRoutine = _window.StartCoroutine(MoveHand());
            }
        }

        public void Hide()
        {
            StopEffects();

            _window.gameObject.SetActive(false);
        }

        public void Dispose()
        {
            _wallet.Changed -= OnWalletChanged;
            _upgradeService.Bought -= OnUpgradeBought;

            for (int index = 0; index < _window.UpgradeViews.Length; index++)
            {
                _window.UpgradeViews[index].Button.onClick.RemoveAllListeners();
            }
        }

        private void OnWalletChanged(int amount)
        {
            Refresh();
        }

        private void OnUpgradeBought(UpgradeType type)
        {
            _hasUpgraded = true;

            if (_handRoutine != null)
            {
                _window.StopCoroutine(_handRoutine);
                _handRoutine = null;
            }

            _window.Hand.gameObject.SetActive(false);
            _window.UpgradesTitle.SetActive(false);

            Refresh();
        }

        private void OnUpgradeClicked(UpgradeView view)
        {
            UpgradeType type = view.Type;

            if (_upgradeService.GetUpgrade(type).IsMax)
            {
                return;
            }

            if (_upgradeService.TryBuy(type))
            {
                PlayFlash(view.FlashImage);
                return;
            }

            PlayPunch(view.ButtonRect);

            for (int index = 0; index < view.ButtonContent.Length; index++)
            {
                PlayShake(view.ButtonContent[index]);
            }

            PlayShake(_window.CoinsPanel);
        }

        private void PlayShake(RectTransform target)
        {
            if (_shakes.TryGetValue(target, out Coroutine running))
            {
                _window.StopCoroutine(running);
            }

            _shakes[target] = _window.StartCoroutine(Shake(target));
        }

        private void PlayPunch(RectTransform target)
        {
            if (_punches.TryGetValue(target, out Coroutine running))
            {
                _window.StopCoroutine(running);
            }

            _punches[target] = _window.StartCoroutine(Punch(target));
        }

        private void PlayFlash(Image target)
        {
            if (_flashes.TryGetValue(target, out Coroutine running))
            {
                _window.StopCoroutine(running);
            }

            _flashes[target] = _window.StartCoroutine(Flash(target));
        }

        private IEnumerator Shake(RectTransform target)
        {
            float startX = _startPositionsX[target];
            float time = 0f;

            while (time < _window.ShakeDuration)
            {
                float offset = Mathf.Sin(time * _window.ShakeSpeed) * _window.ShakeAmplitude * (1f - time / _window.ShakeDuration);
                SetPositionX(target, startX + offset);

                time += Time.deltaTime;
                yield return null;
            }

            SetPositionX(target, startX);
        }
        
        private IEnumerator Punch(RectTransform target)
        {
            float time = 0f;

            while (time < _window.PunchDuration)
            {
                float progress = EaseOutBack(time / _window.PunchDuration);
                target.localScale = Vector3.one * Mathf.LerpUnclamped(_window.PunchStartScale, 1f, progress);

                time += Time.deltaTime;
                yield return null;
            }

            target.localScale = Vector3.one;
        }
        
        private IEnumerator Pulse()
        {
            float time = 0f;

            while (true)
            {
                float wave = 0.5f + 0.5f * Mathf.Sin(time * _window.PulseSpeed);
                SetPulseScale(1f + wave * _window.PulseAmplitude);

                time += Time.deltaTime;
                yield return null;
            }
        }
        
        private IEnumerator MoveHand()
        {
            RectTransform hand = _window.Hand;
            int index = 0;

            hand.position = GetCenter(_window.UpgradeViews[0].ButtonRect);

            while (true)
            {
                UpgradeView view = _window.UpgradeViews[index];

                yield return MoveHandTo(view.ButtonRect);
                yield return ClickHand(view);
                yield return Wait(_window.HandPauseDuration);

                index = (index + 1) % _window.UpgradeViews.Length;
            }
        }

        private IEnumerator MoveHandTo(RectTransform target)
        {
            RectTransform hand = _window.Hand;
            Vector3 start = hand.position;
            Vector3 end = GetCenter(target);
            float time = 0f;

            while (time < _window.HandMoveDuration)
            {
                float progress = Mathf.SmoothStep(0f, 1f, time / _window.HandMoveDuration);
                hand.position = Vector3.Lerp(start, end, progress);

                time += Time.deltaTime;
                yield return null;
            }

            hand.position = end;
        }

        private IEnumerator ClickHand(UpgradeView view)
        {
            RectTransform hand = _window.Hand;
            float half = _window.HandClickDuration * 0.5f;
            float time = 0f;
            bool isPressed = false;

            while (time < _window.HandClickDuration)
            {
                float progress = time < half ? time / half : 1f - (time - half) / half;
                hand.localScale = Vector3.one * Mathf.Lerp(1f, _window.HandClickScale, progress);

                if (isPressed == false && time >= half)
                {
                    isPressed = true;
                    PlayPunch(view.ButtonRect);
                }

                time += Time.deltaTime;
                yield return null;
            }

            hand.localScale = Vector3.one;
        }

        // Центр кнопки в мировых координатах (у кнопки pivot не в центре)
        private static Vector3 GetCenter(RectTransform target)
        {
            return target.TransformPoint(target.rect.center);
        }

        private static IEnumerator Wait(float duration)
        {
            float time = 0f;

            while (time < duration)
            {
                time += Time.deltaTime;
                yield return null;
            }
        }

        private IEnumerator Flash(Image target)
        {
            float time = 0f;

            while (time < _window.FlashDuration)
            {
                SetAlpha(target, Mathf.Lerp(_window.FlashAlpha, 0f, time / _window.FlashDuration));

                time += Time.deltaTime;
                yield return null;
            }

            SetAlpha(target, 0f);
        }
        
        private void StopEffects()
        {
            _window.StopAllCoroutines();
            _shakes.Clear();
            _punches.Clear();
            _flashes.Clear();

            foreach (KeyValuePair<RectTransform, float> start in _startPositionsX)
            {
                SetPositionX(start.Key, start.Value);
            }

            SetPulseScale(1f);

            _handRoutine = null;
            _window.Hand.localScale = Vector3.one;

            for (int index = 0; index < _window.UpgradeViews.Length; index++)
            {
                SetAlpha(_window.UpgradeViews[index].FlashImage, 0f);
                _window.UpgradeViews[index].ButtonRect.localScale = Vector3.one;
            }
        }

        private void Refresh()
        {
            _window.CoinsText.text = _wallet.Amount.ToString();

            for (int index = 0; index < _window.UpgradeViews.Length; index++)
            {
                RefreshUpgrade(_window.UpgradeViews[index]);
            }
        }

        private void RefreshUpgrade(UpgradeView view)
        {
            Upgrade upgrade = _upgradeService.GetUpgrade(view.Type);

            bool isMax = upgrade.IsMax;
            bool isAvailable = isMax == false && _wallet.Amount >= upgrade.Price;

            view.LevelText.text = (upgrade.PurchaseCount + 1).ToString();
            view.PriceText.text = upgrade.Price.ToString();

            if (view.MultiplierText != null)
            {
                view.MultiplierText.text = "X" + Mathf.Round(upgrade.Value * 10f) / 10f;
                view.MultiplierText.color = isMax ? view.MaxColor : view.MultiplierColor;
            }

            for (int index = 0; index < view.StageDots.Length; index++)
            {
                view.StageDots[index].color = index < upgrade.Stage ? view.StageFilledColor : view.StageEmptyColor;
            }

            for (int index = 0; index < view.HiddenWhenMax.Length; index++)
            {
                view.HiddenWhenMax[index].SetActive(isMax == false);
            }

            view.MaxLabel.SetActive(isMax);
            view.PictureImage.color = isMax ? view.MaxColor : view.PictureColor;
            view.ButtonImage.sprite = isAvailable ? view.ButtonAvailableSprite : view.ButtonUnavailableSprite;
            view.ButtonImage.color = isAvailable ? Color.white : view.ButtonUnavailableColor;
        }

        private void SetPulseScale(float scale)
        {
            for (int index = 0; index < _window.PulseTargets.Length; index++)
            {
                _window.PulseTargets[index].localScale = Vector3.one * scale;
            }
        }

        private static float EaseOutBack(float progress)
        {
            const float overshoot = 1.70158f;

            float shifted = progress - 1f;
            return 1f + (overshoot + 1f) * shifted * shifted * shifted + overshoot * shifted * shifted;
        }

        private static void SetPositionX(RectTransform target, float x)
        {
            Vector2 position = target.anchoredPosition;
            position.x = x;
            target.anchoredPosition = position;
        }

        private static void SetAlpha(Image target, float alpha)
        {
            Color color = target.color;
            color.a = alpha;
            target.color = color;
        }
    }
}
