using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace EdgeClient.Tools
{
    public class LogListener : TraceListener
    {
        Action<string> _logCallback;

        public LogListener(Action<string> logCallback)
        {
            _logCallback = logCallback;
        }

        public override void Write(string message)
        {
            _logCallback(message);
        }

        public override void WriteLine(string message)
        {
            _logCallback($"{message}{Environment.NewLine}");
        }
    }
}
