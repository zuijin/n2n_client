using EdgeClient.Model;
using EdgeClient.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EdgeClient.Forms
{
    /// <summary>
    /// OptionWindow.xaml 的交互逻辑
    /// </summary>
    public partial class OptionWindow : Window
    {
        private Config _config;

        public OptionWindow()
        {
            InitializeComponent();

            LoadConfig();
        }

        private void LoadConfig()
        {
            _config = ConfigManager.GetConfig();

            this.chkAutoConnect.IsChecked = _config.AutoConnect;
        }

        private Config GetConfigFromFroms()
        {
            _config = _config ?? ConfigManager.GetConfig();

            _config.AutoConnect = this.chkAutoConnect.IsChecked ?? false;

            return _config;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var config = GetConfigFromFroms();

            ConfigManager.SaveConfig(config);

            var result = MessageBox.Show("保存成功", "保存", MessageBoxButton.OK);
            if (result == MessageBoxResult.OK)
            {
                this.Close();
            }
        }
    }
}
