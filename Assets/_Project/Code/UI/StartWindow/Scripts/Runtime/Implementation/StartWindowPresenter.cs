using System;
using System.Collections;
using System.Collections.Generic;
using DistanceCounterSystem;
using PlaneBehaviour;
using UnityEngine;
using UnityEngine.UI;
using UpgradeSystem;
using WalletSystem;
using Plane = PlaneBehaviour.Plane;

namespace StartWindowUI
{
    public sealed class StartWindowPresenter : IDisposable
    {
        private readonly StartWindow _window;
        private readonly Wallet _wallet;
        private readonly UpgradeService _upgradeService;
        private readonly Plane _plane;
        private readonly DistanceCounter _distanceCounter;

        private readonly Dictionary<RectTransform, float> _startPositionsX = new Dictionary<RectTransform, float>();
        private readonly Dictionary<RectTransform, Coroutine> _shakes = new Dictionary<RectTransform, Coroutine>();
        private readonly Dictionary<RectTransform, Coroutine> _punches = new Dictionary<RectTransform, Coroutine>();
        private readonly Dictionary<Image, Coroutine> _flashes = new Dictionary<Image, Coroutine>();

        private Coroutine _handRoutine;
        private readonly Image _pullHandImage;
        private readonly float _pullHandAlpha;

        private Coroutine _pullHandRoutine;
        private Coroutine _speedometerRoutine;
        private Coroutine _distanceRoutine;
        private bool _hasUpgraded;
        private int _completedRuns;
        private int _shownSpeed = -1;
        private int _shownDistance = -1;

        public StartWindowPresenter(StartWindow window, Wallet wallet, UpgradeService upgradeService, Plane plane, 
            DistanceCounter distanceCounter)
        {
            _window = window;
            _wallet = wallet;
            _upgradeService = upgradeService;
            _plane = plane;
            _distanceCounter = distanceCounter;

            _pullHandImage = _window.PullHand.GetComponent<Image>();
            _pullHandAlpha = _pullHandImage.color.a;

            _wallet.Changed += OnWalletChanged;
            _upgradeService.Bought += OnUpgradeBought;
            _plane.PullStarted += OnPullStarted;
            _plane.PullCanceled += OnPullCanceled;
            _plane.Launched += OnPlaneLaunched;
            _plane.Stopped += OnPlaneStopped;
            _plane.PlaneFlight.VelocityChanged += OnVelocityChanged;
            _plane.PlaneFlight.Landed += OnPlaneLanded;
            _plane.PositionChanged += OnPlanePositionChanged;

            _window.Speedometer.CanvasGroup.alpha = 0f;
            _window.DistanceIndicator.CanvasGroup.alpha = 0f;

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

            _window.SoundButton.onClick.AddListener(OnSoundClicked);
            _window.DownloadButton.onClick.AddListener(OnDownloadClicked);

            Refresh();
        }
        
        public void Show()
        {
            bool showUpgrades = _completedRuns > 0;

            StopEffects();

            SetStartElementsActive(true);
            _window.gameObject.SetActive(true);
            SetSpeed(0f);
            _window.CoinsPanel.gameObject.SetActive(true);
            _window.UpgradesGroup.SetActive(showUpgrades);
            _window.HintGroup.SetActive(showUpgrades == false);
            FadeDistanceIndicator(0f);

            Refresh();

            _window.StartCoroutine(Pulse());
            
            bool showHand = showUpgrades && _hasUpgraded == false;
            _window.Hand.gameObject.SetActive(showHand);
            _window.UpgradesTitle.SetActive(_hasUpgraded == false);

            if (showHand)
            {
                _handRoutine = _window.StartCoroutine(MoveHand());
            }

            bool showPullHand = showUpgrades == false;
            _window.PullHand.gameObject.SetActive(showPullHand);

            if (showPullHand)
            {
                _pullHandRoutine = _window.StartCoroutine(MovePullHand());
            }
        }
        
