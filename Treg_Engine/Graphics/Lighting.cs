using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
namespace Treg_Engine.Graphics
{
    public struct DirectionalLightLocations
    {
        public int Color;
        public int Ambient;
        public int Diffuse;
        public int Direction;
    }
    public static class Lighting
    {
        public static Shader shader;
        public static DirectionalLightLocations enviromental;
        public static void Init()
        {
            shader = Resource.LoadShader("basicrender");

            enviromental.Ambient = GL.GetUniformLocation(shader.programID, "gEnviromentalLight.Base.AmbientIntensity");
            enviromental.Color = GL.GetUniformLocation(shader.programID, "gEnviromentalLight.Base.Color");
            enviromental.Diffuse = GL.GetUniformLocation(shader.programID, "gEnviromentalLight.Base.DiffuseIntensity");
            enviromental.Direction = GL.GetUniformLocation(shader.programID, "gEnviromentalLight.Direction");
        }
        public static void SetupLighting()
        {
            GL.Uniform3(enviromental.Color, new Vector3(0.5f, 0.5f, 0.5f));
            GL.Uniform3(enviromental.Direction, new Vector3((float)Math.Cos(Util.Time), 1.0f, (float)Math.Sin(Util.Time)));
            GL.Uniform1(enviromental.Ambient, 0.5f);
            GL.Uniform1(enviromental.Diffuse, 0.5f);
        }
    }
}
