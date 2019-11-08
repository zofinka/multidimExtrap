using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solver
{
    namespace Tests
    {
        class SinXCosY
        {
            public static string configFile = @"C:\Users\apronina\Syncplicity\Study\multidimExtrap\source\test_data\12.sincos\config.cfg";
            public static string pointFile = @"C:\Users\apronina\Syncplicity\Study\multidimExtrap\source\test_data\12.sincos\points.txt";
            public static string name = "SinCon sin(x1) * con(x2) ";
            public static double func(double[] points)
            {
                return Math.Sin(points[0]) * Math.Cos(points[1]);
            }

            public static double[] derivative(double[] points)
            {
                return new double[2] { Math.Cos(points[0]) * Math.Cos(points[1]), -1 * Math.Sin(points[0]) * Math.Sin(points[1]) };
            }

        }
    }
}
