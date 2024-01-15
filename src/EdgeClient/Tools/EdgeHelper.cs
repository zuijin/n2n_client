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
			switch (config.Version)
			{
				case "v1":
					return GetExcuteInfoV1(config);
				case "v2":
					return GetExcuteInfoV2(config);
				case "v2.7":
					return GetExcuteInfoV27(config);
				case "v2s":
					return GetExcuteInfoV2s(config);
				case "v3":
					return GetExcuteInfoV3(config);
				default:
					throw new Exception("不支持的版本");
			}
		}

		private static ExcuteInfo GetExcuteInfoV1(EdgeConfig config)
		{
			return new ExcuteInfo
			{
				FileName = AppDomain.CurrentDomain.BaseDirectory + "/EdgeFile/edge.1.exe",
				Args = BuilArgs(config)
			};
		}

		private static ExcuteInfo GetExcuteInfoV2(EdgeConfig config)
		{
			return new ExcuteInfo
			{
				FileName = AppDomain.CurrentDomain.BaseDirectory + "/EdgeFile/edge.2.exe",
				Args = BuilArgs(config)
			};
		}

		private static ExcuteInfo GetExcuteInfoV27(EdgeConfig config)
		{
			return new ExcuteInfo
			{
				FileName = AppDomain.CurrentDomain.BaseDirectory + "/EdgeFile/edge.2.7.exe",
				Args = BuilArgs(config)
			};
		}

		private static ExcuteInfo GetExcuteInfoV2s(EdgeConfig config)
		{
			return new ExcuteInfo
			{
				FileName = AppDomain.CurrentDomain.BaseDirectory + "/EdgeFile/edge.2s.exe",
				Args = BuilArgs(config)
			};
		}

		private static ExcuteInfo GetExcuteInfoV3(EdgeConfig config)
		{
			return new ExcuteInfo
			{
				FileName = AppDomain.CurrentDomain.BaseDirectory + "/EdgeFile/edge.3.exe",
				Args = BuilArgs(config)
			};
		}

		private static string BuilArgs(EdgeConfig config)
		{
			List<string> list = new List<string>();
			list.Add("-a " + config.EdgeIP);
			list.Add("-c " + config.EdgeGroup);
			list.Add("-k " + config.EdgePassword);
			list.Add($"-l {config.SuperNodeIP}:{config.SuperNodePort}");
			if (!string.IsNullOrEmpty(config.EdgeNetmask))
			{
				list.Add("-s " + config.EdgeNetmask);
			}
			if (config.ResolveSuperNode)
			{
				list.Add("-b");
			}
			if (config.PacketForwarding)
			{
				list.Add("-r");
			}
			if (config.Multicast)
			{
				list.Add("-E");
			}
			if (config.Verbose)
			{
				list.Add("-v");
			}
			if (config.LocalPort > 0)
			{
				list.Add($"-p {config.LocalPort}");
			}
			if (!string.IsNullOrEmpty(config.MacAddress))
			{
				list.Add("-m " + config.MacAddress);
			}
			if (config.MTU > 0)
			{
				list.Add($"-M {config.MTU}");
			}
			return string.Join(" ", list.ToArray());
		}
	}
}
