using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solver
{
    class TestDefWay: Test
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
            //Tests.SquaresProducts SquaresProducts = new Tests.SquaresProducts();
            //Console.WriteLine("Test " + SquaresProducts.name + " START");
            //interAmount = test(SquaresProducts.configFile, SquaresProducts.pointFile, SquaresProducts);
            //Console.WriteLine("Test " + SquaresProducts.name + " END in " + interAmount + " iterations");

            // 212 iteration
            //Tests.SinXCosY SinXCosY = new Tests.SinXCosY();
            //Console.WriteLine("Test " + SinXCosY.name + " START");
            //interAmount = test(SinXCosY.configFile, SinXCosY.pointFile, SinXCosY);
            //Console.WriteLine("Test " + SinXCosY.name + " END in " + interAmount + " iterations");

            //133 iteration
            Tests.SinXCosXCosY SinXCosXCosY = new Tests.SinXCosXCosY();
            Console.WriteLine(SinXCosXCosY.name + " Test START");
            interAmount = test(SinXCosXCosY.configFile, SinXCosXCosY.pointFile, SinXCosXCosY);
            Console.WriteLine(SinXCosXCosY.name + " Test END in " + interAmount + " iterations");

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

            int i = 0;
            double maxErr = 100000000;
            int totalPointsCount = 0;
            while (i < 100000000 && maxErr > parser.Approximation)
            {
                Shepard model = new Shepard(parser.N_Dimension, points);
                Analyzer analyzer = new Analyzer(model, points);
                //analyzer.do_default_analyse();
                //analyzer.do_best_ever_analyse();
                analyzer.do_quicker_analyse();
                double[][] xx = analyzer.Result;

                int predictionPointAmount = Math.Min(parser.PredictionPointAmount, xx.Length);
                pointAmount = pointAmount + predictionPointAmount;
                points = getNewPoints(points, analyzer.Result, predictionPointAmount, parser.N_Dimension, func);


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

                    double err = (diffs.Sum() / diffs.Length);

                    Console.WriteLine(" \n " + err + " " + String.Join(", ", realFunctionVal) + " " + String.Join(", ", approxFunctionVal) + " \n ");
                    //if (err > tempErr)
                    //{
                    //    tempErr = err;
                    //}
                    totalErr += err;
                    Console.WriteLine("f({0}) real val {1} predict val {2} err {3}", String.Join(", ", xx[k]), String.Join(", ", realFunctionVal), String.Join(", ", approxFunctionVal), err);
                }
                //maxErr = tempErr;
                maxErr = totalErr / new_points.Length;
                i++;
                totalPointsCount = points.Length;

            }

            testResult(parser.N_Dimension, parser.M_Dimension, points, func);
            Console.WriteLine("Calc err avg " + calc_err(func, points.ToList(), parser));
            Console.WriteLine("{0} points was requested from the real function", totalPointsCount);

            return i;
        }


    }
}
