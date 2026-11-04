using UnityEngine;
using UnityEngine.UI;

namespace Utils
{
    public sealed class AdaptiveCanvasScaler : MonoBehaviour
    {
        private const float MatchWidth = 0f;
        private const float MatchHeight = 1f;

        [SerializeField] private CanvasScaler _scaler;

        private int _appliedWidth;
        private int _appliedHeight;

        private void Update()
        {
            if (Screen.width == _appliedWidth && Screen.height == _appliedHeight)
            {
                return;
            }

            _appliedWidth = Screen.width;
            _appliedHeight = Screen.height;

            ApplyMatch();
        }

        private void ApplyMatch()
        {
            float referenceAspect = _scaler.referenceResolution.x / _scaler.referenceResolution.y;
            float screenAspect = (float)_appliedWidth / _appliedHeight;
            bool isNarrowerThanReference = screenAspect < referenceAspect;

            _scaler.matchWidthOrHeight = isNarrowerThanReference ? MatchWidth : MatchHeight;
        }
    }
}
