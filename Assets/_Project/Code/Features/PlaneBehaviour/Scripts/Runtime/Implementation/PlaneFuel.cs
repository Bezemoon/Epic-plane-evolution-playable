using UnityEngine;

namespace PlaneBehaviour
{
    public class PlaneFuel
    {
        public float Amount { get; private set; }

        public bool IsEmpty => Amount <= 0f;

        public void Fill(float amount)
        {
            Amount = amount;
        }

        public void Reduce(float amount)
        {
            Amount = Mathf.Max(0f, Amount - amount);
        }
    }
}