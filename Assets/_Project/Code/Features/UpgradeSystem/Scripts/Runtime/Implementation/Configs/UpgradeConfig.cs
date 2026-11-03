using System;
using UnityEngine;

namespace UpgradeSystem
{
    [Serializable]
    public sealed class UpgradeConfig
    {
        [SerializeField] private UpgradeType _type;
        [SerializeField] private int _basePrice = 20;
        [SerializeField] private float _priceGrowth = 2f;
        [SerializeField] private int _purchasesPerLevel = 3;
        
        [SerializeField] private float[] _values = new float[] { 1f, 1.1f, 1.2f, 2f, 2.1f, 2.2f, 3f };
 
        public UpgradeType Type => _type;
        public int BasePrice => _basePrice;
        public float PriceGrowth => _priceGrowth;
        public int PurchasesPerLevel => _purchasesPerLevel;
        public float[] Values => _values;
    }
}