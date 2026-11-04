using System;
using System.Collections;
using PlaneBehaviour;
using UnityEngine;
using Plane = PlaneBehaviour.Plane;

namespace FlightControlPanel
{
    public sealed class FlightControlPanelPresenter : IDisposable
    {
        private readonly FlightControlPanelView _view;
        private readonly Plane _plane;
        private readonly PlaneInput _input;

        private Vector2 _pressPosition;
        private bool _isLaunched;
        private bool _isFlying;
        private bool _isPressed;
        private bool _isShown;
        private Coroutine _animation;

        public FlightControlPanelPresenter(FlightControlPanelView view, Plane plane)
        {
            _view = view;
            _plane = plane;
            _input = plane.PlaneInput;

            _input.StickPressed += OnStickPressed;
            _input.StickChanged += OnStickChanged;
            _input.StickReleased += OnStickReleased;

            _plane.Launched += OnPlaneLaunched;
            _plane.Stopped += OnPlaneStopped;
            _plane.PlaneFlight.VelocityChanged += OnVelocityChanged;
            _plane.PlaneFlight.Landed += OnPlaneLanded;

            _view.CanvasGroup.alpha = 0f;
        }

        public void Dispose()
        {
            _input.StickPressed -= OnStickPressed;
            _input.StickChanged -= OnStickChanged;
            _input.StickReleased -= OnStickReleased;

            _plane.Launched -= OnPlaneLaunched;
            _plane.Stopped -= OnPlaneStopped;
            _plane.PlaneFlight.VelocityChanged -= OnVelocityChanged;
            _plane.PlaneFlight.Landed -= OnPlaneLanded;
        }

        private void OnStickPressed(Vector2 screenPosition)
        {
            _isPressed = true;
            _pressPosition = screenPosition;

            Refresh();
        }

        private void OnStickChanged(float turn, float pitch)
        {
            if (_isShown == false)
            {
                return;
            }

            Vector2 offset = Vector2.ClampMagnitude(new Vector2(turn, -pitch), 1f);
            _view.Knob.anchoredPosition = offset * _view.KnobRange;
        }

        private void OnStickReleased()
        {
            _isPressed = false;

            Refresh();
        }

        private void OnPlaneLaunched()
        {
            _isLaunched = true;
            _isFlying = false;

            Refresh();
        }

        private void OnVelocityChanged(Vector3 velocity)
        {
            if (_isLaunched == false || _isFlying)
            {
                return;
            }

            _isFlying = true;

            Refresh();
        }

        private void OnPlaneLanded(Vector3 velocity)
        {
            StopFlight();
        }

        private void OnPlaneStopped()
        {
            StopFlight();
        }

        private void StopFlight()
        {
            _isLaunched = false;
            _isFlying = false;

            Refresh();
        }

        private void Refresh()
        {
            bool shouldShow = _isFlying && _isPressed && _plane.HasWings;

            if (shouldShow == _isShown)
            {
                return;
            }

            _isShown = shouldShow;

            if (_animation != null)
            {
                _view.StopCoroutine(_animation);
            }

            if (shouldShow)
            {
                PlaceStick();
                _animation = _view.StartCoroutine(PlayShow());
            }
            else
            {
                _animation = _view.StartCoroutine(PlayHide());
            }
        }

        private void PlaceStick()
        {
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_view.Root, _pressPosition, null, out localPoint);

            _view.Stick.anchoredPosition = localPoint;
            _view.Knob.anchoredPosition = Vector2.zero;
        }

        private IEnumerator PlayShow()
        {
            float startAlpha = _view.CanvasGroup.alpha;
            float time = 0f;

            while (time < _view.ShowDuration)
            {
                float progress = Mathf.SmoothStep(0f, 1f, time / _view.ShowDuration);
                _view.CanvasGroup.alpha = Mathf.Lerp(startAlpha, 1f, progress);
                _view.Stick.localScale = Vector3.one * Mathf.Lerp(_view.ShowStartScale, 1f, progress);

                time += Time.deltaTime;
                yield return null;
            }

            _view.CanvasGroup.alpha = 1f;
            _view.Stick.localScale = Vector3.one;
            _animation = null;
        }

        private IEnumerator PlayHide()
        {
            float startAlpha = _view.CanvasGroup.alpha;
            float time = 0f;

            while (time < _view.HideDuration)
            {
                float progress = Mathf.SmoothStep(0f, 1f, time / _view.HideDuration);
                _view.CanvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, progress);

                time += Time.deltaTime;
                yield return null;
            }

            _view.CanvasGroup.alpha = 0f;
            _animation = null;
        }
    }
}
