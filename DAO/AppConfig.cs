using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    internal class AppConfig
    {
        private string m_file_name;
        private JObject m_configRoot;

        public static readonly AppConfig Instance = new AppConfig();
        public string ConnectionString { get; private set; }
        private AppConfig()
        {
            Init();
        }

        internal void Init(string file_name = @"C:\projects\XfoneHomeAssignment\DAO\ConfigFile.json")
        {
            m_file_name = file_name;

            if (!File.Exists(m_file_name))
            {
                Environment.Exit(-1);
            }

            var reader = File.OpenText(m_file_name);
            string json_string = reader.ReadToEnd();

            JObject jo = (JObject)JsonConvert.DeserializeObject(json_string);
            m_configRoot = (JObject)jo["Website"];
            ConnectionString = m_configRoot["ConnectionString"].Value<string>();
        }
    }
}
