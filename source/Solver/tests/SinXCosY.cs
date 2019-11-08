using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solver
{
    namespace Tests
    {
        class SinXCosY: Tests.IFunction
        {
            public string configFile { get { return @"C:\Users\apronina\Syncplicity\Study\multidimExtrap\source\test_data\12.sincos\config.cfg"; } }
            public string pointFile { get { return @"C:\Users\apronina\Syncplicity\Study\multidimExtrap\source\test_data\12.sincos\points.txt"; } }
            public string name { get { return "SinCon sin(x1) * con(x2) "; } }

            public double func(double[] points)
            {
                return Math.Sin(points[0]) * Math.Cos(points[1]);
            }

            public double[] derivative(double[] points)
            {
                return new double[2] { Math.Cos(points[0]) * Math.Cos(points[1]), -1 * Math.Sin(points[0]) * Math.Sin(points[1]) };
            }

        }
    }
}
