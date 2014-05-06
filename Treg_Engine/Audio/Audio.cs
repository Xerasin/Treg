using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using Un4seen.Bass;
namespace Treg_Engine.Audio
{
    public class Audio
    {
        public int Handle;
        public int Filename;
        public Treg_Engine.Entities.BaseEntity Entity;
       
        public bool Positional;
        public Int32 StartLoop;
        public Int32 EndLoop;
        public bool Looped;
        public Vector3 Position { get; private set; }
        public float Volume { get; private set; }
        public BASS_CHANNELINFO Info;
        public void SetPosition(Vector3 pos)
        {
            if (this.Handle != 0 && this.Positional)
            {
                this.Position = pos;
                BASS_3DVECTOR vec = new BASS_3DVECTOR(pos.X, pos.Y, pos.Z);
                Bass.BASS_ChannelSet3DPosition(this.Handle, vec, null, null);
                Bass.BASS_Apply3D();
            }
        }
        public void SetVolume(float volume)
        {
            
            if (this.Handle != 0 && this.Positional)
            {
                this.Volume = volume;
                Bass.BASS_ChannelSetAttribute(this.Handle, BASSAttribute.BASS_ATTRIB_VOL, volume);
            }
        }
        public void Remove()
        {
            Bass.BASS_ChannelStop(this.Handle);
            Bass.BASS_StreamFree(this.Handle);
        }
    }
}
