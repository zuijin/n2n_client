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
    /// ConfigNameWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ConfigNameWindow : Window
    {
        public ConfigNameWindow()
        {
            InitializeComponent();
        }

        public event Action<string> OnInputCompleted;

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            var edgeConfigName = this.txtEdgeConfigName.Text;

            if (string.IsNullOrEmpty(edgeConfigName))
            {
                this.txtEdgeConfigName.Focus();
                return;
            }

            OnInputCompleted?.Invoke(edgeConfigName);
            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
