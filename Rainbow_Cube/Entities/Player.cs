using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Treg_Engine;
using Treg_Engine.HUD;
using Treg_Engine.Entities;
using OpenTK;
using OpenTK.Input;
namespace Rainbow_Cube.Entities
{
    public class Player : env_camera
    {
        public Player()
            : base()
        {
            LX = Window.Instance.Mouse.X;
            LY = Window.Instance.Mouse.Y;
            Window.Instance.Mouse.Move += Mouse_Move;
        }
        private bool mouseMovedManually = true;
        void Mouse_Move(object sender, MouseMoveEventArgs e)
        {
            if(mouseMovedManually)
            {
                mouseMovedManually = false;
                return;
            }
            if (!GUI.CursorShown)
            {
                System.Drawing.Point mouse = System.Windows.Forms.Cursor.Position;
                System.Drawing.Point newpos = new System.Drawing.Point(mouse.X - e.XDelta, mouse.Y - e.YDelta);
                System.Windows.Forms.Cursor.Position = newpos;
                mouseMovedManually = true;
                this.Angles.Roll += e.YDelta * -1;
                this.Angles.Yaw += e.XDelta * -1;
                //Console.WriteLine(this.Angles.Roll);
            }

        }
        private float LX;
        private float LY;
        public override void OnUpdate(double time)
        {
            Vector3 motion = Vector3.Zero;
            OpenTK.Input.KeyboardDevice keyboard = Window.Instance.Keyboard;
            if (keyboard[Key.W])
            {
                motion += this.Angles.Forward * 2;
            }
            if (keyboard[Key.S])
            {
                motion += this.Angles.Forward * -2;
            }
            if (keyboard[Key.D])
            {
                motion += this.Angles.Right * 2;
            }
            if (keyboard[Key.A])
            {
                motion += this.Angles.Right * -2;
            }
            this.Position += motion * (float)time * 3f;
            if (!GUI.CursorShown)
            {
                float X = Window.Instance.Mouse.X;
                float Y = Window.Instance.Mouse.Y;
                float X_Dif = LX - X;
                float Y_Dif = LY - Y;
                LX = X;
                LY = Y;
            }
        }
    }
}
