using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using Treg_Engine.Graphics;
namespace Treg_Engine.HUD.Elements
{
    class Frame : Panel
    {
        private enum ResizeMode
        {
            None,
            Top,
            Left,
            Right,
            Bottom,
            TopLeft,
            TopRight,
            BottomLeft,
            BottomRight
        }
        private Panel background;
        private Panel titleBar;
        private Vector2 Offset;
        private bool dragging;
        private ResizeMode CurrentResizeMode;
        public Frame() : base()
        {
            titleBar = HUD.Create<Panel>();
            titleBar.Position = Vector2.Zero;
            titleBar.Size = new Vector2(this.Size.X, 25f);
            titleBar.SetParent(this);
            titleBar.Material = Resource.LoadMaterial("frame");
            titleBar.OnMouseDown += titleBar_OnMouseDown;
            titleBar.OnMouseUp += titleBar_OnMouseUp;
            titleBar.OnMouseMove += titleBar_OnMouseMove;
        }
        public override void MouseDown(OpenTK.Input.MouseButtonEventArgs e)
        {
            base.MouseDown(e);
            if (this.touchingBottom())
            {
                CurrentResizeMode = ResizeMode.Bottom;
            }
            if (this.touchingLeft())
            {
                CurrentResizeMode = ResizeMode.Left;
            }
            if (this.touchingRight())
            {
                CurrentResizeMode = ResizeMode.Right;
            }

            if (this.touchingBottom() && this.touchingLeft())
            {
                CurrentResizeMode = ResizeMode.BottomLeft;
            }
            if (this.touchingBottom() && this.touchingRight())
            {
                CurrentResizeMode = ResizeMode.BottomRight;
            }
            Offset = new Vector2(e.X, e.Y) - this.Position;
        }
        public override void Resize(float oldWidth, float oldHeight, float newWidth, float newHeight)
        {
            base.Resize(oldWidth, oldHeight, newWidth, newHeight);
            titleBar.SetWidth(newWidth);
        }
        public override void MouseUp(OpenTK.Input.MouseButtonEventArgs e)
        {
            base.MouseUp(e);
            CurrentResizeMode = ResizeMode.None;
            Offset = Vector2.Zero;
        }
        public override void MouseMove(OpenTK.Input.MouseMoveEventArgs e)
        {
            base.MouseMove(e);
            if (this.touchingBottom())
            {
                if (this.touchingRight())
                {
                    System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.SizeNWSE;
                }
                else if (this.touchingLeft())
                {
                    System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.SizeNESW;
                }
                else
                {
                    System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.SizeNS;
                }
            }
            else
            {
                if (this.touchingLeft() || this.touchingRight())
                {
                    System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.SizeWE;
                }
            }
            
            if(CurrentResizeMode != ResizeMode.None)
            {
                
                this.ResizeThink();
            }
        }
        private void ResizeThink()
        {
            if (CurrentResizeMode == ResizeMode.None) return;
            Vector2 pos = this.GetRealPos();
            Vector2 MousePos = new Vector2(Window.Instance.Mouse.X, Window.Instance.Mouse.Y);
            if (CurrentResizeMode == ResizeMode.Bottom || CurrentResizeMode == ResizeMode.BottomLeft || CurrentResizeMode == ResizeMode.BottomRight)
            {
                this.SetHeight(MousePos.Y - pos.Y);
            }
            if (CurrentResizeMode == ResizeMode.Left || CurrentResizeMode == ResizeMode.BottomLeft)
            {
                float newWidth = (this.Position.X + this.Size.X) - (MousePos.X - Offset.X);
                this.SetWidth(newWidth);
                this.Position.X = MousePos.X - Offset.X;
            }
            if (CurrentResizeMode == ResizeMode.Right || CurrentResizeMode == ResizeMode.BottomRight)
            {
                this.SetWidth(MousePos.X - pos.X);
            }
        }
        void titleBar_OnMouseMove(Panel arg1, OpenTK.Input.MouseMoveEventArgs arg2)
        {
            if (dragging)
            {
                this.Position = new Vector2(arg2.X, arg2.Y) - Offset;
            }
        }

        void titleBar_OnMouseUp(Panel arg1, OpenTK.Input.MouseButtonEventArgs arg2)
        {
            dragging = false;
            Offset = Vector2.Zero;
        }

        void titleBar_OnMouseDown(Panel arg1, OpenTK.Input.MouseButtonEventArgs arg2)
        {
            dragging = true;
            Offset = new Vector2(arg2.X, arg2.Y) - this.Position;
        }

        const int buffer = 10;
        public bool touchingLeft()
        {
            if (IsMouseOver())
            {
                Vector2 pos = this.GetRealPos();
                if ((Window.Instance.Mouse.X - pos.X) < buffer)
                {
                    return true;
                }
            }
            return false;
        }
        public bool touchingTop()
        {
            if (IsMouseOver())
            {
                Vector2 pos = this.GetRealPos();
                if ((Window.Instance.Mouse.Y - pos.Y) < buffer)
                {
                    return true;
                }
            }
            return false;
        }
        public bool touchingBottom()
        {
            if (IsMouseOver())
            {
                Vector2 pos = this.GetRealPos();
                if (((pos.Y + Size.Y) - Window.Instance.Mouse.Y) < buffer)
                {
                    return true;
                }
            }
            return false;
        }
        public bool touchingRight()
        {
            if (IsMouseOver())
            {
                Vector2 pos = this.GetRealPos();
                if (((pos.X + Size.X) - Window.Instance.Mouse.X) < buffer)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
