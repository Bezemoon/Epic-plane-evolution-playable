using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace StartWindowUI
{
    [RequireComponent(typeof(Image))]
    public sealed class PullTutorialHand : MonoBehaviour
    {
        [SerializeField] private float _distance = 300f;
        [SerializeField] private float _moveDuration = 0.6f;
        [SerializeField] private float _fadeDuration = 0.2f;
        [SerializeField] private float _clickDuration = 0.25f;
        [SerializeField] private float _clickScale = 0.8f;
        [SerializeField] private float _pauseDuration = 0.35f;

        private Image _image;
        private Coroutine _routine;
        private float _baseAlpha = -1f;

        public void Play(Camera camera, Transform target)
        {
            Stop();

            gameObject.SetActive(true);

            _routine = StartCoroutine(Animate(camera, target));
        }

        public void FadeOut()
        {
            StopRoutine();

            _routine = StartCoroutine(AnimateFadeOut());
        }

        public void Stop()
        {
            StopRoutine();

            transform.localScale = Vector3.one;
            SetAlpha(GetBaseAlpha());
            gameObject.SetActive(false);
        }

        private void StopRoutine()
        {
            if (_routine == null)
            {
                return;
            }

            StopCoroutine(_routine);
            _routine = null;
        }

        private IEnumerator Animate(Camera camera, Transform target)
        {
            float baseAlpha = GetBaseAlpha();
            float pressStart = _fadeDuration * 0.5f;
            float moveStart = pressStart + _clickDuration * 0.6f;
            float releaseStart = moveStart + _moveDuration * 0.8f;
            float fadeOutStart = releaseStart + _clickDuration * 0.5f;
            float cycle = fadeOutStart + _fadeDuration + _pauseDuration;

            while (true)
            {
                Vector3 start = camera.WorldToScreenPoint(target.position);
                start.z = 0f;
                Vector3 end = start + Vector3.down * _distance * transform.lossyScale.y;
                float time = 0f;

                while (time < cycle)
                {
                    float alpha = GetProgress(time, 0f, _fadeDuration) * (1f - GetProgress(time, fadeOutStart, _fadeDuration));
                    float pressed = GetProgress(time, pressStart, _clickDuration) - GetProgress(time, releaseStart, _clickDuration);

                    transform.position = Vector3.Lerp(start, end, GetProgress(time, moveStart, _moveDuration));
                    transform.localScale = Vector3.one * Mathf.Lerp(1f, _clickScale, pressed);
                    SetAlpha(alpha * baseAlpha);

                    time += Time.deltaTime;
                    yield return null;
                }
            }
        }

        private IEnumerator AnimateFadeOut()
        {
            float startAlpha = GetImage().color.a;
            float time = 0f;

            while (time < _fadeDuration)
            {
                SetAlpha(Mathf.Lerp(startAlpha, 0f, Mathf.SmoothStep(0f, 1f, time / _fadeDuration)));

                time += Time.deltaTime;
                yield return null;
            }

            _routine = null;

            Stop();
        }

        private float GetProgress(float time, float start, float duration)
        {
            return Mathf.SmoothStep(0f, 1f, Mathf.Clamp01((time - start) / duration));
        }

        private float GetBaseAlpha()
        {
            if (_baseAlpha < 0f)
            {
                _baseAlpha = GetImage().color.a;
            }

            return _baseAlpha;
        }

        private Image GetImage()
        {
            if (_image == null)
            {
                _image = GetComponent<Image>();
            }

            return _image;
        }

        private void SetAlpha(float alpha)
        {
            Color color = GetImage().color;
            color.a = alpha;
            GetImage().color = color;
        }
    }
}