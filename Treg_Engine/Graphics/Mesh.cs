using System;
using System.Drawing;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
namespace Treg_Engine.Graphics
{
    public struct Vertex
    {
        public static readonly int SizeInBytes = BlittableValueType.StrideOf(new Vertex());

        public Vector3 Position;
        public Vector3 Normal;
        public Vector2 UV;
        public Vector4 Color;
       

        public Vertex(Vector3 Pos, Vector4 color, Vector3 normal, Vector2 uv)
        {
            Position = Pos;
            Normal = normal;
            UV = uv;
            Color = color;
        }
    }
    public class Mesh
    {
        public uint EBO = 0;
        public uint VAO = 0;
        public uint VBO = 0;
        public int NumElements;
        public int bytesize;
        Random rand = new Random();


        public Mesh(Vertex[] vertices, uint[] elements)
        {
            this.UploadData(vertices, elements);
        }
        public Mesh()
        {

        }
        
        public static Color ColorFromHSV(double hue, double saturation, double value)
        {
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);

            value = value * 255;
            int v = Convert.ToInt32(value);
            int p = Convert.ToInt32(value * (1 - saturation));
            int q = Convert.ToInt32(value * (1 - f * saturation));
            int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

            if (hi == 0)
                return Color.FromArgb(255, v, t, p);
            else if (hi == 1)
                return Color.FromArgb(255, q, v, p);
            else if (hi == 2)
                return Color.FromArgb(255, p, v, t);
            else if (hi == 3)
                return Color.FromArgb(255, p, q, v);
            else if (hi == 4)
                return Color.FromArgb(255, t, p, v);
            else
                return Color.FromArgb(255, v, p, q);
        }
        static Dictionary<string, Mesh> meshes = new Dictionary<string, Mesh>();
        public static Mesh LoadFromFile(string file)
        {
            if (meshes.ContainsKey(file))
            {
                return meshes[file];
            }
            StreamReader reader = new StreamReader(file);
            Mesh mesh = Mesh.LoadFromObj(reader.ReadToEnd());
            meshes[file] = mesh;
            return mesh;
        }
        public static Mesh LoadFromObj(string obj_file)
        {
 

            List<string[]> FaceLines = new List<string[]>();
            List<Vector3> Positions = new List<Vector3>();
            List<Vector3> Normals = new List<Vector3>();
            List<Vector2> UVs = new List<Vector2>();
            List<Vertex> Vertexes = new List<Vertex>();
            List<uint> Telements = new List<uint>();
            uint pos = 0;
            string[] lines = obj_file.Split('\n');
            for (int lineNum = 0; lineNum < lines.Length; lineNum++)
            {
                string line = lines[lineNum];
                string[] lineParams = line.Split(' ');
                if (lineParams.Length > 0)
                {
                    string directive = lineParams[0];
                    switch (directive)
                    {
                        case "v":
                            float x = (float)Convert.ToDouble(lineParams[1]);
                            float y = (float)Convert.ToDouble(lineParams[2]);
                            float z = (float)Convert.ToDouble(lineParams[3]);
                            Positions.Add(new Vector3(x, y, z));
                            break;
                        case "vn":
                            x = (float)Convert.ToDouble(lineParams[1]);
                            y = (float)Convert.ToDouble(lineParams[2]);
                            z = (float)Convert.ToDouble(lineParams[3]);
                            Normals.Add(new Vector3(x, y, z));
                            break;
                        case "vt":
                            x = (float)Convert.ToDouble(lineParams[1]);
                            y = (float)Convert.ToDouble(lineParams[2]);
                            UVs.Add(new Vector2(x, y));
                            break;
                        case "f":
                            for (int I = 1; I <= 4; I++)
                            {
                                if (lineParams.Length > I)
                                {
                                    string[] parts = lineParams[I].Split('/');
                                    Vector3 Position = Vector3.Zero, Normal = Vector3.Zero;
                                    Vector2 UV = Vector2.Zero;
                                    if (parts.Length >= 1)
                                    {
                                        int t = Convert.ToInt32(parts[0]);
                                        Position = Positions[t- 1];
                                    }
                                    if (parts.Length >= 2)
                                    {
                                        int t = Convert.ToInt32(parts[1]);
                                        UV = UVs[t - 1];
                                        UV.Y *= -1;
                                    }
                                    if (parts.Length >= 3)
                                    {
                                        int t = Convert.ToInt32(parts[2]);
                                        Normal = Normals[t - 1];
                                    }
                                    Vertex vert = new Vertex(Position, new Vector4(1f, 1f, 1f, 1f), Normal, UV);
                                    Vertexes.Add(vert);
                                    Telements.Add(pos);
                                    pos++;
                                }
                            }
                            break;
                    }
                }

            }

            Vertex[] vertices = Vertexes.ToArray<Vertex>();
            uint[] elements = Telements.ToArray<uint>();
            return new Mesh(vertices, elements);
            
        }
        
