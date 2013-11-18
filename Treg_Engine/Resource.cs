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
            materials[path] = mat;
            return mat;
        }
    }
}
