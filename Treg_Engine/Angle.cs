using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
namespace Treg_Engine
{
    public class Angle
    {
        public static Angle Zero = new Angle();
        public Matrix4 RotationMatrix = Matrix4.Identity;
        private float pitch = 0;
        private float yaw = 0;
        private float roll = 0;
        public float Pitch {
            get
            {
                return pitch;
            }
            set
            {
                this.pitch = value;
                RecalculateMatrix();
            }
        }
        public float Yaw
        {
            get
            {
                return yaw;
            }
            set
            {
                this.yaw = value;
                RecalculateMatrix();
            }
        }
        public float Roll
        {
            get
            {
                return roll;
            }
            set
            {
                this.roll = value;
                RecalculateMatrix();
            }
        }
        public Vector3 Up
        {
            get
            {
                return new Vector3(this.RotationMatrix.Row1);
            }
        }
        public Vector3 Forward
        {
            get
            {
                return new Vector3(this.RotationMatrix.Row0);
            }
        }
        public Vector3 Right
        {
            get
            {
                return new Vector3(this.RotationMatrix.Row2);
            }
        }
        public Angle(float Pitch = 0, float Yaw = 0, float Roll = 0)
        {
            this.pitch = Pitch;
            this.yaw = Yaw;
            this.roll = Roll;
            RecalculateMatrix();
        }
        public Matrix4 RecalculateMatrix()
        {
            Matrix4 modelmatrix = Matrix4.Identity;
            modelmatrix *= Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(this.Roll));
            modelmatrix *= Matrix4.CreateRotationX(MathHelper.DegreesToRadians(this.Pitch));
            modelmatrix *= Matrix4.CreateRotationY(MathHelper.DegreesToRadians(this.Yaw));
            this.RotationMatrix = modelmatrix;
            return modelmatrix;
        }
    }
}
