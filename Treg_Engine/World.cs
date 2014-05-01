using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Treg_Engine;
using Treg_Engine.Graphics;
using Treg_Engine.Entities;
using Treg_Engine.Scripting;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
namespace Treg_Engine
{
    public class World
    {
        public List<BaseEntity> Entities = new List<BaseEntity>();
        public Dictionary<string, BaseEntityType> typeDic = new Dictionary<string, BaseEntityType>();
        public World()
        {
            MainLua.PreLoad();
        }
        public virtual void OnLoad()
        {
            Treg_Engine.Graphics.Lighting.Init();
            RegisterEntities();
            MainLua.LoadAll();
            
        }

        
        public virtual void OnRender()
        {
            GL.ClearColor(Color4.Black);
            GL.Enable(EnableCap.CullFace);
            View.EyePos = new Vector3(0f, 4f, 25f);
            Matrix4 matrix = Matrix4.LookAt(View.EyePos, new Vector3(0f, 4f, -10f), new Vector3(0f, 1f, 0f));
            //matrix *= Matrix4.CreateRotationY(Util.Time)
            GL.BindTexture(TextureTarget.Texture2D, 0);
            GL.Enable(EnableCap.DepthTest);
            //GL.DepthFunc(DepthFunction.Greater);
            GL.Enable(EnableCap.Blend);
            //GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            //GL.LoadMatrix(ref matrix);
            View.ViewMatrix = matrix;
            Graphics.Lighting.SetupLighting();
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            foreach (BaseEntity entity in Entities)
            {
                
                entity.OnRender();
                
            }
            /*GL.Disable(EnableCap.CullFace);
            Material material = Resource.LoadMaterial("gradient2");
            Mesh mesh = Mesh.LoadFromFile("resources/models/flat.obj");
            Matrix4 projectionMatrix = Matrix4.CreateOrthographicOffCenter(0, 600, 600, 0, 0f, 1000f);
            Matrix4 identity = Matrix4.Identity;
            Matrix4 ModelMatrix = Matrix4.CreateTranslation(Vector3.Zero);
            ModelMatrix *= Matrix4.CreateScale(600, 600, 2.0f);
            mesh.Render(material, ModelMatrix, identity, projectionMatrix);*/
            
        }

        public virtual void OnUpdate(double time)
        {
            Graphics.Lighting.Think();
            foreach (BaseEntity entity in Entities)
            {
                entity.OnUpdate(time);
            }
            Treg_Engine.Scripting.LuaHook.Call("Think");
        }
        public void RegisterEntities()
        {
            Assembly assembly = Assembly.GetCallingAssembly();
            foreach (Type type in assembly.GetTypes())
            {
                Attribute attr = type.GetCustomAttribute(typeof(EntityNameAttribute));
                if (attr != null)
                {
                    EntityNameAttribute eNameAttr = (EntityNameAttribute)attr;
                    if (eNameAttr.addToDic)
                    {
                        BaseEntityType eType = new BaseEntityType(type, new Dictionary<string, object>());
                        typeDic.Add(eNameAttr.name, eType);
                    }
                }
            }
        }
        public void RegisterType(string name, BaseEntity entity)
        {
            BaseEntityType type = new BaseEntityType(entity.GetType(), entity.data);
            typeDic.Add(name, type);
        }
        public BaseEntity Create(string name)
        {
            if (typeDic.ContainsKey(name))
            {
                BaseEntityType type = typeDic[name];
                object obj = Activator.CreateInstance(type.type);
                BaseEntity entity = (BaseEntity)obj;
                foreach (KeyValuePair<string, object> keyValue in type.data)
                {
                    entity.data.Add(keyValue.Key, keyValue.Value);
                }
                Entities.Add(entity);
                entity.EntIndex = Entities.IndexOf(entity);
                return entity;
            }
            return null;
        }
        public T Create<T>() where T:BaseEntity, new() // Thanks Based Foohy
        {
            T entity = new T();

            Entities.Add(entity);
            entity.EntIndex = Entities.IndexOf(entity);
            return entity;
        }
    }
}
