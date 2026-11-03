using System;

namespace WalletSystem
{
    public class Wallet
    {
        public event Action<int> Changed;

        public int Amount { get; private set; }

        public void Add(int amount)
        {
            Amount += amount;
            Changed?.Invoke(Amount);
        }

        public bool TrySpend(int amount)
        {
            if (amount > Amount)
            {
                return false;
            }

            Amount -= amount;
            Changed?.Invoke(Amount);
            return true;
        }
    }
}
