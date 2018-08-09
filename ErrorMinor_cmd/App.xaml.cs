using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;

namespace ErrorMinor_cmd
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public enum CheckResult 
        {
            CheckPass = 0,
            HasError = 233,
            NotFindFile = 255
        }
        [DllImport("Kernel32.dll")]
        public static extern bool AttachConsole(int processID);

        public void WriteToConsole(string message)
        {
            AttachConsole(-1);
            Console.WriteLine(message);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            if (e.Args.Length > 0)
            {
                    //输入一个参数的时候默认为文件路径
                    if (e.Args.Length == 1)
                    {
                        string filePath = e.Args[0].ToString();
                        if (File.Exists(filePath))
                        {
                            //ConfigData config = new ConfigData();
                            LogChecker logCheck = new LogChecker(".\\CheckInfo.log");
                            List<string> errLog = logCheck.StartCheck(filePath);
                            if (logCheck.config.ShowResult == true)
                            {
                                foreach (string log in errLog)
                                {
                                    //print something
                                }
                            }

                            if (errLog.Count > 0)
                            {
                                this.Shutdown((int)CheckResult.HasError);
                            }
                            else
                            {
                                this.Shutdown();
                            }
                        }
                        else
                        {
                            this.Shutdown((int)CheckResult.NotFindFile);
                        }
                    }
                
            }
            else
            {
                new MainWindow().ShowDialog();
                this.Shutdown();
            }   
        }
    }
}
