using System.Collections;
using UnityEngine;

namespace ResultFlagSystem
{
    public class ResultFlag : MonoBehaviour
    {
        [SerializeField] private Transform _transform;
        [SerializeField] private float _appearDuration = 0.5f;

        private Vector3 _finalScale;
        private Coroutine _appearCoroutine;

        private void Awake()
        {
            _finalScale = _transform.localScale;
        }

        public void Place(Vector3 position)
        {
            _transform.position = new Vector3(position.x, _transform.position.y, position.z);
            _transform.gameObject.SetActive(true);

            if (_appearCoroutine != null)
            {
                StopCoroutine(_appearCoroutine);
            }

            _appearCoroutine = StartCoroutine(AnimateAppear());
        }

        public void Hide()
        {
            if (_appearCoroutine != null)
            {
                StopCoroutine(_appearCoroutine);

                _appearCoroutine = null;
            }

            _transform.gameObject.SetActive(false);
        }

        private IEnumerator AnimateAppear()
        {
            float time = 0f;

            while (time < _appearDuration)
            {
                float progress = EaseOutBack(time / _appearDuration);
                _transform.localScale = _finalScale * progress;

                time += Time.deltaTime;
                yield return null;
            }

            _transform.localScale = _finalScale;
            _appearCoroutine = null;
        }

        private static float EaseOutBack(float progress)
        {
            const float overshoot = 1.70158f;

            float shifted = progress - 1f;
            return 1f + (overshoot + 1f) * shifted * shifted * shifted + overshoot * shifted * shifted;
        }
    }
}
