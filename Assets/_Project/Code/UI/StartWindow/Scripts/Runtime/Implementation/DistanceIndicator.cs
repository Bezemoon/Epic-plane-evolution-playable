using TMPro;
using UnityEngine;

namespace StartWindowUI
{
    public class DistanceIndicator : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _fadeDuration = 0.25f;

        public TMP_Text Text => _text;
        public CanvasGroup CanvasGroup => _canvasGroup;
        public float FadeDuration => _fadeDuration;
    }
}
