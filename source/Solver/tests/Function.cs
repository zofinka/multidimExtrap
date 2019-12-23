using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solver
{
    namespace Tests
    {
        public abstract class AFunction
        {
            public abstract string configFile { get; }
            public abstract string pointFile { get; }
            public abstract string name { get; }
            public virtual string tableFile { get { return null; } }

            public abstract double[] func(double[] points);

            public abstract double[] derivative(double[] points);

            public Dictionary<double[], double[]> table = null;
        }
    }
}
