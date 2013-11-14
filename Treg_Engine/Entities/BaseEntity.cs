﻿using System;
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
        public Vector3 Position = Vector3.Zero;
        public Vector3 Scale = Vector3.One;
        public Angle Angles = Angle.Zero;
        public Mesh mesh;
        public Material material = Material.debugWhite;
        public virtual void OnUpdate(double time)
        {

        }
        public virtual void OnRender()
        {
            if (mesh != null)
            {
                mesh.Render(this.material, Position, Angles, Scale);
            }
        }
    }
}
