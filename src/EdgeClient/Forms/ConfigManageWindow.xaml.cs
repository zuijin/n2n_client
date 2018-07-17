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
    /// ManageConfig.xaml 的交互逻辑
    /// </summary>
    public partial class ConfigManageWindow : Window
    {
        public ConfigManageWindow()
        {
            InitializeComponent();

            LoadConfig();
        }

        private void LoadConfig()
        {
            this.lstConfig.Items.Clear();

            var configNames = ConfigManager.GetAllEdgeConfigName();
            foreach (var name in configNames.OrderBy(it => it))
            {
                this.lstConfig.Items.Add(new Label()
                {
                    Content = name
                });
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            ShowConfigEditWindow("");
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = (Label)this.lstConfig.SelectedItem;
            if (selectedItem != null)
            {
                string name = (string)selectedItem.Content;
                ShowConfigEditWindow(name);
            }
        }

        /// <summary>
        /// 展示配置编辑窗口
        /// </summary>
        /// <param name="edgeConfigName"></param>
        private void ShowConfigEditWindow(string edgeConfigName)
        {
            var form = new ConfigEditWindow(edgeConfigName);
            form.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            form.OnEditCompleted += (s) =>
            {
                LoadConfig();
                this.Show();
            };

            this.Hide();
            form.ShowDialog();
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = (Label)this.lstConfig.SelectedItem;
            if (selectedItem != null)
            {
                string name = (string)selectedItem.Content;
                ConfigManager.DelEdgeConfig(name);
                LoadConfig();
            }
        }
    }
}
