using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;

using Treg_Engine.Graphics;
namespace Treg_Engine.Entities
{
    public class BaseEntity
    {
        public Vector3 Position = Vector3.Zero;
        public Vector3 Scale = Vector3.One;
        public Vector4 Color = Vector4.One;
        public Angle Angles = new Angle(0, 0, 0);
        public Mesh mesh;
        public Material material = Material.debugWhite;
        public int EntIndex;
        public Dictionary<string, object> data = new Dictionary<string, object>();

        public virtual void OnUpdate(double time)
        {

        }
        public virtual void OnRender()
        {
            if (mesh != null)
            {
                mesh.Render(this.material, Position, Angles, Scale, this.Color);
            }
        }
        public string GetClass()
        {
            object[] attrs = this.GetType().GetCustomAttributes(typeof(EntityNameAttribute), false);
            if (attrs.Length > 0)
            {
                return ((EntityNameAttribute)attrs[0]).name;
            }
            else
            {
                return "internal";
            }
        }
        public override string ToString()
        {
            string str = this.GetClass();
            return str + "[" + this.EntIndex + "]";
        }
    }
    public struct BaseEntityType
    {
        public Type type;
        public Dictionary<string, object> data;
        public BaseEntityType(Type type, Dictionary<string, object> data)
        {
            this.type = type;
            this.data = new Dictionary<string, object>();
            foreach (KeyValuePair<string, object> keyValue in data)
            {
                this.data.Add(keyValue.Key, keyValue.Value);
            }
        }
    }
    public class EntityNameAttribute : Attribute
    {
        public string name;
        public bool addToDic;
        public EntityNameAttribute(string name, bool addToDic)
        {
            this.name = name;
            this.addToDic = addToDic;
        }
    }
}