        public void Hide()
        {
            StopEffects();

            SetStartElementsActive(false);
        }

        public void HideUpgrades()
        {
            if (_handRoutine != null)
            {
                _window.StopCoroutine(_handRoutine);
                _handRoutine = null;
            }

            if (_pullHandRoutine != null)
            {
                _window.StopCoroutine(_pullHandRoutine);
                _pullHandRoutine = null;
            }

            if (_window.PullHand.gameObject.activeSelf)
            {
                _pullHandRoutine = _window.StartCoroutine(HidePullHand());
            }

            _window.Hand.gameObject.SetActive(false);
            _window.UpgradesGroup.SetActive(false);
            _window.CoinsPanel.gameObject.SetActive(false);
        }

        public void Dispose()
        {
            _wallet.Changed -= OnWalletChanged;
            _upgradeService.Bought -= OnUpgradeBought;
            _plane.PullStarted -= OnPullStarted;
            _plane.PullCanceled -= OnPullCanceled;
            _plane.Launched -= OnPlaneLaunched;
            _plane.Stopped -= OnPlaneStopped;
            _plane.PlaneFlight.VelocityChanged -= OnVelocityChanged;
            _plane.PlaneFlight.Landed -= OnPlaneLanded;
            _plane.PositionChanged -= OnPlanePositionChanged;

            _window.SoundButton.onClick.RemoveListener(OnSoundClicked);
            _window.DownloadButton.onClick.RemoveListener(OnDownloadClicked);

            for (int index = 0; index < _window.UpgradeViews.Length; index++)
            {
                _window.UpgradeViews[index].Button.onClick.RemoveAllListeners();
            }
        }

        private void OnPullStarted()
        {
            HideUpgrades();
            FadeSpeedometer(1f);
        }

        private void OnPullCanceled()
        {
            Show();
            FadeSpeedometer(0f);
        }

        private void OnPlaneLaunched()
        {
            Hide();
            FadeDistanceIndicator(1f);
        }

        private void OnPlaneStopped()
        {
            _completedRuns++;
            FadeSpeedometer(0f);
            FadeDistanceIndicator(0f);
        }

        private void OnVelocityChanged(Vector3 velocity)
        {
            SetSpeed(velocity.magnitude * _window.Speedometer.SpeedPerMeterPerSecond);
        }

        private void OnPlaneLanded(Vector3 velocity)
        {
            SetSpeed(0f);
        }

