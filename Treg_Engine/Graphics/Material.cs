using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
namespace Treg_Engine.Graphics
{
    public class Texture
    {
        public int textureID = 0;
        public Texture()
        {
        }

        public Texture(string file)
        {
            Bitmap bitmap = new Bitmap(Bitmap.FromFile(file));
            System.Drawing.Imaging.BitmapData data = bitmap.LockBits(
                new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                System.Drawing.Imaging.ImageLockMode.ReadOnly,
                 System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            int tex = GL.GenTexture();
              GL.BindTexture(TextureTarget.Texture2D, tex);
            GL.BindTexture(TextureTarget.Texture2D, tex);
            GL.TexImage2D(
              TextureTarget.Texture2D,
              0,
              PixelInternalFormat.Rgba,
              data.Width, data.Height,
              0,
              PixelFormat.Bgra,
              PixelType.UnsignedByte,
              data.Scan0);
            bitmap.UnlockBits(data);
            GL.TexParameter(
                TextureTarget.Texture2D,
                TextureParameterName.TextureMinFilter,
                (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D,
                TextureParameterName.TextureMagFilter,
                (int)TextureMagFilter.Linear);
            this.textureID = tex;   
        }
    }
    public class Material
    {
        public static Material debugWhite;
        public Shader shader = Shader.DefaultShader;
        public Texture texture = new Texture();
        public Texture texture2 = new Texture();
        public Texture texture3 = new Texture();
        public Dictionary<string, object> shaderVars = new Dictionary<string, object>();
        public Material()
        {

        }
        public Material(string file)
        {
            texture = new Texture(file);
        }
        
        public static void Init()
        {
            debugWhite = Resource.LoadMaterial("debugwhite");
        }
       
        public void Bind()
        {
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, this.texture.textureID);
            GL.ActiveTexture(TextureUnit.Texture1);
            GL.BindTexture(TextureTarget.Texture2D, this.texture2.textureID);
            GL.ActiveTexture(TextureUnit.Texture2);
            GL.BindTexture(TextureTarget.Texture2D, this.texture3.textureID);
            GL.ActiveTexture(TextureUnit.Texture1);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureCompareMode, (int)TextureCompareMode.None);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            this.shader.Bind();
            this.shader.SetUniformInt("ngl_texture0", 0);
            this.shader.SetUniformInt("ngl_texture1", 1);
            this.shader.SetUniformInt("ngl_texture2", 2);
        }
        public void UnBind()
        {
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, 0);
            GL.ActiveTexture(TextureUnit.Texture1);
            GL.BindTexture(TextureTarget.Texture2D, 0);
            GL.ActiveTexture(TextureUnit.Texture2);
            GL.BindTexture(TextureTarget.Texture2D, 0);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.UseProgram(0);
            
            //this.shader.SetUniformInt("ngl_texture0", 0);
        }
    }
}
