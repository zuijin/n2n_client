using EdgeClient.Tools;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Windows;

namespace EdgeClient
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// 重写OnStartup
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            StartNetManager();

            CheckAdministrator();

            Console.Write("");
        }

        /// <summary>
        /// 检查是否以管理员身份启动
        /// </summary>
        private void CheckAdministrator()
        {
            var wi = WindowsIdentity.GetCurrent();
            var wp = new WindowsPrincipal(wi);

            bool runAsAdmin = wp.IsInRole(WindowsBuiltInRole.Administrator);

            if (!runAsAdmin)
            {
                // It is not possible to launch a ClickOnce app as administrator directly,
                // so instead we launch the app as administrator in a new process.
                var processInfo = new ProcessStartInfo(Assembly.GetExecutingAssembly().CodeBase);

                // The following properties run the new process as administrator
                processInfo.UseShellExecute = true;
                processInfo.Verb = "runas";

                // Start the new process
                try
                {
                    Process.Start(processInfo);
                }
                catch { }

                // Shut down the current process
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// 打开网络连接窗口
        /// </summary>
        private void StartNetManager()
        {
            var p = ProcessHelper.StartProcess(new Model.ExcuteInfo()
            {
                FileName = @"cmd.exe"
            }, (sender, e) => { });

            p.StandardInput.WriteLine("ncpa.cpl & exit");
            p.StandardInput.AutoFlush = true;
            p.WaitForExit();

            p.Close();
        }
    }
}
