using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solver
{
namespace Classifiers
{
    public struct LabeledData
    {
        public double[] data; // add generic type instead of double.
        public Object label;

        public LabeledData(double[] data, Object label)
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
}