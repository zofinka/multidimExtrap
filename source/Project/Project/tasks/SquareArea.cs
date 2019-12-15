using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    class SquareArea : BaseTask<SquareArea>
    {
        public SquareArea()
        {
            //configFile = @"C:\Users\Sofya\multidimExtrap\source\test_data\1.SquareArea\config.cfg";
            //pointFile = @"C:\Users\Sofya\multidimExtrap\source\test_data\1.SquareArea\points.txt";
            //Name = "Square Area x1 * x2 * ... ";
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

        public override double[] func(double[] points)
        {
            double[] res = { 1 };
            for (int i = 0; i < points.Length - 1; i++)
            {
                res[0] *= points[i];
            }
            return res;
        }
    }
}
