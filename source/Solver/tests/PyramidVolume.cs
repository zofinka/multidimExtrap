using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solver
{
    namespace Tests
    {
        class PyramidVolume: Tests.IFunction
        {
            public string configFile { get { return @"..\..\..\test_data\3.PyramidVolume\config.cfg"; } }
            public string pointFile { get { return @"..\..\..\test_data\3.PyramidVolume\points.txt"; } }
            public string name { get { return "Pyramid volume "; } }

            public double func(double[] points)
            {
                double res = 1;
                for (int i = 0; i < points.Length - 1; i++)
                {
                    res *= points[i];
                }
                return res / 3;
            }

            public double[] derivative(double[] points)
            {
                double[] derivative = new double[points.Length - 1];
                for (int i = 0; i < points.Length - 1; i++)
                {
                    double res = 1;

                    for (int j = 0; j < points.Length - 1; j++)
                    {
                        if (i != j)
                        {
                            res *= points[j];
                        }
                    }
                    derivative[i] = res / 3;
                }
                return derivative;
            }
        }
    }
}
