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
        private static Mesh panelMesh;
        public static void Init()
        {
            panelMesh = Mesh.LoadFromFile("resources/models/flat.obj");
            Panel panel = HUD.Create<Frame>();
            Window.Instance.Mouse.ButtonDown += Mouse_ButtonDown;
            Window.Instance.Mouse.ButtonUp += Mouse_ButtonUp;
            Window.Instance.Mouse.Move += Mouse_Move;
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
                    break;
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
        public static void RenderRectangle(Vector2 pos, Vector2 size, Material mat)
        {
            Matrix4 projectionMatrix = Matrix4.CreateOrthographicOffCenter(0, Util.ScreenSize.X, Util.ScreenSize.Y, 0, 0f, 1000f);
            Matrix4 identity = Matrix4.Identity;
            Matrix4 ModelMatrix = Matrix4.Identity;
            ModelMatrix *= Matrix4.CreateScale(size.X, size.Y, 2.0f);
            ModelMatrix *= Matrix4.CreateTranslation(new Vector3(pos));
            mat.Bind();
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureCompareMode, (int)TextureCompareMode.None);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            mat.shader.SetUniformFloat("Width", size.X);
            mat.shader.SetUniformFloat("Height", size.Y);
            panelMesh.Render(mat, ModelMatrix, identity, projectionMatrix);
        }
    }
}
