namespace BreakLinq
{
    public class Customer
    {
        public string Id { get; set; }

        public string Phone { get; set; }

        public DateTime CreatedOn { get; set; }

        public List<Purchase> Purchases { get; set; }
    }
}
