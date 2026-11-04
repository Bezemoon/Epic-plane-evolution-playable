using TMPro;
using UnityEngine;

namespace StartWindowUI
{
    public class Speedometer : MonoBehaviour
    {
        [SerializeField] private RectTransform _needle;
        [SerializeField] private TMP_Text _speedText;
        [SerializeField] private CanvasGroup _canvasGroup;

        [Space]
        [SerializeField] private float _maxSpeed = 300f;
        [SerializeField] private float _speedPerMeterPerSecond = 3.6f;
        [SerializeField] private float _minAngle = -135f;
        [SerializeField] private float _maxAngle = 135f;
        [SerializeField] private float _fadeDuration = 0.25f;

        public RectTransform Needle => _needle;
        public TMP_Text SpeedText => _speedText;
        public CanvasGroup CanvasGroup => _canvasGroup;
        public float MaxSpeed => _maxSpeed;
        public float SpeedPerMeterPerSecond => _speedPerMeterPerSecond;
        public float MinAngle => _minAngle;
        public float MaxAngle => _maxAngle;
        public float FadeDuration => _fadeDuration;
    }
}
