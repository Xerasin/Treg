using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Treg_Engine;
using Treg_Engine.Entities;
using Treg_Engine.Graphics;
using OpenTK;
namespace Rainbow_Cube.Entities
{
    [EntityNameAttribute("ent_skybox", true)]
    class ent_skybox : BaseEntity
    {
        public ent_skybox()
        {
            mesh = Mesh.LoadFromFile("resources/models/skybox.obj");
            material = Treg_Engine.Resource.LoadMaterial("skybox");
            //this.Scale = Vector3.One * -1;
        }

        public override void OnRender()
        {
            Angle ang = new Angle((Util.Time * 20) % 360, 0, 0);
            this.material.Bind();
            this.material.shader.SetUniformVector3("sunPos", ang.Up);
            base.OnRender();
        }

    }
}
