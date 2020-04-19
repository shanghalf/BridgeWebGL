using System;

namespace WebGLEditor
{

    public class Matrix 
    {
        public double[] Lout; // matrix line 
        public double[,] Sout;// matrix square 

        public  double[] Create()
        {
            double[] Out = new double[] { 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1 };
            return Out;
        }



        public double this[int index]
        {
            get { return Lout[index];}
            set { Lout[index] = index; }
        }




        public  Matrix()
        {
           // identity 
           Lout = new double[] 
           { 
               1, 0, 0, 0, 
               0, 1, 0, 0, 
               0, 0, 1, 0, 
               0, 0, 0, 1 
           };
        }


        public void  Rotate( Matrix mat, double angle,double[] axis)
        {
            double x = axis[0];
            double y = axis[1];
            double z = axis[2];


            double len = Math.Sqrt(x * x + y * y + z * z);
            double s = 0;
            double c = 0;
            double t = 0;
            double a00 = 0;
            double a01 = 0;
            double a02 = 0;
            double a03 = 0;
            double a10 = 0;
            double a11 = 0;
            double a12 = 0;
            double a13 = 0;
            double a20 = 0;
            double a21 = 0;
            double a22 = 0;
            double a23 = 0;
            double b00 = 0;
            double b01 = 0;
            double b02 = 0;
            double b10 = 0;
            double b11 = 0;
            double b12 = 0;
            double b20 = 0;
            double b21 = 0;
            double b22 = 0;

            if (Math.Abs(len) < 0.000001)
                return ;

            len = 1 / len;
            x *= len;
            y *= len;
            z *= len;

            s = Math.Sin(angle);
            c = Math.Cos(angle);
            t = 1 - c;

            a00 = mat[0]; a01 = mat[1]; a02 = mat[2]; a03 = mat[3];
            a10 = mat[4]; a11 = mat[5]; a12 = mat[6]; a13 = mat[7];
            a20 = mat[8]; a21 = mat[9]; a22 = mat[10]; a23 = mat[11];

            // Construct the elements of the rotation matrix
            b00 = x * x * t + c; b01 = y * x * t + z * s; b02 = z * x * t - y * s;
            b10 = x * y * t - z * s; b11 = y * y * t + c; b12 = z * y * t + x * s;
            b20 = x * z * t + y * s; b21 = y * z * t - x * s; b22 = z * z * t + c;

  // Perform rotation-specific matrix multiplication
            Lout[0] = a00* b00 + a10* b01 + a20* b02;
            Lout[1] = a01* b00 + a11* b01 + a21* b02;
            Lout[2] = a02* b00 + a12* b01 + a22* b02;
            Lout[3] = a03* b00 + a13* b01 + a23* b02;
            Lout[4] = a00* b10 + a10* b11 + a20* b12;
            Lout[5] = a01* b10 + a11* b11 + a21* b12;
            Lout[6] = a02* b10 + a12* b11 + a22* b12;
            Lout[7] = a03* b10 + a13* b11 + a23* b12;
            Lout[8] = a00* b20 + a10* b21 + a20* b22;
            Lout[9] = a01* b20 + a11* b21 + a21* b22;
            Lout[10] = a02* b20 + a12* b21 + a22* b22;
            Lout[11] = a03* b20 + a13* b21 + a23* b22;

            if (mat.Lout != Lout) {
                // If the source and destination differ, copy the unchanged last row
                Lout[12] = mat[12];
                Lout[13] = mat[13];
                Lout[14] = mat[14];
                Lout[15] = mat[15];
            }
}


public void Rotate ( Matrix a , double angle )
        {
            double s = Math.Sin(angle);
            double  c = Math.Cos(angle);

            Lout[0] = c* a[0] + s* a[3];
            Lout[1] = c* a[1] + s* a[4];
            Lout[2] = c* a[2] + s* a[5];

            Lout[3] = c* a[3] - s* a[0];
            Lout[4] = c* a[4] - s* a[1];
            Lout[5] = c* a[5] - s* a[2];

            Lout[6] = a[6];
            Lout[7] = a[7];
            Lout[8] = a[8];


        }



        public void perspective( double fovy, double aspect, double near, double far)
        {
            double f = 1.0 / Math.Tan(fovy / 2);
            double nf = 1 / (near - far);
            Lout[0] = f / aspect;
            Lout[1] = 0;
            Lout[2] = 0;
            Lout[3] = 0;
            Lout[4] = 0;
            Lout[5] = f;
            Lout[6] = 0;
            Lout[7] = 0;
            Lout[8] = 0;
            Lout[9] = 0;
            Lout[10] = (far + near) * nf;
            Lout[11] = -1;
            Lout[12] = 0;
            Lout[13] = 0;
            Lout[14] = 2 * far * near * nf;
            Lout[15] = 0;

        }

        public void translate( Matrix a, double[] v)
        {
            double x = v[0], y = v[1], z = v[2];
            double a00 = 0;
            double a01 = 0;
            double a02 = 0;
            double a03 = 0;
            double a10 = 0;
            double a11 = 0;
            double a12 = 0;
            double a13 = 0;
            double a20 = 0;
            double a21 = 0;
            double a22 = 0;
            double a23 = 0;


            if (a.Lout == Lout)
            {
                Lout[12] = a[0] * x + a[4] * y + a[8] * z + a[12];
                Lout[13] = a[1] * x + a[5] * y + a[9] * z + a[13];
                Lout[14] = a[2] * x + a[6] * y + a[10] * z + a[14];
                Lout[15] = a[3] * x + a[7] * y + a[11] * z + a[15];
            }
            else
            {
                a00 = a[0]; a01 = a[1]; a02 = a[2]; a03 = a[3];
                a10 = a[4]; a11 = a[5]; a12 = a[6]; a13 = a[7];
                a20 = a[8]; a21 = a[9]; a22 = a[10]; a23 = a[11];

                Lout[0] = a00; Lout[1] = a01; Lout[2] = a02; Lout[3] = a03;
                Lout[4] = a10; Lout[5] = a11; Lout[6] = a12; Lout[7] = a13;
                Lout[8] = a20; Lout[9] = a21; Lout[10] = a22; Lout[11] = a23;

                Lout[12] = a00 * x + a10 * y + a20 * z + a[12];
                Lout[13] = a01 * x + a11 * y + a21 * z + a[13];
                Lout[14] = a02 * x + a12 * y + a22 * z + a[14];
                Lout[15] = a03 * x + a13 * y + a23 * z + a[15];
            }

        }


    }
}