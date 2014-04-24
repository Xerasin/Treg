﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using NLua;
using NLua.Exceptions;

namespace Treg_Engine.Scripting
{
    public static class MainLua
    {
        public static Lua LuaInstance;
        private static Dictionary<string, object> PermaLocals = new Dictionary<string, object>();

        public static void RegisterLuaFunction(object c, string methodname, string luaname)
        {
            Type pPrgType = (c).GetType();
            MethodInfo mInfo = pPrgType.GetMethod(methodname);
            LuaInstance.RegisterFunction(luaname, c, mInfo);
        }
        public static void RegisterLuaFunction(Type c, MethodInfo mInfo, string luaname)
        {
            LuaInstance.RegisterFunction(luaname, c, mInfo);
        }
        public static void RegisterLuaFunction(Type c, string methodname, string luaname)
        {
            Type pPrgType = c;
            MethodInfo mInfo = pPrgType.GetMethod(methodname);
            LuaInstance.RegisterFunction(luaname, c, mInfo);
        }
        public static void PreLoad()
        {
            LuaInstance = new Lua();
            LuaInstance.NewTable("timer");
            LuaInstance.NewTable("hook");
        }
        public static void LoadAll()
        {
            registerAttributesFromClass(typeof(LuaTimer));
            registerAttributesFromClass(typeof(LuaHook));
            IncludeFolder("modules");
            IncludeFolder("autorun");
        }
        public static LuaTable GetNewTable()
        {
            LuaInstance.NewTable("t");
            return (LuaTable)LuaInstance["LuaTable"];
        }
        private static void ReadInternalLua(string lua)
        {
            Assembly _assembly;
            _assembly = Assembly.GetExecutingAssembly();
            string assembly = _assembly.GetName().FullName;
            Stream stream = _assembly.GetManifestResourceStream("Treg_Engine"+"."+lua);
            StreamReader reader = new StreamReader(stream);
            DoString(reader.ReadToEnd());
        }

        private static void IncludeFolder(string folder)
        {
            try
            {
                foreach (string file in Directory.GetFiles(@"Resources\lua\"+folder))
                {
                    try
                    {
                        LuaInstance.DoFile(file);
                    }
                    catch (LuaException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
            catch (TargetException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (DirectoryNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }
        }
       

        private static Dictionary<string, object> MergeTab(Dictionary<string, object> table1, Dictionary<string, object> table2)
        {
            Dictionary<string, object> NewTable = new Dictionary<string,object>();
            foreach (KeyValuePair<string, object> entry in table1)
            {
                NewTable[entry.Key] = entry.Value;
            }
            foreach (KeyValuePair<string, object> entry in table2)
            {
                NewTable[entry.Key] = entry.Value;
            }
            return NewTable;
        }

        public static void AddPermaLocal(string key, object value)
        {
            if (value == null && PermaLocals.ContainsKey(key))
            {
                PermaLocals.Remove(key);
            }
            else
            {
                PermaLocals[key] = value;
            }
           
        }
        public static void registerAttributesFromClass(Type pTargetType)
        {
            foreach (MethodInfo method in pTargetType.GetMethods())
            {
                foreach (Attribute attribute in Attribute.GetCustomAttributes(method))
                {
                    if (attribute.GetType() == typeof(RegisterLuaFunction))
                    {
                        RegisterLuaFunction function = (RegisterLuaFunction)attribute;
                        RegisterLuaFunction(pTargetType, method, function.name);
                    }
                }
            }
        }
        public static void print(string text)
        {

        }

        public static void DoString(string code, params object[] t)
        {
            Dictionary<string, object> stuff = new Dictionary<string, object>();
            if (t.Length % 2 == 0)
            {
                for (int I = 0; I < t.Length; I += 2)
                {
                    object key = t[I];
                    object value = t[I + 1];
                    if (key.GetType() == typeof(string))
                    {
                        stuff[(string)key] = value;
                    }
                }
                DoString(code, stuff);
            }
            else
            {
                throw new Exception("Invalid arguments");
            }
        }

        public static void DoString(string code, Dictionary<string, object> locals)
        {
            string header = "";
            Dictionary<string, object> newLocals = MergeTab(PermaLocals, locals);
            foreach (KeyValuePair<string, object> local in newLocals)
            {
                //LuaInstance[local.Key] = local.Value;
                header += "local " + local.Key + " = " + local.Key + " ";
            }

            LuaInstance.DoString(header + "\n" + code);

            /*foreach (KeyValuePair<string, object> local in newLocals)
            {
                //LuaInstance[local.Key] = null;
            }*/
        }
    }
}