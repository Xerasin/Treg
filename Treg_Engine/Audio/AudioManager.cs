using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Un4seen.Bass;

namespace Treg_Engine.Audio
{
    public static class AudioManager
    {
        private static List<Audio> _audiolist = new List<Audio>();
        public static void Init()
        {
            Bass.BASS_Init(-1, 44100, BASSInit.BASS_DEVICE_3D, IntPtr.Zero);


            Bass.BASS_Set3DFactors(1.0f, 1.0f, 1.0f);
            Bass.BASS_Apply3D();
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
