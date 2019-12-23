using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solver
{
    namespace Tests
    {
        class SinXCosXCosY : Tests.AFunction
        {
            public override string configFile { get { return @"..\..\..\test_data\17.sinXcosYcosX\config.cfg"; } }
            public override string pointFile { get { return @"..\..\..\test_data\17.sinXcosYcosX\points.txt"; } }
            public override string name { get { return "SinXCosXCosY "; } }

            // sin(x1)* cos(x1) * cos(x2)
            public override double[] func(double[] points)
            {
                return new double[1] { Math.Sin(points[0]) * Math.Cos(points[0]) * Math.Cos(points[1]) };
            }

            public override double[] derivative(double[] points)
            {
                return new double[2] { Math.Cos(2 * points[0]) * Math.Cos(points[1]), -1 * Math.Sin(points[0]) * Math.Cos(points[0]) * Math.Sin(points[1]) };
            }
        }
    }
}
