using System;
using UnityEngine;

namespace UpgradeSystem
{
    public class Upgrade
    {
        private readonly UpgradeConfig _config;

        public Upgrade(UpgradeConfig config)
        {
            _config = config;
        }
 
        public int PurchaseCount { get; private set; }
 
        public int Level => PurchaseCount / _config.PurchasesPerLevel;
        public int Stage => PurchaseCount % _config.PurchasesPerLevel;
        public bool IsMax => PurchaseCount >= _config.Values.Length - 1;
        public int Price => Mathf.RoundToInt(_config.BasePrice * Mathf.Pow(_config.PriceGrowth, PurchaseCount));
        public float Value => _config.Values[Mathf.Min(PurchaseCount, _config.Values.Length - 1)];
 
        public event Action Bought;
        public event Action<int> LevelReached;
 
        public void Buy()
        {
            PurchaseCount++;

            Bought?.Invoke();
 
            if (Stage == 0)
            {
                LevelReached?.Invoke(Level);
            }
        }
    }
}