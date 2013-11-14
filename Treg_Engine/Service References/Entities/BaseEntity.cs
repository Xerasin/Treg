using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;

using Treg_Engine.Graphics;
namespace Treg_Engine.Entities
{
    public class BaseEntity
    {
        public Vector3 Position;
        public Mesh mesh;
        public virtual void OnUpdate()
        {

        }
        public virtual void OnRender()
        {
            if (mesh != null)
            {
                mesh.Render();
            }
        }
    }
}