        public void UploadData(Vertex[] vertices, uint[] elements)
        {
            if (VAO <= 0)
            {
                GL.GenVertexArrays(1, out this.VAO);
                GL.BindVertexArray(this.VAO);

                GL.GenBuffers(1, out VBO);
                GL.GenBuffers(1, out EBO);
            }
            GL.BindVertexArray(this.VAO);
            int size;
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertices.Length * BlittableValueType.StrideOf(vertices)), vertices,
                          BufferUsageHint.StaticDraw);
            GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out size);
            if (vertices.Length * BlittableValueType.StrideOf(vertices) != size)
                throw new ApplicationException("Vertex data not uploaded correctly");
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, Vertex.SizeInBytes, 0);

            //Normals
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, true, Vertex.SizeInBytes, 3 * sizeof(float));

            //UVs
            GL.EnableVertexAttribArray(2);
            GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, true, Vertex.SizeInBytes, 6 * sizeof(float));

            //Color
            GL.EnableVertexAttribArray(3);
            GL.VertexAttribPointer(3, 4, VertexAttribPointerType.Float, true, Vertex.SizeInBytes, 8 * sizeof(float));


            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(elements.Length * sizeof(uint)), elements,
                            BufferUsageHint.StaticDraw);
            GL.GetBufferParameter(BufferTarget.ElementArrayBuffer, BufferParameterName.BufferSize, out size);
            if (elements.Length * sizeof(uint) != size)
                throw new ApplicationException("Element data not uploaded correctly");
            GL.BindVertexArray(0);
            this.NumElements = elements.Length;
        }
        public void Render(Material mat, Vector3 Origin, Angle Angle, Vector3 Scale)
        {
            this.Render(mat, Origin, Angle, Scale);
        }
        public void Render(Material mat, Vector3 Origin, Angle Angle, Vector3 Scale, Vector4 Color)
        {

            Matrix4 modelmatrix = Matrix4.Identity;

            modelmatrix *= Matrix4.CreateScale(Scale);
            modelmatrix *= Angle.RotationMatrix;
            modelmatrix *= Matrix4.CreateTranslation(Origin);
            this.Render(mat, modelmatrix, View.ProjectionMatrix, View.ViewMatrix, Color);
        }
        public void Render(Material mat, Matrix4 ModelMatrix, Matrix4 ProjectionMatrix, Matrix4 ViewMatrix)
        {
            this.Render(mat, ModelMatrix, ProjectionMatrix, ViewMatrix, Vector4.One);
        }
        public void Render(Material mat, Matrix4 ModelMatrix, Matrix4 ProjectionMatrix, Matrix4 ViewMatrix, Vector4 Color)
        {

            GL.BindTexture(TextureTarget.Texture2D, 0);
            GL.EnableClientState(ArrayCap.VertexArray);
            GL.EnableClientState(ArrayCap.NormalArray);
            GL.EnableClientState(ArrayCap.TextureCoordArray);
            GL.EnableClientState(ArrayCap.ColorArray);
            GL.BindVertexArray(this.VAO);
            //GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            //GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);

            GL.VertexPointer(3, VertexPointerType.Float, Vertex.SizeInBytes, (IntPtr)(0));
            GL.NormalPointer(NormalPointerType.Float, Vertex.SizeInBytes, (IntPtr)(3 * sizeof(float)));
            GL.TexCoordPointer(2, TexCoordPointerType.Float, Vertex.SizeInBytes, (IntPtr)(6 * sizeof(float)));
            GL.ColorPointer(3, ColorPointerType.Float, Vertex.SizeInBytes, (IntPtr)(8 * sizeof(float)));
            mat.Bind();

            mat.shader.SetUniformMatrix4("ModelMatrix", ModelMatrix);
            mat.shader.SetUniformMatrix4("ProjectionMatrix", ProjectionMatrix);
            mat.shader.SetUniformMatrix4("ViewMatrix", ViewMatrix);
            mat.shader.SetUniformFloat("time", Util.Time);
            mat.shader.SetUniformVector3("eyePos", View.EyePos);
            mat.shader.SetUniformVector4("overrideColor", Color);
            GL.DrawElements(BeginMode.Triangles, this.NumElements, DrawElementsType.UnsignedInt, IntPtr.Zero);

            GL.DisableClientState(ArrayCap.VertexArray);
            GL.DisableClientState(ArrayCap.NormalArray);
            GL.DisableClientState(ArrayCap.TextureCoordArray);
            GL.DisableClientState(ArrayCap.ColorArray);
            mat.UnBind();
        }
    }
}
