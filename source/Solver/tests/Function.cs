using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solver
{
    namespace Tests
    {
        abstract class Function
        {
            public static string configFile;
            public static string pointFile;
            public static string name;

            public static double func(double[] points)
            {
                return 0;
            }

            public static double[] derivative(double[] points)
            {
                return new double[1] { 0 };
            }
        }
    }
}
