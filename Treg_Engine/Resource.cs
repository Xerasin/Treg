using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Treg_Engine.Graphics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace Treg_Engine
{
    public static class Resource
    {
        const string ResourcePath = "Resources/";
        const string MaterialPath = "materials/";
        const string MaterialExt = ".tregmat";
        const string ShaderPath = "shaders/";
        const string FragShaderExt = ".fs";
        const string VertShaderExt = ".vs";
        static Dictionary<string, Shader> shaders = new Dictionary<string, Shader>();
        public static Shader LoadShader(string path)
        {
            if (shaders.ContainsKey(path))
            {
                return shaders[path];
            }
            string folder = ResourcePath + ShaderPath;
            Shader shader = Shader.LoadFromFile(folder + path + VertShaderExt, folder + path + FragShaderExt);
            shaders[path] = shader;
            return shader;
        }
        static Dictionary<string, Texture> textures = new Dictionary<string, Texture>();
        public static Texture LoadTexture(string path)
        {
            if (textures.ContainsKey(path))
            {
                return textures[path];
            }
            Texture tex = new Texture(ResourcePath + MaterialPath + path);
            textures[path] = tex;
            return tex;
        }
        public static void ParseShaderJValue(Material mat, JValue obj, string name2)
        {
            string val = obj.ToString();
            string[] split = val.Split(' ');
            if (split.Length == 4)
            {
                float x = float.Parse(split[0]);
                float y = float.Parse(split[1]);
                float z = float.Parse(split[2]);
                float w = float.Parse(split[3]);
                mat.shaderVars[name2] = new OpenTK.Vector4(x, y, z, w);
            }
            else
            {
                float value = float.Parse(obj.ToString());
                mat.shaderVars[name2] = value;
            }
        }
        public static void ParseShaderVars(Material mat, JArray array, string name = "")
        {
            int I = 0;
            foreach(JToken obj in array.Children())
            {
                string name2 = name + string.Format("[{0}]", I);
                if (obj.GetType() == typeof(JArray))
                {
                    ParseShaderVars(mat, (JArray)obj, name2);
                }
                else if (obj.GetType() == typeof(JValue))
                {
                    ParseShaderJValue(mat, (JValue)obj, name2);
                }
                I++;
            }
        }
        public static void ParseShaderVars(Material mat, JObject array)
        {
            foreach( KeyValuePair<string, JToken> Object in array)
            {
                if(Object.Value.GetType() == typeof(JArray))
                {
                    ParseShaderVars(mat, (JArray)Object.Value, Object.Key);
                }
                else if (Object.Value.GetType() == typeof(JValue))
                {
                    ParseShaderJValue(mat, (JValue)Object.Value, Object.Key);
                }
            }
        }
        static Dictionary<string, Material> materials = new Dictionary<string, Material>();
        public static Material LoadMaterial(string path)
        {
            if (materials.ContainsKey(path))
            {
                return materials[path];
            }
            string info = "";
            using (StreamReader reader = new StreamReader(ResourcePath + MaterialPath + path + MaterialExt))
            {
                info = reader.ReadToEnd();
            }
            JObject json = null;
            try
            {
                json = JObject.Parse(info);
            }
            catch (Exception e)
            {
                Console.WriteLine("failed to load material: " + path);
                return Material.debugWhite;
            }
            Material mat = new Material();
            if (json["shader"] != null)
            {
                mat.shader = Resource.LoadShader(json["shader"].ToString());
            }

            if (json["material"] != null)
            {
                mat.texture = Resource.LoadTexture(json["material"].ToString());
            }
            if (json["shadervars"] != null)
            {
                ParseShaderVars(mat, (JObject)json["shadervars"]);
            }
            materials[path] = mat;
            return mat;
        }
    }
}
