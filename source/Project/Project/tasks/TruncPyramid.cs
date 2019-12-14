using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    class TruncPyramid : BaseTask<TruncPyramid>
    {
        public TruncPyramid()
        {
            configFile = @"C:\Users\Sofya\multidimExtrap\source\test_data\4.TruncPyramidVolume\config.cfg";
            pointFile = @"C: \Users\Sofya\multidimExtrap\source\test_data\4.TruncPyramidVolume\points.txt";
            Name = "Trunc Pyramid Volume 1/3 * x3 * (x1 + x2 + sqrt(x1*x2))";
        }

        public override double[] derivative(double[] points)
        {
            throw new NotImplementedException();
        }

        public override double func(double[] points)
        {
            double a = points[0] + points[1] + Math.Sqrt(points[0] * points[1]);
            double b = points[2] * a;
            double c = (1 / 3) * b;
            double v = b / 3;
            return points[2] * (points[0] + points[1] + Math.Sqrt(points[0] * points[1])) / 3;
        }
    }
}
