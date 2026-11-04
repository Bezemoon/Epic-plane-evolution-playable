using TMPro;
using UnityEngine;
using Utils;

namespace StartWindowUI
{
    public class Speedometer : MonoBehaviour
    {
        [SerializeField] private RectTransform _needle;
        [SerializeField] private TMP_Text _speedText;
        [SerializeField] private FadeEffect _fade;

        [Space]
        [SerializeField] private float _maxSpeed = 300f;
        [SerializeField] private float _speedPerMeterPerSecond = 3.6f;
        [SerializeField] private float _minAngle = -135f;
        [SerializeField] private float _maxAngle = 135f;

        private int _shownSpeed = -1;

        public void Show()
        {
            _fade.FadeTo(1f);
        }

        public void Hide()
        {
            _fade.FadeTo(0f);
        }

        public void HideInstantly()
        {
            _fade.SetAlpha(0f);
        }

        public void SetVelocity(float metersPerSecond)
        {
            float speed = metersPerSecond * _speedPerMeterPerSecond;
            float progress = Mathf.Clamp01(speed / _maxSpeed);
            float angle = Mathf.Lerp(_minAngle, _maxAngle, progress);

            _needle.localRotation = Quaternion.Euler(0f, 0f, -angle);

            int roundedSpeed = Mathf.RoundToInt(speed);

            if (roundedSpeed == _shownSpeed)
            {
                return;
            }

            _shownSpeed = roundedSpeed;
            _speedText.text = roundedSpeed.ToString();
        }
    }
}