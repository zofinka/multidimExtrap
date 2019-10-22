using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solver
{
    namespace Tests
    {
        class SinXCosXCosY
        {
            public static string configFile = @"C:\Users\Sofya\multidimExtrap\source\test_data\17.sinXcosYcosX\config.cfg";
            public static string pointFile = @"C: \Users\Sofya\multidimExtrap\source\test_data\17.sinXcosYcosX\points.txt";
            public static string name = "SinXCosXCosY ";

            // sin(x1)* cos(x1) * cos(x2)
            public static double func(double[] points)
            {
                return Math.Sin(points[0]) * Math.Cos(points[0]) * Math.Cos(points[1]);
            }

            public static double[] derivative(double[] points)
            {
                return new double[2] { Math.Cos(2 * points[0]) * Math.Cos(points[1]), -1 * Math.Sin(points[0]) * Math.Cos(points[0]) * Math.Sin(points[1]) };
            }
        }
    }
}
