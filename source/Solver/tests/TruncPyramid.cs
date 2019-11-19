using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solver
{
    namespace Tests
    {
        class TruncPyramid : Tests.AFunction
        {
            public override string configFile { get { return @"..\..\..\test_data\4.TruncPyramidVolume\config.cfg"; } }
            public override string pointFile { get { return @"..\..\..\test_data\4.TruncPyramidVolume\points.txt"; } }
            public override string name { get { return "Trunc Pyramid Volume 1/3 * x3 * (x1 + x2 + sqrt(x1*x2))"; } }

            public override double[] func(double[] points)
            {
                double a = points[0] + points[1] + Math.Sqrt(points[0] * points[1]);
                double b = points[2] * a;
                double c = (1 / 3) * b;
                double v = b / 3;
                return new double[1] { points[2] * (points[0] + points[1] + Math.Sqrt(points[0] * points[1])) / 3 };
            }

            public override double[] derivative(double[] points)
            {
                return new double[1] { 0 };
            }
        }
    }
}
