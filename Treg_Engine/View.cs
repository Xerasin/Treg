using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
namespace Treg_Engine
{
    public static class View
    {
        private static Matrix4 projection;
        public static Matrix4 ProjectionMatrix
        {
            get
            {
                return projection; //test
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
    }
}
