using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Treg_Engine;
using Treg_Engine.Entities;
using Treg_Engine.Graphics;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
namespace Rainbow_Cube.Entities
{
    [EntityNameAttribute("ent_wave", true)]
    class Wave : BaseEntity
    {
        public Wave()
        {
            mesh = Mesh.LoadFromFile("Resources/models/cube.obj");
            material = Treg_Engine.Resource.LoadMaterial("white");
            this.Scale = new Vector3(50f, 1f, 50f);
        }
        private float lastUpdate = 0f;
        public void UpdateMesh(Treg_Engine.Audio.Audio audio)
        {
            if (Util.Time - lastUpdate < 1 / 60f) return;
            float[] fft = audio.GetFFT(Un4seen.Bass.BASSData.BASS_DATA_FFT256);
            float stride = 0.05f;
            List<Vertex> vertexes = new List<Vertex>();
            List<uint> elements = new List<uint>();
            uint lv1 = 0, lv2 = 0;
            int index = 0;
            for (float X = -1; X <= 1; X += stride)
            {
                float Y = fft[index] * 20;
                Vertex vertex1 = new Vertex(new Vector3(X, Y, -1), Vector4.One, Vector3.Zero, new Vector2((X + 1)/2, 0));
                Vertex vertex2 = new Vertex(new Vector3(X, Y, 1), Vector4.One, Vector3.Zero, new Vector2((X + 1) / 2, 1));
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
                lastUpdate = Util.Time;
                index++;
            }
            mesh.UploadData(vertexes.ToArray(), elements.ToArray());
        }
        public override void OnUpdate(double time)
        {
            
        }
    }
}
