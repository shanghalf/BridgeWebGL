using Bridge;
using Bridge.Html5;
using Bridge.WebGL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;

namespace WebGLEditor
{
    public class App
    {






        public static int VERTEX_SHADER { get; private set; }
        public static double[] positions = new double[] {
          // Front face
          -1.0, -1.0, 1.0,
           1.0, -1.0, 1.0,
           1.0, 1.0, 1.0,
          -1.0, 1.0, 1.0,


          // Back face
          -1.0, -1.0, -1.0,
          -1.0, 1.0, -1.0,
           1.0, 1.0, -1.0,
           1.0, -1.0, -1.0,

          // Top face
          -1.0, 1.0, -1.0,
          -1.0, 1.0, 1.0,
           1.0, 1.0, 1.0,
           1.0, 1.0, -1.0,

          // Bottom face
          -1.0, -1.0, -1.0,
           1.0, -1.0, -1.0,
           1.0, -1.0, 1.0,
          -1.0, -1.0, 1.0,

           // Right face
           1.0, -1.0, -1.0,
           1.0, 1.0, -1.0,
           1.0, 1.0, 1.0,
           1.0, -1.0, 1.0,

          // Left face
          -1.0, -1.0, -1.0,
          -1.0, -1.0, 1.0,
          -1.0, 1.0, 1.0,
          -1.0, 1.0, -1.0
        };


        const string fsSource =
            "varying lowp vec4 vColor;"+
            "void main()" +
            "{" +
            "gl_FragColor = vColor;" +
            "}";

        const string vsSource =
            "attribute vec4 aVertexPosition;" +
            "attribute vec4 aVertexColor;" +
            "uniform mat4 uModelViewMatrix;" +
            "uniform mat4 uProjectionMatrix;" +
            "varying lowp vec4 vColor;" +
            "void main()" +
            "{" +
            "gl_Position = uProjectionMatrix * uModelViewMatrix * aVertexPosition;" +
            "vColor = aVertexColor;"+
            "}";

        public static float then = 0.0f;
        public static WebGLRenderingContext gl;
        public static Programinfo P;

        static void TickTimer(object state)
        {

            then += 0.01f;

            Console.Clear();
            Console.WriteLine(then.ToString());
            DrawScene(gl, P, then);
            
           // Thread.Sleep(500);
        }

  

        public static Timer T1 = new Timer(new TimerCallback(TickTimer),null,0,10);

        public static void Main()
        {

            


            // Write a message to the Console
            Console.WriteLine("Welcome to Bridge.NET");

            HTMLCanvasElement canvas = Document.GetElementById("canvas").As<HTMLCanvasElement>();
            gl = canvas.GetContext("webgl").As<WebGLRenderingContext>();
            string message;

            if (gl != null && gl != Script.Undefined)
            {
                Console.WriteLine("GL CANVAS INIT");
            }
            else
            {
                Console.WriteLine("WebGLRenderingContext initialization failed");
            }


                GLStuff(gl);
        }
        public static WebGLProgram InitShaderProgram(WebGLRenderingContext gl, string vsource, string fsource)
        {

            Union<int, WebGLProgram> shaderprogram = gl.CreateProgram();

            WebGLShader vertexshader = LoadShader(gl, gl.VERTEX_SHADER, vsource);
            WebGLShader fragmentshader = LoadShader(gl, gl.FRAGMENT_SHADER, fsource);

            gl.AttachShader((WebGLProgram)shaderprogram, vertexshader);
            gl.AttachShader((WebGLProgram)shaderprogram, fragmentshader);

            gl.LinkProgram((WebGLProgram)shaderprogram);

            if ((bool)gl.GetProgramParameter((WebGLProgram)shaderprogram, gl.LINK_STATUS) == false)
            {
                Console.WriteLine("cannot link" + gl.GetProgramInfoLog((WebGLProgram)shaderprogram));
                return null;

            }
            else
                Console.WriteLine("linked du cul" + gl.GetProgramInfoLog((WebGLProgram)shaderprogram));

            return (WebGLProgram)shaderprogram;

        }

        public static WebGLProgram Theprog;
        public static void GLStuff(WebGLRenderingContext gl)
        {
            Theprog = InitShaderProgram(gl, vsSource, fsSource);
            P = new Programinfo(gl, Theprog);
            gl.ClearColor(0.0, 0.0, 0.0, 1.0);
            gl.Clear(gl.COLOR_BUFFER_BIT);
        }


        public class Programinfo


