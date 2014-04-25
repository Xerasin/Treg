using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Treg_Engine;
using Treg_Engine.Entities;
using Treg_Engine.Graphics;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
namespace Rainbow_Cube.Entities
{
    [EntityNameAttribute("ent_popcorn", true)]
    class Popcorn : BaseEntity
    {
        public Popcorn()
        {
            mesh = Mesh.LoadFromFile("resources/models/popcornmachine.obj");
            material = Resource.LoadMaterial("popcorn");
            this.Scale = Vector3.One * 4f;
        }

        public override void OnUpdate(double time)
        {
            //this.Angles.Pitch += 2;
            //this.Angles.Roll += 2;
        }
    }
}
