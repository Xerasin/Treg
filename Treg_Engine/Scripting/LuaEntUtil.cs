using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLua;
using NLua.Exceptions;
using Treg_Engine.Entities;
namespace Treg_Engine.Scripting
{
    public static class LuaEntUtil
    {
        [RegisterLuaFunction("ents.GetAll")]
        public static LuaTable GetAll()
        {
            LuaTable table = MainLua.GetNewTable();
            World world = Window.GetWorld();
            foreach (BaseEntity entity in world.Entities)
            {
                table[entity.EntIndex] = entity;
            }
            return table;
        }

        [RegisterLuaFunction("ents.Create")]
        public static BaseEntity Create(string type)
        {
            World world = Window.GetWorld();
            return world.Create(type);
        }
    }
}
