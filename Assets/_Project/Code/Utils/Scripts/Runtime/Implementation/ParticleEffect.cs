using UnityEngine;

namespace Utils
{
    public class ParticleEffect : MonoBehaviour
    {
        [SerializeField] private ParticleSystem[] _particles;

        public void Play()
        {
            for (int index = 0; index < _particles.Length; index++)
            {
                _particles[index].Clear();
                _particles[index].Play();
            }
        }

        public void Stop()
        {
            for (int index = 0; index < _particles.Length; index++)
            {
                _particles[index].Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }
        }
    }
}
