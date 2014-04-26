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
            
            //this.Create<Teapot>();

            ent_skybox skybox = this.Create<ent_skybox>();
            skybox.Scale = Vector3.One * 30;
            Treg_Engine.Entities.BaseEntity Enttiy = this.Create<Popcorn>();
            Enttiy.Position = new Vector3(7, 0, -5);
            Enttiy = this.Create<Popcorn>();
            Enttiy.Position = new Vector3(-7, 0, -5);
            Enttiy = this.Create<Wave>();
            Enttiy.Position = new Vector3(0, 0, 0);
            /*env_pointlight light = this.Create<env_pointlight>();
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
            light2.Position = new Vector3(-5f, 0.0f, -5.0f);
            light2.Color = new Vector3(1f, 0f, 0f);
            light2.AmbientIntensity = 0f;
            light2.DiffuseIntensity = 1f;
            light2.Constant = 0f;
            light2.Linear = 0.1f;
            light2.Direction = new Vector3(0, -1, 0);
            light2.Cutoff = 0.1f;*/
            
            mesh = Mesh.LoadFromFile("resources/models/cube.obj");
            RegisterEntities();
            base.OnLoad();
            
        }
        public override void OnRender()
        {
            base.OnRender();
        }
    }
}
