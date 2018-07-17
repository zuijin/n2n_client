using EdgeClient.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EdgeClient.Tools
{
    public class EdgeHelper
    {
        private static Random _random = new Random();

        /// <summary>
        /// 获取对应的运行参数信息
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static ExcuteInfo GetExcuteInfo(EdgeConfig config)
        {
            var version = config.Version;
            switch (version)
            {
                case "v1":
                    return GetExcuteInfoV1(config);
                case "v2":
                    return GetExcuteInfoV2(config);
                case "v2.1":
                    return GetExcuteInfoV2_1(config);
                default:
                    throw new Exception("不支持的版本");
            }
        }

        private static ExcuteInfo GetExcuteInfoV1(EdgeConfig config)
        {
            return new ExcuteInfo()
            {
                FileName = $"{AppDomain.CurrentDomain.BaseDirectory}/EdgeFile/edge.exe",
                Args = BuilArgs(config)
            };
        }

        private static ExcuteInfo GetExcuteInfoV2(EdgeConfig config)
        {
            return new ExcuteInfo()
            {
                FileName = $"{AppDomain.CurrentDomain.BaseDirectory}/EdgeFile/edge2.exe",
                Args = BuilArgs(config)
            };
        }

        private static ExcuteInfo GetExcuteInfoV2_1(EdgeConfig config)
        {
            return new ExcuteInfo()
            {
                FileName = $"{AppDomain.CurrentDomain.BaseDirectory}/EdgeFile/edge21.exe",
                Args = BuilArgs(config)
            };
        }

        /// <summary>
        /// 组装参数
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        private static string BuilArgs(EdgeConfig config)
        {
            List<string> args = new List<string>();

            args.Add($"-d n2n{_random.Next()}");
            args.Add($"-a {config.EdgeIP}");
            args.Add($"-c {config.EdgeGroup}");
            args.Add($"-k {config.EdgePassword}");
            args.Add($"-l {config.SuperNodeIP}:{config.SuperNodePort}");

            if (!string.IsNullOrEmpty(config.EdgeNetmask))
            {
                args.Add($"-s {config.EdgeNetmask}");
            }

            if (config.ResolveSuperNode)
            {
                args.Add($"-b");
            }

            if (config.PacketForwarding)
            {
                args.Add($"-r");
            }

            if (config.Multicast)
            {
                args.Add($"-E");
            }

            if (config.Verbose)
            {
                args.Add($"-v");
            }

            if (config.LocalPort > 0)
            {
                args.Add($"-p {config.LocalPort}");
            }

            if (!string.IsNullOrEmpty(config.MacAddress))
            {
                args.Add($"-m {config.MacAddress}");
            }
            
            if (config.MTU > 0)
            {
                args.Add($"-M {config.MTU}");
            }
            
            return string.Join(" ", args.ToArray());
        }
    }
}
