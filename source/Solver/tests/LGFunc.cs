using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Solver
{
    namespace Tests
    {
        class LGFunc : Tests.AFunction
        {
            public override string configFile { get { return @"..\..\..\test_data\20.LGFunc\config.cfg"; } }
            public override string pointFile { get { return @"..\..\..\test_data\20.LGFunc\points.txt"; } }
            public override string tableFile { get { return @"..\..\..\test_data\20.LGFunc\table.txt"; } }
            public override string name { get { return "18.sqrtXsqrtY "; } }

            public override  double[] func(double[] points)
            {
                if (points.Length != 1)
                {
                    throw new System.ArgumentException("point shall have 1 argument");
                }

                if (table == null)
                {
                    throw new System.Exception("this func requires a table!");
                }
                return table[points];
            }

            public override double[] derivative(double[] points)
            {
                throw new NotImplementedException("LGFunc derivative is not implemented," +
                                                  "as LGFunc is dicrete function.");
            }
        }
    }
}
