using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Treg_Engine.Entities;
using Treg_Engine.Graphics;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
namespace Rainbow_Cube.Entities
{
    class Teapot : BaseEntity
    {
        public Teapot()
        {
            mesh = Mesh.LoadFromFile("resources/models/popcornmachine.obj");
            material = new Material("resources/materials/popcorn_machine.png");
            this.Scale = Vector3.One * 4f;
        }

        public override void OnUpdate(double time)
        {
            //this.Angles.Pitch += 2;
            //this.Angles.Roll += 2;
        }
    }
}
