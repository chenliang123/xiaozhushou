using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RueMng.util
{
    class CmdArgParser
    {
         List<CmdArg> _arguments;
         public static CmdArgParser Parse(string[] args) {
             return new CmdArgParser(args);
         }

         public CmdArgParser(string[] args)
         {
             _arguments = new List<CmdArg>();
     
             List<string> strlist = new List<string>();
             for (int i = 0; i < args.Length; i++)
             {
                 string v = args[i];
                 if(v.StartsWith("-") && v.Length>2)
                 {
                     string v1 = v.Substring(0, 2);
                     string v2 = v.Substring(2);
                     strlist.Add(v1);
                     strlist.Add(v2);
                 }
                 else
                 {
                     strlist.Add(v);
                 }
             }

             for (int j= 0; j < strlist.Count; j++)
             {
                 _arguments.Add(new CmdArg(_arguments,j, strlist[j]));
             }
     
         }

         public CmdArg Get(string argumentName)
         {
             return _arguments.FirstOrDefault(p => p == argumentName);
         }
     
         public bool Has(string argumentName) {
             return _arguments.Count(p=>p==argumentName)>0;
         }
    }
}
