using System.Collections;
using UnityEngine;

namespace Utils
{
    public sealed class ShakeEffect : MonoBehaviour
    {
        [SerializeField] private float _duration = 0.35f;
        [SerializeField] private float _amplitude = 14f;
        [SerializeField] private float _speed = 60f;

        private RectTransform _target;
        private Coroutine _routine;
        private float _startX;

        public void Play()
        {
            Stop();

            _routine = StartCoroutine(Animate());
        }

        public void Stop()
        {
            RememberStartPosition();

            if (_routine != null)
            {
                StopCoroutine(_routine);
                _routine = null;
            }

            SetPositionX(_startX);
        }

        private IEnumerator Animate()
        {
            float time = 0f;

            while (time < _duration)
            {
                float offset = Mathf.Sin(time * _speed) * _amplitude * (1f - time / _duration);
                SetPositionX(_startX + offset);

                time += Time.deltaTime;
                yield return null;
            }

            SetPositionX(_startX);
            _routine = null;
        }

        private void RememberStartPosition()
        {
            if (_target != null)
            {
                return;
            }

            _target = (RectTransform)transform;
            _startX = _target.anchoredPosition.x;
        }

        private void SetPositionX(float x)
        {
            Vector2 position = _target.anchoredPosition;
            position.x = x;
            _target.anchoredPosition = position;
        }
    }
}