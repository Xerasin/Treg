using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Treg_Engine.Graphics;
using OpenTK;
namespace Treg_Engine.Entities
{
    [EntityNameAttribute("env_spotlight", true)]
    public class env_spotlight : BaseEntity
    {
        public bool Enabled { get; set; }
        public Vector3 Direction { get; set; }
        public float Cutoff { get; set; }
        public float Constant { get; set; }
        public float AmbientIntensity { get; set; }
        public float DiffuseIntensity { get; set; }
        public float Linear { get; set; }
        public override void OnUpdate(double time)
        {
            if(Enabled)
            {
                SpotLight light = new SpotLight();
                light.Base.Atten.Constant = Constant;
                light.Base.Atten.Linear = this.Linear;
                light.Base.Base.Color = this.Color;
                light.Base.Position = this.Position;
                light.Base.Base.Ambient = AmbientIntensity;
                light.Base.Base.Diffuse = DiffuseIntensity;
                light.Direction = this.Direction;
                light.Cutoff = this.Cutoff;
                light.DieTime = Util.Time + 1;
                Treg_Engine.Graphics.Lighting.SetSpotLight(light, EntIndex);
                base.OnUpdate(time);
            }
        }
    }
}
