using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Treg_Engine;
using Rainbow_Cube.Entities;
namespace Rainbow_Cube
{
    class Program
    {
        static void Main(string[] args)
        {
            RainbowWorld world = new RainbowWorld();
            using (Window window = new Window(world, new WindowSettings(), "Rainbow Teapot Simulator " + (DateTime.Now.Year + 1)))
            {

                window.Run();
            }
        }
    }
}
