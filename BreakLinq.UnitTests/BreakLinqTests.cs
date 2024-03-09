using break_linq_tests;
using BreakLinq;

namespace BreakLinqUnitTests
{
    /// <summary>
    /// Tests to show LINQ limitations of using a OrderBy linked with another OrderBy or MaxBy.
    /// </summary>
    [TestClass]
    public class BreakLinqTests
    {
        /// <summary>
        /// Should return valid customer from linq query using <see cref="ComparablePurchase"/> because it has a <see cref="IComparable"/> interface implemented.
        /// However, <see cref="DateTime"/> already has the <see cref="IComparable"/> interface implemented.
        /// </summary>
        [TestMethod]
        public void ValidCustomerReturn()
        {
            var breakLinq = new BreakLinq.BreakLinq();

            Assert.IsInstanceOfType<Customer>(breakLinq.GetCustomerWithLatestPurchaseTimeMaxBy(GenerateCustomers(true)));
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> because we are comparing a <see cref="IEnumerable{Customer}"/> of objects that has a <see cref="IList{}"/> of objects.
        /// The <see cref="List{NonComparablePurchase}"/> should be able to compare on <see cref="DateTime"/> but it does not for some reason. It says that an <see cref="IComparable"/> must be
        /// implemented on one of the objects. Is there a limitation of doing a double linq query without any comparables even though <see cref="DateTime"/> already has it implemented.
        /// </summary>
        [TestMethod]
        public void ThrowsExceptionComparingDateTime()
        {
            var breakLinq = new BreakLinq.BreakLinq();

            var action = () => breakLinq.GetCustomerWithLatestPurchaseTimeMaxBy(GenerateCustomers(false));

            Assert.ThrowsException<ArgumentException>(action);
        }

        /// <summary>
        /// Similar to <see cref="ThrowsExceptionComparingDateTime"/> function but now uses a double 'OrderBy' paired with a 'Last' instead of using a 'MaxBy'. We see similar results either way.
        /// </summary>
        [TestMethod]
        public void ThrowsExceptionWithDoubleOrderBy()
        {
            var breakLinq = new BreakLinq.BreakLinq();

            var action = () => breakLinq.GetCustomerWithLatestPurchaseTimeOrderBy(GenerateCustomers(false));

            Assert.ThrowsException<ArgumentException>(action);
        }

        /// <summary>
        /// Compares <see cref="Purchase"/> objects of type <see cref="NonComparablePurchase"/> using the <see cref="DateTime"/> to compare. It works as expected.
        /// </summary>
        [TestMethod]
        public void CompareDateTimeRaw()
        {
            var breakLinq = new BreakLinq.BreakLinq();

            Assert.IsInstanceOfType<Purchase>(breakLinq.GetLatestPurchaseByPurchaseTime(GeneratePurchases(false).AsEnumerable()));
        }


        /// <summary>
        /// Compares <see cref="Customer"/> objects using the parameter <see cref="Customer.CreatedOn"/> <see cref="DateTime"/> and it works as expected.
        /// </summary>
        [TestMethod]
        public void CompareCustomersOnCreatedOn()
        {
            var breakLinq = new BreakLinq.BreakLinq();

            Assert.IsInstanceOfType<Customer>(breakLinq.GetLatestCustomerByCreatedOn(GenerateCustomers()));
        }

        /// <summary>
        /// Successful comparisons.
        /// </summary>
        /// <param name="_">Name of the test.</param>
        /// <param name="comparable">Create <see cref="Purchase"/> that implements <see cref="IComparable"/> or not.</param>
        /// <param name="numCustomers">Number of <see cref="Customer"/> to generate.</param>
        /// <param name="numPurchases">Number of <see cref="Purchase"/> to generate.</param>
        [TestMethod]
        [DynamicData(nameof(BreakLinqTestsData.CompareSuccessData), typeof(BreakLinqTestsData), DynamicDataSourceType.Method)]
        public void CompareSuccess(string _, bool comparable, int numCustomers, int numPurchases)
        {
            var breakLinq = new BreakLinq.BreakLinq();

            Assert.IsInstanceOfType<Customer>(breakLinq.GetCustomerWithLatestPurchaseTimeMaxBy(GenerateCustomers(comparable, numCustomers, numPurchases)));
        }

        /// <summary>
        /// Expects an <see cref="ArgumentException"/> because lack of comparison implementation.
        /// </summary>
        /// <param name="_">Name of the test.</param>
        /// <param name="comparable">Create <see cref="Purchase"/> that implements <see cref="IComparable"/> or not.</param>
        /// <param name="numCustomers">Number of <see cref="Customer"/> to generate.</param>
        /// <param name="numPurchases">Number of <see cref="Purchase"/> to generate.</param>
        [TestMethod]
        [DynamicData(nameof(BreakLinqTestsData.CompareFailureData), typeof(BreakLinqTestsData), DynamicDataSourceType.Method)]
        public void CompareFail(string _, bool comparable, int numCustomers, int numPurchases)
        {
            var breakLinq = new BreakLinq.BreakLinq();

            var action = () => breakLinq.GetCustomerWithLatestPurchaseTimeMaxBy(GenerateCustomers(comparable, numCustomers, numPurchases));

            Assert.ThrowsException<ArgumentException>(action);
        }

        private IEnumerable<Customer> GenerateCustomers(bool comparable = false, int numCustomers = 10, int numPurchases = 10)
        {
            var customers = new List<Customer>();

            for (int i = 0; i < numCustomers; i++)
            {
                customers.Add(new Customer
                {
                    Id = $"{i}",
                    CreatedOn = GenerateRandomDateTime(),
                    Phone = "5551237744",
                    Purchases = GeneratePurchases(comparable, numPurchases),
                });
            }

            return customers.AsEnumerable();
        }

        private List<Purchase> GeneratePurchases(bool comparable = false, int numPurchases = 10)
        {
            List<Purchase> purchases = new List<Purchase>();

            for (int i = 0; i < numPurchases; i++)
            {
                var id = new Random().Next();

                if (comparable)
                {
                    purchases.Add(new ComparablePurchase
                    {
                        Id = $"{id}",
                        PurchaseTime = GenerateRandomDateTime(),
                    });
                }
                else
                {
                    purchases.Add(new NonComparablePurchase
                    {
                        Id = $"{id}",
                        PurchaseTime = GenerateRandomDateTime(),
                    });
                }
            }

            return purchases;
        }

        private DateTime GenerateRandomDateTime()
        {
            var year = new Random().Next(1971, 2024);
            var month = new Random().Next(3, 12);
            var day = new Random().Next(1, 30);
            var hour = new Random().Next(1, 24);

            return new DateTime(year, month, day, hour, 0, 0);
        }
    }
}