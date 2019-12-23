using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solver
{
    namespace Tests
    {
        class SinFromSumOnSum : Tests.AFunction
        {
            public override string configFile { get { return @"..\..\..\test_data\16.sin(x+y)on(x+y)\config.cfg"; } }
            public override string pointFile { get { return @"..\..\..\test_data\16.sin(x+y)on(x+y)\points.txt"; } }
            public override string name { get { return "SinFromSumOnSum sin(x1 + x2) / (x1 + x2)"; }}

            // sin(x + y) / x + y
            public override double[] func(double[] points)
            {
                return new double[1] { Math.Sin(points[0] + points[1]) / points[0] + points[1] };
            }

            public override double[] derivative(double[] points)
            {
                return new double[2] { (Math.Cos(points[0] + points[1]) * (points[0] + points[1]) - Math.Sin(points[0] + points[1])) / (points[0] + points[1]),
                                   Math.Cos(points[0]) * (points[0] + points[1]) - Math.Sin(points[0] + points[1])  / (points[0] + points[1])};
            }

        }
    }
}
