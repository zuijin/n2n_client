using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EdgeClient.Model
{
    public class EdgeConfig
    {
        /// <summary>
        /// N2N版本
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// 中心节点ip
        /// </summary>
        public string SuperNodeIP { get; set; }
        /// <summary>
        /// 中心节点端口
        /// </summary>
        public int SuperNodePort { get; set; }
        /// <summary>
        /// 边缘节点ip
        /// </summary>
        public string EdgeIP { get; set; }
        /// <summary>
        /// 子网掩码
        /// </summary>
        public string EdgeNetmask { get; set; }
        /// <summary>
        /// 边缘节点分组
        /// </summary>
        public string EdgeGroup { get; set; }
        /// <summary>
        /// 边缘节点密码
        /// </summary>
        public string EdgePassword { get; set; }
        /// <summary>
        /// 本地端口
        /// </summary>
        public int LocalPort { get; set; }
        /// <summary>
        /// 本地mac地址
        /// </summary>
        public string MacAddress { get; set; }
        /// <summary>
        /// 周期检查是否连通
        /// </summary>
        public bool ResolveSuperNode { get; set; }
        /// <summary>
        /// 允许服务器转发数据
        /// </summary>
        public bool PacketForwarding { get; set; }
        /// <summary>
        /// MTU
        /// </summary>
        public int MTU { get; set; }
        /// <summary>
        ///  接受多播MAC地址
        /// </summary>
        public bool Multicast { get; set; }
        /// <summary>
        /// 启用详细输出模式
        /// </summary>
        public bool Verbose { get; set; }
        /// <summary>
        /// 扩展参数
        /// </summary>
        public string ExtensionArgs { get; internal set; }
    }
}
