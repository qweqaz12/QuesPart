using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace M_Utils{

    public class Utils {

        public static Utils instance;
        public Dictionary<string, Dictionary<string, string>> ConnectInfo = new Dictionary<string, Dictionary<string, string>>();
        

        public static Utils getinstance()
        {
            if (instance == null)
                instance = new Utils();
            return instance;
        }
        public void ReadConnectInfo(string path)
        {
            FileStream filestream = new FileStream(path, FileMode.Open, FileAccess.Read);
            StreamReader read = new StreamReader(filestream); 
            string str = read.ReadLine();
            string str_pre="";
            Dictionary<string,string> Info = new Dictionary<string,string>();
            int i = 0;
            while(str!=null)
            {
                if (str.Contains("[") && str.Contains("]"))
                {
                    str=str.Replace("[", "");
                    str=str.Replace("]", "");
                    str_pre = str;
                    if (!ConnectInfo.ContainsKey(str))
                        ConnectInfo.Add(str, Info);
                    else
                        ConnectInfo[str] = Info;
                }
                else
                {
                    string[] str2 = str.Split('=');
                    Info.Add(str2[0], str2[1]);
                }
                
                str = read.ReadLine();
            }
            ConnectInfo[str_pre] = Info;
        }

        public  string ReturnInfo(string key,string keyinfo)
        {
            Dictionary<string, string> info = ConnectInfo[key];
            return info[keyinfo];
        }

        public List<T> parse<T>(string json)
        {
            List<T> t2  = JsonConvert.DeserializeObject<List<T>>(json);
            return t2;
        }
    
    }
}

