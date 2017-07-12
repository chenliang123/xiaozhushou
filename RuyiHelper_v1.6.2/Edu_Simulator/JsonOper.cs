using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Edu_Simulator
{
    public class JsonOper
    {
        public static T DeserializeJsonToObject<T>(string json) where T : class
        {
            JsonSerializer serializer = new JsonSerializer();
            StringReader sr = new StringReader(json);
            object o = serializer.Deserialize(new JsonTextReader(sr), typeof(T));
            T t = o as T;
            return t;
        }

        public static string GetString(string text, int length, string replacetxt)
        {
            int strLength = 0;
            StringBuilder strb = new StringBuilder();
            char[] Temp = text.ToCharArray();
            for (int i = 0; i != Temp.Length; i++)
            {
                if (strLength >= length) //
                {
                    strb.Append(replacetxt);
                    break;
                }
                else
                {
                    if (((int)Temp[i]) < 255) 
                    {
                        strLength++;
                    }
                    else
                    {
                        strLength = strLength + 2;
                    }
                    strb.Append(Temp[i]);
                }
            }
            return strb.ToString();
        }

        public static string ReturnSplitBrString(string str)
        {
            string newStr="";
            for (int i = 0; i < str.Length; i++)
            {
                if ((i + 1) % 43 == 0)
                {
                    newStr += str[i] + "\r\n";
                }
                else
                {
                    newStr += str[i];
                }
            }
            return newStr;
        }
    }
}
