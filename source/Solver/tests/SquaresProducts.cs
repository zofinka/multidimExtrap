using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solver
{
    namespace Tests
    {
        class SquaresProducts
        {
            public static string configFile = @"C:\Users\Sofya\multidimExtrap\source\test_data\11.SquaresProducts\config.cfg";
            public static string pointFile = @"C: \Users\Sofya\multidimExtrap\source\test_data\11.SquaresProducts\points.txt";
            public static string name = "SquaresProducts x1^2 * x2^2 * ..."; 

            // x^2*y^2 + 2
            public static double func(double[] points)
            {
                double res = 1;
                for (int i = 0; i < points.Length - 1; i++)
                {
                    res *= Math.Pow(points[i], 2);
                }
                return res + 2;
            }

            public static double[] derivative(double[] points)
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
