using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegisterZing
{
    class Program
    {
        static void Main(string[] args)
        {
            int id = 4;

            var cService = ChromeDriverService.CreateDefaultService();
            cService.HideCommandPromptWindow = true;

            // Đường dẫn tới thư mục chứa Chrome Driver
            // Tải về tại đây https://chromedriver.chromium.org/downloads
            // Thay đổi cho đúng với trường hợp của bạn
            string chromeDriverPath = @"./";
            var options = new ChromeOptions();
            options.AddExtensions("./mpbjkejclgfgadiemmefgebjfooflfhl.zip");
            options.AddArgument("no-sandbox");
            // Chạy ngầm không pop up trình duyệt ra ngoài 
            //options.AddArgument("headless");
            var driver = new ChromeDriver(cService, options, TimeSpan.FromDays(20));
            var _processId = cService.ProcessId;
            Console.WriteLine(_processId);
            Console.WriteLine(driver.CurrentWindowHandle);
            var a = Process.GetProcessById(_processId);

            IntPtr handle = a.Handle;

            KAutoHelper.CaptureHelper.CaptureWindowToFile(handle,"./1.jpg", ImageFormat.Jpeg);

            Console.ReadKey();
            string url = "https://id.zing.vn/";
            driver.Url = url;

            
            //driver.Navigate().GoToUrl(url);
            driver.FindElement(By.XPath("/html/body/div[3]/div[2]/p[2]/a")).Click();


            var name = driver.FindElement(By.XPath("/html/body/div[3]/div/div[2]/form/div[2]/input"));
            name.SendKeys("nhan nguyen");

            var user = driver.FindElement(By.XPath("/html/body/div[3]/div/div[2]/form/div[4]/input"));
            user.SendKeys(string.Format("nhatmong95_{0}", id));

            var pass = driver.FindElement(By.XPath("/html/body/div[3]/div/div[2]/form/div[6]/input[2]"));
            pass.SendKeys("liuyifei1@A");

            var pass2 = driver.FindElement(By.XPath("/html/body/div[3]/div/div[2]/form/div[8]/input[2]"));
            pass2.SendKeys("liuyifei1@A");

        }
    }
}
