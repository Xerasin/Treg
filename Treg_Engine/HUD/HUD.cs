using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Treg_Engine.HUD.Elements;
using Treg_Engine.Graphics;
using OpenTK;
using OpenTK.Graphics.OpenGL;
namespace Treg_Engine.HUD
{
    public static class HUD
    {
        private static List<Panel> _panelList = new List<Panel>();
        public static void Init()
        {
            Mesh.LoadFromFile("resources/models/flat.obj");
            Panel panel = HUD.Create<Panel>();
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
            for (int I = 0; I < _panelList.Count; I++)
            {
                _panelList[I].OnRender();
            }
            GL.Enable(EnableCap.CullFace);
        }
        public static T Create<T>() where T : Panel, new() // Thanks Based Foohy
        {
            T panel = new T();

            _panelList.Add(panel);
            return panel;
        }
    }
}
