using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Utils
{
    [RequireComponent(typeof(Image))]
    public sealed class FlashEffect : MonoBehaviour
    {
        [SerializeField] private float _duration = 0.25f;
        [SerializeField] private float _alpha = 0.85f;

        private Image _image;
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

            SetAlpha(0f);
        }

        private IEnumerator Animate()
        {
            float time = 0f;

            while (time < _duration)
            {
                SetAlpha(Mathf.Lerp(_alpha, 0f, time / _duration));

                time += Time.deltaTime;
                yield return null;
            }

            SetAlpha(0f);
            _routine = null;
        }

        private void SetAlpha(float alpha)
        {
            if (_image == null)
            {
                _image = GetComponent<Image>();
            }

            Color color = _image.color;
            color.a = alpha;
            _image.color = color;
        }
    }
}