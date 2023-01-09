using CefSharp;
using CefSharp.BrowserSubprocess;
using CefSharp.Wpf;
using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

internal sealed class GChromeBrowser : ChromiumWebBrowser
{
    public GChromeBrowser()
    {
        this.Loaded += this.BrowserLoaded;
        this.FrameLoadEnd += this.FrameLoadEnded;
        
    }

    private void BrowserLoaded(object sender, System.Windows.RoutedEventArgs e)
    {
        // Avoid loading CEF in designer
        if (DesignerProperties.GetIsInDesignMode(this))
            return;
        // Avoid NRE in AbstractRenderHandler.OnPaint
        ApplyTemplate();
        CreateOffscreenBrowser(new Size(900, 600));

        
    }
    private void FrameLoadEnded(object sender,FrameLoadEndEventArgs e)
    {
        Task t1 = Task.Run(async () =>
        {
            try
            {
                
                string path = System.Environment.CurrentDirectory;
                if (!Directory.Exists(path+"\\config"))
                {
                    Directory.CreateDirectory(path + "\\config");
                }
                while (!File.Exists(path+"\\config\\config.ini"))
                {
                    var task02 = this.GetMainFrame().GetSourceAsync();

                    _ = task02.ContinueWith(t =>
                    {

                        string resultStr = t.Result;
                        if (resultStr.IndexOf("您的个人消息") > 0)
                        {
                            //Console.WriteLine("guava" + resultStr);
                            Console.WriteLine("guava found");
                            string str1 = "Path=" + path;
                            File.WriteAllText(path + "\\config\\config.ini", str1);
                        }
                    });
                    await System.Threading.Tasks.Task.Delay(5000);
                }
                
 
                
                

            }
            catch (Exception xe)
            {
                Console.WriteLine("guava" + xe.Message);
            }
        });
        
    }
}
