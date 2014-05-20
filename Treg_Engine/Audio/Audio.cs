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
        public void Play(bool fromBeginning)
        {
            if (this.Handle != 0)
            {
                Bass.BASS_ChannelPlay(this.Handle, fromBeginning);
            }
        }
        public void Stop()
        {
            if (this.Handle != 0)
            {
                Bass.BASS_ChannelStop(this.Handle);
            }
        }
        public void Pause()
        {
            if (this.Handle != 0)
            {
                Bass.BASS_ChannelPause(this.Handle);
            }
        }
        public bool IsPlaying()
        {
            if (this.Handle != 0)
            {
                Un4seen.Bass.BASSActive active = Bass.BASS_ChannelIsActive(this.Handle);
                return active != BASSActive.BASS_ACTIVE_STOPPED && active != BASSActive.BASS_ACTIVE_PAUSED;
            }

            return false;
        }
        public void SetFrequency(float freq)
        {
            if (this.Handle != 0)
            {
                Bass.BASS_ChannelSetAttribute(this.Handle, BASSAttribute.BASS_ATTRIB_FREQ, freq);
            }
        }
    }
}
