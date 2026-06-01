using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;

namespace MobileUITests.Pages
{
    public class CalculatorPage : BasePage
    {
        //private AppiumElement DigitButton(int digit) => _driver.FindElement(MobileBy.Id($"com.google.android.calculator:id/digit_{digit}"));
        private AppiumElement DigitButton(int digit) => _driver.FindElement(MobileBy.AccessibilityId($"{digit}"));

        private AppiumElement PlusButton => _driver.FindElement(MobileBy.AccessibilityId("plus"));
        private AppiumElement MinusButton => _driver.FindElement(MobileBy.AccessibilityId("minus"));
        private AppiumElement MultiplyButton => _driver.FindElement(MobileBy.AccessibilityId("multiply"));
        private AppiumElement DivideButton => _driver.FindElement(MobileBy.AccessibilityId("divide"));
        private AppiumElement EqualsButton => _driver.FindElement(MobileBy.AccessibilityId("equals"));
        private AppiumElement DotButton => _driver.FindElement(MobileBy.AccessibilityId("point"));
        //private AppiumElement ResultField => _driver.FindElement(MobileBy.Id("com.google.android.calculator:id/result_final"));
        private AppiumElement ParenthesisButton => _driver.FindElement(MobileBy.Id("com.google.android.calculator:id/parens"));
        private AppiumElement ClearButton => _driver.FindElement(MobileBy.AccessibilityId("clear"));
        private AppiumElement DeleteButton => _driver.FindElement(MobileBy.AccessibilityId("delete"));
        private AppiumElement ResultField
        {
            get
            {
                try
                {
                    // Try to get final result (only present after an operation)
                    return _driver.FindElement(MobileBy.Id("com.google.android.calculator:id/result_final"));
                }
                catch (NoSuchElementException)
                {
                    // If final result is not found → return formula (single operand)
                    return _driver.FindElement(MobileBy.Id("com.google.android.calculator:id/formula"));
                }
            }
        }

        private AppiumElement ErrorMessageField => _driver.FindElement(MobileBy.Id("com.google.android.calculator:id/result_preview"));

        public CalculatorPage(AndroidDriver driver) : base(driver)
        {
        }

        public void TapDigit(int digit) => DigitButton(digit).Click();

        public void TapPlus() => PlusButton.Click();
        public void TapMinus() => MinusButton.Click();
        public void TapMultiply() => MultiplyButton.Click();
        public void TapDivide() => DivideButton.Click();

        public void TapEquals() => EqualsButton.Click();

        public void TapDot() => DotButton.Click();

        public void TapParenthesis() => ParenthesisButton.Click();

        public void TapClear() => ClearButton.Click();
        public void TapDelete() => DeleteButton.Click();

        public string GetCalculationResult()
        {
            var text = ResultField.Text?.Trim();

            if (string.IsNullOrEmpty(text))
                return "0"; // empty result is considered as zero

            // Normalize Unicode minus to ASCII minus
            text = text.Replace('−', '-');

            return text;
        }

        public void EnterNumber(string number)
        {
            foreach (char c in number)
            {
                if (c == '-') TapMinus();
                else if (c == '.') TapDot();
                else TapDigit(int.Parse(c.ToString()));
            }

            // Handle negative numbers by tapping "0 -" first, then entering the digits
            // This is necessary because the calculator app may not allow directly entering a negative sign before digits
            // Example: to enter "-5", we tap "0", then "-", then "5"
            //if (number.StartsWith('-'))
            //{
            //    // Enter 0 -
            //    TapDigit(0);
            //    TapMinus();

            //    // Remove the leading minus
            //    number = number.Substring(1);
            //}

            //// Common logic for entering digits and decimal points
            //foreach (char c in number)
            //{
            //    if (c == '.')
            //        TapDot();
            //    else
            //        TapDigit(int.Parse(c.ToString()));
            //}
        }

        public string GetErrorMessage() => ErrorMessageField.Text;
    }
}
