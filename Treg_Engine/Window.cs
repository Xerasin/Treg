using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Audio;
using OpenTK.Audio.OpenAL;
using OpenTK.Input;
using OpenTK.Platform.Windows;
namespace Treg_Engine
{
    public class Window : GameWindow
    {
        public static Window Instance;
        public World CurrentWorld;
        public Window(World world, WindowSettings settings, string WindowTitle)
            : base(settings.WindowWidth, settings.WindowHeight, settings.GraphicsMode, WindowTitle,
            settings.WindowFlags, settings.device, settings.minor, settings.major,
            settings.ContextFlags)
        {
            this.CurrentWorld = world;
        }
        public static World GetWorld()
        {
            return Window.Instance.CurrentWorld;
        }
        protected override void OnLoad(EventArgs e)
        {
            Treg_Engine.Graphics.Shader.Init();
            Treg_Engine.Graphics.Material.Init();
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Texture2D);
            //GL.Enable(EnableCap.Blend);
            //GL.Enable(EnableCap.ColorMaterial);
            GL.Enable(EnableCap.Multisample);
            GL.Enable(EnableCap.CullFace);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.PixelStore(PixelStoreParameter.UnpackAlignment, 1); 
            GL.CullFace(CullFaceMode.Back);
            Window.Instance = this;
            this.CurrentWorld.OnLoad();

        }
        protected override void OnUnload(EventArgs e)
        {
            base.OnUnload(e);
        }
        protected override void OnResize(EventArgs e)
        {
            if (this.ClientSize.Height == 0)
                this.ClientSize = new System.Drawing.Size(this.ClientSize.Width, 1);
            GL.Viewport(0, 0, this.ClientSize.Width, this.ClientSize.Height);
            float aspect_ratio = Width / (float)Height;
            Matrix4 perspective = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver3, aspect_ratio, 0.1f, 1000f);
            //GL.MatrixMode(MatrixMode.Projection);
            //GL.LoadMatrix(ref perspective);
            View.ProjectionMatrix = perspective;

        }
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            CurrentWorld.OnUpdate(e.Time);
            Util.Think(e);
        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            CurrentWorld.OnRender();
            this.SwapBuffers();
        }
    }
}
