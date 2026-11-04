using System.Collections;
using UnityEngine;

namespace StartWindowUI
{
    public sealed class UpgradeTutorialHand : MonoBehaviour
    {
        [SerializeField] private float _clickDuration = 0.5f;
        [SerializeField] private float _clickScale = 0.9f;
        [SerializeField] private float _pauseDuration = 1f;

        private Coroutine _routine;

        public void Play(UpgradeView view)
        {
            Stop();

            gameObject.SetActive(true);

            _routine = StartCoroutine(Animate(view));
        }

        public void Stop()
        {
            if (_routine != null)
            {
                StopCoroutine(_routine);
                _routine = null;
            }

            transform.localScale = Vector3.one;
            gameObject.SetActive(false);
        }

        private IEnumerator Animate(UpgradeView view)
        {
            transform.position = GetCenter(view.ButtonRect);

            while (true)
            {
                yield return Click(view);
                yield return Wait(_pauseDuration);
            }
        }

        private IEnumerator Click(UpgradeView view)
        {
            float half = _clickDuration * 0.5f;
            float time = 0f;
            bool isPressed = false;

            while (time < _clickDuration)
            {
                float progress = time < half ? time / half : 1f - (time - half) / half;
                transform.localScale = Vector3.one * Mathf.Lerp(1f, _clickScale, progress);

                if (isPressed == false && time >= half)
                {
                    isPressed = true;
                    view.PlayPress();
                }

                time += Time.deltaTime;
                yield return null;
            }

            transform.localScale = Vector3.one;
        }

        private IEnumerator Wait(float duration)
        {
            float time = 0f;

            while (time < duration)
            {
                time += Time.deltaTime;
                yield return null;
            }
        }

        private Vector3 GetCenter(RectTransform target)
        {
            return target.TransformPoint(target.rect.center);
        }
    }
}