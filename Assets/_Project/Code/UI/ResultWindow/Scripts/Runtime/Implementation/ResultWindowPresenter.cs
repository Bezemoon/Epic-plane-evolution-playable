using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ResultWindow
{
    public sealed class ResultWindowPresenter : IDisposable
    {
        private readonly ResultWindowView _window;

        private int _bestDistance;

        public ResultWindowPresenter(ResultWindowView window)
        {
            _window = window;

            _window.CollectButton.onClick.AddListener(OnCollectClicked);
            _window.NextButton.onClick.AddListener(OnNextClicked);

            _window.gameObject.SetActive(false);
        }

        public event Action NextClicked;

        public void Show(int distance, float multiplier, int total, bool isVictory)
        {
            bool isRecord = _bestDistance > 0 && distance > _bestDistance;
            _bestDistance = Mathf.Max(_bestDistance, distance);

            _window.gameObject.SetActive(true);
            _window.StopAllCoroutines();
            _window.StartCoroutine(PlayShow(distance, multiplier, total, isRecord, isVictory));
        }

        public void Hide()
        {
            _window.StopAllCoroutines();
            _window.gameObject.SetActive(false);
        }

        public void Dispose()
        {
            _window.CollectButton.onClick.RemoveListener(OnCollectClicked);
            _window.NextButton.onClick.RemoveListener(OnNextClicked);
        }

        private void OnCollectClicked()
        {
            _window.StartCoroutine(Punch((RectTransform)_window.CollectButton.transform, 0.8f));
        }

        private void OnNextClicked()
        {
            NextClicked?.Invoke();
        }

        private void Reset(float multiplier, bool isVictory)
        {
            _window.TitleText.text = isVictory ? _window.VictoryTitle : _window.DefeatTitle;
            _window.DistanceText.text = "0";
            _window.TotalText.text = "0";
            _window.MultiplierText.text = "X" + Mathf.Round(multiplier * 10f) / 10f;

            _window.Title.localScale = Vector3.zero;
            _window.DistanceRow.localScale = Vector3.one;
            _window.MultiplierRow.localScale = Vector3.zero;
            _window.TotalPanel.localScale = Vector3.one;
            _window.RecordStamp.localScale = Vector3.zero;
            _window.CollectButton.transform.localScale = Vector3.zero;
            _window.NextButton.transform.localScale = Vector3.zero;

            for (int index = 0; index < _window.Stars.Length; index++)
            {
                _window.Stars[index].color = _window.StarEmptyColor;
                _window.Stars[index].transform.localScale = Vector3.one;
            }
        }

        private IEnumerator PlayShow(int distance, float multiplier, int total, bool isRecord, bool isVictory)
        {
            Reset(multiplier, isVictory);

            yield return Punch(_window.Title, 0f);
            yield return CountUp(_window.DistanceText, distance);
            yield return Punch(_window.DistanceRow, 0.9f);
            yield return Punch(_window.MultiplierRow, 0f);
            yield return Wait(_window.StepPause);

            for (int index = 0; index < _window.Stars.Length; index++)
            {
                if (distance < _window.StarDistances[index])
                {
                    break;
                }

                _window.Stars[index].color = _window.StarFilledColor;

                yield return Punch((RectTransform)_window.Stars[index].transform, 0f);
                yield return Wait(_window.StarInterval);
            }

            if (isRecord)
            {
                yield return Punch(_window.RecordStamp, 0f);
            }

            yield return CountUp(_window.TotalText, total);
            yield return Punch(_window.TotalPanel, 0.9f);
            yield return Punch((RectTransform)_window.CollectButton.transform, 0f);
            yield return Punch((RectTransform)_window.NextButton.transform, 0f);

            yield return Pulse((RectTransform)_window.CollectButton.transform);
        }

        private IEnumerator CountUp(TMPro.TMP_Text target, int value)
        {
            int shown = -1;
            float time = 0f;

            while (time < _window.CountDuration)
            {
                float progress = 1f - Mathf.Pow(1f - time / _window.CountDuration, 3f);
                int current = Mathf.RoundToInt(value * progress);

                if (current != shown)
                {
                    shown = current;
                    target.text = current.ToString();
                }

                time += Time.deltaTime;
                yield return null;
            }

            target.text = value.ToString();
        }

        private IEnumerator Punch(RectTransform target, float startScale)
        {
            float time = 0f;

            while (time < _window.PunchDuration)
            {
                float progress = EaseOutBack(time / _window.PunchDuration);
                target.localScale = Vector3.one * Mathf.LerpUnclamped(startScale, 1f, progress);

                time += Time.deltaTime;
                yield return null;
            }

            target.localScale = Vector3.one;
        }

        private IEnumerator Pulse(RectTransform target)
        {
            float time = 0f;

            while (true)
            {
                float wave = 0.5f + 0.5f * Mathf.Sin(time * _window.PulseSpeed);
                target.localScale = Vector3.one * (1f + wave * _window.PulseAmplitude);

                time += Time.deltaTime;
                yield return null;
            }
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

        private static float EaseOutBack(float progress)
        {
            const float overshoot = 1.70158f;

            float shifted = progress - 1f;
            return 1f + (overshoot + 1f) * shifted * shifted * shifted + overshoot * shifted * shifted;
        }
    }
}
