using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Treg_Engine;
using Treg_Engine.Entities;
using Treg_Engine.Graphics;
using OpenTK;
using OpenTK.Graphics.OpenGL;
namespace Rainbow_Cube.Entities
{
    [EntityNameAttribute("ent_skybox", false)]
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
            GL.DepthMask(false);
            GL.DepthFunc(DepthFunction.Lequal);
            Angle ang = new Angle(0, 0, 90);
            this.material.Bind();
            this.material.shader.SetUniformVector3("sunPos", ang.Up);
            Matrix4 c = Matrix4.Identity;
            c *= Matrix4.CreateScale(0.5f);
            c *= Matrix4.CreateTranslation(View.EyePos);
            float test = Vector3.Dot(new Vector3(-1, 0, 0), new Vector3(1, 0, 0));
            if (mesh != null)
            {
                mesh.Render(material, c, View.ProjectionMatrix, View.ViewMatrix);
            }
            GL.DepthMask(true);
            GL.DepthFunc(DepthFunction.Less);
        }

    }
}
