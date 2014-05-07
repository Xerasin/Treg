using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Treg_Engine.Graphics;
using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL;
namespace Treg_Engine.HUD.Elements
{
    public class Panel
    {
        public enum DockType
        {
            NODOCK,
            FILL,
            LEFT,
            RIGHT,
            TOP,
            BOTTOM
        }
        [Flags]
        public enum Anchor
        {
            None = 0x0,
            Bottom = 0x1,
            Left = 0x2,
            Right = 0x4,
            Top = 0x8
        }

        public Material Material { get; set; }
        public Panel Parent { get; set; }
        public Panel TopParent { get; set; }
        public List<Panel> Children = new List<Panel>();
        public bool IsVisible { get; set; }
        public bool Enabled { get; set; }
        public bool PassThrough { get; set; }
        public Vector2 Position;
        public Vector2 Size;
        public Anchor AnchorStyle { get; set; }
        public DockType DockStyle { get; set; }
        public float padLeft { get; set; }
        public float padRight { get; set; }
        public float padUp { get; set; }
        public float padDown { get; set; }
        public bool dockPosition { get; set; }
        public event Action<Panel, MouseButtonEventArgs> OnMouseDown;
        public event Action<Panel, MouseButtonEventArgs> OnMouseUp;
        public event Action<Panel, MouseMoveEventArgs> OnMouseMove;

        public Panel()
        {
            Size = new Vector2(200f, 200f);
            Material = Resource.LoadMaterial("border");
            this.Position = new Vector2(10f, 10f);
            this.IsVisible = true;
            this.Enabled = true;
            this.dockPosition = true;
        }

        public virtual void OnUpdate(double time)
        {

        }

        public static implicit operator bool(Panel p)
        {
            return p != null;
        }

