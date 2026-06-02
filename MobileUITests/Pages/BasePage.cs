using OpenQA.Selenium.Appium.Android;

namespace MobileUITests.Pages
{
    public abstract class BasePage
    {
        protected AndroidDriver _driver;

        protected BasePage(AndroidDriver driver)
        {
            _driver = driver;
        }
    }
}
