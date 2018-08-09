using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading.Tasks;

namespace ErrorMinor_cmd
{
    class LogChecker
    {
        public readonly ConfigData config;
        string errLogPath;
        string executeLogOutputPath;

        public LogChecker()
        {
            this.config = new ConfigData();
            this.errLogPath = "";
            this.executeLogOutputPath = ".\\checkLog.log";
        }

        public LogChecker(ConfigData config)
        {
            this.config = config;
            this.errLogPath = "";
            this.executeLogOutputPath = ".\\checkLog.log";
        }

        public LogChecker(string executeLogOutputPath)
        {
            try
            {
                this.config = new ConfigData(".\\config.json");
            }
            catch(FileNotFoundException e)
            {
                File.Create()
            }
            this.errLogPath = "";
            this.executeLogOutputPath = executeLogOutputPath;
        }
        
        public LogChecker(string executeLogOutputPath, ConfigData config)
        {
            this.config = config;
            this.errLogPath = "";
            this.executeLogOutputPath = executeLogOutputPath;
        }

        public List<string> StartCheck(string filePath)
        {
            List<string> errLogs;
            if (File.Exists(filePath))
            {
                this.errLogPath = filePath;
                using (StreamReader logFileStream = new StreamReader(filePath))
                {

                    errLogs = ExtractErrLog_FixedSizeMark(logFileStream, "", this.config);
                }
                return errLogs;
            }
            else
            {
                throw new FileNotFoundException();
            }
        }

        private List<string> ExtractErrLog_FixedSizeMark(StreamReader logFileStream, string executeLogOutputPath, ConfigData config)
        {
            string file_text = "";
            List<string> errorLogs = new List<string>();
            string[] slices;
            bool needIgnore;
            int i;
            try
            {
                file_text = logFileStream.ReadToEnd();
                slices = Regex.Split(file_text, config.SliceMark, RegexOptions.Multiline);
                foreach (string slice in slices)
                {
                    //最初或者最末尾会产生空的分片
                    if(slice.Length != 0)
                    {
                        needIgnore = false;
                        i = 0;
                        while (!needIgnore && i < config.WhiteListForRegex.Count)
                        {
                            needIgnore = Regex.IsMatch(slice, config.WhiteListForRegex[i]);
                            i = i + 1;
                        }
                        if (!needIgnore)
                        {
                            errorLogs.Add(slice);
                        }
                        else
                        {
                            //some log
                        }
                    }
                    else
                    {
                        //some log
                    }
                }
            }
            catch
            {

            }

            return errorLogs;
        }
    }
}
