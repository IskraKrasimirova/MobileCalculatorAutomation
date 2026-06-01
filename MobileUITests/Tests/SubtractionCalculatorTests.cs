namespace MobileUITests.Tests
{
    [Category("SubtractionTests")]
    public class SubtractionCalculatorTests : BaseTest
    {
        [Test, TestCaseSource(nameof(SubtractionDataForTwoNumbers))]
        [Category("Smoke")]
        public void SubtractionOfTwoNumbers(string testedCase, string number1, string number2, string expectedResult)
        {
            calculatorPage.EnterNumber(number1);
            calculatorPage.TapMinus();
            calculatorPage.EnterNumber(number2);
            calculatorPage.TapEquals();

            var result = calculatorPage.GetCalculationResult();

            Assert.That(result, Is.EqualTo(expectedResult), testedCase);
        }

        private static IEnumerable<TestCaseData> SubtractionDataForTwoNumbers()
        {
            // BASIC POSITIVE/NEGATIVE
            yield return new TestCaseData("Subtract two positive numbers", "5", "3", "2");
            yield return new TestCaseData("Subtract larger from smaller", "3", "5", "-2");
            yield return new TestCaseData("Subtract zero", "7", "0", "7");
            yield return new TestCaseData("Zero minus positive", "0", "8", "-8");

            // NEGATIVE NUMBERS
            yield return new TestCaseData("Negative minus positive", "-5", "3", "-8");

            // DECIMALS
            yield return new TestCaseData("Subtract two positive decimals", "5.5", "2.2", "3.3");
            yield return new TestCaseData("Subtract negative and positive decimal", "-2.5", "3.4", "-5.9");

            // PRECISION
            yield return new TestCaseData("Subtract very small decimals", "0.0003", "0.0001", "0.0002");

            // MULTI-DIGIT
            yield return new TestCaseData("Subtract multi-digit numbers", "1234", "567", "667");
        }

        private static IEnumerable<TestCaseData> SubtractionDataForTwoNumbersNotSupportedByCalculator()
        {
            // NEGATIVE NUMBERS
            yield return new TestCaseData("Negative minus negative", "-5", "-3", "-2");

            // CRITICAL CASE: minus negative number
            yield return new TestCaseData("Subtract negative number from positive", "5", "-3", "8");
            yield return new TestCaseData("Subtract negative from zero", "0", "-4", "4");
            yield return new TestCaseData("Subtract negative multi-digit", "123", "-456", "579");

            // DECIMALS
            yield return new TestCaseData("Subtract positive and negative decimal", "2.5", "-3.4", "5.9");
            yield return new TestCaseData("Subtract two negative decimals", "-5.5", "-4.8", "-0.7");

            // PRECISION
            yield return new TestCaseData("Subtract very small negative decimal", "0.001", "-0.0002", "0.0012");
        }

        [Test, TestCaseSource(nameof(SubtractionDataForMultipleNumbers))]
        [Category("Regression")]
        public void SubtractionOfMultipleNumbers(string testedCase, string[] numbers, string expectedResult)
        {
            // Case 1: Empty array
            if (numbers.Length == 0)
            {
                var result = calculatorPage.GetCalculationResult();
                Assert.That(result, Is.EqualTo(expectedResult), testedCase);
                return;
            }

            // Case 2: Single element
            if (numbers.Length == 1)
            {
                calculatorPage.EnterNumber(numbers[0]);
                calculatorPage.TapEquals();
                var result = calculatorPage.GetCalculationResult();
                Assert.That(result, Is.EqualTo(expectedResult), testedCase);
                return;
            }

            // Case 3: Normal multi-subtraction logic
            calculatorPage.EnterNumber(numbers[0]);

            for (int i = 1; i < numbers.Length; i++)
            {
                calculatorPage.TapMinus();
                calculatorPage.EnterNumber(numbers[i]);
            }

            calculatorPage.TapEquals();
            var finalResult = calculatorPage.GetCalculationResult();

            Assert.That(finalResult, Is.EqualTo(expectedResult), testedCase);
        }

        private static IEnumerable<TestCaseData> SubtractionDataForMultipleNumbers()
        {
            yield return new TestCaseData("Subtract three positive integers", new[] { "10", "3", "2" }, "5");

            yield return new TestCaseData("Subtract decimals", new[] { "5.5", "1.2", "0.3" }, "4");

            // ⭐ DIFFERENT DECIMAL PRECISION
            yield return new TestCaseData("Subtract decimals with different precision",
                new[] { "10.500", "0.5", "0.05000", "0.005" }, "9.945");

            yield return new TestCaseData("Mixed integers and decimals with different precision",
                new[] { "100", "2.5", "0.25", "0.005" }, "97.245");

            // ⭐ VERY SMALL DECIMALS
            yield return new TestCaseData("Subtract very small decimals",
                new[] { "0.001", "0.0002", "0.0003" }, "0.0005");

            yield return new TestCaseData("Subtract extremely small decimals (precision test)",
                new[] { "0.0000009", "0.0000004", "0.0000003" }, "0.0000002");

            // ⭐ VERY LARGE NUMBERS
            yield return new TestCaseData("Subtract large multi-digit numbers",
                new[] { "1000000", "250000", "250000" }, "500000");

            yield return new TestCaseData("Subtract extremely large numbers",
                new[] { "999999999", "123456789", "111111111" }, "765432099");

            // LARGE DATA SETS
            yield return new TestCaseData("Ten very small decimals",
                new[] { "0.0000010", "0.0000002", "0.0000003", "0.0000001", "0.0000004", "0.0000005", "0.0000002", "0.0000001", "0.0000003", "0.0000001" }, "-0.0000012");

            yield return new TestCaseData("Five very large integers",
                new[] { "999999999", "123456789", "111111111", "222222222", "100000000" }, "443209877");

            // EXTREME CASES
            yield return new TestCaseData("Single element array", new[] { "5" }, "5");

            yield return new TestCaseData("Empty array", Array.Empty<string>(), "0");

            yield return new TestCaseData("Array with zeroes only", new[] { "0", "0", "0" }, "0");

            // ⭐ MIXED TYPES (integers + decimals + negatives)
            yield return new TestCaseData("Mixed integers and decimals",
                new[] { "10", "2.5", "1.25", "0.75" }, "5.5");

            // ⭐ MIXED VERY LARGE + VERY SMALL
            yield return new TestCaseData("Large number minus tiny decimals",
                new[] { "1000000", "0.0001", "0.0002" }, "999999.9997");
        }

        private static IEnumerable<TestCaseData> SubtractionDataForMultipleNumbersNotSupportedByCalculator()
        {
            // LARGE DATA SETS
            yield return new TestCaseData("Ten mixed integers",
                new[] { "100", "2", "15", "-3", "7", "0", "50", "-10", "8", "1" },
                "30");

            yield return new TestCaseData("Ten decimals with different precision",
                new[] { "10.500", "0.5", "-1.25", "0.05000", "3.14159", "-0.333", "0.0004", "2.75", "-0.004", "1.2" }, "4.44501");

            yield return new TestCaseData("Ten mixed integers and decimals",
                new[] { "50", "2.5", "-10", "0.75", "100.125", "-0.005", "3", "0.333", "-1.111", "0.0001" }, "-45.5921");

            // Calculator loses precision with long chains of large subtractions.
            yield return new TestCaseData("Ten very large integers",
               new[] { "999999999", "123456789", "111111111", "222222222", "100000000", "55555555", "33333333", "77777777", "88888888", "44444444" }, "-1111111110");

            // ⭐ MIXED TYPES (integers + decimals + negatives)
            yield return new TestCaseData("Mixed large + small + negative",
                new[] { "1000", "-0.5", "0.25", "-999.25" }, "1.5");

            // ⭐ MIXED VERY LARGE + VERY SMALL
            yield return new TestCaseData("Large negative minus tiny negative decimals",
                new[] { "-500000", "-0.0003", "-0.0002" }, "-499999.9995");
        }
    }
}
