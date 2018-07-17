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
    /// ConfigEditWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ConfigEditWindow : Window
    {
        private string _edgeConfigName;
        private EdgeConfig _edgeConfig;

        public event Action<object> OnEditCompleted;

        public ConfigEditWindow()
        {
            InitializeComponent();
        }

        public ConfigEditWindow(string edgeConfigName) : this()
        {
            _edgeConfigName = edgeConfigName;

            if (!string.IsNullOrEmpty(_edgeConfigName))
            {
                this.Title = _edgeConfigName;
                _edgeConfig = ConfigManager.GetEdgeConfig(_edgeConfigName);
                SetToForms(_edgeConfig);
            }
        }

        private void SetToForms(EdgeConfig edgeConfig)
        {
            this.cbbVersion.Text = edgeConfig.Version;
            this.txtSuperNodeIP.Text = edgeConfig.SuperNodeIP;
            this.txtSuperNodePort.Text = edgeConfig.SuperNodePort.ToString();
            this.txtEdgeIP.Text = edgeConfig.EdgeIP;
            this.txtEdgeNetmask.Text = edgeConfig.EdgeNetmask;
            this.txtEdgeGroup.Text = edgeConfig.EdgeGroup;
            this.txtEdgePassword.Text = edgeConfig.EdgePassword;
            this.txtMacAddress.Text = edgeConfig.MacAddress;
            this.txtMTU.Text = edgeConfig.MTU.ToString();
            this.txtExtensionArgs.Text = edgeConfig.ExtensionArgs;
            this.chkMulticast.IsChecked = edgeConfig.Multicast;
            this.chkPacketForwarding.IsChecked = edgeConfig.PacketForwarding;
            this.chkResolveSuperNode.IsChecked = edgeConfig.ResolveSuperNode;
            this.chkVerbose.IsChecked = edgeConfig.Verbose;
        }

        private EdgeConfig GetFromFroms()
        {
            _edgeConfig = _edgeConfig ?? new EdgeConfig();

            _edgeConfig.Version = this.cbbVersion.Text;
            _edgeConfig.SuperNodeIP = this.txtSuperNodeIP.Text;
            _edgeConfig.SuperNodePort = ParseInt(this.txtSuperNodePort.Text);
            _edgeConfig.EdgeIP = this.txtEdgeIP.Text;
            _edgeConfig.EdgeNetmask = this.txtEdgeNetmask.Text;
            _edgeConfig.EdgeGroup = this.txtEdgeGroup.Text;
            _edgeConfig.EdgePassword = this.txtEdgePassword.Text;
            _edgeConfig.MacAddress = this.txtMacAddress.Text;
            _edgeConfig.MTU = ParseInt(this.txtMTU.Text);
            _edgeConfig.ExtensionArgs = this.txtExtensionArgs.Text;
            _edgeConfig.Multicast = this.chkMulticast.IsChecked ?? false;
            _edgeConfig.PacketForwarding = this.chkPacketForwarding.IsChecked ?? false;
            _edgeConfig.ResolveSuperNode = this.chkResolveSuperNode.IsChecked ?? false;
            _edgeConfig.Verbose = this.chkVerbose.IsChecked ?? false;

            return _edgeConfig;
        }

        private int ParseInt(string value)
        {
            int tmp;
            int.TryParse(value, out tmp);
            return tmp;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var edgeConfig = GetFromFroms();
            if (string.IsNullOrEmpty(_edgeConfigName))
            {
                var form = new ConfigNameWindow();
                form.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                form.Owner = this;
                form.OnInputCompleted += (name) =>
                {
                    _edgeConfigName = name;
                };

                form.ShowDialog();
            }

            ConfigManager.SaveEdgeConfig(_edgeConfigName, edgeConfig);

            OnEditCompleted?.Invoke(sender);
            this.Close();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            OnEditCompleted?.Invoke(sender);
        }
    }
}
