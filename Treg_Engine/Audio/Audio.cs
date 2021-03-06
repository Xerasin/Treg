﻿using System;
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
        public float[] GetFFT(BASSData data)
        {
            int length = 0;
            switch(data)
            {
                case BASSData.BASS_DATA_FFT256:
                    length = 128;
                    break;
                case BASSData.BASS_DATA_FFT512:
                    length = 256;
                    break;
                case BASSData.BASS_DATA_FFT1024:
                    length = 512;
                    break;
                case BASSData.BASS_DATA_FFT2048:
                    length = 1024;
                    break;
                case BASSData.BASS_DATA_FFT4096:
                    length = 2048;
                    break;
                case BASSData.BASS_DATA_FFT8192:
                    length = 4096;
                    break;
                case BASSData.BASS_DATA_FFT16384:
                    length = 8192;
                    break;
                default:
                    return new float[1];
            }
            float[] fftData = new float[length];
            Bass.BASS_ChannelGetData(this.Handle, fftData, (int)data);
            return fftData;
        }
        public void Seek(double seconds)
        {
            if (this.Handle != -1)
            {
                long index = Bass.BASS_ChannelSeconds2Bytes(this.Handle, seconds);
                Bass.BASS_ChannelSetPosition(this.Handle, index, BASSMode.BASS_POS_BYTES);
            }
        }
        public void SeekBytes(long index)
        {
            if(this.Handle != -1)
            {
                Bass.BASS_ChannelSetPosition(this.Handle, index, BASSMode.BASS_POS_BYTES);
            }
        }
        public double GetLength()
        {
            long byte_Length = this.GetLengthBytes();
            if(this.Handle != -1)
            {
                return Bass.BASS_ChannelBytes2Seconds(this.Handle, byte_Length);
            }
            return -1;
        }
        public long GetLengthBytes()
        {
            if (this.Handle != -1)
            {
                return Bass.BASS_ChannelGetLength(this.Handle);
            }
            return 0;
        }
    }
}
