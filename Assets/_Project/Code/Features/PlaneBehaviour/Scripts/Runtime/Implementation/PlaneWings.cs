using System.Collections;
using UnityEngine;

namespace PlaneBehaviour
{
    public sealed class PlaneWings : MonoBehaviour
    {
        private const float OvershootStrength = 1.70158f;

        [SerializeField] private Transform[] _wingParts;
        [SerializeField] private Transform _punchTarget;

        [Header("Appear")]
        [SerializeField] private float _partDuration = 0.45f;
        [SerializeField] private float _partDelay = 0.07f;
        [SerializeField] private float _punchScale = 1.12f;
        [SerializeField] private float _punchDuration = 0.35f;

        private Vector3[] _finalPositions;
        private Vector3[] _finalScales;
        private Coroutine _appearCoroutine;
        private bool _isVisible;

        public void Show()
        {
            StopAppear();

            _isVisible = true;

            SetAllPartsProgress(1f);
        }

        public void Hide()
        {
            StopAppear();

            _isVisible = false;

            SetAllPartsProgress(0f);
        }

        public void PlayAppear()
        {
            if (_isVisible)
            {
                return;
            }

            _isVisible = true;
            _appearCoroutine = StartCoroutine(AnimateAppear());
        }

        private void SetAllPartsProgress(float progress)
        {
            for (int index = 0; index < _wingParts.Length; index++)
            {
                SetPartProgress(index, progress);
            }

            SetPunchProgress(0f);
        }

        private void StopAppear()
        {
            if (_appearCoroutine == null)
            {
                return;
            }

            StopCoroutine(_appearCoroutine);
            _appearCoroutine = null;
        }

        private IEnumerator AnimateAppear()
        {
            float lastPartDelay = _partDelay * (_wingParts.Length - 1);
            float totalDuration = lastPartDelay + _partDuration;
            float elapsedTime = 0f;

            while (elapsedTime < totalDuration)
            {
                elapsedTime += Time.deltaTime;

                for (int index = 0; index < _wingParts.Length; index++)
                {
                    float partTime = (elapsedTime - _partDelay * index) / _partDuration;
                    SetPartProgress(index, CalculateEaseOutBack(Mathf.Clamp01(partTime)));
                }

                SetPunchProgress(Mathf.Clamp01(elapsedTime / _punchDuration));

                yield return null;
            }

            SetAllPartsProgress(1f);

            _appearCoroutine = null;
        }

        private float CalculateEaseOutBack(float time)
        {
            float shifted = time - 1f;
            float curve = OvershootStrength + 1f;

            return 1f + curve * shifted * shifted * shifted + OvershootStrength * shifted * shifted;
        }

        private void SetPartProgress(int partIndex, float progress)
        {
            RememberFinalPlacement();

            Transform part = _wingParts[partIndex];

            part.gameObject.SetActive(progress > 0f);
            part.localPosition = _finalPositions[partIndex] * progress;
            part.localScale = _finalScales[partIndex] * progress;
        }

        private void SetPunchProgress(float progress)
        {
            float scale = Mathf.Lerp(1f, _punchScale, Mathf.Sin(progress * Mathf.PI));

            _punchTarget.localScale = Vector3.one * scale;
        }
        
        private void RememberFinalPlacement()
        {
            if (_finalPositions != null)
            {
                return;
            }

            _finalPositions = new Vector3[_wingParts.Length];
            _finalScales = new Vector3[_wingParts.Length];

            for (int index = 0; index < _wingParts.Length; index++)
            {
                _finalPositions[index] = _wingParts[index].localPosition;
                _finalScales[index] = _wingParts[index].localScale;
            }
        }
    }
}