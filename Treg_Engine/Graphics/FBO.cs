using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
namespace Treg_Engine.Graphics
{
    public class FBO
    {
        public uint FboHandle;
        public uint ColorTexture;
        public uint DepthRenderbuffer;
        public int Width;
        public int Height;
        public Texture Texture
        {
            get
            {
                Texture mat = new Texture();
                mat.textureID = (int)this.ColorTexture;
                return mat;
            }
        }
        public FBO(int width, int height)
        {
            // Create Color Texture
            GL.GenTextures(1, out ColorTexture);
            GL.BindTexture(TextureTarget.Texture2D, ColorTexture);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Clamp);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Clamp);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba8, width, height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, IntPtr.Zero);


            GL.BindTexture(TextureTarget.Texture2D, 0); // prevent feedback, reading and writing to the same image is a bad idea

            // Create Depth Renderbuffer
            GL.Ext.GenRenderbuffers(1, out DepthRenderbuffer);
            GL.Ext.BindRenderbuffer(RenderbufferTarget.RenderbufferExt, DepthRenderbuffer);
            GL.Ext.RenderbufferStorage(RenderbufferTarget.RenderbufferExt, (RenderbufferStorage)All.DepthComponent32, width, height);

            // test for GL Error here (might be unsupported format)

            // Create a FBO and attach the textures
            GL.Ext.GenFramebuffers(1, out FboHandle);
            GL.Ext.BindFramebuffer(FramebufferTarget.FramebufferExt, FboHandle);
            GL.Ext.FramebufferTexture2D(FramebufferTarget.FramebufferExt, FramebufferAttachment.ColorAttachment0Ext, TextureTarget.Texture2D, ColorTexture, 0);
            GL.Ext.FramebufferRenderbuffer(FramebufferTarget.FramebufferExt, FramebufferAttachment.DepthAttachmentExt, RenderbufferTarget.RenderbufferExt, DepthRenderbuffer);
            GL.DrawBuffer((DrawBufferMode)FramebufferAttachment.ColorAttachment0Ext);
            this.Width = width;
            this.Height = height;

        }
        public void Bind()
        {
            GL.PushAttrib(AttribMask.ViewportBit);
            GL.BindFramebuffer(FramebufferTarget.FramebufferExt, FboHandle);
            GL.Viewport(0, 0, Width, Height);
        }
        public void Trash()
        {
            GL.Ext.DeleteRenderbuffers(1, ref this.DepthRenderbuffer);
            GL.DeleteTexture(this.ColorTexture);
            GL.DeleteFramebuffer(this.FboHandle);
            
        }
        public void UnBind()
        {
            GL.BindFramebuffer(FramebufferTarget.FramebufferExt, 0);
            GL.PopAttrib();
        }
    }
}
