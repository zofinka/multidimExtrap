using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Approx
{
    public class Grid
    {
        int N, NM;
        double[] min;
        double[] max;

        public readonly double[] step;
        public readonly int[] count;
        public readonly int[] rount;
        public readonly double[][] Node;
        //public readonly double[] Error;

        public Grid(int N, int M, double[] min, double[] max, int[] count)
        {
            if (count.Length != N)
                throw new Exception("Grid: несоответствие длин массивов");
            for (int i = 0; i < N; i++)
            {
                if (count[i] < 1) throw new Exception("Grid: число узлов по оси сетки не может быть меньше 1");
            }
            this.N = N;
            this.NM = N + M;
            this.min = min;
            this.max = max;
            this.count = count;

            step = new double[N];
            for (int i = 0; i < N; i++)
                step[i] = (max[i] - min[i]) / count[i];

            //создаю сетку
            rount = new int[N];
            int bas = 1; for (int i = 0; i < N; i++) { rount[i] = bas; bas *= count[i]; }
            Node = new double[bas][];
            //Error = new double[bas];
            int[] counter = new int[N]; counter.Reset();
            int pos = 0;
            while (true)
            {
                double[] x = new double[NM];
                for (int j = 0; j < N; j++)
                    x[j] = min[j] + ((count[j] > 1) ? counter[j] * (max[j] - min[j]) / (count[j] - 1) : 0);
                Node[pos] = x;
                //Error[pos] = 0;
                //Console.WriteLine("{0} -> {1} = {2}", counter.Str(), pos, ToIndex(Node[pos]));
                pos++;
                if (!counter.Inc(count)) break;
            }
        }


        public bool ToIndex(double[] x, out int index)
        {
            index = 0;
            bool valid = true;
            for (int j = 0; j < N; j++)
            {
                double jpos = (max[j] == min[j]) ? 0 : (count[j] - 1) * (x[j] - min[j]) / (max[j] - min[j]);
                if (jpos < 0) { jpos = 0; valid = false; }
                if (jpos >= count[j] - 2) { jpos = count[j] - 2; valid = false; }
                index += rount[j] * (int)jpos;
            }
            return valid;
        }

        public bool ToIndex(int[] counter, out int index)
        {
            index = 0;
            bool valid = true;
            for (int j = 0; j < N; j++)
            {
                int jpos = counter[j];
                if (jpos < 0) { jpos = 0; valid = false; }
                if (jpos >= count[j]) { jpos = count[j] - 1; valid = false; }
                index += rount[j] * counter[j];
            }
            return valid;
        }

        public bool ToCounter(int index, int[] counter)
        {
            if (index < 0 || index >= Node.Length) return false;
            for (int j = N - 1; j >= 0; j--)
            {
                counter[j] = index / rount[j];
                index = index % rount[j];
            }
            return true;
        }

        public IEnumerable<int> Neighbours(int index)
        {
            if (index < 0 || index >= Node.Length) yield break;
            int idx = index;
            for (int j = N - 1; j >= 0; j--)
            {
                int pos = idx / rount[j];
                idx = idx % rount[j];
                if (pos > 0) yield return index - rount[j];
                if (pos < count[j] - 1) yield return index + rount[j];
            }
        }
    }
}
