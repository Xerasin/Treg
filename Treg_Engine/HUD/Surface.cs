using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using Treg_Engine;
using Treg_Engine.Graphics;
using QuickFont;
namespace Treg_Engine.HUD
{
    public class TFont
    {
        public QFont Font { get; set; }
        public void RenderText(string text, 
            Vector2 vec, 
            OpenTK.Graphics.Color4 color,
             QFontAlignment align = QFontAlignment.Left,
            float maxWidth = 10000)
        {
            Surface.SetupForFontRendering();
            Font.Options.Colour = color;
            Font.Print(text, maxWidth, align, vec);
        }
        public void RenderText(string text, 
            Vector2 vec, 
             QFontAlignment align = QFontAlignment.Left,
            float maxWidth = 10000)
        {
            RenderText(text, vec, Color4.Black, align, maxWidth);
        }
    }
    public static class Surface
    {
        private static Mesh panelMesh;
        public static void Init()
        {
            panelMesh = Mesh.LoadFromFile("resources/models/flat.obj");
        }


        private static Dictionary<string, TFont> fonts = new Dictionary<string, TFont>();
        public static TFont GetFont(string name)
        {
            if (fonts.ContainsKey(name))
            {
                return fonts[name];
            }
            return null;
        }
        public static void SetFont(string name, TFont font)
        {
            fonts[name] = font;
        }
        public static TFont CreateFont(string name, string fontPath, float size, bool dropShadow)
        {
            QFontBuilderConfiguration config = new QFontBuilderConfiguration(dropShadow);
            //config.TransformToCurrentOrthogProjection = true;
            QFont font = new QFont("resources/fonts/" + fontPath, size, config);
            TFont tfont = new TFont();
            tfont.Font = font;
            fonts[name] = tfont;
            return tfont;
        }
        public static void SetupForFontRendering()
        {
            Matrix4 projectionMatrix = Matrix4.CreateOrthographicOffCenter(0, Util.ScreenSize.X, Util.ScreenSize.Y, 0, 0f, 1000f);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projectionMatrix);

        }
        public static void DrawBox(Vector2 pos, Vector2 size, Material mat)
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
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            mat.shader.SetUniformFloat("Width", size.X);
            mat.shader.SetUniformFloat("Height", size.Y);
            panelMesh.Render(mat, ModelMatrix, identity, projectionMatrix);
        }
    }
}
