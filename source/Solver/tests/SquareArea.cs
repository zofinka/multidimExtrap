using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solver
{
    namespace Tests
    {
        class SquareArea: Tests.IFunction
        {
            public string configFile { get { return @"C:\Users\Sofya\multidimExtrap\source\test_data\1.SquareArea\config.cfg"; } }
            public string pointFile { get { return @"C:\Users\Sofya\multidimExtrap\source\test_data\1.SquareArea\points.txt"; } }
            public string name { get { return "Square Area x1 * x2 * ... "; } }

            public double func(double[] points)
            {
                double res = 1;
                for (int i = 0; i < points.Length - 1; i++)
                {
                    res *= points[i];
                }
                return res;
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
                            res *= res;
                        }
                    }
                    derivative[i] = res;
                }
                return derivative;
            }
        }
        
    }
}
