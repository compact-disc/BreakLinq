namespace break_linq_tests
{
    public class BreakLinqTestsData
    {
        public static IEnumerable<object[]> CompareSuccessData()
        {
            yield return new object[]
            {
                "NonComparable, 1 Customer, 1 Purchase",
                false,
                1,
                1,
            };

            yield return new object[]
            {
                "Comparable, 1 Customer, 1 Purchase",
                true,
                1,
                1,
            };

            yield return new object[]
            {
                "Comparable, 2 Customers, 1 Purchase",
                true,
                2,
                1,
            };

            yield return new object[]
            {
                "Comparable, 1 Customer, 2 Purchases",
                true,
                2,
                1,
            };

            yield return new object[]
{
                "NonComparable, 1 Customer, 2 Purchases",
                false,
                1,
                2,
            };
        }

        public static IEnumerable<object[]> CompareFailureData()
        {
            yield return new object[]
            {
                "NonComparable, 2 Customers, 2 Purchase",
                false,
                2,
                2,
            };

            yield return new object[]
            {
                "NonComparable, 2 Customers, 1 Purchase",
                false,
                2,
                1,
            };
        }
    }
}
