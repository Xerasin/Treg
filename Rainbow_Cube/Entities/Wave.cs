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
    [EntityNameAttribute("ent_wave", true)]
    class Wave : BaseEntity
    {
        public Wave()
        {
            mesh = Mesh.LoadFromFile("resources/models/cube.obj");
            material = Treg_Engine.Resource.LoadMaterial("white");
            this.Scale = new Vector3(200f, 1f, 200f);
        }
    }
}
