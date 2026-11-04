using System.Collections;
using UnityEngine;

namespace StartWindowUI
{
    public sealed class UpgradeTutorialHand : MonoBehaviour
    {
        [SerializeField] private float _moveDuration = 0.6f;
        [SerializeField] private float _clickDuration = 0.25f;
        [SerializeField] private float _clickScale = 0.8f;
        [SerializeField] private float _pauseDuration = 0.35f;

        private Coroutine _routine;

        public void Play(UpgradeView[] views)
        {
            Stop();

            gameObject.SetActive(true);

            _routine = StartCoroutine(Animate(views));
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

        private IEnumerator Animate(UpgradeView[] views)
        {
            int index = 0;

            transform.position = GetCenter(views[0].ButtonRect);

            while (true)
            {
                UpgradeView view = views[index];

                yield return MoveTo(view.ButtonRect);
                yield return Click(view);
                yield return Wait(_pauseDuration);

                index = (index + 1) % views.Length;
            }
        }

        private IEnumerator MoveTo(RectTransform target)
        {
            Vector3 start = transform.position;
            Vector3 end = GetCenter(target);
            float time = 0f;

            while (time < _moveDuration)
            {
                float progress = Mathf.SmoothStep(0f, 1f, time / _moveDuration);
                transform.position = Vector3.Lerp(start, end, progress);

                time += Time.deltaTime;
                yield return null;
            }

            transform.position = end;
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