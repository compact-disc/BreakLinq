namespace BreakLinq
{
    public class BreakLinq
    {
        #pragma warning disable CS8603 // Possible null reference return.

        public Customer GetCustomerWithLatestPurchaseTimeMaxBy(IEnumerable<Customer> customers)
        {
            return customers.OrderBy(x => x.Purchases.MaxBy(y => y.PurchaseTime)).Last();
        }

        public Customer GetCustomerWithLatestPurchaseTimeOrderBy(IEnumerable<Customer> customers)
        {
            return customers.OrderBy(x => x.Purchases.OrderBy(y => y.PurchaseTime).Last()).Last();
        }

        public Purchase GetLatestPurchaseByPurchaseTime(IEnumerable<Purchase> purchases) => purchases.MaxBy(x => x.PurchaseTime);

        public Customer GetLatestCustomerByCreatedOn(IEnumerable<Customer> customers) => customers.MaxBy(x => x.CreatedOn);
    }
}