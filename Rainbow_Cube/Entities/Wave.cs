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
    class Wave : BaseEntity
    {
        public Wave()
        {
            mesh = Mesh.LoadFromFile("resources/models/box.obj");
            material = new Material("resources/materials/popcorn_machine.png");
        }
    }
}
