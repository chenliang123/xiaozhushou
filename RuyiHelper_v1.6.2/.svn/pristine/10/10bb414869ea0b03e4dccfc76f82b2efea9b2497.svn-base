using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RueMng.util
{
    class CmdArg
    {
        List<CmdArg> _arguments;
        int _index;
        string _argumentText;
        public CmdArg Next
         {
             get {
                 if (_index < _arguments.Count - 1) {
                     return _arguments[_index + 1];
                 }
 
                 return null;
             }
         }
         public CmdArg Previous
         {
             get {
                 if (_index > 0)
                 {
                     return _arguments[_index - 1];
                 }
 
                 return null;
             }
         }
         internal CmdArg(List<CmdArg> args, int index, string argument)
         {
             _arguments = args;
             _index = index;
             _argumentText = argument;
         }
 
         public CmdArg Take() {
             return Next;
         }
 
         public IEnumerable<CmdArg> Take(int count)
         {
             var list = new List<CmdArg>();
             var parent = this;
             for (int i = 0; i < count; i++)
             {
                 var next = parent.Next;
                 if (next == null)
                     break;
 
                 list.Add(next);
 
                 parent = next;
             }
 
             return list;
         }
 
         public static implicit operator string(CmdArg argument)
         {
             return argument._argumentText;
         }
 
         public override string ToString()
         {
             return _argumentText;
         }
    }
}
