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
    public class Panel
    {
        private Mesh panelMesh;
        public Vector2 Position { get; set; }
        public Material Material { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public Panel()
        {
            Width = 100;
            Height = 100;
            panelMesh = Mesh.LoadFromFile("resources/models/flat.obj");
            Material = Resource.LoadMaterial("border");
            this.Position = new Vector2(10f, 10f);
        }
        public virtual void OnUpdate(double time)
        {

        }
        public virtual void OnRender()
        {
            Matrix4 projectionMatrix = Matrix4.CreateOrthographicOffCenter(0, Util.ScreenSize.X, Util.ScreenSize.Y, 0, 0f, 1000f);
            Matrix4 identity = Matrix4.Identity;
            Matrix4 ModelMatrix = Matrix4.Identity;
            ModelMatrix *= Matrix4.CreateScale(this.Width, this.Height, 2.0f);
            ModelMatrix *= Matrix4.CreateTranslation(new Vector3(this.Position));
            this.Material.Bind();
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureCompareMode, (int)TextureCompareMode.None);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            this.Material.shader.SetUniformFloat("Width", this.Width);
            this.Material.shader.SetUniformFloat("Height", this.Height);
            panelMesh.Render(this.Material, ModelMatrix, identity, projectionMatrix);
        }
    }
}
