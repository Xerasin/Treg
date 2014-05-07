using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Treg_Engine.Entities
{
    [EntityName("env_camera", true)]
    public class env_camera : BaseEntity
    {
        public env_camera()
            : base()
        {

        }
        public void SetActiveCamera()
        {
            View.Camera = this;
        }
    }
}
