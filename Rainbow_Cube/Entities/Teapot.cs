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
using Treg_Engine;
namespace Rainbow_Cube.Entities
{
    class Teapot : BaseEntity
    {
        public Teapot()
        {
            mesh = Mesh.LoadFromFile("resources/models/popcornmachine.obj");
            material = Resource.LoadMaterial("popcorn");
            this.Scale = Vector3.One * 4f;
        }

        public override void OnUpdate(double time)
        {
            this.Angles.Pitch += 2;
            this.Scale.X += 0.1f;
            //this.Angles.Roll += 2;
        }
    }
}
