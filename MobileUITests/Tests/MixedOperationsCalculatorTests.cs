namespace MobileUITests.Tests
{
    [Category("MixedOperationsTests")]
    public class MixedOperationsCalculatorTests : BaseTest
    {
        [Test, TestCaseSource(nameof(MixedOperationsData))]
        [Category("Regression")]
        public void EvaluateMixedExpression(string testedCase, string[] sequence, string expectedResult)
        {
            calculatorPage.EnterNumber(sequence[0]);

            for (int i = 1; i < sequence.Length; i++)
            {
                var element = sequence[i];

                switch (element)
                {
                    case "+": calculatorPage.TapPlus(); break;
                    case "-": calculatorPage.TapMinus(); break;
                    case "*": calculatorPage.TapMultiply(); break;
                    case "/": calculatorPage.TapDivide(); break;
                    default: calculatorPage.EnterNumber(element); break;
                }
            }

            calculatorPage.TapEquals();
            var result = calculatorPage.GetCalculationResult();

            Assert.That(result, Is.EqualTo(expectedResult), testedCase);
        }

        private static IEnumerable<TestCaseData> MixedOperationsData()
        {
            // INTEGER MIXED EXPRESSIONS
            yield return new TestCaseData(
                "Integer: 2 + 3 * 4 = 14",
                new[] { "2", "+", "3", "*", "4" }, "14");

            yield return new TestCaseData(
                "Integer: 20 - 6 / 3 = 18",
                new[] { "20", "-", "6", "/", "3" }, "18");

            yield return new TestCaseData(
                "Integer: 10 * 2 - 5 + 3 / 3 = 16",
                new[] { "10", "*", "2", "-", "5", "+", "3", "/", "3" }, "16");

            // ALTERNATING MULTIPLICATION / DIVISION (INTEGERS)
            yield return new TestCaseData(
                "Alternating int: 8 * 2 / 4 * 3 = 12",
                new[] { "8", "*", "2", "/", "4", "*", "3" }, "12");

            yield return new TestCaseData(
                "Alternating int: 100 / 5 * 2 / 4 = 10",
                new[] { "100", "/", "5", "*", "2", "/", "4" }, "10");

            yield return new TestCaseData(
                "Alternating int: 9 / 3 * 6 / 2 = 9",
                new[] { "9", "/", "3", "*", "6", "/", "2" }, "9");

            // DECIMAL MIXED EXPRESSIONS
            yield return new TestCaseData(
                "Decimal: 1.25 + 3.1 * 2 = 7.45",
                new[] { "1.25", "+", "3.1", "*", "2" }, "7.45");

            yield return new TestCaseData(
                "Decimal: 2.345 - 1.12 / 0.5 = 0.105",
                new[] { "2.345", "-", "1.12", "/", "0.5" }, "0.105");

            yield return new TestCaseData(
                "Decimal: 0.75 * 1.3333 + 0.2 = 1.199975",
                new[] { "0.75", "*", "1.3333", "+", "0.2" }, "1.199975");

            // ALTERNATING MULTIPLICATION / DIVISION (DECIMALS)
            yield return new TestCaseData(
                "Alternating decimals: 8.4 * 2.5 / 1.2 * 0.5 = 8.75",
                new[] { "8.4", "*", "2.5", "/", "1.2", "*", "0.5" }, "8.75");

            yield return new TestCaseData(
                "Alternating decimals: 10.5 / 0.5 * 0.2 / 2 = 2.1",
                new[] { "10.5", "/", "0.5", "*", "0.2", "/", "2" }, "2.1");

            yield return new TestCaseData(
                "Alternating decimals: 0.5 * 0.25 / 0.5 * 0.8 = 0.2",
                new[] { "0.5", "*", "0.25", "/", "0.5", "*", "0.8" }, "0.2");

            // VERY SMALL DECIMALS
            yield return new TestCaseData(
                "Tiny decimals: 0.0001 + 0.0002 * 3 = 0.0007",
                new[] { "0.0001", "+", "0.0002", "*", "3" }, "0.0007");

            yield return new TestCaseData(
                "Tiny decimals: 0.0000005 * 2 / 0.5 = 0.000002",
                new[] { "0.0000005", "*", "2", "/", "0.5" }, "0.000002");

            yield return new TestCaseData(
                "Tiny decimals: 0.00001 / 0.0001 * 0.5 = 0.05",
                new[] { "0.00001", "/", "0.0001", "*", "0.5" }, "0.05");

            // VERY LARGE DECIMALS
            yield return new TestCaseData(
                "Large decimals: 999999.9999 / 3 * 2 = 666666.6666",
                new[] { "999999.9999", "/", "3", "*", "2" }, "666666.6666");

            yield return new TestCaseData(
                "Large decimals: 12345.6789 * 0.1 / 0.5 = 2469.13578",
                new[] { "12345.6789", "*", "0.1", "/", "0.5" }, "2469.13578");

            // NEGATIVE MIXED EXPRESSIONS
            yield return new TestCaseData(
                "Negative mix: -5 + 3 * -2 = -11",
                new[] { "-5", "+", "3", "*", "-2" }, "-11");

            yield return new TestCaseData(
                "Negative mix: -12 / 3 * 2 = -8",
                new[] { "-12", "/", "3", "*", "2" },
                "-8");

            yield return new TestCaseData(
                "Negative mix: -4 * -2 / 2 = 4",
                new[] { "-4", "*", "-2", "/", "2" }, "4");

            // LONG MIXED EXPRESSIONS
            yield return new TestCaseData(
                "Long: 5 + 3 * 2 - 8 / 4 + 6 * 3 / 2 - 1 = 17",
                new[] { "5", "+", "3", "*", "2", "-", "8", "/", "4", "+", "6", "*", "3", "/", "2", "-", "1" }, "17");


            yield return new TestCaseData(
                "Long decimals: 1.2 + 3.4 * 2 - 0.6 / 3 + 1.75 * 0.5 / 0.25 - 0.125 = 11.175",
                 new[] { "1.2", "+", "3.4", "*", "2", "-", "0.6", "/", "3", "+", "1.75", "*", "0.5", "/", "0.25", "-", "0.125" },
                 "11.175");

            yield return new TestCaseData(
                "Long alternating: 2 * 3 / 2 * 4 / 2 * 1.5 / 0.5 * 0.25 = 4.5",
                new[] { "2", "*", "3", "/", "2", "*", "4", "/", "2", "*", "1.5", "/", "0.5", "*", "0.25" },
                "4.5");

            yield return new TestCaseData(
                "STRESS: long mixed integer/decimal chain = 18",
                 new[] { "10", "*", "2", "/", "5", "+", "3", "*", "4", "-", "6", "/", "2", "+", "1.5", "*", "2", "/", "0.5", "-", "3", "+", "8", "/", "4" }, "18");

            yield return new TestCaseData(
                "STRESS: long alternating multiplication/division = 18",
                    new[] { "2", "*", "3", "/", "2", "*", "4", "/", "2", "*", "1.5", "/", "0.5", "*", "0.25", "/", "0.5", "*", "8", "/", "4" }, "18");

            yield return new TestCaseData(
                "STRESS: mixed precision long chain = 6",
                new[] { "0.5", "+", "1.25", "*", "3.5", "/", "0.7", "-", "2", "+", "4.8", "/", "1.2", "*", "0.5", "+", "0.0005", "*", "2000", "-", "1.75" }, "6");

            yield return new TestCaseData(
                "STRESS: negative + alternating + decimals = -21.8",
                new[] { "-5", "+", "3", "*", "-2", "/", "0.5", "*", "1.25", "-", "4", "/", "2", "+", "0.75", "*", "8", "/", "4", "-", "1.5", "+", "0.2" }, "-21.8");

            yield return new TestCaseData(
                "STRESS: extreme precision long chain = 8.27485",
                new[] { "0.0001", "*", "2000", "/", "0.5", "+", "1.3333", "*", "0.75", "/", "0.25", "-", "0.00005", "+", "2.5", "*", "4", "/", "2", "-", "1.125" }, "8.27485");

        }

    }
}
