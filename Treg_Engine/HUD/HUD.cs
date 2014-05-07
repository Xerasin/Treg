using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Treg_Engine.HUD.Elements;
using Treg_Engine.Graphics;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using QuickFont;
namespace Treg_Engine.HUD
{
    public static class GUI
    {
        private static List<Panel> _panelList = new List<Panel>();
        
        public static void Init()
        {
            
            
            Window.Instance.Mouse.ButtonDown += Mouse_ButtonDown;
            Window.Instance.Mouse.ButtonUp += Mouse_ButtonUp;
            Window.Instance.Mouse.Move += Mouse_Move;
            Surface.Init();
            Panel panel = GUI.Create<Frame>();
        }

        static void Mouse_Move(object sender, OpenTK.Input.MouseMoveEventArgs e)
        {
            foreach (Panel p in _panelList)
            {
                p.MouseMove(e);
            }
        }

        static void Mouse_ButtonUp(object sender, OpenTK.Input.MouseButtonEventArgs e)
        {
            for (int I = _panelList.Count - 1; I >= 0; I--)
            {
                _panelList[I].MouseUp(e);
            }
        }

        static void Mouse_ButtonDown(object sender, OpenTK.Input.MouseButtonEventArgs e)
        {
            for (int I = _panelList.Count - 1; I >= 0; I--)
            {
                Panel panel = _panelList[I];
                if (panel.IsVisible && panel.IsMouseOver())
                {
                    if (!panel.Parent)
                    {
                        panel.MouseDown(e);
                    }
                    else if (panel.TopParent.Enabled && panel.TopParent.IsVisible)
                    {
                        panel.TopParent.MouseDown(e);
                    }
                    Panel p = panel.TopParent ? panel.TopParent : panel;
                    if(p.IsVisible)
                    {
                        break;
                    }
                }
            }
        }
        public static void Update(double time)
        {
            for (int I = 0; I < _panelList.Count; I++)
            {
                _panelList[I].OnUpdate(time);
            }
        }
        public static void Render()
        {
            GL.Disable(EnableCap.CullFace);
            GL.DepthFunc(DepthFunction.Always);
            for (int I = 0; I < _panelList.Count; I++)
            {
                if (!_panelList[I].Parent)
                {
                    _panelList[I].OnRender();
                }
            }
            GL.Enable(EnableCap.CullFace);
            GL.DepthFunc(DepthFunction.Less);
        }
        public static T Create<T>() where T : Panel, new() // Thanks Based Foohy
        {
            T panel = new T();

            _panelList.Add(panel);
            return panel;
        }
    }
}
