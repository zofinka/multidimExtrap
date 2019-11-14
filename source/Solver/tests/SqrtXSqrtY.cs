using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solver
{
    namespace Tests
    {
        class SqrtXSqrtY
        {
            public string configFile { get { return @"..\..\..\test_data\18.sqrtXsqrtY\config.cfg"; } }
            public string pointFile { get { return @"..\..\..\test_data\18.sqrtXsqrtY\points.txt"; } }
            public string name { get { return "18.sqrtXsqrtY "; } }

            public double func(double[] points)
            {
                return Math.Sqrt(points[0]) * Math.Sqrt(points[1]);
            }

            public double[] derivative(double[] points)
            {
                return new double[2] { 0, 0 };
            }
        }
    }
}
