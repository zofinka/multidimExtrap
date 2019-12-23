using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Approx
{
    public interface IApprox
    {
        void Calculate(double[] xf);
        double GetError(double[] oldPoint, double[] newPoint);
    }

    class ShepardApprox : AFunction, IApprox
    {
        alglib.idwint.idwinterpolant[] z = null;
        double[] xy;

        public ShepardApprox(int N, double[][] xf, int[] index = null)
        {
            int n = (index == null) ? xf.Length : index.Length;
            int NM = xf[0].Length;
            this.N = N;
            this.M = NM - N;
            this.Min = (index == null) ? Tools.Min(xf) : Tools.Min(xf, index);
            this.Max = (index == null) ? Tools.Max(xf) : Tools.Max(xf, index);
            this.z = null;
            this.xy = null;
            if (n < 1) return;
            if (n < 2) { xy = (double[])xf[index[0]].Clone(); return; }
            this.z = new alglib.idwint.idwinterpolant[M];
            for (int m = 0; m < M; m++)
            {
                double[,] xy = new double[n, N + 1];
                for (int i = 0; i < n; i++)
                {
                    int id = (index == null) ? i : index[i];
                    for (int j = 0; j < N; j++)
                        xy[i, j] = xf[id][j];
                    xy[i, N] = xf[id][N + m];
                }
                z[m] = new alglib.idwint.idwinterpolant();
                alglib.idwint.idwbuildmodifiedshepard(xy, n, N, 2, 15, 20, z[m]);
            }
        }

        public override void Calculate(double[] xf)
        {
            if (z != null)
                for (int m = 0; m < M; m++)
                    xf[N + m] = alglib.idwint.idwcalc(z[m], xf);
            else if (xy != null)
                for (int m = 0; m < M; m++)
                    xf[N + m] = xy[N + m];
        }

        public double GetError(double[] oldPoint, double[] newPoint)
        {
            double result = 0;
            for (int i = 0; i < M; i++)
            {
                double[] diffs = oldPoint.Zip(newPoint, (d1, d2) => Math.Abs(d1 - d2)).ToArray();
                double err = (diffs.Sum() / diffs.Length);
                result += err;
            }
            return result;
        }
    }
}
