using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
namespace Treg_Engine
{
    public static class Util
    {
        public static float Time { get; set;}
        public static Vector2 ScreenSize { get; set; }
        public static void Think(FrameEventArgs e)
        {
            Time += (float)e.Time;
        }
    }
}
