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
        public static Material debugWhite;
        public Shader shader = Shader.DefaultShader;
        public Texture texture = new Texture();
        public void Bind()
        {
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, this.texture.textureID);
            GL.UseProgram(this.shader.programID);
            this.shader.SetUniformInt("ngl_texture0", 0);
        }
    }
}
