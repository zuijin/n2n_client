using EdgeClient.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace EdgeClient.Tools
{
    public class ProcessHelper
    {

        public static Process StartProcess(ExcuteInfo excuteInfo, DataReceivedEventHandler outputHandler)
        {
            var startInfo = new ProcessStartInfo()
            {
                FileName = excuteInfo.FileName,
                Arguments = excuteInfo.Args,
                CreateNoWindow = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                StandardOutputEncoding = Encoding.Default,
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = false
            };

            var process = new Process() { StartInfo = startInfo, };
            process.OutputDataReceived += outputHandler;
            process.ErrorDataReceived += outputHandler;
            process.EnableRaisingEvents = true;

            process.Start();
            process.BeginOutputReadLine();
            return process;
        }

        public static void StopProcess(Process process)
        {
            if (process != null && !process.HasExited)
            {
                process.Kill();
                process.Close();
                process.Dispose();
            }
        }
    }
}
