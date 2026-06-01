namespace MobileUITests.Tests
{
    [Category("AdditionTests")]
    public class AdditionCalculatorTests : BaseTest
    {
        [Test, TestCaseSource(nameof(AdditionDataForTwoNumbers))]
        [Category("Smoke")]
        public void AdditionOfTwoNumbers(string testedCase, string number1, string number2, string expectedResult)
        {
            calculatorPage.EnterNumber(number1);
            calculatorPage.TapPlus();
            calculatorPage.EnterNumber(number2);
            calculatorPage.TapEquals();

            var result = calculatorPage.GetCalculationResult();

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        private static IEnumerable<TestCaseData> AdditionDataForTwoNumbers()
        {
            yield return new TestCaseData("Addition of two positive numbers", "2", "3", "5");
            yield return new TestCaseData("Addition of a positive and a negative number", "5", "-3", "2");
            yield return new TestCaseData("Addition of a negative and a positive number", "-6", "4", "-2");
            yield return new TestCaseData("Addition of two negative numbers", "-5", "-3", "-8");
            yield return new TestCaseData("Addition of a number and zero", "7", "0", "7");
            yield return new TestCaseData("Addition of a zero and a positive number", "0", "8", "8");
            yield return new TestCaseData("Addition of a zero and a negative number", "0", "-4", "-4");
            yield return new TestCaseData("Addition of two positive decimals", "1.5", "2.2", "3.7");
            yield return new TestCaseData("Addition of a negative and a positive decimal", "-1.5", "2.2", "0.7");
            yield return new TestCaseData("Addition of a positive and a negative decimal", "2.5", "-3.4", "-0.9");
            yield return new TestCaseData("Addition of two negative decimals", "-5.5", "-4.8", "-10.3");
            yield return new TestCaseData("Addition of multi-digit numbers", "123", "456", "579");
            yield return new TestCaseData("Addition of decimals with different precision", "1.25", "3.1", "4.35");
            yield return new TestCaseData("Addition of very small decimals", "0.0001", "0.0002", "0.0003");
            yield return new TestCaseData("Addition of a decimal and a whole positive number", "2.5", "3", "5.5");
            yield return new TestCaseData("Addition of a decimal and a negative whole number", "2.7", "-4", "-1.3");
            yield return new TestCaseData("Addition of zero and zero", "0", "0", "0");
            yield return new TestCaseData("Addition of zero and decimal", "0", "-1.5", "-1.5");
        }

        [Test, TestCaseSource(nameof(AdditionDataForMultipleNumbers))]
        [Category("Regression")]
        public void AdditionOfMultipleNumbers(string testedCase, string[] addends, string expectedResult)
        {
            // Case 1: Empty array
            if (addends.Length == 0)
            {
                var result = calculatorPage.GetCalculationResult();

                Assert.That(result, Is.EqualTo(expectedResult), testedCase);

                return;
            }

            // Case 2: Single element
            if (addends.Length == 1)
            {
                calculatorPage.EnterNumber(addends[0]);
                calculatorPage.TapEquals();
                var result = calculatorPage.GetCalculationResult();    

                Assert.That(result, Is.EqualTo(expectedResult), testedCase);

                return;
            }

            // Case 3: Normal multi-addend logic
            calculatorPage.EnterNumber(addends[0]);

            for (int i = 1; i < addends.Length; i++)
            {
                calculatorPage.TapPlus();
                calculatorPage.EnterNumber(addends[i]);
            }

            calculatorPage.TapEquals();
            var finalResult = calculatorPage.GetCalculationResult();

            Assert.That(finalResult, Is.EqualTo(expectedResult), testedCase);
        }

        private static IEnumerable<TestCaseData> AdditionDataForMultipleNumbers()
        {
            yield return new TestCaseData("Add three positive integers", new[] { "2", "3", "4" }, "9");

            yield return new TestCaseData("Add mixed integers", new[] { "-5", "10", "-2" }, "3");

            yield return new TestCaseData("Add multiple decimals", new[] { "0.1", "0.2", "0.3" }, "0.6");

            yield return new TestCaseData("Add many small decimals", new[] { "0.0001", "0.0002", "0.0003", "0.0004" }, "0.001");

            yield return new TestCaseData("Add large multi-digit numbers", new[] { "123456", "789012", "345678" }, "1258146");

            // LARGE DATA SETS (10+ ADDENDS)
            yield return new TestCaseData("Ten positive integers",
                new[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" }, "55");

            yield return new TestCaseData("Ten mixed integers",
                new[] { "-1", "2", "-3", "4", "-5", "6", "-7", "8", "-9", "10" }, "5");

            yield return new TestCaseData("Ten decimals",
                new[] { "0.1", "0.2", "0.3", "0.4", "0.5", "0.6", "0.7", "0.8", "0.9", "1.0" }, "5.5");

            // VERY LARGE DATA SETS (20+ ADDENDS)
            yield return new TestCaseData("Twenty ones (performance sanity)", Enumerable.Repeat("1", 20).ToArray(), "20");

            yield return new TestCaseData("Twenty mixed decimals",
                new[] {
                        "0.1","-0.2","0.3","-0.4","0.5","-0.6","0.7","-0.8","0.9","-1.0",
                        "1.1","-1.2","1.3","-1.4","1.5","-1.6","1.7","-1.8","1.9","-2.0"
                       },
                "-1"
            );

            // EXTREME CASES
            yield return new TestCaseData("Very large multi-digit numbers", new[] { "123456", "789012", "345678" }, "1258146");

            yield return new TestCaseData("Very small decimals (precision test)",
                new[] { "0.0000001", "0.0000002", "0.0000003", "0.0000004" },
                "0.000001"
            );

            yield return new TestCaseData("Large + small decimals", new[] { "999999.9999", "0.0001", "0.0001" }, "1000000.0001");

            // EDGE CASES
            yield return new TestCaseData("Single element array", new[] { "5" }, "5");

            yield return new TestCaseData("Empty array", Array.Empty<string>(), "0");

            yield return new TestCaseData("Array with zeroes only", new[] { "0", "0", "0", "0" }, "0");

            yield return new TestCaseData("Array with negative zero", new[] { "-0", "0", "-0" }, "0");

            // MIXED TYPES (integers + decimals + negatives)
            yield return new TestCaseData("Mixed integers and decimals", new[] { "5", "2.5", "-1.25", "0.75" }, "7");

            yield return new TestCaseData("Mixed large + small + negative", new[] { "1000", "-0.5", "0.25", "-999.25" }, "0.5");
        }
    }
}
