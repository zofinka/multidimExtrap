using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    public struct LabeledData
    {
        public double[] data; // add generic type instead of double.
        public Object label;

        public LabeledData(double[] data, int label)
        {
            this.data = data;
            this.label = label;
        }
    }

    public abstract class AMLAlgorithmParams
    {
        // Training set
        public LabeledData[] XY { get; set; }

        public AMLAlgorithmParams(LabeledData[] xy)
        {
            XY = xy;
        }
    }

    public interface IClassifierML
    {
        void train<T>(AMLAlgorithmParams param);
        void infer(double[] x, out Object label);
        void validate<T>(LabeledData[] XY, out double modelPrecision) where T : IComparable;
    }
}
