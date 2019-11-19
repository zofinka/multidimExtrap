using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solver
{
    namespace Tests
    {
        class SqrtXSqrtY : Tests.AFunction
        {
            public override string configFile { get { return @"..\..\..\test_data\18.sqrtXsqrtY\config.cfg"; } }
            public override string pointFile { get { return @"..\..\..\test_data\18.sqrtXsqrtY\points.txt"; } }
            public override string name { get { return "18.sqrtXsqrtY "; } }

            public override double[] func(double[] points)
            {
                return new double[1] { Math.Sqrt(points[0]) * Math.Sqrt(points[1]) };
            }

            public override double[] derivative(double[] points)
            {
                return new double[2] { 0, 0 };
            }
        }
    }
}
