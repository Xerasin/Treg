using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLua;
using NLua.Exceptions;
namespace Treg_Engine.Scripting
{
    public static class LuaEntUtil
    {
        [RegisterLuaFunction("ents.GetAll")]
        public static LuaTable GetAll()
        {
            return MainLua.GetNewTable();
        }
    }
}
