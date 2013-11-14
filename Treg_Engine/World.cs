using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Treg_Engine.Entities;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
namespace Treg_Engine
{
    public class World
    {
        public List<BaseEntity> Entities = new List<BaseEntity>();
        public World()
        {

        }
        public virtual void OnLoad()
        {

        }

        public virtual void OnRender()
        {
            
            Matrix4 matrix = Matrix4.LookAt(new Vector3(0f, 15f, 15f), new Vector3(0.1f, 0f, 0.1f), new Vector3(0f, 1f, 0f));
            GL.BindTexture(TextureTarget.Texture2D, 0);
            GL.Enable(EnableCap.DepthTest);
            //GL.DepthFunc(DepthFunction.Greater);
            GL.Enable(EnableCap.Blend);
            //GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            //GL.LoadMatrix(ref matrix);
            View.ViewMatrix = matrix;
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            foreach (BaseEntity entity in Entities)
            {
                entity.OnRender();
            }
        }

        public virtual void OnUpdate(double time)
        {
            foreach (BaseEntity entity in Entities)
            {
                entity.OnUpdate(time);
            }
        }
        public T Create<T>() where T:BaseEntity, new() // Thanks Based Foohy
        {
            T entity = new T();

            Entities.Add(entity);

            return entity;
        }
    }
}
