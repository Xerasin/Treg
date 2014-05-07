using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Treg_Engine.HUD.Elements
{
    class Label : Panel
    {
        private TFont font;
        public string Text { get; set; }
        public QuickFont.QFontAlignment Alignment { get; set; }
        public Label()
            : base()
        {
            font = Surface.CreateFont("labelFont", "anonymous.ttf", 8, false);
            Text = "No can do";
            Alignment = QuickFont.QFontAlignment.Left;
            PassThrough = true;
        }
        public override void OnRender()
        {
            if (!this.IsVisible) return;
            font.RenderText(this.Text, this.GetRealPos(), this.Alignment, this.Size.X);
            DrawChildren();
        }
    }
}
