
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

public class End2EndBase : IDisposable
{
    protected readonly ChromeDriver driver;
    protected readonly string screenshotPath = "../../screenshots/";

    public End2EndBase()
    {
        var options = new ChromeOptions();
        options.AddArgument("--start-maximized");
        driver = new ChromeDriver(options);
    }

    protected void MakeScreenshot(string fileName)
    {
        Directory.CreateDirectory(screenshotPath);
        var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
        screenshot.SaveAsFile(screenshotPath + fileName + ".png");
    }

    public void Dispose()
    {
        driver.Quit();
    }
}