using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Treg_Engine.Audio
{
    public class CachedAudio
    {
        public string Name { get; set; }
        public IntPtr bufferPointer { get; set; }
        public int bufferLength { get; set; }
        public string Filename { get; set; }
        public GCHandle DataHandle;

        public CachedAudio(string name, IntPtr ptr, int length, GCHandle handle)
        {

            DataHandle = handle;
            Name = name;
            bufferPointer = ptr;
            bufferLength = length;
        }
    }
}
