using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EdgeClient.Model
{
    public class Config
    {
        /// <summary>
        /// 当前连接配置名称
        /// </summary>
        public string CurrentEdgeConfigName { get; set; }
        /// <summary>
        /// 启动后自动连接
        /// </summary>
        public bool AutoConnect { get; set; }
    }
}
