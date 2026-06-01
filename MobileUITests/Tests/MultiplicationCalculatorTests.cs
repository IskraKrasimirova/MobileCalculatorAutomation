namespace MobileUITests.Tests
{
    [Category("MultiplicationTests")]
    public class MultiplicationCalculatorTests : BaseTest
    {
        [Test, TestCaseSource(nameof(MultiplicationDataForTwoNumbers))]
        [Category("Smoke")]
        public void MultiplicationOfTwoNumbers(string testedCase, string number1, string number2, string expectedResult)
        {
            calculatorPage.EnterNumber(number1);
            calculatorPage.TapMultiply();
            calculatorPage.EnterNumber(number2);
            calculatorPage.TapEquals();

            var result = calculatorPage.GetCalculationResult();

            Assert.That(result, Is.EqualTo(expectedResult), testedCase);
        }

        private static IEnumerable<TestCaseData> MultiplicationDataForTwoNumbers()
        {
            // BASIC POSITIVE/NEGATIVE
            yield return new TestCaseData("Multiply two positive numbers", "5", "3", "15");
            yield return new TestCaseData("Multiply positive by zero", "7", "0", "0");
            yield return new TestCaseData("Multiply zero by positive", "0", "8", "0");

            // NEGATIVE NUMBERS
            yield return new TestCaseData("Multiply positive by negative", "5", "-3", "-15");
            yield return new TestCaseData("Multiply negative by positive", "-5", "3", "-15");
            yield return new TestCaseData("Multiply two negative numbers", "-5", "-3", "15");

            // DECIMALS
            yield return new TestCaseData("Multiply two decimals", "1.5", "2.0", "3");
            yield return new TestCaseData("Multiply decimal by negative decimal", "2.5", "-3.4", "-8.5");
            yield return new TestCaseData("Multiply negative decimal by positive", "-2.5", "3.4", "-8.5");

            // DIFFERENT PRECISION
            yield return new TestCaseData("Multiply decimals with different precision", "1.250", "3.1", "3.875");

            // VERY SMALL DECIMALS. Expected result is 0.00000002 equal to 2e-8, but some calculators may display it in scientific notation or as a decimal with many zeros. Both formats are accepted.
            yield return new TestCaseData("Multiply very small decimals", "0.0001", "0.0002", "2.E-8");

            // VERY LARGE NUMBERS
            yield return new TestCaseData("Multiply large multi-digit numbers", "12345", "6789", "83810205");
        }

        [Test, TestCaseSource(nameof(MultiplicationDataForMultipleNumbers))]
        [Category("Regression")]
        public void MultiplicationOfMultipleNumbers(string testedCase, string[] numbers, string expectedResult)
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

            // Case 3: Normal multi-multiplication logic
            calculatorPage.EnterNumber(numbers[0]);

            for (int i = 1; i < numbers.Length; i++)
            {
                calculatorPage.TapMultiply();
                calculatorPage.EnterNumber(numbers[i]);
            }

            calculatorPage.TapEquals();
            var finalResult = calculatorPage.GetCalculationResult();

            Assert.That(finalResult, Is.EqualTo(expectedResult), testedCase);
        }

        private static IEnumerable<TestCaseData> MultiplicationDataForMultipleNumbers()
        {
            // BASIC
            yield return new TestCaseData("Multiply three positive integers",
                new[] { "2", "3", "4" }, "24");

            yield return new TestCaseData("Multiply mixed integers",
                new[] { "-2", "3", "-4" }, "24");

            // DECIMALS
            yield return new TestCaseData("Multiply decimals",
                new[] { "1.5", "2.0", "0.5" }, "1.5");

            // DIFFERENT PRECISION
            yield return new TestCaseData("Multiply decimals with different precision",
                new[] { "1.250", "0.5", "0.05000" }, "0.03125");

            // VERY SMALL DECIMALS
            // Expected result is 0.00000000006 equal to 6e-11, but some calculators may display it in scientific notation or as a decimal with many zeros. Both formats are accepted.
            yield return new TestCaseData("Multiply very small decimals",
                new[] { "0.001", "0.0002", "0.0003" }, "6.E-11");

            // VERY LARGE NUMBERS (realistic)
            yield return new TestCaseData("Multiply large multi-digit numbers",
                new[] { "1000", "250", "2" }, "500000");

            // MIXED TYPES
            yield return new TestCaseData("Mixed integers and decimals",
                new[] { "5", "2.5", "-1.25", "0.75" }, "-11.71875");

            // ⭐ ALTERNATING NEGATIVE SIGNS (important case)
            yield return new TestCaseData("Multiply with alternating negative signs",
                new[] { "-2", "-3", "-4", "-5" }, "120");

            // LARGE DATA SETS
            yield return new TestCaseData("Ten mixed integers (1-digit, 2-digit, 3-digit)",
                new[] { "2", "-3", "12", "-15", "7", "100", "-25", "3", "50", "-2" },
                "5670000000");

            // EXTREME CASES
            yield return new TestCaseData("Single element array", new[] { "5" }, "5");

            yield return new TestCaseData("Empty array", Array.Empty<string>(), "0");

            yield return new TestCaseData("Array with zero", new[] { "5", "3", "0", "10" }, "0");

            // MIXED VERY LARGE + VERY SMALL
            yield return new TestCaseData("Large negative times tiny negative decimals",
                new[] { "-500000", "-0.0003", "0.0002" }, "0.03");
        }

        //Precision issues
        private static IEnumerable<TestCaseData> MultiplicationDataForMultipleNumbersNotSupportedByCalculator()
        {
            // MIXED VERY LARGE + VERY SMALL
            yield return new TestCaseData("Large number times tiny decimals",
                new[] { "1000000", "0.0001", "0.0002" }, "20");

            yield return new TestCaseData("Ten decimals with different precision",
                new[] { "1.5", "0.5", "-2.25", "0.05000", "3.14159", "-0.333", "0.0004", "2.75", "-0.004", "1.2" },
                "-0.00000046546302");

            yield return new TestCaseData("Ten mixed integers and decimals",
                new[] { "5", "2.5", "-1.25", "0.75", "100.125", "-0.005", "3", "0.333", "-1.111", "0.0001" },
                "-0.0006510518924428");

            yield return new TestCaseData("Ten very small decimals",
                new[] { "0.0000010", "0.0000002", "0.0000003", "0.0000001", "0.0000004", "0.0000005", "0.0000002", "0.0000001", "0.0000003", "0.0000001" },
                "-0.0000012");

            yield return new TestCaseData("Multiplying extremely large integers should overflow",
                 new[] { "999999999", "123456789", "111111111", "222222222", "100000000", "55555555", "33333333", "77777777", "88888888", "44444444" },
                "1.7345493523125306E80");
        }

        [Test]
        [Category("Regression")]
        [Ignore("Not supported")]
        public void RepeatedMultiplyWithoutSecondOperand()
        {
            calculatorPage.EnterNumber("5");
            calculatorPage.TapMultiply();
            calculatorPage.TapEquals();

            var result = calculatorPage.GetCalculationResult();

            Assert.That(result, Is.EqualTo("25"), "5 × = should repeat the operation and return 25");
        }

        [Test]
        [Category("Regression")]
        [Ignore("Not supported")]
        public void RepeatedMultiplicationSeveralTimes()
        {
            // Start with 2 ×
            calculatorPage.EnterNumber("2");
            calculatorPage.TapMultiply();

            var expectedValues = new[] { "4", "16", "256", "65536", "4294967296", "1.8446744073709555e+19" };

            for (int i = 0; i < expectedValues.Length; i++)
            {
                calculatorPage.TapEquals();
                var result = calculatorPage.GetCalculationResult();

                Assert.That(result, Is.EqualTo(expectedValues[i]), $"Expected {expectedValues[i]} but got {result}");

                // Add "*" only if this is NOT the last iteration
                if (i < expectedValues.Length - 1)
                {
                    calculatorPage.TapMultiply();
                }
            }
        }
    }
}
