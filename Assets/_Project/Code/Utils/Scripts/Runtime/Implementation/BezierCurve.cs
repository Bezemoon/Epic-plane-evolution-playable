using UnityEngine;

namespace Utils
{
    public class BezierCurve
    {
        private const int SampleCount = 16;

        private readonly float[] _sampleLengths = new float[SampleCount + 1];

        private Vector3 _start;
        private Vector3 _control;
        private Vector3 _end;

        public float Length { get; private set; }

        public void Set(Vector3 start, Vector3 control, Vector3 end)
        {
            _start = start;
            _control = control;
            _end = end;

            MeasureLength();
        }

        public Vector3 CalculatePosition(float distance)
        {
            return CalculatePositionAtPoint(CalculatePoint(distance));
        }

        public Vector3 CalculateDirection(float distance)
        {
            float point = CalculatePoint(distance);
            Vector3 direction = (1f - point) * (_control - _start) + point * (_end - _control);

            if (direction.sqrMagnitude <= 0f)
            {
                return (_end - _start).normalized;
            }

            return direction.normalized;
        }
        
        private void MeasureLength()
        {
            Vector3 previousPosition = _start;
            _sampleLengths[0] = 0f;

            for (int sampleIndex = 1; sampleIndex <= SampleCount; sampleIndex++)
            {
                Vector3 position = CalculatePositionAtPoint((float)sampleIndex / SampleCount);
                _sampleLengths[sampleIndex] =
                    _sampleLengths[sampleIndex - 1] + Vector3.Distance(previousPosition, position);
                previousPosition = position;
            }

            Length = _sampleLengths[SampleCount];
        }
        
        private float CalculatePoint(float distance)
        {
            for (int sampleIndex = 1; sampleIndex <= SampleCount; sampleIndex++)
            {
                if (distance > _sampleLengths[sampleIndex])
                {
                    continue;
                }

                float sampleLength = _sampleLengths[sampleIndex] - _sampleLengths[sampleIndex - 1];
                float sampleProgress =
                    sampleLength <= 0f ? 0f : (distance - _sampleLengths[sampleIndex - 1]) / sampleLength;
                return (sampleIndex - 1 + sampleProgress) / SampleCount;
            }

            return 1f;
        }

        private Vector3 CalculatePositionAtPoint(float point)
        {
            float remaining = 1f - point;
            return remaining * remaining * _start
                   + 2f * remaining * point * _control
                   + point * point * _end;
        }
    }
}