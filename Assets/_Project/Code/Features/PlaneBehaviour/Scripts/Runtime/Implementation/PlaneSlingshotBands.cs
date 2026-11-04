using System.Collections;
using UnityEngine;

namespace PlaneBehaviour
{
    public sealed class PlaneSlingshotBands : MonoBehaviour
    {
        [SerializeField] private Transform _leftPostTip;
        [SerializeField] private Transform _rightPostTip;
        [SerializeField] private Transform _leftBand;
        [SerializeField] private Transform _rightBand;

        [Header("Bands")]
        [SerializeField] private Vector3 _attachOffset = new Vector3(0f, 0.3f, -0.3f);
        [SerializeField] private float _restThickness = 0.16f;
        [SerializeField] private float _stretchedThickness = 0.06f;
        [SerializeField] private float _maxExtraLength = 4f;
        [SerializeField] private float _hangDepth = 1f;

        [Header("Recoil")]
        [SerializeField] private float _recoilDuration = 1.2f;
        [SerializeField] private float _recoilDamping = 6f;
        [SerializeField] private float _recoilFrequency = 14f;

        private Transform _planeRoot;
        private Coroutine _recoilCoroutine;
        private Vector3 _attachPoint;
        private float _restLength;
        private bool _isAttached;

        public void Initialize(Transform planeRoot)
        {
            _planeRoot = planeRoot;
            _restLength = Vector3.Distance(_leftPostTip.position, CalculateAttachPoint());

            Attach();
        }

        public void Tick()
        {
            float postZ = (_leftPostTip.position.z + _rightPostTip.position.z) * 0.5f;

            if (_planeRoot.position.z < postZ)
            {
                Attach();
                return;
            }

            if (_isAttached)
            {
                Detach();
            }
        }

        private void Attach()
        {
            StopRecoil();

            _isAttached = true;
            _attachPoint = CalculateAttachPoint();

            DrawBands(_attachPoint);
        }

        private void Detach()
        {
            _isAttached = false;
            _recoilCoroutine = StartCoroutine(PlayRecoil(_attachPoint));
        }

        private void StopRecoil()
        {
            if (_recoilCoroutine == null)
            {
                return;
            }

            StopCoroutine(_recoilCoroutine);
            _recoilCoroutine = null;
        }

        private IEnumerator PlayRecoil(Vector3 fromPoint)
        {
            Vector3 hangPoint = CalculateHangPoint();
            float elapsedTime = 0f;

            while (elapsedTime < _recoilDuration)
            {
                elapsedTime += Time.deltaTime;

                float decay = Mathf.Exp(-_recoilDamping * elapsedTime);
                float swing = Mathf.Cos(_recoilFrequency * elapsedTime);

                DrawBands(hangPoint + (fromPoint - hangPoint) * (decay * swing));

                yield return null;
            }

            DrawBands(hangPoint);

            _recoilCoroutine = null;
        }

        private Vector3 CalculateAttachPoint()
        {
            return _planeRoot.position + _planeRoot.rotation * _attachOffset;
        }

        private Vector3 CalculateHangPoint()
        {
            Vector3 middle = (_leftPostTip.position + _rightPostTip.position) * 0.5f;
            return middle + Vector3.down * _hangDepth;
        }

        private void DrawBands(Vector3 endPoint)
        {
            float length = Vector3.Distance(_leftPostTip.position, endPoint);
            float stretch = Mathf.Clamp01((length - _restLength) / _maxExtraLength);
            float thickness = Mathf.Lerp(_restThickness, _stretchedThickness, stretch);

            DrawBand(_leftBand, _leftPostTip.position, endPoint, thickness);
            DrawBand(_rightBand, _rightPostTip.position, endPoint, thickness);
        }

        private void DrawBand(Transform band, Vector3 start, Vector3 end, float thickness)
        {
            Vector3 direction = end - start;

            if (direction.sqrMagnitude < Mathf.Epsilon)
            {
                return;
            }

            band.position = (start + end) * 0.5f;
            band.rotation = Quaternion.LookRotation(direction);
            band.localScale = new Vector3(thickness, thickness, direction.magnitude);
        }
    }
}