        private void OnPlanePositionChanged(Vector3 position)
        {
            int distance = Mathf.FloorToInt(_distanceCounter.CalculateDistance(position));

            if (distance == _shownDistance)
            {
                return;
            }

            _shownDistance = distance;
            _window.DistanceIndicator.Text.text = distance + "m";
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

        private void OnSoundClicked()
        {
            PlayPunch((RectTransform)_window.SoundButton.transform);
        }

        private void OnDownloadClicked()
        {
            PlayPunch((RectTransform)_window.DownloadButton.transform);
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

        private IEnumerator MovePullHand()
        {
            RectTransform hand = _window.PullHand;

            float fade = _window.PullHandFadeDuration;
            float press = _window.HandClickDuration;
            float move = _window.PullHandMoveDuration;

            float pressStart = fade * 0.5f;
            float moveStart = pressStart + press * 0.6f;
            float releaseStart = moveStart + move * 0.8f;
            float fadeOutStart = releaseStart + press * 0.5f;
            float cycle = fadeOutStart + fade + _window.HandPauseDuration;

            while (true)
            {
                Vector3 start = _plane.Camera.WorldToScreenPoint(_plane.PlaneRoot.position);
                start.z = 0f;
                Vector3 end = start + Vector3.down * _window.PullHandDistance * hand.lossyScale.y;
                float time = 0f;

                while (time < cycle)
                {
                    float alpha = GetProgress(time, 0f, fade) * (1f - GetProgress(time, fadeOutStart, fade));
                    float pressed = GetProgress(time, pressStart, press) - GetProgress(time, releaseStart, press);

                    hand.position = Vector3.Lerp(start, end, GetProgress(time, moveStart, move));
                    hand.localScale = Vector3.one * Mathf.Lerp(1f, _window.HandClickScale, pressed);
                    SetAlpha(_pullHandImage, alpha * _pullHandAlpha);

                    time += Time.deltaTime;
                    yield return null;
                }
            }
        }

        private static float GetProgress(float time, float start, float duration)
        {
            return Mathf.SmoothStep(0f, 1f, Mathf.Clamp01((time - start) / duration));
        }

        private IEnumerator HidePullHand()
        {
            yield return FadeImage(_pullHandImage, _pullHandImage.color.a, 0f, _window.PullHandFadeDuration);

            _window.PullHand.gameObject.SetActive(false);
            _pullHandRoutine = null;
        }

        private static IEnumerator FadeImage(Image target, float from, float to, float duration)
        {
            float time = 0f;

            while (time < duration)
            {
                SetAlpha(target, Mathf.Lerp(from, to, Mathf.SmoothStep(0f, 1f, time / duration)));

                time += Time.deltaTime;
                yield return null;
            }

            SetAlpha(target, to);
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
            _pullHandRoutine = null;
            _window.Hand.localScale = Vector3.one;
            _window.PullHand.localScale = Vector3.one;
            SetAlpha(_pullHandImage, _pullHandAlpha);
            _window.SoundButton.transform.localScale = Vector3.one;
            _window.DownloadButton.transform.localScale = Vector3.one;

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

        private void FadeSpeedometer(float alpha)
        {
            Speedometer speedometer = _window.Speedometer;

            Fade(speedometer, speedometer.CanvasGroup, alpha, speedometer.FadeDuration, ref _speedometerRoutine);
        }

        private void FadeDistanceIndicator(float alpha)
        {
            DistanceIndicator indicator = _window.DistanceIndicator;

            Fade(indicator, indicator.CanvasGroup, alpha, indicator.FadeDuration, ref _distanceRoutine);
        }

        private static void Fade(MonoBehaviour host, CanvasGroup target, float alpha, float duration, 
            ref Coroutine routine)
        {
            if (routine != null)
            {
                host.StopCoroutine(routine);
            }

            routine = host.StartCoroutine(FadeCanvasGroup(target, alpha, duration));
        }

        private static IEnumerator FadeCanvasGroup(CanvasGroup target, float alpha, float duration)
        {
            float startAlpha = target.alpha;
            float time = 0f;

            while (time < duration)
            {
                target.alpha = Mathf.Lerp(startAlpha, alpha, Mathf.SmoothStep(0f, 1f, time / duration));

                time += Time.deltaTime;
                yield return null;
            }

            target.alpha = alpha;
        }

        private bool IsPersistent(Transform element)
        {
            return element == _window.Speedometer.transform
                || element == _window.DistanceIndicator.transform
                || element == _window.SoundButton.transform
                || element == _window.DownloadButton.transform;
        }

        private void SetStartElementsActive(bool isActive)
        {
            Transform window = _window.transform;

            for (int index = 0; index < window.childCount; index++)
            {
                Transform child = window.GetChild(index);

                if (IsPersistent(child) == false)
                {
                    child.gameObject.SetActive(isActive);
                }
            }
        }

        private void SetSpeed(float speed)
        {
            Speedometer speedometer = _window.Speedometer;

            float progress = Mathf.Clamp01(speed / speedometer.MaxSpeed);
            float angle = Mathf.Lerp(speedometer.MinAngle, speedometer.MaxAngle, progress);

            speedometer.Needle.localRotation = Quaternion.Euler(0f, 0f, -angle);

            int roundedSpeed = Mathf.RoundToInt(speed);

            if (roundedSpeed == _shownSpeed)
            {
                return;
            }

            _shownSpeed = roundedSpeed;
            speedometer.SpeedText.text = roundedSpeed.ToString();
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
