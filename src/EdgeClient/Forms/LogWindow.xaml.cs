using EdgeClient.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// LogWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LogWindow : Window
    {
        private LogListener _logListener;

        public LogWindow()
        {
            InitializeComponent();

            _logListener = new LogListener(LogCallback);
            Trace.Listeners.Add(_logListener);
        }
        
        public void LogCallback(string message)
        {
            this.LogTextBox.Dispatcher.Invoke(new Action(() =>
            {
                this.LogTextBox.AppendText(message);
                this.ScrollViewer.ScrollToEnd();
            }));
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Trace.Listeners.Remove(_logListener);
        }
    }
}
