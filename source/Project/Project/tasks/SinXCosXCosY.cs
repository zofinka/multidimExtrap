using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    class SzinXCosXCosY : BaseTask<SzinXCosXCosY>
    {
        public SzinXCosXCosY()
        {
            //configFile = @"C:\Users\Sofya\multidimExtrap\source\test_data\17.sinXcosYcosX\config.cfg";
            //pointFile = @"C: \Users\Sofya\multidimExtrap\source\test_data\17.sinXcosYcosX\points.txt";
            //Name = "SinXCosXCosY ";
    }

        public override double[] derivative(double[] points)
        {
            return new double[2] { Math.Cos(2 * points[0]) * Math.Cos(points[1]), -1 * Math.Sin(points[0]) * Math.Cos(points[0]) * Math.Sin(points[1]) };
        }

        // sin(x1)* cos(x1) * cos(x2)
        public override double[] func(double[] points)
        {
            double[] result = { Math.Sin(points[0]) * Math.Cos(points[0]) * Math.Cos(points[1]) };
            return result;
        }
    }
}
