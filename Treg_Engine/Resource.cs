using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Treg_Engine.Graphics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenTK;
using OpenTK.Graphics.OpenGL;
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
        public static void ParseShaderJValue(Dictionary<string,object> shaderVars, JValue obj, string name2)
        {
            string val = obj.ToString();
            string[] split = val.Split(' ');
            if (split.Length == 4)
            {
                float x = float.Parse(split[0]);
                float y = float.Parse(split[1]);
                float z = float.Parse(split[2]);
                float w = float.Parse(split[3]);
                shaderVars[name2] = new OpenTK.Vector4(x, y, z, w);
            }
            else
            {
                float value = float.Parse(obj.ToString());
                shaderVars[name2] = value;
            }
        }
        public static void ParseShaderVars(Dictionary<string, object> shaderVars, JArray array, string name = "")
        {
            int I = 0;
            foreach(JToken obj in array.Children())
            {
                string name2 = name + string.Format("[{0}]", I);
                if (obj.GetType() == typeof(JArray))
                {
                    ParseShaderVars(shaderVars, (JArray)obj, name2);
                }
                else if (obj.GetType() == typeof(JValue))
                {
                    ParseShaderJValue(shaderVars, (JValue)obj, name2);
                }
                else if (obj.GetType() == typeof(JObject))
                {
                    ParseShaderVars(shaderVars, (JObject)obj, name2 + ".");
                }
                I++;
            }
        }
        public static void ParseShaderVars(Dictionary<string, object> shaderVars, JObject array, string name = "")
        {
            foreach( KeyValuePair<string, JToken> Object in array)
            {
                if(Object.Value.GetType() == typeof(JArray))
                {
                    ParseShaderVars(shaderVars, (JArray)Object.Value, name + Object.Key);
                }
                else if (Object.Value.GetType() == typeof(JValue))
                {
                    ParseShaderJValue(shaderVars, (JValue)Object.Value, name + Object.Key);
                }
                else if (Object.Value.GetType() == typeof(JObject))
                {
                    ParseShaderVars(shaderVars, (JObject)Object.Value, name + "." + Object.Key);
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
                ParseShaderVars(mat.shaderVars, (JObject)json["shadervars"]);
                mat.shader.Bind();
                foreach (KeyValuePair<string, object> vars in mat.shaderVars)
                {
                    if (vars.Value.GetType() == typeof(float))
                    {
                        mat.shader.SetUniformFloat(vars.Key, (float)vars.Value);
                    }
                    else if (vars.Value.GetType() == typeof(Vector4))
                    {
                        mat.shader.SetUniformVector4(vars.Key, (Vector4)vars.Value);
                    }
                }
            }
            if (json["skyboxVars"] != null)
            {
                Dictionary<string, object> vars = new Dictionary<string, object>();
                int quality = (int)json["skyboxVars"]["quality"];
                Mesh mesh = Mesh.LoadFromFile("resources/models/flat.obj");
                ParseShaderVars(vars, (JObject)json["skyboxVars"]["sky"]);
                Material material = Resource.LoadMaterial("gradient2");
                
                FBO fbo = new FBO(quality, quality);
                material.Bind();
                fbo.Bind();
                GL.ClearColor(System.Drawing.Color.Blue);   
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
                GL.Disable(EnableCap.CullFace);
                Matrix4 projectionMatrix = Matrix4.CreateOrthographicOffCenter(0, quality, quality, 0, 0f, 1024f);
                Matrix4 identity = Matrix4.Identity;
                Matrix4 ModelMatrix = Matrix4.CreateTranslation(Vector3.Zero);
                ModelMatrix *= Matrix4.CreateScale(quality, quality, 2f);
                
                foreach (KeyValuePair<string, object> var in vars)
                {
                    if (var.Value.GetType() == typeof(float))
                    {
                        material.shader.SetUniformFloat(var.Key, (float)var.Value);
                    }
                    else if (var.Value.GetType() == typeof(Vector4))
                    {
                        material.shader.SetUniformVector4(var.Key, (Vector4)var.Value);
                    }
                }
                mesh.Render(material, ModelMatrix, identity, projectionMatrix);
                
                fbo.UnBind();
                mat.texture2 = fbo.Texture;
                vars = new Dictionary<string, object>();
                ParseShaderVars(vars, (JObject)json["skyboxVars"]["sun"]);

                fbo = new FBO(quality, quality);
                material.Bind();
                fbo.Bind();
                GL.ClearColor(System.Drawing.Color.Black);
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
                GL.Disable(EnableCap.CullFace);
                foreach (KeyValuePair<string, object> var in vars)
                {
                    if (var.Value.GetType() == typeof(float))
                    {
                        material.shader.SetUniformFloat(var.Key, (float)var.Value);
                    }
                    else if (var.Value.GetType() == typeof(Vector4))
                    {
                        material.shader.SetUniformVector4(var.Key, (Vector4)var.Value);
                    }
                }
                mesh.Render(material, ModelMatrix, identity, projectionMatrix);

                fbo.UnBind();
                material.UnBind();
                GL.Enable(EnableCap.CullFace);
                mat.texture3 = Resource.LoadTexture("glow.png");
            }
            materials[path] = mat;
            return mat;
        }
    }
}
