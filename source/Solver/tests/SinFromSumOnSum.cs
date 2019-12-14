using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solver
{
    namespace Tests
    {
        class SinFromSumOnSum
        {
            public static string configFile = @"C:\Users\Sofya\multidimExtrap\source\test_data\16.sin(x+y)on(x+y)\config.cfg";
            public static string pointFile = @"C: \Users\Sofya\multidimExtrap\source\test_data\16.sin(x+y)on(x+y)\points.txt";
            public static string name = "SinFromSumOnSum sin(x1 + x2) / (x1 + x2)";

            // sin(x + y) / x + y
            public static double func(double[] points)
            {
                return Math.Sin(points[0] + points[1]) / points[0] + points[1];
            }

            public static double[] derivative(double[] points)
            {
                return new double[2] { (Math.Cos(points[0] + points[1]) * (points[0] + points[1]) - Math.Sin(points[0] + points[1])) / (points[0] + points[1]),
                                   Math.Cos(points[0]) * (points[0] + points[1]) - Math.Sin(points[0] + points[1])  / (points[0] + points[1])};
            }

        }
    }
}
