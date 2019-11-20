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
            public override string name { get { return "20.LGFunc "; } }

            public override double[] func(double[] points)
            {
                if (table == null)
                {
                    throw new System.Exception("this func requires a table!");
                }

                var reqPoint = new double[] { points[0] };
                var knownPoints = table.Keys;

                double[] candidate = null;
                double minDiff = 0.1;
                foreach (var kPoint in knownPoints)
                {
                    var diff = kPoint.Zip(reqPoint, (d1, d2) => Math.Abs(d1 - d2)).ToArray()[0];
                    if (diff < minDiff)
                    {
                        minDiff = diff;
                        candidate = kPoint;
                    }
                }

                return candidate == null ? null : table[candidate];
            }

            public override double[] derivative(double[] points)
            {
                throw new NotImplementedException("LGFunc derivative is not implemented," +
                                                  "as LGFunc is dicrete function.");
            }
        }
    }
}
