using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp;
using CefSharp.OffScreen;
using utility;

namespace system_status
{
    public partial class Form1 : Form
    {
        public myinclude my = null;

        public Form1()
        {

            my = new myinclude();

            InitializeComponent();
        }
       
        public ChromiumWebBrowser browser;

        public int Main()
        {
            string testUrl = "https://3wa.tw/test/test_webp.html";



            var settings = new CefSettings()
            {
                //By default CefSharp will use an in-memory cache, you need to specify a Cache Folder to persist data
                CachePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CefSharp\\Cache")
            };


            //Perform dependency check to make sure all relevant resources are in our output directory.
            //Cef.Initialize(settings, performDependencyCheck: true, browserProcessHandler: null);

            // Create the offscreen Chromium browser.
            browser = new ChromiumWebBrowser(testUrl);

            // An event that is fired when the first page is finished loading.
            // This returns to us from another thread.
            browser.LoadingStateChanged += BrowserLoadingStateChanged;

            // We have to wait for something, otherwise the process will exit too soon.
           

            // Clean up Chromium objects.  You need to call this in your application otherwise
            // you will get a crash when closing.
           // Cef.Shutdown();

            return 0;
        }

        private  void BrowserLoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            // Check to see if loading is complete - this event is called twice, one when loading starts
            // second time when it's finished
            // (rather than an iframe within the main frame).
            if (!e.IsLoading)
            {
                // Remove the load event handler, because we only want one snapshot of the initial page.
                browser.LoadingStateChanged -= BrowserLoadingStateChanged;

                var scriptTask = browser.EvaluateScriptAsync("document.getElementById('lst-ib').value = 'CefSharp Was Here!'");

                scriptTask.ContinueWith(t =>
                {
                    //Give the browser a little time to render
                    Thread.Sleep(500);
                    // Wait for the screenshot to be taken.
                    var task = browser.ScreenshotAsync();
                    task.ContinueWith(x =>
                    {
                        // Make a file to save it to (e.g. C:\Users\jan\Desktop\CefSharp screenshot.png)
                        var screenshotPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "CefSharp screenshot.png");

                        Console.WriteLine();
                        Console.WriteLine("Screenshot ready. Saving to {0}", screenshotPath);

                        // Save the Bitmap to the path.
                        // The image type is auto-detected via the ".png" extension.
                        task.Result.Save(screenshotPath);

                        // We no longer need the Bitmap.
                        // Dispose it to avoid keeping the memory alive.  Especially important in 32-bit applications.
                        task.Result.Dispose();

                        Console.WriteLine("Screenshot saved.  Launching your default image viewer...");

                        // Tell Windows to launch the saved image.
                        Process.Start(new ProcessStartInfo(screenshotPath)
                        {
                            // UseShellExecute is false by default on .NET Core.
                            UseShellExecute = true
                        });

                        Console.WriteLine("Image viewer launched.  Press any key to exit.");
                    }, TaskScheduler.Default);
                });
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            Main();
        }
    }



}
