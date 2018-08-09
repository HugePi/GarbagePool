using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;


namespace ErrorMinor_cmd
{
    class ConfigData
    {
        public List<string> WhiteListForRegex
        {
            get; set;
        }

        public string SliceMark
        {
            get; set;
        }

        public bool ShowResult
        {
            get; set;
        }

        public ConfigData()
        {
            this.SliceMark = @"^【[0-9]{4}-[0-9]{2}-[0-9]{2} [0-9]{2}:[0-9]{2}:[0-9]{2}】"; //目标字符串示例： 【2018-06-05 17:11:40】
            this.ShowResult = true;
            this.WhiteListForRegex = new List<string>();
            /*
            this.WhiteListForRegex = new List<string>();
            this.WhiteListForRegex.Add(@"当应用程序不是以 UserInteractive 模式运行时");
            this.WhiteListForRegex.Add(@"产品安装参数【客户名称】为空");
            */
        }

        public ConfigData(string filePath)
        {
            if (File.Exists(filePath))
            {
                ConfigData tmp = ConfigData.GetConfig(filePath);
                this.SliceMark = tmp.SliceMark;
                this.ShowResult = tmp.ShowResult;
                this.WhiteListForRegex = tmp.WhiteListForRegex;
            }
            else
            {
                throw new FileNotFoundException();
            }
        }

        public static ConfigData GetConfig(string filePath)
        {
            if (File.Exists(filePath))
            {
                using (StreamReader sReader = new StreamReader(filePath))
                {
                    string configText = sReader.ReadToEnd();
                    return JsonConvert.DeserializeObject<ConfigData>(configText);
                }
            }
            else
            {
                throw new FileNotFoundException();
            }

        }
    }
}
