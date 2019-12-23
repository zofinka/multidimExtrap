using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solver
{
    class CalcOnGrid: Test
    {
        override public void run()
        {
            Console.WriteLine("TESTS: Default way\n");
            int interAmount;

            // 1 iteration on def algorithm 
            /*Console.WriteLine(Tests.SquareArea.name + " START \n");
            interAmount = testDefWay(Tests.SquareArea.config, Tests.SquareArea.pointFile, Tests.SquareArea);
            Console.WriteLine(Tests.SquareArea.name +  " END in " + interAmount + " iterations\n");*/

            // 1 interation with def algorithm 

            //Tests.PyramidVolume PyramidVolume = new Tests.PyramidVolume();
            //Console.WriteLine(PyramidVolume.name + " Test START");
            //interAmount = test(PyramidVolume.configFile, PyramidVolume.pointFile, PyramidVolume);
            //Console.WriteLine(PyramidVolume.name + " Test END in " + interAmount + " iterations");


            //Tests.TruncPyramid TruncPyramid = new Tests.TruncPyramid();
            //Console.WriteLine(TruncPyramid.name + " Test START");
            //interAmount = test(TruncPyramid.configFile, TruncPyramid.pointFile, TruncPyramid);
            //Console.WriteLine(TruncPyramid.name + " Test END in " + interAmount + " iterations");

            //126 interation
            Tests.SquaresProducts SquaresProducts = new Tests.SquaresProducts();
            Console.WriteLine("Test " + SquaresProducts.name + " START");
            interAmount = test(SquaresProducts.configFile, SquaresProducts.pointFile, SquaresProducts);
            Console.WriteLine("Test " + SquaresProducts.name + " END in " + interAmount + " iterations");

            // 212 iteration
            //Tests.SinXCosY SinXCosY = new Tests.SinXCosY();
            //Console.WriteLine("Test " + SinXCosY.name + " START");
            //interAmount = test(SinXCosY.configFile, SinXCosY.pointFile, SinXCosY);
            //Console.WriteLine("Test " + SinXCosY.name + " END in " + interAmount + " iterations");

            //133 iteration
            //Tests.SinXCosXCosY SinXCosXCosY = new Tests.SinXCosXCosY();
            //Console.WriteLine(SinXCosXCosY.name + " Test START");
            //interAmount = test(SinXCosXCosY.configFile, SinXCosXCosY.pointFile, SinXCosXCosY);
            //Console.WriteLine(SinXCosXCosY.name + " Test END in " + interAmount + " iterations");

            //14 interation
            //Tests.SinFromSumOnSum SinFromSumOnSum = new Tests.SinFromSumOnSum();
            //Console.WriteLine(SinFromSumOnSum.name + " Test START");
            //interAmount = test(SinFromSumOnSum.configFile, SinFromSumOnSum.pointFile, SinFromSumOnSum);
            //Console.WriteLine(SinFromSumOnSum.name + " Test END in " + interAmount + " iterations");

            // interation
            //Tests.SqrtXSqrtY SqrtXSqrtY = new Tests.SqrtXSqrtY();
            //Console.WriteLine(SqrtXSqrtY.name + " Test START");
            //interAmount = test(SqrtXSqrtY.configFile, SqrtXSqrtY.pointFile, SqrtXSqrtY);
            //Console.WriteLine(SqrtXSqrtY.name + " Test END in " + interAmount + " iterations");

            // interation
            //Tests.LGFunc LGFunc = new Tests.LGFunc();
            //Console.WriteLine(LGFunc.name + " Test START");
            //interAmount = test(LGFunc.configFile, LGFunc.pointFile, LGFunc, LGFunc.tableFile);
            //Console.WriteLine(LGFunc.name + " Test END in " + interAmount + " iterations");
        }

        private int test(string configFile, string pointFile, Tests.AFunction function, string tableFile = null)
        {
            Parser parser = new Parser(configFile, pointFile, tableFile);
            if (tableFile != null)
            {
                function.table = parser.getTable();
            }

            Func<double[], double[]> func = function.func;

            int pointAmount = parser.PointAmount;
            double[][] points = new double[parser.PointAmount][];
            for (int j = 0; j < parser.PointAmount; j++)
            {
                points[j] = (double[])parser.Points[j].Clone();

            }

            int ngrid = 9;
            Shepard model = new Shepard(parser.N_Dimension, points);

            //int[] count = new int[model.N]; for (int j = 0; j < model.N; j++) count[j] = (model.Min[j] == model.Max[j]) ? 1 : ngrid;
            //Grid grid_big = new Grid(model.N, model.M, model.Min, model.Max, count);
            points = new double[0][];
            pointAmount = 0;

            int i = 0;
            double maxErr = 100000000;
            int totalPointsCount = 0;
            List<double[]> excludes = new List<double[]>();
            //while (maxErr > parser.Approximation)
            double oldIntegAccuracy = 10000000;
            double IntegAccuracy = 10000000 - 100;
            //while (maxErr > parser.Approximation)


            //int ngrid_little =  System.Convert.ToInt32(ngrid * parser.Approximation);
            int ngrid_little_x = 14;
            int inc_parameter = 1;
            int ngrid_little_y = 15;
            //while (IntegAccuracy > parser.Approximation)
            while (ngrid_little_x < 21)
            {
                points = new double[0][];
                pointAmount = 0;
                List<double[]> results = new List<double[]>();

                int[] count = new int[model.N];
                for (int j = 0; j < model.N; j++)
                {
                    if (j == 0)
                    {
                        count[j] = (model.Min[j] == model.Max[j]) ? 1 :  ngrid_little_x;
                    }
                    if (j == 1)
                    {
                        count[j] = (model.Min[j] == model.Max[j]) ? 1 : ngrid_little_y;
                    }
                }
                Grid grid_little = new Grid(model.N, model.M, model.Min, model.Max, count);

                for (int j = 0; j < grid_little.Node.Length; j+=1)
                {
                    results.Add(grid_little.Node[j]);
                    //Console.WriteLine("{0} {1}", String.Join(" ", grid_little.Node[j]), String.Join(" ", function.func(grid_little.Node[j])));
                    //Console.WriteLine("{0}", String.Join(" ", function.func(grid_little.Node[j])));

                }

                double[][] xx = results.ToArray();

                int predictionPointAmount = xx.Length;
                pointAmount = pointAmount + predictionPointAmount;
                points = getNewPoints(points, results.ToArray(), predictionPointAmount, parser, func);


                double[][] new_points = new double[predictionPointAmount][];
                for (int j = 0; j < predictionPointAmount; j++)
                {
                    new_points[j] = new double[xx[j].Length];
                    Array.Copy(xx[j], new_points[j], xx[j].Length);
                    model.Calculate(new_points[j]);
                }

                //Нужно не максимальную ошибку считать, а среднюю.
                double tempErr = 0;
                double totalErr = 0.0;

                for (int k = 0; k < new_points.Length; k++)
                {
                    var realPoint = points[pointAmount - predictionPointAmount + k];
                    double[] realFunctionVal = new double[parser.M_Dimension];

                    for (int l = 0; l < parser.M_Dimension; ++l)
                    {
                        realFunctionVal[l] = realPoint[parser.N_Dimension + l];
                    }

                    var approxPoint = new_points[k];
                    double[] approxFunctionVal = new double[parser.M_Dimension];

                    for (int l = 0; l < parser.M_Dimension; ++l)
                    {
                        approxFunctionVal[l] = approxPoint[parser.N_Dimension + l];
                    }

                    double[] diffs = realFunctionVal.Zip(approxFunctionVal, (d1, d2) => Math.Abs(d1 - d2)).ToArray();

                    //double err = diffs.Max();
                    double err = (diffs.Sum() / diffs.Length);
                    //Console.WriteLine(" \n " + err + " " + String.Join(", ", realFunctionVal) + " " + String.Join(", ", approxFunctionVal) + " \n ");
                    if (err > tempErr)
                    {
                        totalErr = err;
                    }
                    // totalErr += err;
                    //Console.WriteLine("f({0}) real val {1} predict val {2} err {3}", String.Join(", ", xx[k]), String.Join(", ", realFunctionVal), String.Join(", ", approxFunctionVal), err);
                    //Console.WriteLine("{0};{1};{2};{3}", String.Join(", ", xx[k]), String.Join(", ", realFunctionVal));
                    //Console.WriteLine("{0};{1}", String.Join(";", xx[k]), String.Join(";", realFunctionVal));

                }
                maxErr = totalErr;
                //maxErr = totalErr / new_points.Length;
                i++;
                totalPointsCount = points.Length;
                oldIntegAccuracy = IntegAccuracy;
                IntegAccuracy = calc_err(func, points.ToList(), parser);
                Console.WriteLine("Accuracy {0}", IntegAccuracy);
                Console.WriteLine("ngrid little count {0}", ngrid_little_x * ngrid_little_y);
                ngrid_little_x += inc_parameter;
                
            }

            testResult(parser.N_Dimension, parser.M_Dimension, points, func);
            Console.WriteLine("Calc err avg " + calc_err(func, points.ToList(), parser));
            Console.WriteLine("{0} points was requested from the real function", totalPointsCount);

            Console.WriteLine("Points amount in grid {0}", ngrid - 1);
            Console.WriteLine("Ngrid count needed {0}", (ngrid_little_x - inc_parameter) * ngrid_little_y);

            return i;
        }
    }
}
