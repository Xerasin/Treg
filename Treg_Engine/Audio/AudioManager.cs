using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Un4seen.Bass;
using System.Runtime.InteropServices;
//https://github.com/Foohy/gravity_car/blob/master/Oleg%20Engine/Audio.cs

namespace Treg_Engine.Audio
{
    public static class AudioManager
    {
        private static List<Audio> _audiolist = new List<Audio>();
        private static Dictionary<string, CachedAudio> _precachedAudioList = new Dictionary<string, CachedAudio>();
        private static SYNCPROC _SyncDel;
        public static void Init()
        {
            try
            {
                BassNet.Registration("swkauker@yahoo.com", "2X2832371834322");
                Bass.BASS_Init(-1, 44100, BASSInit.BASS_DEVICE_3D, IntPtr.Zero);


                Bass.BASS_Set3DFactors(1.0f, 1.0f, 1.0f);
                Bass.BASS_Apply3D();

                _SyncDel = new SYNCPROC(SyncThink);
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
            }
        }
        public static bool Precache(string filename)
        {
            byte[] bytes = null;
            try
            {
                bytes = System.IO.File.ReadAllBytes(filename);

            }
            catch (Exception ex)
            {
                Log.Warning("Failed to load audio file. " + ex.Message);
                return false;
            }
            int length = bytes.Length;
            GCHandle rawDataHandle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            CachedAudio memAudio = new CachedAudio(filename, rawDataHandle.AddrOfPinnedObject(), length, rawDataHandle);
            memAudio.Filename = filename; //may as well store this

            if (!_precachedAudioList.ContainsKey(filename))
            {
                _precachedAudioList.Add(filename, memAudio);
            }

            return true;
        }
        //Called when a stream reaches its end
        private static void SyncThink(int handle, int channel, int data, IntPtr user)
        {
            Audio audio = GetAudioFromHandle(channel);
            if (audio == null) { Bass.BASS_StreamFree(channel); return; }

            //If the end cue point wasn't caught in the above think function, set it to the start cue here
            if (audio.Looped && audio.StartLoop > 0 && audio.EndLoop > 0)
            {
                long seconds = Bass.BASS_ChannelGetPosition(audio.Handle, BASSMode.BASS_POS_BYTES);
                Bass.BASS_ChannelSetPosition(audio.Handle, audio.StartLoop, BASSMode.BASS_POS_BYTES);
            }
            else
            {
                //Remove the channel
                Bass.BASS_StreamFree(channel);
            }
        }
        public static Audio LoadSong(string filename, bool Loop = false, bool Positional = false, bool decode = false, Entities.BaseEntity ent = null)
        {
            int handle = 0;
            BASSFlag flags = BASSFlag.BASS_MUSIC_PRESCAN;
            if (Loop) flags = flags | BASSFlag.BASS_SAMPLE_LOOP;
            if (decode) flags = flags | BASSFlag.BASS_MUSIC_DECODE;
            if (Positional || ent != null) flags = flags | BASSFlag.BASS_SAMPLE_3D | BASSFlag.BASS_SAMPLE_MONO;

            handle = Bass.BASS_StreamCreateFile(filename, 0, 0, flags);

            if (handle != 0)
            {
                Audio audio = new Audio();
                audio.Handle = handle;
                audio.Positional = (Positional || ent != null);
                audio.Entity = ent;
                audio.Looped = Loop;

                //Add the audio object to our list to keep track of it
                _audiolist.Add(audio);

                //Set its volume so it's in line with our global volume
                audio.SetVolume(audio.Volume);

                //Create a callback so we can control playback looping and stuff
                Bass.BASS_ChannelSetSync(audio.Handle, BASSSync.BASS_SYNC_END, 0, _SyncDel, IntPtr.Zero);

                return audio;
            }
            else
            {
                Log.Error("Failed to create music stream '" + filename + "'!");
                Log.Error(Bass.BASS_ErrorGetCode());
                if (Bass.BASS_ErrorGetCode() == BASSError.BASS_ERROR_NO3D) Log.Error("Are you sure the audio file is mono?");
            }

            return new Audio();
        }
        public static void PlaySound(string name, float volume = 1.0f, int frequency = 44100)
        {
            if (!_precachedAudioList.ContainsKey(name))
            {
                if (Precache(name))
                    Log.Warning("Sound '" + name + "' was not precached!");
                else return;
            }

            int handle = Bass.BASS_StreamCreateFile(_precachedAudioList[name].bufferPointer, 0, _precachedAudioList[name].bufferLength, BASSFlag.BASS_DEFAULT | BASSFlag.BASS_STREAM_AUTOFREE); //Bass.BASS_StreamCreatePush( 44100, 1, BASSFlag.BASS_DEFAULT, IntPtr.Zero );
            if (handle == 0) Log.Error("Failed to play precached sound! " + Bass.BASS_ErrorGetCode());
            Bass.BASS_ChannelSetAttribute(handle, BASSAttribute.BASS_ATTRIB_VOL, volume);
            Bass.BASS_ChannelSetAttribute(handle, BASSAttribute.BASS_ATTRIB_FREQ, frequency);
            Bass.BASS_ChannelPlay(handle, true);
        }
        private static Audio GetAudioFromHandle(int handle)
        {
            for (int i = 0; i < _audiolist.Count; i++)
            {
                if (_audiolist[i].Handle == handle) return _audiolist[i];
            }

            //Nothing found with that handle
            return null;
        }
        public static void Think()
        {
            OpenTK.Vector3 Forward = View.EyeAngles.Forward;
            BASS_3DVECTOR pos = new BASS_3DVECTOR(View.EyePos.X, View.EyePos.Y, View.EyePos.Z);
            BASS_3DVECTOR fwd = new BASS_3DVECTOR(Forward.X, Forward.Y, Forward.Z);
            BASS_3DVECTOR up = new BASS_3DVECTOR(0, 1, 0);
            Bass.BASS_Set3DPosition(pos, null, fwd, up);

            for (int I = 0; I < _audiolist.Count; I++)
            {
                Audio audio = _audiolist[I];
                if (audio.Entity != null)
                {
                    audio.SetPosition(audio.Entity.Position);
                }
                long seconds = Bass.BASS_ChannelGetPosition(audio.Handle, BASSMode.BASS_POS_BYTES);
                long length = Bass.BASS_ChannelGetLength(audio.Handle, BASSMode.BASS_POS_BYTES);
                if (audio.Handle != 0 && !audio.Looped)
                {
                    if (seconds >= length)
                    {
                        Remove(audio);
                        I--;
                    }
                }
                else if (audio.StartLoop > 0 && audio.EndLoop > 0)
                {
                    if (seconds > audio.EndLoop)
                    {
                        Bass.BASS_ChannelSetPosition(audio.Handle, audio.StartLoop, BASSMode.BASS_POS_BYTES);
                    }
                }
            }
        }
        public static void Remove(Audio audio)
        {
            _audiolist.Remove(audio);
            audio.Remove();
        }
    }
}
