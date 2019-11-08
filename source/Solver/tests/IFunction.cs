using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solver
{
    namespace Tests
    {
        interface IFunction
        {
            string configFile { get; }
            string pointFile { get; }
            string name { get; }

            double func(double[] points);
            double[] derivative(double[] points);
        }
    }
}
