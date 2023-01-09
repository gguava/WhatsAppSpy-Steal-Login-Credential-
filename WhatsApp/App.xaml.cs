using CefSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace WhatsApp
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private void App_Startup(object sender, StartupEventArgs e){
            MainWindow mainWindow = new MainWindow();
         
            //mainWindow.WindowState = WindowState.Minimized;
            
            mainWindow.Show();
            //mainWindow.Activate();



        }

    }
}
