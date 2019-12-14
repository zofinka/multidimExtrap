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
        public int label;

        public LabeledData(double[] data, int label)
        {
            this.data = data;
            this.label = label;
        }
    }
    public abstract class AClassifierParams
    {
        // Training set
        public LabeledData[] XY { get; set; }

        public AClassifierParams(LabeledData[] xy)
        {
            XY = xy;
        }
    }

    public interface IClassifier
    {
        void train(AClassifierParams param);
        void infer(double[] x, out int label);
        void validate(LabeledData[] XY, out double modelPrecision);
    }
}
