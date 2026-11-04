using System.Collections;
using UnityEngine;

namespace Utils
{
    public sealed class PunchEffect : MonoBehaviour
    {
        private const float OvershootStrength = 1.70158f;

        [SerializeField] private float _duration = 0.25f;
        [SerializeField] private float _startScale = 0.8f;

        private Coroutine _routine;

        public void Play()
        {
            Stop();

            _routine = StartCoroutine(Animate());
        }

        public void Stop()
        {
            if (_routine != null)
            {
                StopCoroutine(_routine);
                _routine = null;
            }

            transform.localScale = Vector3.one;
        }

        private IEnumerator Animate()
        {
            float time = 0f;

            while (time < _duration)
            {
                float progress = CalculateEaseOutBack(time / _duration);
                transform.localScale = Vector3.one * Mathf.LerpUnclamped(_startScale, 1f, progress);

                time += Time.deltaTime;
                yield return null;
            }

            transform.localScale = Vector3.one;
            _routine = null;
        }

        private float CalculateEaseOutBack(float progress)
        {
            float shifted = progress - 1f;
            return 1f + (OvershootStrength + 1f) * shifted * shifted * shifted + OvershootStrength * shifted * shifted;
        }
    }
}