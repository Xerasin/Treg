using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using Treg_Engine.Entities;
namespace Treg_Engine
{
    public static class View
    {
        private static Matrix4 projection;
        public static Matrix4 ProjectionMatrix
        {
            get
            {
                return projection;
            }
            set
            {
                projection = value;
            }
        }
        private static Matrix4 view;
        public static Matrix4 ViewMatrix
        {
            get
            {
                return view;
            }
            set
            {
                view = value;
            }
        }
        public static Vector3 EyePos
        {
            get
            {
                if (Camera != null)
                {
                    return Camera.Position;
                }
                return Vector3.Zero;
            }
        }
        public static Angle EyeAngles
        {
            get
            {
                if (Camera != null)
                {
                    return Camera.Angles;
                }
                return new Angle(0, 0, 0);
            }
        }
        public static BaseEntity Camera { get; set; }
        private static Matrix4 ortho;
        public static Matrix4 OrthoMatrix
        {
            get
            {
                return ortho;
            }
            set
            {
                ortho = value;
            }

        }
    }
}
