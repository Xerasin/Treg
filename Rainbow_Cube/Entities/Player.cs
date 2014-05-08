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
        }
        private Vector3 motion = Vector3.Zero;
        private Vector2 mouse_motion = Vector2.Zero;
        public override void OnUpdate(double time)
        {
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
            if (keyboard[Key.Space])
            {
                motion += this.Angles.Up * 2;
            }
            if (keyboard[Key.AltLeft])
            {
                motion += this.Angles.Up * -2;
            }
            this.Position += motion * (float)time * 3f;
            motion *= 0.5f;
            if (!GUI.CursorShown && Window.Instance.Focused)
            {
                float LX = Window.Instance.Width / 2;
                float LY = Window.Instance.Height / 2;
                float X = Window.Instance.Mouse.X;
                float Y = Window.Instance.Mouse.Y;
                float X_Dif = LX - X;
                float Y_Dif = LY - Y;
                mouse_motion.X += X_Dif;
                mouse_motion.Y += Y_Dif;
                this.Angles.Yaw += mouse_motion.X * (float)time * 6f;
                this.Angles.Roll = Math.Min(Math.Max(this.Angles.Roll + mouse_motion.Y * (float)time * 6f, -85), 85);
                mouse_motion *= 0.5f;
                System.Windows.Forms.Cursor.Position = Window.Instance.PointToScreen(new System.Drawing.Point((int)LX, (int)LY));
            }
        }
    }
}
