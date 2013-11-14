using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Treg_Engine
{
    public class Angle
    {
        public static Angle Zero = new Angle();
        public float Pitch { get; set; }
        public float Yaw { get; set; }
        public float Roll { get; set; }
        public Angle(float Pitch = 0, float Yaw = 0, float Roll = 0)
        {
            this.Pitch = Pitch;
            this.Yaw = Yaw;
            this.Roll = Roll;
        }
    }
}
