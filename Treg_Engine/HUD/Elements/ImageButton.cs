using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Treg_Engine.Graphics;
using OpenTK;
using OpenTK.Graphics.OpenGL;
namespace Treg_Engine.HUD.Elements
{
    public class ImageButton : Button
    {
        public Material mat { get; set; }
        public ImageButton()
            : base()
        {
    
        }
        public override void OnRender()
        {
            //base.OnRender();
            Surface.DrawBox(this.GetRealPos(), this.Size, mat);
        }
    }
}
