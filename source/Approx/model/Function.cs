using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Approx
{
    public interface IFunction
    {
        /// <summary>
        /// Число входных параметров
        /// </summary>
        int N { get; }

        /// <summary>
        /// Число выходных значений
        /// </summary>
        int M { get; }

        /// <summary>
        /// N-мерный вектор минимальных значений входных парамеров 
        /// </summary>
        double[] Min { get; }

        /// <summary>
        /// N-мерный вектор максимальных значений входных парамеров 
        /// </summary>
        double[] Max { get; }

        /// <summary>
        /// По первым N значениям вектора xy вычисляет
        /// оставшиеся M значений вектора xy 
        /// </summary>
        void Calculate(double[] xf);
    }

    public abstract class AFunction : IFunction
    {
        public int N { get; protected set; }

        public int M { get; protected set; }

        public double[] Min { get; set; } = null;

        public double[] Max { get; set; } = null;

        //public abstract void Calculate(MeasuredPoint xf);

        public abstract void Calculate(double[] xf);
    }

    public delegate void FunctionDelegate(double[] xy);

    public class Function : IFunction
    {
        public Function(int n, int m, double[] min, double[] max, FunctionDelegate f)
        {
            this.N = n;
            this.M = m;
            this.Min = min;
            this.Max = max;
            this.f = f;
        }

        private readonly FunctionDelegate f;

        public int N { get; private set; }
        public int M { get; private set; }
        public double[] Min { get; private set; }
        public double[] Max { get; private set; }
        public void Calculate(double[] xy) { f(xy); }
    }
}
