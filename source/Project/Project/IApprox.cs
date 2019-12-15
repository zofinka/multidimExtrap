using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    public interface IApprox
    {
        void Calculate(MeasuredPoint xf);
        double GetError(double[] oldPoint, double[] newPoint);
    }

    class ShepardApprox : AFunction, IApprox
    {
        alglib.idwint.idwinterpolant[] z = null;
        MeasuredPoint xy;

        //public ShepardApprox(int N, double[][] xf, int[] index = null)
        public ShepardApprox(int N, MeasuredPoint[] xf, int[] index = null)
        {
            int n = (index == null) ? xf.Length : index.Length;
            //int NM = xf[0].Length;
            this.N = N;
            this.M = xf[0].outputValues.Length;
            this.Min = (index == null) ? Tools.Min(xf) : Tools.Min(xf, index);
            this.Max = (index == null) ? Tools.Max(xf) : Tools.Max(xf, index);
            this.z = null;
            this.xy = null;
            if (n < 1) return;
            if (n < 2) { xy = (MeasuredPoint)xf.Clone(); return; }
            this.z = new alglib.idwint.idwinterpolant[M];
            for (int m = 0; m < M; m++)
            {
                double[,] xy = new double[n, N + 1];
                for (int i = 0; i < n; i++)
                {
                    int id = (index == null) ? i : index[i];
                    for (int j = 0; j < N; j++)
                        xy[i, j] = xf[id].inputValues[j];
                    xy[i, N] = xf[id].outputValues[m];
                }
                z[m] = new alglib.idwint.idwinterpolant();
                alglib.idwint.idwbuildmodifiedshepard(xy, n, N, 2, 15, 20, z[m]);
            }
        }

        public override void Calculate(MeasuredPoint xf)
        {
            double[] array = new double[N + M];
            for (int i = 0; i < array.Length; i++)
            {
                if (i < N)
                    array[i] = xf.inputValues[i];
                else
                    array[i] = xf.outputValues[i - N];
            }
            if (z != null)
                for (int m = 0; m < M; m++)
                    xf.outputValues[m] = alglib.idwint.idwcalc(z[m], array);
            else if (xy != null)
                for (int m = 0; m < M; m++)
                    xf.outputValues[m] = xy.outputValues[m];
        }

        public double GetError(double[] oldPoint, double[] newPoint)
        {
            double result = 0;
            for (int i = 0; i < M; i++)
            {
                double diff = Math.Abs(oldPoint[i] - newPoint[i]);
                result += diff;
            }
            result = result / M;
            return result;
        }
    }
}
