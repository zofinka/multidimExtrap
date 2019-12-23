using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solver
{
    namespace Tests
    {
        class SquaresProducts : Tests.AFunction
        {
            public override string configFile { get { return @"..\..\..\test_data\11.SquaresProducts\config.cfg"; } }
            public override string pointFile { get { return @"..\..\..\test_data\11.SquaresProducts\points.txt"; } }
            public override string name { get { return "SquaresProducts x1^2 * x2^2 * ..."; } }

            // x^2*y^2 + 2
            public override double[] func(double[] points)
            {
                double res = 1;
                for (int i = 0; i < points.Length - 1; i++)
                {
                    res *= Math.Pow(points[i], 2);
                }
                return new double[1] { res + 2 };
            }


            public override double[] derivative(double[] points)
            {
                double[] derivative = new double[points.Length - 1];
                for (int i = 0; i < points.Length - 1; i++)
                {
                    double res = points[i] * 2;

                    for (int j = 0; j < points.Length - 1; j++)
                    {
                        if (i != j)
                        {
                            res *= Math.Pow(points[i], 2);
                        }
                    }
                    derivative[i] = res;
                }
                return derivative;
            }
        }
    }
}
