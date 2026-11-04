using TMPro;
using UnityEngine;
using Utils;

namespace StartWindowUI
{
    public class DistanceIndicator : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private FadeEffect _fade;

        private int _shownDistance = -1;

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

        public void SetDistance(int distance)
        {
            if (distance == _shownDistance)
            {
                return;
            }

            _shownDistance = distance;
            _text.text = distance + "m";
        }
    }
}