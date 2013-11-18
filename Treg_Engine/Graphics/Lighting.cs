using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Treg_Engine.Graphics
{
    public static class Lighting
    {
        public static Shader shader;
        public static void Init()
        {
            shader = Resource.LoadShader("basicrender");
        }
        public static void SetupLighting()
        {

        }
    }
}
