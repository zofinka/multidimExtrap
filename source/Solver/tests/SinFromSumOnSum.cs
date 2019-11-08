using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solver
{
    namespace Tests
    {
        class SinFromSumOnSum : Tests.IFunction
        {
            public string configFile { get { return @"C:\Users\apronina\Syncplicity\Study\multidimExtrap\source\test_data\16.sin(x+y)on(x+y)\config.cfg"; } }
            public string pointFile { get { return @"C:\Users\apronina\Syncplicity\Study\multidimExtrap\source\test_data\16.sin(x+y)on(x+y)\points.txt"; } }
            public string name { get { return "SinFromSumOnSum sin(x1 + x2) / (x1 + x2)"; }}

            // sin(x + y) / x + y
            public double func(double[] points)
            {
                return Math.Sin(points[0] + points[1]) / points[0] + points[1];
            }

            public double[] derivative(double[] points)
            {
                return new double[2] { (Math.Cos(points[0] + points[1]) * (points[0] + points[1]) - Math.Sin(points[0] + points[1])) / (points[0] + points[1]),
                                   Math.Cos(points[0]) * (points[0] + points[1]) - Math.Sin(points[0] + points[1])  / (points[0] + points[1])};
            }

        }
    }
}
