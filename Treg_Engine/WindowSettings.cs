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
    public class WindowSettings
    {
        public int WindowWidth = 600;
        public int WindowHeight = 600;
        public GraphicsMode GraphicsMode = GraphicsMode.Default;
        public GameWindowFlags WindowFlags = GameWindowFlags.Default;
        public DisplayDevice device = DisplayDevice.Default;
        public int minor = 0;
        public int major = 2;
        public GraphicsContextFlags ContextFlags = GraphicsContextFlags.Default;

        public WindowSettings(int WindowWidth = 600, int WindowHeight = 600,
            int minor = 0,
            int major = 2)
        {
            this.WindowWidth = WindowWidth;
            this.WindowHeight = WindowHeight;
            this.minor = minor;
            this.major = major;
        }
    }
}
