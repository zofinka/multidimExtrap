using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solver
{
    namespace Tests
    {
        class LnXY3
        {
            public string configFile { get { return @"..\..\..\test_data\19.log2(x)y^3\config.cfg"; } }
            public string pointFile { get { return @"..\..\..\test_data\19.log2(x)y^3\points.txt"; } }
            public string name { get { return "SinCon sin(x1) * con(x2) "; } }

            public double func(double[] points)
            {
                return Math.Log10(points[0]) * Math.Pow(points[1], 3);
            }

            public double[] derivative(double[] points)
            {
                return new double[2] { 0, 0 };
            }
        }
    }

    
}