        public Vector2 GetRealPos()
        {
            Panel cur = this;
            Vector2 pos = Vector2.Zero;

            while (cur != null)
            {
                pos += cur.Position;
                cur = cur.Parent;
            }
            return pos;
        }
        public Panel GetHighestChildAtPoint(Vector2 point)
        {
            for (int I = Children.Count - 1; I >= 0; I--)
            {
                Panel p = Children[I];
                if (p.ContainsPoint(point) && !p.PassThrough)
                {
                    return p;
                }
            }
            return null;
        }
        public virtual void MouseDown(OpenTK.Input.MouseButtonEventArgs e)
        {
            if (!this.Enabled) return;

            Panel highestChild = this.GetHighestChildAtPoint(new Vector2(Window.Instance.Mouse.X, Window.Instance.Mouse.Y));
            if (highestChild)
            {
                highestChild.MouseDown(e);
            }

            if (this.OnMouseDown != null)
            {
                this.OnMouseDown(this, e);
            }
        }
        public virtual void MouseUp(OpenTK.Input.MouseButtonEventArgs e)
        {
            if (!this.Enabled) return;

            Panel highestChild = this.GetHighestChildAtPoint(new Vector2(Window.Instance.Mouse.X, Window.Instance.Mouse.Y));
            if (highestChild)
            {
                highestChild.MouseUp(e);
            }

            if (this.OnMouseUp != null)
            {
                this.OnMouseUp(this, e);
            }
        }
        public virtual void MouseMove(OpenTK.Input.MouseMoveEventArgs e)
        {
            if (!this.Enabled) return;

            foreach (Panel child in Children)
            {
                child.MouseMove(e);
            }

            if (OnMouseMove != null)
            {
                OnMouseMove(this, e);
            }
        }
        public void SetWidth(float width)
        {
            float old = this.Size.X;
            this.Size.X = width;
            Resize(old, this.Size.Y, this.Size.X, this.Size.Y);
        }
        public void SetHeight(float height)
        {
            float old = this.Size.Y;
            this.Size.Y = height;
            Resize(this.Size.X, old, this.Size.X, this.Size.Y);
        }
        public virtual void ParentResized(float oldWidth, float oldHeight, float newWidth, float newHeight)
        {
            if (!this.Parent) return;
            float oldWidth2 = this.Size.X;
            float oldHeight2 = this.Size.Y;
            HandleAnchor(newWidth - oldWidth, newHeight - oldHeight);
            HandleDocking();

            this.Resize(oldWidth2, oldHeight2, this.Size.X, this.Size.Y);

        }
        public virtual void Resize(float oldWidth, float oldHeight, float newWidth, float newHeight)
        {
            foreach (Panel child in Children)
            {
                child.ParentResized(oldWidth, oldHeight, newWidth, newHeight);
            }
        }
        public void Dock(DockType dock)
        {
            this.DockStyle = dock;
            this.HandleDocking();
        }
        public void DockPadding(float left = 0, float right = 0, float top = 0, float bottom = 0)
        {
            this.padLeft = left;
            this.padRight = right;
            this.padUp = top;
            this.padDown = bottom;
        }
        public virtual void HandleAnchor(float deltaX, float deltaY)
        {
            if (this.AnchorStyle == Anchor.None) return;
            if ((this.AnchorStyle & Anchor.Bottom) != 0)
            {
                if ((this.AnchorStyle & Anchor.Top) != 0)
                {
                    this.Size.Y += deltaY;
                    this.Position.Y += deltaY / 2.0f;
                }
                else
                {
                    this.Position.Y += deltaY;
                }
            }
            if ((this.AnchorStyle & Anchor.Right) != 0)
            {
                if ((this.AnchorStyle & Anchor.Left) != 0)
                {
                    this.Size.X += deltaX;
                    this.Position.X += deltaX / 2.0f;
                }
                else
                {
                    this.Position.X += deltaX;
                }
            }
        }
        public void HandleDocking()
        {
            if (this.DockStyle == DockType.NODOCK) return;
            if (!this.Parent) return;
            if (this.DockStyle == DockType.FILL)
            {
                this.Size.X = this.Parent.Size.X - (padLeft + padRight);
                this.Size.Y = this.Parent.Size.Y - (padUp + padDown);
                if (this.dockPosition)
                {
                    this.Position = new Vector2(padLeft, padUp);
                }
            }
            if (this.DockStyle == DockType.TOP)
            {
                if(this.dockPosition)
                {
                    this.Position = new Vector2(padLeft, padUp);
                }
                this.Size.X = this.Parent.Size.X - (padLeft + padRight);
            }
            if (this.DockStyle == DockType.BOTTOM)
            {
                if (this.dockPosition)
                {
                    this.Position = new Vector2(padLeft, this.Parent.Size.Y - (padDown + this.Size.Y));
                }
                this.Size.X = this.Parent.Size.X - (padLeft + padRight);
            }
            if (this.DockStyle == DockType.LEFT)
            {
                if (this.dockPosition)
                {
                    this.Position = new Vector2(padLeft, padUp);
                }
                this.Size.Y = this.Parent.Size.Y - (padUp + padDown);
            }
            if (this.DockStyle == DockType.RIGHT)
            {
                if (this.dockPosition)
                {
                    this.Position = new Vector2(this.Parent.Size.X - (padRight + this.Size.X), padUp);
                }
                this.Size.Y = this.Parent.Size.Y - (padUp + padDown);
            }
        }
        public bool IsMouseOver()
        {
            return this.ContainsPoint(new Vector2(Window.Instance.Mouse.X, Window.Instance.Mouse.Y));
        }

        public bool ContainsPoint(Vector2 point)
        {
            Vector2 pos = this.GetRealPos();
            if (point.X < pos.X) return false;
            if (point.Y < pos.Y) return false;
            if (point.X > pos.X + this.Size.X) return false;
            if (point.Y > pos.Y + this.Size.Y) return false;

            return true;
        }
        public void UpdateTopParent()
        {
            Panel cur = this;
            while (cur)
            {
                this.TopParent = cur;
                cur = cur.Parent;
            }
            foreach (Panel panel in Children) panel.SetTopParent(this.TopParent);
        }
        public void SetTopParent(Panel parent)
        {
            this.TopParent = parent;

            foreach (Panel panel in Children) panel.SetTopParent(parent);
        }
        public void SetParent(Panel parent)
        {
            if (parent)
            {
                parent.Children.Add(this);
            }
            if (this.Parent)
            {
                this.Parent.Children.Remove(this);
            }
            this.Parent = parent;
            this.UpdateTopParent();
        }
        public virtual void OnRender()
        {
            if (!this.IsVisible) return;
            Surface.DrawBox(this.GetRealPos(), this.Size, this.Material);
            DrawChildren();
        }
        public void DrawChildren()
        {
            for (int I = 0; I < this.Children.Count; I++)
            {
                this.Children[I].OnRender();
            }
        }
    }
}
