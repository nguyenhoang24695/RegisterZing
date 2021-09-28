using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RegisterZing
{
    class Program
    {
        static void Main(string[] args)
        {
            var bm = new Bitmap("./pic/capcha.jpg");
            var x = KAutoHelper.Get_Text_From_Image.Get_Text(bm);
            Console.WriteLine("ID: ");
            var id = int.Parse(Console.ReadLine());


            // Đường dẫn tới thư mục chứa Chrome Driver
            // Tải về tại đây https://chromedriver.chromium.org/downloads
            // Thay đổi cho đúng với trường hợp của bạn
            string chromeDriverPath = @"./";
            var cService = ChromeDriverService.CreateDefaultService(chromeDriverPath);
            cService.HideCommandPromptWindow = true;
            var options = new ChromeOptions();
            options.AddExtensions("./mpbjkejclgfgadiemmefgebjfooflfhl.zip");
            options.AddArgument("no-sandbox");
            options.AddArguments(new List<string>() { "disable-gpu" });
            while (id < 10)
            {
                Console.WriteLine("Register with id =  " + id);
                // Chạy ngầm không pop up trình duyệt ra ngoài 
                //options.AddArgument("headless");
                //
                IEnumerable<int> pidsBefore = Process.GetProcessesByName("chrome").Select(p => p.Id);
                //
                var driver = new ChromeDriver(cService, options);

                //
                IEnumerable<int> pidsAfter = Process.GetProcessesByName("chrome").Select(p => p.Id);
                IEnumerable<int> newFirefoxPids = pidsAfter.Except(pidsBefore);

                var b = newFirefoxPids.Where(p => Process.GetProcessById(p).MainWindowHandle != IntPtr.Zero).Select(p => Process.GetProcessById(p)).ToList();
                Console.WriteLine(b[0].Id);

                //KAutoHelper.CaptureHelper.CaptureScreenToFile("./1.jpg",ImageFormat.Jpeg);

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
                KAutoHelper.CaptureHelper.CaptureWindowToFile(b[0].MainWindowHandle, "./1.jpg", ImageFormat.Jpeg);
                //driver.FindElement(By.XPath("/html/body/div[3]/div/div[2]/form/div[15]/a")).Click();
                var mainBitmap = KAutoHelper.CaptureHelper.CaptureWindow(b[0].MainWindowHandle);
                var point = KAutoHelper.ImageScanOpenCV.FindOutPoint((Bitmap)mainBitmap, new Bitmap(string.Format("./pic/register_Btn.jpg")));
                if (point.HasValue)
                {
                    KAutoHelper.AutoControl.SendClickOnPosition(b[0].MainWindowHandle, point.Value.X, point.Value.Y);
                }
                //KAutoHelper.CaptureHelper.CaptureWindowToFile(b[0].MainWindowHandle, "./1.jpg", ImageFormat.Jpeg);

                Thread.Sleep(1000);
                int count = 0;
                while (count < 3)
                {
                    Console.WriteLine("Find count: " + (count + 1));
                    try
                    {
                        mainBitmap = KAutoHelper.CaptureHelper.CaptureWindow(b[0].MainWindowHandle);

                        point = KAutoHelper.ImageScanOpenCV.FindOutPoint((Bitmap)mainBitmap, new Bitmap(string.Format("./pic/20908.jpg")));
                        if (point.HasValue)
                        {
                            KAutoHelper.AutoControl.SendClickOnPosition(b[0].MainWindowHandle, point.Value.X, point.Value.Y);
                        }
                        else
                        {
                            count++;
                        }
                        Thread.Sleep(2000);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        break;
                    }
                }

                driver.Dispose();
                id++;
            }
        }
    }
}
