using System;

namespace Solver
{
    public class Shepard : AFunction
    {
        alglib.idwint.idwinterpolant[] z = null;
        double[] xy;

        public Shepard(int N, double[][] xf, int[] index = null)
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
    }
}
