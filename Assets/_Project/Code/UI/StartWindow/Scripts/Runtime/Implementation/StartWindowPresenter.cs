using System;
using DistanceCounterSystem;
using PlaneBehaviour;
using UnityEngine;
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

        private bool _hasUpgraded;
        private int _completedRuns;

        public StartWindowPresenter(StartWindow window, Wallet wallet, UpgradeService upgradeService, Plane plane, 
            DistanceCounter distanceCounter)
        {
            _window = window;
            _wallet = wallet;
            _upgradeService = upgradeService;
            _plane = plane;
            _distanceCounter = distanceCounter;

            _wallet.Changed += OnWalletChanged;
            _upgradeService.Bought += OnUpgradeBought;
            _plane.PullStarted += OnPullStarted;
            _plane.PullCanceled += OnPullCanceled;
            _plane.Launched += OnPlaneLaunched;
            _plane.Stopped += OnPlaneStopped;
            _plane.PlaneFlight.VelocityChanged += OnVelocityChanged;
            _plane.PlaneFlight.Landed += OnPlaneLanded;
            _plane.PositionChanged += OnPlanePositionChanged;

            _window.Speedometer.HideInstantly();
            _window.DistanceIndicator.HideInstantly();

            for (int index = 0; index < _window.UpgradeViews.Length; index++)
            {
                UpgradeView view = _window.UpgradeViews[index];
                view.Button.onClick.AddListener(() => OnUpgradeClicked(view));
            }

            _window.SoundButton.onClick.AddListener(OnSoundClicked);
            _window.DownloadButton.onClick.AddListener(OnDownloadClicked);

            Refresh();
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

        public void Show()
        {
            bool showUpgrades = _completedRuns > 0;
            bool showUpgradeHand = showUpgrades && _hasUpgraded == false;

            _window.ResetEffects();
            _window.SetStartElementsActive(true);
            _window.gameObject.SetActive(true);
            _window.Speedometer.SetVelocity(0f);
            _window.DistanceIndicator.Hide();
            _window.CoinsPanel.SetActive(true);
            _window.UpgradesGroup.SetActive(showUpgrades);
            _window.UpgradesTitle.SetActive(_hasUpgraded == false);
            _window.HintGroup.SetActive(showUpgrades == false);
            _window.StartPulse();

            Refresh();

            if (showUpgradeHand)
            {
                _window.UpgradeHand.Play(_window.UpgradeViews);
            }
            else
            {
                _window.UpgradeHand.Stop();
            }

            if (showUpgrades)
            {
                _window.PullHand.Stop();
            }
            else
            {
                _window.PullHand.Play(_plane.Camera, _plane.PlaneRoot);
            }
        }

        public void Hide()
        {
            _window.ResetEffects();
            _window.SetStartElementsActive(false);
        }

        public void HideUpgrades()
        {
            _window.UpgradeHand.Stop();

            if (_window.PullHand.gameObject.activeSelf)
            {
                _window.PullHand.FadeOut();
            }

            _window.UpgradesGroup.SetActive(false);
            _window.CoinsPanel.SetActive(false);
        }

        private void OnPullStarted()
        {
            HideUpgrades();
            _window.Speedometer.Show();
        }

        private void OnPullCanceled()
        {
            Show();
            _window.Speedometer.Hide();
        }

        private void OnPlaneLaunched()
        {
            Hide();
            _window.DistanceIndicator.Show();
        }

        private void OnPlaneStopped()
        {
            _completedRuns++;
            _window.Speedometer.Hide();
            _window.DistanceIndicator.Hide();
        }

        private void OnVelocityChanged(Vector3 velocity)
        {
            _window.Speedometer.SetVelocity(velocity.magnitude);
        }

        private void OnPlaneLanded(Vector3 velocity)
        {
            _window.Speedometer.SetVelocity(0f);
        }

        private void OnPlanePositionChanged(Vector3 position)
        {
            _window.DistanceIndicator.SetDistance(Mathf.FloorToInt(_distanceCounter.CalculateDistance(position)));
        }

        private void OnWalletChanged(int amount)
        {
            Refresh();
        }

        private void OnUpgradeBought(UpgradeType type)
        {
            _hasUpgraded = true;

            _window.UpgradeHand.Stop();
            _window.UpgradesTitle.SetActive(false);

            Refresh();
        }

        private void OnUpgradeClicked(UpgradeView view)
        {
            if (_upgradeService.GetUpgrade(view.Type).IsMax)
            {
                return;
            }

            if (_upgradeService.TryBuy(view.Type))
            {
                view.PlayPurchased();
                return;
            }

            view.PlayRejected();
            _window.ShakeCoins();
        }

        private void OnSoundClicked()
        {
            _window.PunchSoundButton();
        }

        private void OnDownloadClicked()
        {
            _window.PunchDownloadButton();
        }

        private void Refresh()
        {
            _window.SetCoins(_wallet.Amount);

            for (int index = 0; index < _window.UpgradeViews.Length; index++)
            {
                UpgradeView view = _window.UpgradeViews[index];
                Upgrade upgrade = _upgradeService.GetUpgrade(view.Type);
                bool isAvailable = upgrade.IsMax == false && _wallet.Amount >= upgrade.Price;

                view.Refresh(upgrade, isAvailable);
            }
        }
    }
}