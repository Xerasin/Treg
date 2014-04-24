using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Treg_Engine.Scripting
{
    class RegisterLuaFunction : Attribute
    {
        public string details;
        public string name;
        public string args;
        public RegisterLuaFunction(string name, string details = "", string args = "")
        {
            this.details = details;
            this.name = name;
            this.args = args;
        }

        
    }
}
