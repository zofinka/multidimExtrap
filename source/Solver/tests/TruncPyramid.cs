using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solver
{
    namespace Tests
    {
        class TruncPyramid
        {
            public static string configFile = @"C:\Users\Sofya\multidimExtrap\source\test_data\4.TruncPyramidVolume\config.cfg";
            public static string pointFile = @"C: \Users\Sofya\multidimExtrap\source\test_data\4.TruncPyramidVolume\points.txt";
            public static string name = "Trunc Pyramid Volume 1/3 * x3 * (x1 + x2 + sqrt(x1*x2))";

            public static double func(double[] points)
            {
                double a = points[0] + points[1] + Math.Sqrt(points[0] * points[1]);
                double b = points[2] * a;
                double c = (1 / 3) * b;
                double v = b / 3;
                return points[2] * (points[0] + points[1] + Math.Sqrt(points[0] * points[1])) / 3;
            }
        }
    }
}
