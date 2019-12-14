using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    class SinXCosY : BaseTask<SinXCosY>
    {
        public SinXCosY()
        {
            configFile = @"C:\Users\Sofya\multidimExtrap\source\test_data\12.sincos\config.cfg";
            pointFile = @"C: \Users\Sofya\multidimExtrap\source\test_data\12.sincos\points.txt";
            Name = "SinCon sin(x1) * con(x2) ";
        }

        public override double[] derivative(double[] points)
        {
            return new double[2] { Math.Cos(points[0]) * Math.Cos(points[1]), -1 * Math.Sin(points[0]) * Math.Sin(points[1]) };
        }

        public override double func(double[] points)
        {
            return Math.Sin(points[0]) * Math.Cos(points[1]);
        }
    }
}
