using System.Collections;
using UnityEngine;

namespace Utils
{
    [RequireComponent(typeof(CanvasGroup))]
    public sealed class FadeEffect : MonoBehaviour
    {
        [SerializeField] private float _duration = 0.25f;

        private CanvasGroup _canvasGroup;
        private Coroutine _routine;

        public void FadeTo(float alpha)
        {
            Stop();

            _routine = StartCoroutine(Animate(alpha));
        }

        public void SetAlpha(float alpha)
        {
            Stop();

            GetCanvasGroup().alpha = alpha;
        }

        private void Stop()
        {
            if (_routine == null)
            {
                return;
            }

            StopCoroutine(_routine);
            _routine = null;
        }

        private IEnumerator Animate(float alpha)
        {
            CanvasGroup canvasGroup = GetCanvasGroup();
            float startAlpha = canvasGroup.alpha;
            float time = 0f;

            while (time < _duration)
            {
                canvasGroup.alpha = Mathf.Lerp(startAlpha, alpha, Mathf.SmoothStep(0f, 1f, time / _duration));

                time += Time.deltaTime;
                yield return null;
            }

            canvasGroup.alpha = alpha;
            _routine = null;
        }

        private CanvasGroup GetCanvasGroup()
        {
            if (_canvasGroup == null)
            {
                _canvasGroup = GetComponent<CanvasGroup>();
            }

            return _canvasGroup;
        }
    }
}