        {
            public Programinfo(WebGLRenderingContext glin, WebGLProgram progin)
            {
                gl = glin;
                program = progin;

            }
            public WebGLProgram program;
            public WebGLRenderingContext gl;
            public int vertexPosition { get { return gl.GetAttribLocation(program, "aVertexPosition"); } }
            public int vertexColor { get { return gl.GetAttribLocation(program, "aVertexColor"); } }

            public WebGLUniformLocation projectionMatrix { get { return gl.GetUniformLocation(program, "uProjectionMatrix"); } }
            public WebGLUniformLocation modelviewmatrix { get { return gl.GetUniformLocation(program, "uModelViewMatrix"); } }

        }


        public static WebGLShader LoadShader(WebGLRenderingContext gl, int type, string source)
        {
            WebGLShader shader = gl.CreateShader(type);
            gl.ShaderSource(shader, source);
            gl.CompileShader(shader);
            if ((bool)gl.GetShaderParameter(shader, gl.COMPILE_STATUS) == false)
            {
                Console.WriteLine(gl.GetShaderInfoLog(shader));
                return null;
            }
            return shader;
        }

        public enum BufType
        {
            BUFFF_POS = 1,
            BUFF_COLOR = 2
        }

        public static WebGLBuffer initBuffers(WebGLRenderingContext gl, BufType bt)
        {
            double[] pos = new double[] { -1.0, 1.0, 1.0, 1.0, -1.0, -1.0, 1.0, -1.0 };
            double[] colors = new double[] { 1.0, 1.0, 1.0, 1.0, 1.0, 0.0, 0.0, 1.0, 0.0, 1.0, 0.0, 1.0, 0.0, 0.0, 1.0, 1.0 };

            WebGLBuffer colorBuffer = gl.CreateBuffer();
            WebGLBuffer posbuffer = gl.CreateBuffer();

            gl.BindBuffer(gl.ARRAY_BUFFER, colorBuffer);
            gl.BufferData(gl.ARRAY_BUFFER, new Float32Array(colors), gl.STATIC_DRAW);

            gl.BindBuffer(gl.ARRAY_BUFFER, posbuffer);
            gl.BufferData(gl.ARRAY_BUFFER, new Float32Array(pos), gl.STATIC_DRAW);

            switch (bt)
            {
                case BufType.BUFFF_POS:
                    return posbuffer;
                case BufType.BUFF_COLOR:
                    return colorBuffer;
                default:
                    return posbuffer;
                    

            }
            


        }

       



        public static void DrawScene(WebGLRenderingContext gl, Programinfo P,float deltatime )
        {
            gl.ClearColor(0.0, 0.0, 0.0, 1.0);  // Clear to black, fully opaque
            gl.ClearDepth(1.0);                 // Clear everything
            gl.Enable(gl.DEPTH_TEST);           // Enable depth testing
            gl.DepthFunc(gl.LEQUAL);            // Near things obscure far things
            // Clear the canvas before we start drawing on it.
            gl.Clear(gl.COLOR_BUFFER_BIT | gl.DEPTH_BUFFER_BIT);

            double fieldOfView = 45 * Math.PI / 180;   // in radians
            double aspect = (gl.Canvas.ClientWidth / gl.Canvas.ClientHeight);
            double zNear = 0.1;
            double zFar = 100.0;
            Matrix projectionMatrix = new Matrix();
            Matrix modelViewMatrix = new Matrix();
            modelViewMatrix.translate( modelViewMatrix, new double[] { -0, 0, -6 });
            projectionMatrix.perspective( fieldOfView, aspect, zNear, zFar);
            modelViewMatrix.Rotate(modelViewMatrix,deltatime,new double[] {0,1,1});
            //int numcomponent = 2;
            int Type = gl.FLOAT;
            bool normalize = false;
            int stride = 0;
            int offset = 0;

            gl.BindBuffer(gl.ARRAY_BUFFER, initBuffers(gl,BufType.BUFFF_POS));
            gl.VertexAttribPointer(P.vertexPosition, 2, Type, normalize, stride, offset);
            gl.EnableVertexAttribArray(P.vertexPosition);

            gl.BindBuffer(gl.ARRAY_BUFFER, initBuffers(gl, BufType.BUFF_COLOR));
            gl.VertexAttribPointer(P.vertexColor, 4, Type, normalize, stride, offset);
            gl.EnableVertexAttribArray(P.vertexColor);



            gl.UseProgram(Theprog);
            gl.UniformMatrix4fv(P.projectionMatrix, false, projectionMatrix.Lout);
            gl.UniformMatrix4fv(P.modelviewmatrix, false, modelViewMatrix.Lout);

            int off = 0;
            int vertexcount = 4;
            gl.DrawArrays(gl.TRIANGLE_STRIP, off, vertexcount);




        }









    }
}


