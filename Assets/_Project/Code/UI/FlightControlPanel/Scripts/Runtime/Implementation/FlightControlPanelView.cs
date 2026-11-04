using UnityEngine;

namespace FlightControlPanel
{
    public class FlightControlPanelView : MonoBehaviour
    {
        [SerializeField] private RectTransform _stick;
        [SerializeField] private RectTransform _knob;
        [SerializeField] private CanvasGroup _canvasGroup;

        [Space]
        [SerializeField] private float _knobRange = 100f;
        [SerializeField] private float _showStartScale = 0.6f;
        [SerializeField] private float _showDuration = 0.12f;
        [SerializeField] private float _hideDuration = 0.2f;

        public RectTransform Root => (RectTransform)transform;
        public RectTransform Stick => _stick;
        public RectTransform Knob => _knob;
        public CanvasGroup CanvasGroup => _canvasGroup;

        public float KnobRange => _knobRange;
        public float ShowStartScale => _showStartScale;
        public float ShowDuration => _showDuration;
        public float HideDuration => _hideDuration;
    }
}
