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
    struct BaseLightLocations
    {
        public int Color;
        public int Ambient;
        public int Diffuse;
    }
    struct DirectionalLightLocations
    {
        public BaseLightLocations Base;
        public int Direction;
    }

    struct AttenuationLocations
    {
        public int Constant;
        public int Linear;
        public int Exp;
    }
    struct PointLightLocations
    {
        public BaseLightLocations Base;
        public int Position;
        public AttenuationLocations Atten;
    }
    struct SpotLightLocations
    {
        public PointLightLocations Base;
        public int Direction;
        public int Cutoff;
    }
    public struct BaseLight
    {
        public Vector3 Color;
        public float Ambient;
        public float Diffuse;
    }
    public struct DirectionalLight
    {
        public BaseLight Base;
        public Vector3 Direction;
    }
    public struct Attenuation
    {
        public float Constant;
        public float Linear;
        public float Exp;
    }
    public struct PointLight
    {
        public BaseLight Base;
        public Vector3 Position;
        public Attenuation Atten;
        public float DieTime;
    }
    public struct SpotLight
    {
        public PointLight Base;
        public Vector3 Direction;
        public float Cutoff;
    }
    public static class Lighting
    {
        const int MAX_SPOTLIGHTS = 4;
        const int MAX_POINTLIGHTS = 4;
        public static Shader shader;
        private static DirectionalLightLocations enviromental;
        private static PointLightLocations[] pointLightLocations = new PointLightLocations[MAX_POINTLIGHTS];
        private static SpotLightLocations[] spotLightLocations = new SpotLightLocations[MAX_SPOTLIGHTS];
        private static Dictionary<int, PointLight> pointLights = new Dictionary<int, PointLight>();
        public static void Init()
        {
            shader = Resource.LoadShader("basicrender");
            GL.UseProgram(shader.programID);
            enviromental.Base.Ambient = GL.GetUniformLocation(shader.programID, "gEnviromentalLight.Base.AmbientIntensity");
            enviromental.Base.Color = GL.GetUniformLocation(shader.programID, "gEnviromentalLight.Base.Color");
            enviromental.Base.Diffuse = GL.GetUniformLocation(shader.programID, "gEnviromentalLight.Base.DiffuseIntensity");
            enviromental.Direction = GL.GetUniformLocation(shader.programID, "gEnviromentalLight.Direction");

            for (int I = 0; I < MAX_POINTLIGHTS; I++)
            {
                string path = string.Format("gPointLights[{0}]", I);
                pointLightLocations[I].Position = GL.GetUniformLocation(shader.programID, path + ".Position");

                pointLightLocations[I].Atten.Constant = GL.GetUniformLocation(shader.programID, path + ".Atten.Constant");
                pointLightLocations[I].Atten.Exp = GL.GetUniformLocation(shader.programID, path + ".Atten.Exp");
                pointLightLocations[I].Atten.Linear = GL.GetUniformLocation(shader.programID, path + ".Atten.Linear");

                pointLightLocations[I].Base.Ambient = GL.GetUniformLocation(shader.programID, path + ".Base.AmbientIntensity");
                pointLightLocations[I].Base.Color = GL.GetUniformLocation(shader.programID, path + ".Base.Color");
                pointLightLocations[I].Base.Diffuse = GL.GetUniformLocation(shader.programID, path + ".Base.DiffuseIntensity");

            }
        }
        public static void SetupEnviromentalLight(DirectionalLight light)
        {
            GL.Uniform3(enviromental.Base.Color, light.Base.Color);
            GL.Uniform3(enviromental.Direction, light.Direction);
            GL.Uniform1(enviromental.Base.Ambient, light.Base.Ambient);
            GL.Uniform1(enviromental.Base.Diffuse, light.Base.Diffuse);
        } 

        public static void SetupPointLight(PointLight light, int I)
        {
            GL.Uniform3(pointLightLocations[I].Base.Color, light.Base.Color);
            GL.Uniform3(pointLightLocations[I].Position, light.Position);
            GL.Uniform1(pointLightLocations[I].Base.Ambient, light.Base.Ambient);
            GL.Uniform1(pointLightLocations[I].Base.Diffuse, light.Base.Diffuse);

            GL.Uniform1(pointLightLocations[I].Atten.Linear, light.Atten.Linear);
            GL.Uniform1(pointLightLocations[I].Atten.Exp, light.Atten.Exp);
            GL.Uniform1(pointLightLocations[I].Atten.Constant, light.Atten.Constant);
        }

        public static void SetPointLight(PointLight light, int I)
        {
            pointLights[I] = light;
        }
        public static void Think()
        {
            List<int> lightsToBeRemoved = new List<int>();
            foreach (KeyValuePair<int, PointLight> entry in pointLights)
            {
                PointLight pLight = entry.Value;
                if (pLight.DieTime <= Util.Time)
                {
                    lightsToBeRemoved.Add(entry.Key);
                }
            }
            foreach (int light in lightsToBeRemoved)
            {
                pointLights.Remove(light);
            }
        }
        public static void SetupLighting()
        {

            DirectionalLight light;
            light.Base.Color = new Vector3(0.5f, 0.5f, 0.5f);
            light.Base.Ambient = 0.5f;
            light.Base.Diffuse = 1f;
            light.Direction = new Vector3(1f, 1f, 1f);
            int I = 0;
            List<PointLight> lightsToBeRemoved = new List<PointLight>();
            foreach (KeyValuePair<int, PointLight> entry in pointLights)
            {
                PointLight pLight = entry.Value;
                if (I < MAX_POINTLIGHTS && pLight.DieTime >= Util.Time)
                {
                    SetupPointLight(pLight, I);
                    I++;
                }
            }
            SetupEnviromentalLight(light);
        }
    }
}
