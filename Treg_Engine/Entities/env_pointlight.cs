using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Treg_Engine.Graphics;
namespace Treg_Engine.Entities
{
    public class env_pointlight : BaseEntity
    {
        public bool Enabled { get; set; }
        public float Constant { get; set; }
        public float AmbientIntensity { get; set; }
        public float DiffuseIntensity { get; set; }
        public float Linear { get; set; }
        public override void OnUpdate(double time)
        {
            if(Enabled)
            {
                PointLight light = new PointLight();
                light.Atten.Constant = Constant;
                light.Atten.Linear = this.Linear;
                light.Base.Color = this.Color;
                light.DieTime = Util.Time + 1;
                light.Position = this.Position;
                light.Base.Ambient = AmbientIntensity;
                light.Base.Diffuse = DiffuseIntensity;
                Treg_Engine.Graphics.Lighting.SetPointLight(light, EntIndex);
                base.OnUpdate(time);
            }
        }
    }
}
