using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
namespace Treg_Engine.Graphics
{
    public class Shader
    {
        public static void Init()
        {
            DefaultShader = Shader.LoadFromFile("resources/shaders/basicrender.vs", "resources/shaders/basicrender.fs");
            Shader2D = Shader.LoadFromFile("resources/shaders/hud.vs", "resources/shaders/hud.fs");
        }
        public static Shader Shader2D;
        public static Shader DefaultShader;
        public int vertexObject;
        public int fragmentObject;
        public int programID;
        private Dictionary<string, int> uniformlocs = new Dictionary<string,int>();
        
        public Shader(string vertex = "", string fragment = "")
        {
            int status_code;
            string info;

            vertexObject = GL.CreateShader(ShaderType.VertexShader);
            fragmentObject = GL.CreateShader(ShaderType.FragmentShader);
            programID = GL.CreateProgram();
            if (vertex != "")
            {
                // Compile vertex shader
                GL.ShaderSource(vertexObject, vertex);
                GL.CompileShader(vertexObject);
                GL.GetShaderInfoLog(vertexObject, out info);
                GL.GetShader(vertexObject, ShaderParameter.CompileStatus, out status_code);

                if (status_code != 1)
                     throw new ApplicationException(info);
                GL.AttachShader(programID, vertexObject);
            }
            if (fragment != "")
            {
                // Compile vertex shader
                GL.ShaderSource(fragmentObject, fragment);
                GL.CompileShader(fragmentObject);
                GL.GetShaderInfoLog(fragmentObject, out info);
                GL.GetShader(fragmentObject, ShaderParameter.CompileStatus, out status_code);

                if (status_code != 1)
                    throw new ApplicationException(info);
                GL.AttachShader(programID, fragmentObject);
                }
            GL.LinkProgram(programID);
            GL.UseProgram(programID);
        }
        public static Shader LoadFromFile(string vertex = "", string fragment = "")
        {
            string vertex_shader = "", fragment_shader = "";
            if (vertex != "")
            {
                using (StreamReader vertexReader = new StreamReader(vertex))
                {
                    vertex_shader = vertexReader.ReadToEnd();
                }
            }
            if (fragment != "")
            {
                using (StreamReader fragmentReader = new StreamReader(fragment))
                {
                    fragment_shader = fragmentReader.ReadToEnd();
                }
            }
            return new Shader(vertex_shader, fragment_shader);
        }
        public int GetLocation(string name)
        {
            int location;
            if (!uniformlocs.ContainsKey(name))
            {
                location = GL.GetUniformLocation(programID, name);
                uniformlocs[name] = location;
            }
            return uniformlocs[name];
        }

        public void SetUniformInt(string name, int num)
        {
            int location = GetLocation(name);
            GL.Uniform1(location, num);
        }

        public void SetUniformFloat(string name, float num)
        {
            int location = GetLocation(name);
            GL.Uniform1(location, num);
        }

        public void SetUniformMatrix4(string name, Matrix4 mat)
        {

            int location = GetLocation(name);
            GL.UniformMatrix4(location, false, ref mat);
        }

        public void SetUniformVector3(string name, Vector3 vec)
        {

            int location = GetLocation(name);
            GL.Uniform3(location, vec);
        }
        public void SetUniformVector4(string name, Vector4 vec)
        {

            int location = GetLocation(name);
            GL.Uniform4(location, vec);
        }
        public void Bind()
        {
            GL.UseProgram(programID);
        }
    }
}
