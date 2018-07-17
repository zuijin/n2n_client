using EdgeClient.Model;
using EdgeClient.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EdgeClient.Forms
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private System.Windows.Forms.NotifyIcon _notifyIcon;

        private Process _edgeProcess;
        private bool _isStarted = false;
        private Config _config;
        private EdgeConfig _edgeConfig;

        public MainWindow()
        {
            InitializeComponent();

            //加载配置
            _config = ConfigManager.GetConfig();
            _edgeConfig = ConfigManager.GetEdgeConfig(_config.CurrentEdgeConfigName);

            //自动连接
            if (_config.AutoConnect && !string.IsNullOrEmpty(_config.CurrentEdgeConfigName))
            {
                Connect(_config.CurrentEdgeConfigName);
            }

            RefreshStatus();

            SetNotifyIcon();
        }

        /// <summary>
        /// 设置托盘图标
        /// </summary>
        private void SetNotifyIcon()
        {
            this._notifyIcon = new System.Windows.Forms.NotifyIcon();
            this._notifyIcon.Icon = new System.Drawing.Icon(@"EdgeClient.ico");
            this._notifyIcon.Visible = true;

            //打开
            System.Windows.Forms.MenuItem open = new System.Windows.Forms.MenuItem("打开");
            open.Click += (s, e) =>
            {
                this.Show();
            };

            //退出
            System.Windows.Forms.MenuItem exit = new System.Windows.Forms.MenuItem("退出");
            exit.Click += (s, e) =>
            {
                menuExit_Click(s, null);
            };

            //关联托盘控件
            System.Windows.Forms.MenuItem[] childen = new System.Windows.Forms.MenuItem[] { open, exit };
            _notifyIcon.ContextMenu = new System.Windows.Forms.ContextMenu(childen);

            this._notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler((o, e) =>
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    this.Show();
                }
            });
        }

        /// <summary>
        /// 关于
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuAbout_Click(object sender, RoutedEventArgs e)
        {
            var form = new AboutWindow();
            form.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            form.Owner = this;
            form.ShowDialog();
        }

        /// <summary>
        /// 连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuConnect_Click(object sender, RoutedEventArgs e)
        {
            var configName = (string)((MenuItem)sender).Header;

            _config = ConfigManager.GetConfig();
            _config.CurrentEdgeConfigName = configName;
            ConfigManager.SaveConfig(_config);

            Connect(configName);
        }

        /// <summary>
        /// 连接
        /// </summary>
        /// <param name="configName"></param>
        private void Connect(string configName)
        {
            _edgeConfig = ConfigManager.GetEdgeConfig(configName);
            if (_edgeConfig != null)
            {
                var excuteInfo = EdgeHelper.GetExcuteInfo(_edgeConfig);

                Trace.WriteLine($"正在执行：{excuteInfo.FileName} {excuteInfo.Args}");

                _edgeProcess = ProcessHelper.StartProcess(excuteInfo, (a, b) =>
                {
                    Trace.WriteLine(b.Data);
                });

                _isStarted = true;

                RefreshStatus();
            }
        }

        /// <summary>
        /// 断开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuDisconnect_Click(object sender, RoutedEventArgs e)
        {
            ProcessHelper.StopProcess(_edgeProcess);
            _edgeProcess = null;

            _isStarted = false;

            if (sender != null)
            {
                RefreshStatus();
            }
        }

        private void menuExit_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(this, "确认退出吗？", "退出", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                menuDisconnect_Click(sender, e);

                this.Closing -= Window_Closing;
                this.Close();

                Application.Current.Shutdown(0);
            }
        }

        /// <summary>
        /// 刷新状态
        /// </summary>
        private void RefreshStatus()
        {
            if (_isStarted)
            {
                this.menuConnect.IsEnabled = false;
                this.menuDisconnect.IsEnabled = true;

                this.lblIP.Content = _edgeConfig.EdgeIP;
                this.lblNetmask.Content = _edgeConfig.EdgeNetmask;
                this.lblSuperNode.Content = $"{_edgeConfig.SuperNodeIP}:{_edgeConfig.SuperNodePort}";
                this.lblStatus.Content = $"当前配置：{_config.CurrentEdgeConfigName}（已连接）";
            }
            else
            {
                this.menuConnect.IsEnabled = true;
                this.menuDisconnect.IsEnabled = false;

                string text = "-";
                this.lblIP.Content = text;
                this.lblNetmask.Content = text;
                this.lblSuperNode.Content = text;
                this.lblStatus.Content = "未连接";
            }
        }

        /// <summary>
        /// 关闭主窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuConfig_Click(object sender, RoutedEventArgs e)
        {
            var form = new ConfigManageWindow();
            form.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            form.ShowDialog();
        }

        private void menuLog_Click(object sender, RoutedEventArgs e)
        {
            var form = new LogWindow();
            form.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            form.Show();
        }

        private void menuConnect_MouseEnter(object sender, MouseEventArgs e)
        {
            var configNames = ConfigManager.GetAllEdgeConfigName();
            this.menuConnect.Items.Clear();

            if (configNames.Any())
            {
                foreach (var name in configNames.OrderBy(it => it))
                {
                    var menu = new MenuItem();
                    menu.Header = name;
                    menu.Click += menuConnect_Click;

                    this.menuConnect.Items.Add(menu);
                }
            }
            else
            {
                var menu = new MenuItem();
                menu.Header = "添加配置..";
                menu.Click += menuConfig_Click;
                this.menuConnect.Items.Add(menu);
            }
        }

        private void menuInstallDevice_Click(object sender, RoutedEventArgs e)
        {
            RefreshInstallStatus(false);

            new Thread(() =>
             {
                 var excuteInfo = new ExcuteInfo()
                 {
                     FileName = "Driver\\tapinstall.exe",
                     Args = "install Driver\\OemWin2k.inf tap0901"
                 };

                 Process process = ProcessHelper.StartProcess(excuteInfo, (a, b) => Trace.WriteLine(b.Data));
                 process.Exited += (a, b) =>
                 {
                     RefreshInstallStatus(true);
                     MessageBox.Show("安装完毕", "安装驱动");
                 };
             }).Start();
        }

        private void menuUninstallDevice_Click(object sender, RoutedEventArgs e)
        {
            RefreshInstallStatus(false);

            new Thread(() =>
            {
                var excuteInfo = new ExcuteInfo()
                {
                    FileName = "Driver\\tapinstall.exe",
                    Args = "remove tap0901"
                };

                Process process = ProcessHelper.StartProcess(excuteInfo, (a, b) => Trace.WriteLine(b.Data));
                process.Exited += (a, b) =>
                {
                    RefreshInstallStatus(true);
                    MessageBox.Show("卸载完毕", "卸载驱动");
                };
            }).Start();
        }

        private void RefreshInstallStatus(bool enable)
        {
            menuUninstallDevice.Dispatcher.Invoke(new Action(() =>
            {
                menuUninstallDevice.IsEnabled = enable;
            }));

            menuInstallDevice.Dispatcher.Invoke(new Action(() =>
            {
                menuInstallDevice.IsEnabled = enable;
            }));
        }

        private void menuOption_Click(object sender, RoutedEventArgs e)
        {
            var form = new OptionWindow();
            form.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            form.ShowDialog();
        }
    }
}
