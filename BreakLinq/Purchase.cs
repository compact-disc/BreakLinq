namespace BreakLinq
{
    public class NonComparablePurchase : Purchase
    {
        public string Id { get; set; }

        public DateTime PurchaseTime { get; set; }
    }

    public class ComparablePurchase : IComparable, Purchase
    {
        public string Id { get; set; }

        public DateTime PurchaseTime { get; set; }

        public int CompareTo(object? obj)
        {
            var val = (obj as ComparablePurchase).PurchaseTime.CompareTo(PurchaseTime);

            return val;
        }
    }

    public interface Purchase
    {
        string Id { get; set; }

        DateTime PurchaseTime { get; set; }
    }
}
