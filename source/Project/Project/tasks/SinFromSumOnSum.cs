using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    class SinFromSumOnSum : BaseTask<SinFromSumOnSum>
    {
        public SinFromSumOnSum()
        {
            //configFile = @"C:\Users\Sofya\multidimExtrap\source\test_data\16.sin(x+y)on(x+y)\config.cfg";
            //pointFile = @"C: \Users\Sofya\multidimExtrap\source\test_data\16.sin(x+y)on(x+y)\points.txt";
            //Name = "SinFromSumOnSum sin(x1 + x2) / (x1 + x2)";
        }

        public override double[] derivative(double[] points)
        {
            return new double[2] { (Math.Cos(points[0] + points[1]) * (points[0] + points[1]) - Math.Sin(points[0] + points[1])) / (points[0] + points[1]),
                                   Math.Cos(points[0]) * (points[0] + points[1]) - Math.Sin(points[0] + points[1])  / (points[0] + points[1])};
        }

        // sin(x + y) / x + y
        public override double[] func(double[] points)
        {
            double[] result = { Math.Sin(points[0] + points[1]) / points[0] + points[1] };
            return result;
        }
    }
}
