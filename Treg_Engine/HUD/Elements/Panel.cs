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
        
        public Material Material { get; set; }
        public Panel Parent { get; set; }
        public Panel TopParent { get; set; }
        public List<Panel> Children = new List<Panel>();
        public bool IsVisible { get; set; }
        public bool Enabled { get; set; }

        public Vector2 Position;
        public Vector2 Size;

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
                if (p.ContainsPoint(point))
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
        public virtual void Resize(float oldWidth, float oldHeight, float newWidth, float newHeight)
        {

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
            HUD.RenderRectangle(this.GetRealPos(), this.Size, this.Material);
            for (int I = 0; I < this.Children.Count; I++)
            {
                this.Children[I].OnRender();
            }
        }
    }
}
