using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using Treg_Engine;
using Treg_Engine.Entities;
using Treg_Engine.Graphics;
using Rainbow_Cube.Entities;
namespace Rainbow_Cube
{
    class RainbowWorld : World
    {
        Mesh mesh;
        Wave wave;
        Treg_Engine.Audio.Audio audio;
        public override void OnLoad()
        {
            
            //this.Create<Teapot>();
            env_camera camera = this.Create<Player>();
            camera.Position = new Vector3(0f, 4f, 25f);
            camera.Angles = new Angle(0, 90, 0);
            View.Camera = camera;
            ent_skybox skybox = this.Create<ent_skybox>();
            //skybox.Scale = Vector3.One * 30;
            Treg_Engine.Entities.BaseEntity Enttiy = this.Create<Popcorn>();
            Enttiy.Position = new Vector3(7, 0, -5);
            Enttiy = this.Create<Popcorn>();
            Enttiy.Position = new Vector3(-7, 0, -5);
            Enttiy = this.Create<Wave>();
            Enttiy.Position = new Vector3(0, 0, 0);
            wave = this.Create<Wave>();
            /*env_pointlight light = this.Create<env_pointlight>();
            light.Enabled = true;
            light.Position = new Vector3(0, -5, -5);
            light.Color = new Vector3(0.5f, 0f, 0f);
            light.AmbientIntensity = 0.5f;
            light.DiffuseIntensity = 1f;
            light.Constant = 0f;
            light.Linear = 0.25f;
            mesh = Mesh.LoadFromFile("resources/models/cube.obj");

            light = this.Create<env_pointlight>();
            light.Enabled = true;
            light.Position = new Vector3(0, 4, -5);
            light.Color = new Vector3(0.5f, 0.5f, 0f);
            light.AmbientIntensity = 0.5f;
            light.DiffuseIntensity = 1f;
            light.Constant = 0f;
            light.Linear = 0.25f;
            
            env_spotlight light2 = this.Create<env_spotlight>();
            light2.Enabled = true;
            light2.Position = new Vector3(-5f, 0.0f, -5.0f);
            light2.Color = new Vector3(1f, 0f, 0f);
            light2.AmbientIntensity = 0f;
            light2.DiffuseIntensity = 1f;
            light2.Constant = 0f;
            light2.Linear = 0.1f;
            light2.Direction = new Vector3(0, -1, 0);
            light2.Cutoff = 0.1f;*/
            /*audio = Treg_Engine.Audio.AudioManager.LoadSong("Resources//sounds//music.ogg", false, false, true);
            audio.Play(true);
            audio.SetVolume(0);
            double stride = 0.1;
            double length = audio.GetLength();
            int interations = (int)(length / stride);
            List<float> data = new List<float>();
            for (int I = 0; I < interations; I ++)
            {
                double second = stride * I;
                audio.Seek(second);
                float[] fft = audio.GetFFT(Un4seen.Bass.BASSData.BASS_DATA_FFT256);
                float fft_average = 0;
                for (int I2 = 0; I2 < fft.Length; I2++)
                {
                    fft_average += fft[I2];
                }
                fft_average /= fft.Length;
                fft_average *= 20;
                data.Add(fft_average);
            }
            for (int start = 0; start < data.Count; start += 128)
            {
                List<float> box = data.GetRange(start, Math.Min(128, (data.Count - start) - 1));
                Mountain test = this.Create<Mountain>();
                test.Color = new Vector4(0.8f, 0.8f, 1.0f, 1.0f);
                test.Setup(box.ToArray());
                test.Position.X = start * 0.05f;
                test.Position.Z = -1;
                test.Scale = Vector3.One * 20;
                Mountain front = this.Create<Mountain>();
                front.Color = new Vector4(0.0f, 0.0f, 1.0f, 1.0f);
                front.Setup(box.ToArray());
                front.Position.X = start * 0.05f;
                front.Position.Z = -0.98f;
                front.Scale = new Vector3(1f, 0.5f, 1f) * 20;
            }*/
                mesh = Mesh.LoadFromFile("resources/models/cube.obj");
            RegisterEntities();
            base.OnLoad();
        }
        public override void OnUpdate(double time)
        {
            base.OnUpdate(time);
            //wave.UpdateMesh(audio);
        }
        public override void OnRender()
        {
            base.OnRender();
        }
    }
}
