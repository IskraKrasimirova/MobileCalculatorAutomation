namespace MobileUITests.Tests
{
    [Category("DivisionTests")]
    public class DivisionCalculatorTests : BaseTest
    {
        [Test, TestCaseSource(nameof(DivisionDataForTwoNumbers))]
        [Category("Smoke")]
        public void DivisionOfTwoNumbers(string testedCase, string number1, string number2, string expectedResult)
        {
            _calculatorPage.EnterNumber(number1);
            _calculatorPage.TapDivide();
            _calculatorPage.EnterNumber(number2);
            _calculatorPage.TapEquals();

            var result = _calculatorPage.GetCalculationResult();

            Assert.That(result, Is.EqualTo(expectedResult), testedCase);
        }

        private static IEnumerable<TestCaseData> DivisionDataForTwoNumbers()
        {
            // BASIC
            yield return new TestCaseData("Divide two positive integers", "10", "2", "5");
            yield return new TestCaseData("Divide smaller by larger", "3", "9", "0.3333333333333");
            yield return new TestCaseData("Divide larger by smaller", "9", "3", "3");

            // ZERO CASES
            yield return new TestCaseData("Zero divided by positive", "0", "5", "0");

            // NEGATIVE NUMBERS
            yield return new TestCaseData("Negative divided by positive", "-10", "2", "-5");
            yield return new TestCaseData("Positive divided by negative", "10", "-2", "-5");

            // DECIMALS
            yield return new TestCaseData("Divide decimals", "5.5", "2.2", "2.5");
            yield return new TestCaseData("Small decimals", "0.06", "0.02", "3");

            // MIXED TYPES
            yield return new TestCaseData("Integer divided by decimal", "10", "0.5", "20");
            yield return new TestCaseData("Decimal divided by integer", "7.5", "3", "2.5");

            // PRECISION (safe)
            yield return new TestCaseData("Different precision decimals", "1.000", "0.25", "4");

            // EXTREMELY SMALL DECIMALS
            yield return new TestCaseData("Tiny decimals division", "0.0000001", "0.0000002", "0.5");

            // EXTREMELY LARGE NUMBERS
            yield return new TestCaseData("Large numbers division", "999999999", "3", "333333333");

            // MIXED LARGE + SMALL
            yield return new TestCaseData("Large divided by tiny decimal", "1000000", "0.0000001", "1.E13");

            // NEGATIVE + TINY DECIMALS
            yield return new TestCaseData("Negative tiny decimals", "-0.0000004", "0.0000002", "-2");
        }

        private static IEnumerable<TestCaseData> DivisionDataForTwoNumbersNotSupportedByCalculator()
        {
            // LONG DECIMALS (UI loses precision)
            yield return new TestCaseData("Long decimal division", "1.23456789", "0.0000123", "100373.0");
        }

        [Test, TestCaseSource(nameof(DivisionByZeroData))]
        [Category("Regression")]
        public void DivisionByZero_ShowsErrorMessage(string testedCase, string number)
        {
            _calculatorPage.EnterNumber(number);
            _calculatorPage.TapDivide();
            _calculatorPage.EnterNumber("0");
            _calculatorPage.TapEquals();

            var result = _calculatorPage.GetCalculationResult();

            Assert.That(result, Is.EqualTo($"{number}÷0"), testedCase);

            var error = _calculatorPage.GetErrorMessage();

            Assert.That(error, Is.EqualTo("Can't divide by 0"));
        }

        private static IEnumerable<TestCaseData> DivisionByZeroData()
        {
            yield return new TestCaseData("Positive number divided by zero", "5");
            yield return new TestCaseData("Negative number divided by zero", "-300000");
            yield return new TestCaseData("Positive decimal divided by zero", "0.0001");
            yield return new TestCaseData("Negative decimal divided by zero", "-12345.6789");
        }

        [Test, TestCaseSource(nameof(DivisionDataForMultipleNumbers))]
        [Category("Regression")]
        public void DivisionOfMultipleNumbers(string testedCase, string[] numbers, string expectedResult)
        {
            // Case 1: Empty array
            if (numbers.Length == 0)
            {
                var result = _calculatorPage.GetCalculationResult();
                Assert.That(result, Is.EqualTo(expectedResult), testedCase);
                return;
            }

            // Case 2: Single element
            if (numbers.Length == 1)
            {
                _calculatorPage.EnterNumber(numbers[0]);
                _calculatorPage.TapEquals();
                var result = _calculatorPage.GetCalculationResult();
                Assert.That(result, Is.EqualTo(expectedResult), testedCase);
                return;
            }

            // Case 3: Normal multi-division logic
            _calculatorPage.EnterNumber(numbers[0]);

            for (int i = 1; i < numbers.Length; i++)
            {
                _calculatorPage.TapDivide();
                _calculatorPage.EnterNumber(numbers[i]);
            }

            _calculatorPage.TapEquals();
            var finalResult = _calculatorPage.GetCalculationResult();

            Assert.That(finalResult, Is.EqualTo(expectedResult), testedCase);
        }

        private static IEnumerable<TestCaseData> DivisionDataForMultipleNumbers()
        {
            yield return new TestCaseData("Divide three integers",
                new[] { "100", "2", "5" }, "10");

            yield return new TestCaseData("Divide decimals",
                new[] { "10.0", "2.5", "2" }, "2");

            yield return new TestCaseData("Decimal chain fractional result",
                new[] { "7.5", "2", "3" }, "1.25");

            yield return new TestCaseData("Mixed integers and decimals",
                new[] { "50", "2.5", "1" }, "20");

            yield return new TestCaseData("Small decimals",
                new[] { "0.1", "0.01", "10" }, "1");

            yield return new TestCaseData("Negative division chain",
                new[] { "-120", "3", "2" }, "-20");

            yield return new TestCaseData("Alternating signs with decimals",
                new[] { "-50.0", "-2.5", "4" }, "5");

            yield return new TestCaseData("Mixed large + small + negative",
                new[] { "1000", "-0.5", "0.25" }, "-8000");

            yield return new TestCaseData("Mixed decimals with different precision",
                new[] { "500000", "-0.25", "0.0001" }, "-2.E10");

            yield return new TestCaseData("Empty array", Array.Empty<string>(), "0");

            yield return new TestCaseData("Single element array", new[] { "5" }, "5");
        }

        private static IEnumerable<TestCaseData> DivisionDataForMultipleNumbersNotSupportedByCalculator()
        {
            yield return new TestCaseData("Large + tiny decimals",
                new[] { "1000000", "0.0001", "0.0002" }, "50000000000");

            yield return new TestCaseData("Ten very small decimals",
                new[] { "0.000001", "0.000002", "0.000003", "0.000004", "0.000005" },
                "41.6666667");

            yield return new TestCaseData("Long decimal chain",
                new[] { "1.23456789", "0.0000123", "0.00000123" }, "8163265300");

            yield return new TestCaseData("Large integers chain",
                new[] { "999999999", "3", "3", "3" }, "12345679.875");

            yield return new TestCaseData("Large number divided by tiny decimals chain",
                new[] { "1000000", "0.0001", "0.0002", "0.0003" }, "16666666666666.6666666667");

            yield return new TestCaseData("Mixed precision decimals",
                new[] { "100.125", "0.0003", "0.0002" }, "166875000");
        }

        [Test, TestCaseSource(nameof(DivisionByZeroMultipleData))]
        [Category("Regression")]
        public void DivisionOfMultipleNumbers_ByZero_ShowsError(string testedCase, string[] numbers)
        {
            _calculatorPage.EnterNumber(numbers[0]);

            for (int i = 1; i < numbers.Length; i++)
            {
                _calculatorPage.TapDivide();
                _calculatorPage.EnterNumber(numbers[i]);
            }

            _calculatorPage.TapEquals();

            var result = _calculatorPage.GetCalculationResult();
            Assert.That(result, Is.EqualTo(string.Join("÷", numbers)));

            var error = _calculatorPage.GetErrorMessage();
            Assert.That(error, Is.EqualTo("Can't divide by 0"));
        }

        private static IEnumerable<TestCaseData> DivisionByZeroMultipleData()
        {
            yield return new TestCaseData("Divide by zero in middle",
                new[] { "100", "0", "5" });

            yield return new TestCaseData("Divide by zero at end",
                new[] { "5", "-2", "0" });

            yield return new TestCaseData("Decimal divided by zero in middle",
                new[] { "0.0001", "0", "-123.45" });

            yield return new TestCaseData("Decimal divided by zero at end",
                new[] { "-12345.6789", "1.23", "-50", "0" });
        }
    }
}
