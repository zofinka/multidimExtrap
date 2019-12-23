using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Approx
{
    public static class Tools
    {
        public static void Reset(this int[] index)
        {
            for (int i = 0; i < index.Length; i++) index[i] = 0; ;
        }

        public static bool Inc(this int[] index, int[] BASE)
        {
            int com = 0;
            while (com < index.Length)
            {
                index[com]++;
                if (index[com] < BASE[com]) break;
                index[com] = 0;
                com++;
            }
            return (com < index.Length);
        }

        public static string Str(this int[] index)
        {
            string s = "";
            for (int i = 0; i < index.Length; i++)
                s += string.Format("{0} ", index[i]);
            return s.TrimEnd(' ');
        }

        private static Random rnd = new Random(0);

        public static double Rand(double min, double max)
        {
            return min + rnd.NextDouble() * (max - min);
        }

        public static double EPS = 0.00000001;

        public static bool Equal(int N, double[] a, double[] b)
        {
            for (int i = 0; i < N; i++)
                if (Math.Abs(a[i] - b[i]) > EPS) return false;
            return true;
        }

        public static double[][] Union(this double[][] items, double[][] other)
        {
            double[][] union = new double[items.Length + other.Length][];
            for (int i = 0; i < items.Length; i++)
                union[i] = items[i];
            for (int i = 0; i < other.Length; i++)
                union[items.Length + i] = other[i];
            return union;
        }

        public static double[][] Sub(this double[][] items, int[] index)
        {
            double[][] sub = new double[index.Length][];
            for (int i = 0; i < index.Length; i++)
                sub[i] = items[index[i]];
            return sub;
        }

        public static int[] Sub(int[] index, int start, int count)
        {
            int[] sub = new int[count];
            for (int i = 0; i < count; i++)
                sub[i] = index[start + i];
            return sub;
        }

        public static void Split(double[][] xf, int[] index, int component, double value, out int[] left, out int[] right)
        {
            int count = index.Length;
            int left_count = 0;
            for (int i = 0; i < count; i++) if (xf[index[i]][component] < value) left_count++;
            left = new int[left_count];
            right = new int[count - left_count];
            int l = 0, r = 0;
            for (int i = 0; i < count; i++)
            {
                if (xf[index[i]][component] < value)
                    left[l++] = index[i];
                else
                    right[r++] = index[i];
            }
        }



        public static void Mix(int[] index, int start = 0)
        {
            int n = index.Length;
            for (int i = start; i < n; i++)
            {
                int j = rnd.Next(n);
                int t = index[i]; index[i] = index[j]; index[j] = t;
            }

        }

        public static int[] CreateIndex(double[] values)
        {
            return CreateIndex(values.Length);
        }

        public static int[] CreateIndex(double[][] xf)
        {
            return CreateIndex(xf.Length);
        }

        public static int[] CreateIndex(int length)
        {
            int[] index = new int[length];
            for (int i = 0; i < length; i++) index[i] = i;
            return index;
        }

        public static void Sort(double[][] xf, int[] index, int component)
        {
            int count = index.Length;
            int temp;
            for (int i = 0; i < count - 1; i++)
                for (int j = i + 1; j < count; j++)
                    if (xf[index[j]][component] < xf[index[i]][component])
                    {
                        temp = index[j];
                        index[j] = index[i];
                        index[i] = temp;
                    }
        }

        public static void Sort(double[] values, int[] index)
        {
            int count = index.Length;
            int temp;
            for (int i = 0; i < count - 1; i++)
                for (int j = i + 1; j < count; j++)
                    if (values[index[j]] < values[index[i]])
                    {
                        temp = index[j];
                        index[j] = index[i];
                        index[i] = temp;
                    }
        }

        public static double[] Min(double[][] xf)
        {
            int count = xf.Length;
            double[] min = (double[])xf[0].Clone();
            int n = min.Length;
            for (int i = 0; i < xf.Length; i++)
                for (int j = 0; j < min.Length; j++)
                    if (min[j] > xf[i][j]) min[j] = xf[i][j];
            return min;
        }

        public static double[] Max(double[][] xf)
        {
            int count = xf.Length;
            double[] max = (double[])xf[0].Clone();
            int n = max.Length;
            for (int i = 0; i < xf.Length; i++)
                for (int j = 0; j < max.Length; j++)
                    if (max[j] < xf[i][j]) max[j] = xf[i][j];
            return max;
        }

        public static double[] Min(double[][] xf, int[] index)
        {
            int count = index.Length;
            double[] min = (double[])xf[index[0]].Clone();
            int n = min.Length;
            foreach (var i in index)
                for (int j = 0; j < n; j++)
                    if (min[j] > xf[i][j]) min[j] = xf[i][j];
            return min;
        }

        public static double[] Max(double[][] xf, int[] index)
        {
            int count = index.Length;
            double[] max = (double[])xf[index[0]].Clone();
            int n = max.Length;
            foreach (var i in index)
                for (int j = 0; j < n; j++)
                    if (max[j] < xf[i][j]) max[j] = xf[i][j];
            return max;
        }

    }
}
