using System;
using System.Collections.Generic;
using WalletSystem;

namespace UpgradeSystem
{
    public class UpgradeService
    {
        private readonly Wallet _wallet;
        private readonly Dictionary<UpgradeType, Upgrade> _upgrades;

        public event Action<UpgradeType> Bought;

        public UpgradeService(Wallet wallet, Dictionary<UpgradeType, Upgrade> upgrades)
        {
            _wallet = wallet;
            _upgrades = upgrades;
        }

        public Upgrade GetUpgrade(UpgradeType type)
        {
            return _upgrades[type];
        }

        public bool TryBuy(UpgradeType type)
        {
            Upgrade upgrade = _upgrades[type];

            if (_wallet.TrySpend(upgrade.Price) == false)
            {
                return false;
            }

            upgrade.Buy();
            
            Bought?.Invoke(type);
            
            return true;
        }
    }
}