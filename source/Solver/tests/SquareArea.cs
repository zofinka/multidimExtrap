using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solver
{
    namespace Tests
    {
        class SquareArea : Tests.AFunction
        {
            public override string configFile { get { return @"..\..\..\test_data\1.SquareArea\config.cfg"; } }
            public override string pointFile { get { return @"..\..\..\test_data\1.SquareArea\points.txt"; } }
            public override string name { get { return "Square Area x1 * x2 * ... "; } }

            public override double[] func(double[] points)
            {
                double res = 1;
                for (int i = 0; i < points.Length - 1; i++)
                {
                    res *= points[i];
                }
                return new double[1] { res };
            }

            public override double[] derivative(double[] points)
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
