using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using Treg_Engine;
using Treg_Engine.Entities;
using Treg_Engine.Graphics;
using Rainbow_Cube.Entities;
namespace Rainbow_Cube
{
    class RainbowWorld : World
    {
        Mesh mesh;
        public override void OnLoad()
        {
            base.OnLoad();
            //this.Create<Teapot>();
            Treg_Engine.Entities.BaseEntity Enttiy = this.Create<Popcorn>();
            Enttiy.Position = new Vector3(5, -5, -5);
            Enttiy = this.Create<Popcorn>();
            Enttiy.Position = Vector3.One * -5;
            env_pointlight light = this.Create<env_pointlight>();
            light.Enabled = true;
            light.Position = new Vector3(0, -5, -5);
            light.Color = new Vector3(0.5f, 0f, 0f);
            light.AmbientIntensity = 0.5f;
            light.DiffuseIntensity = 1f;
            light.Constant = 0f;
            light.Linear = 0.25f;
            mesh = Mesh.LoadFromFile("resources/models/cube.obj");

            light = this.Create<env_pointlight>();
            light.Enabled = true;
            light.Position = new Vector3(0, 4, -5);
            light.Color = new Vector3(0.5f, 0.5f, 0f);
            light.AmbientIntensity = 0.5f;
            light.DiffuseIntensity = 1f;
            light.Constant = 0f;
            light.Linear = 0.25f;

            env_spotlight light2 = this.Create<env_spotlight>();
            light2.Enabled = true;
            light2.Position = new Vector3(-5f, 4.0f, -5.0f);
            light2.Color = new Vector3(1f, 0f, 0f);
            light2.AmbientIntensity = 0.5f;
            light2.DiffuseIntensity = 1f;
            light2.Constant = 0f;
            light2.Linear = 0.1f;
            light2.Direction = new Vector3(0, -1, 0);
            light2.Cutoff = 0.1f;
            mesh = Mesh.LoadFromFile("resources/models/cube.obj");
            
        }
        public override void OnRender()
        {
            base.OnRender();
            GL.PushMatrix();
            Matrix4 mat = Matrix4.Identity;
            mat = Matrix4.Mult(Matrix4.CreateScale(Vector3.One * 50), mat);
            GL.MultMatrix(ref mat);
            //mesh.Render();
            GL.PopMatrix();
        }
    }
}
