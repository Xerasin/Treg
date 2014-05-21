using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using Treg_Engine.Entities;
using Treg_Engine.Graphics;
namespace Rainbow_Cube.Entities
{
    class Mountain : BaseEntity
    {
        public Mountain()
            : base()
        {
            mesh = new Mesh();
            material = Treg_Engine.Resource.LoadMaterial("white");
            this.Scale = new Vector3(50f, 1f, 50f);
        }
        public void Setup(float[] values)
        {
            float stride = 0.05f;
            List<Vertex> vertexes = new List<Vertex>();
            List<uint> elements = new List<uint>();
            uint lv1 = 0, lv2 = 0;
            for (int index = 0; index < values.Length; index++)
            {
                float X = index * stride;
                float Y = values[index];

                Vertex vertex1 = new Vertex(new Vector3(X, Y, 0), Vector4.One, Vector3.Zero, new Vector2((X + 1) / 2, 0));
                Vertex vertex2 = new Vertex(new Vector3(X, 0, 0), Vector4.One, Vector3.Zero, new Vector2((X + 1) / 2, 1));
                vertexes.Add(vertex1);
                vertexes.Add(vertex2);
                uint v1 = (uint)vertexes.IndexOf(vertex1);
                uint v2 = (uint)vertexes.IndexOf(vertex2);
                if (X != -1)
                {
                    elements.Add(v1); elements.Add(lv1); elements.Add(v2);
                    elements.Add(lv1); elements.Add(lv2); elements.Add(v2);
                }
                lv1 = v1;
                lv2 = v2;
            }
            mesh.UploadData(vertexes.ToArray(), elements.ToArray());
        }
    }
}
