using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using Treg_Engine;
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
