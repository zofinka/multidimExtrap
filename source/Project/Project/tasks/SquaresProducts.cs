using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    class SquaresProducts : BaseTask<SquaresProducts>
    {
        public SquaresProducts()
        {
            //configFile = @"C:\Users\Sofya\multidimExtrap\source\test_data\11.SquaresProducts\config.cfg";
            //pointFile = @"C: \Users\Sofya\multidimExtrap\source\test_data\11.SquaresProducts\points.txt";
            //Name = "SquaresProducts x1^2 * x2^2 * ..."; 
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

        // x^2*y^2 + 2
        public override double[] func(double[] points)
        {
            double[] res = { 1 };
            for (int i = 0; i < points.Length - 1; i++)
            {
                res[0] *= Math.Pow(points[i], 2);
            }
            res[0] += 2;
            return res;
        }
    }
}
