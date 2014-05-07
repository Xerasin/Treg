using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Treg_Engine.HUD.Elements
{
    public class Button : Panel
    {
        public event Action<Panel, OpenTK.Input.MouseButtonEventArgs> OnClick;
        public Button()
            : base()
        {
            
        }
        public override void MouseDown(OpenTK.Input.MouseButtonEventArgs e)
        {
            base.MouseDown(e);
            if (OnClick != null)
            {
                OnClick(this, e);
            }
        }
    }
}
