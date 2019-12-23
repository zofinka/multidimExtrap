using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Approx
{
    public class Task
    {
        public double[][] points;
        public int N;
        public int M;
        public Func<double[], double[]> function;

        public Task(double[][] points, int N)
        {
            this.N = N;
            this.M = (points[0].Length - N);
            this.points = points;
        }

        public Task(int pointAmount, int N, int M)
        {
            this.N = N;
            this.M = M;
            points = new double[pointAmount][];
            for (int i = 0; i < pointAmount; i++)
            {
                points[i] = new double[N + M];
            }
        }
    }
}
