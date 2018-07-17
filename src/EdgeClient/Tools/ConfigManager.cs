using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using EdgeClient.Model;

namespace EdgeClient.Tools
{
    public class ConfigManager
    {
        private static string EDGE_CONFIG_DIR = "Config";

        /// <summary>
        /// 获取配置
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static EdgeConfig GetEdgeConfig(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }

            string path = $"{EDGE_CONFIG_DIR}/{name}.json";
            if (!File.Exists(path))
            {
                return null;
            }

            return JsonConvert.DeserializeObject<EdgeConfig>(File.ReadAllText(path));
        }

        public static string GetFirstEdgeConfigName()
        {
            return GetAllEdgeConfigName().FirstOrDefault();
        }

        public static List<string> GetAllEdgeConfigName()
        {
            List<string> list = new List<string>();
            string dir = EDGE_CONFIG_DIR;

            if (!Directory.Exists(dir))
            {
                return list;
            }

            var files = Directory.GetFiles(dir);
            foreach (var path in files)
            {
                var tmp = path;
                tmp = tmp.Substring(tmp.LastIndexOf("\\") + 1);
                tmp = tmp.Substring(0, tmp.LastIndexOf("."));

                list.Add(tmp);
            }

            return list.OrderBy(it => it).ToList();
        }

        public static void SaveEdgeConfig(string name, EdgeConfig config)
        {
            string path = $"{EDGE_CONFIG_DIR}/{name}.json";
            string configText = JsonConvert.SerializeObject(config, Formatting.Indented);

            if (!Directory.Exists(EDGE_CONFIG_DIR))
            {
                Directory.CreateDirectory(EDGE_CONFIG_DIR);
            }

            File.WriteAllText(path, configText);
        }

        public static void DelEdgeConfig(string name)
        {
            string path = $"{EDGE_CONFIG_DIR}/{name}.json";
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public static Config GetConfig()
        {
            string path = "Config.json";
            if (!File.Exists(path))
            {
                return new Config();
            }

            return JsonConvert.DeserializeObject<Config>(File.ReadAllText(path));
        }

        public static void SaveConfig(Config config)
        {
            string path = "Config.json";
            string configText = JsonConvert.SerializeObject(config, Formatting.Indented);

            File.WriteAllText(path, configText);
        }
    }
}
