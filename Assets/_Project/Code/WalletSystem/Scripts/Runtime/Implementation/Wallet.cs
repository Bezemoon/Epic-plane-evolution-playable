namespace WalletSystem
{
    public class Wallet
    {
        public int Amount { get; private set; }

        public void Add(int amount)
        {
            Amount += amount;
        }

        public bool TrySpend(int amount)
        {
            if (amount > Amount)
            {
                return false;
            }

            Amount -= amount;
            return true;
        }
    }
}
