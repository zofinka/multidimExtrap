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
            interAmount = testDefWay(Tests.SquareArea.config, Tests.SquareArea.pointFile, Tests.SquareArea.func);
            Console.WriteLine(Tests.SquareArea.name +  " END in " + interAmount + " iterations\n");*/

            // 1 interation with def algorithm 
            /*Tests.PyramidVolume PyramidVolume  = new Tests.PyramidVolume();
            Console.WriteLine(PyramidVolume.name + " Test START");
            interAmount = test(PyramidVolume.configFile, PyramidVolume.pointFile, PyramidVolume.func);
            Console.WriteLine(PyramidVolume.name + " Test END in " + interAmount + " iterations");*/

            //Tests.TruncPyramid TruncPyramid = new Tests.TruncPyramid();
            //Console.WriteLine(TruncPyramid.name + " Test START");
            //interAmount = test(TruncPyramid.configFile, TruncPyramid.pointFile, TruncPyramid.func);
            //Console.WriteLine(TruncPyramid.name + " Test END in " + interAmount + " iterations");

            //126 interation
            /* Tests.SquaresProducts SquaresProducts = new Tests.SquaresProducts();
             Console.WriteLine("Test " + SquaresProducts.name + " START");
             interAmount = test(SquaresProducts.configFile, SquaresProducts.pointFile, SquaresProducts.func);
             Console.WriteLine("Test " + SquaresProducts.name + " END in " + interAmount + " iterations");*/

            // 212 iteration
            /*Tests.SinXCosY SinXCosY = new Tests.SinXCosY();
            Console.WriteLine("Test " + SinXCosY.name + " START");
            interAmount = test(SinXCosY.configFile, SinXCosY.pointFile, SinXCosY.func);
            Console.WriteLine("Test " + SinXCosY.name + " END in " + interAmount + " iterations");*/

            //133 iteration
            /*Tests.SinXCosXCosY SinXCosXCosY = new Tests.SinXCosXCosY();
            Console.WriteLine(SinXCosXCosY.name + " Test START");
            interAmount = test(SinXCosXCosY.configFile, SinXCosXCosY.pointFile, SinXCosXCosY.func);
            Console.WriteLine(SinXCosXCosY.name + " Test END in " + interAmount + " iterations");*/

            //14 interation
            /*Tests.SinFromSumOnSum SinFromSumOnSum = new Tests.SinFromSumOnSum();
            Console.WriteLine(SinFromSumOnSum.name + " Test START");
            interAmount = test(SinFromSumOnSum.configFile, SinFromSumOnSum.pointFile, SinFromSumOnSum.func);
            Console.WriteLine(SinFromSumOnSum.name + " Test END in " + interAmount + " iterations");*/

            // interation
            /*Tests.SqrtXSqrtY SqrtXSqrtY = new Tests.SqrtXSqrtY();
            Console.WriteLine(SqrtXSqrtY.name + " Test START");
            interAmount = test(SqrtXSqrtY.configFile, SqrtXSqrtY.pointFile, SqrtXSqrtY.func);
            Console.WriteLine(SqrtXSqrtY.name + " Test END in " + interAmount + " iterations");*/

            // interation
            Tests.LnXY3 LnXY3 = new Tests.LnXY3();
            Console.WriteLine(LnXY3.name + " Test START");
            interAmount = test(LnXY3.configFile, LnXY3.pointFile, LnXY3.func);
            Console.WriteLine(LnXY3.name + " Test END in " + interAmount + " iterations");
        }

        private int test(string configFile, string pointFile, Func<double[], double> func)
        {
            Parser parser = new Parser(configFile, pointFile);
            int pointAmount = parser.PointAmount;
            double[][] points = new double[parser.PointAmount][];
            for (int j = 0; j < parser.PointAmount; j++)
            {
                points[j] = (double[])parser.Points[j].Clone();

            }

            int i = 0;
            double maxErr = 10;
            int totalPointsCount = 0;
            while (i < 100000000 && maxErr > parser.Approximation)
            {
                Shepard model = new Shepard(parser.FunctionDimension, points);
                Analyzer analyzer = new Analyzer(model, points);
                //analyzer.do_default_analyse();
                //analyzer.do_best_ever_analyse();
                analyzer.do_quicker_analyse();
                double[][] xx = analyzer.Result;

                int predictionPointAmount = Math.Min(parser.PredictionPointAmount, xx.Length);
                pointAmount = pointAmount + predictionPointAmount;
                points = getNewPoints(points, analyzer.Result, predictionPointAmount, parser.FunctionDimension, func);


                double[][] new_points = new double[predictionPointAmount][];
                for (int j = 0; j < predictionPointAmount; j++)
                {
                    new_points[j] = new double[xx[j].Length];
                    Array.Copy(xx[j], new_points[j], xx[j].Length);
                    model.Calculate(new_points[j]);
                }

                //Нужно не максимальную ошибку считать, а среднюю.
                double tempErr = 0;
                for (int k = 0; k < new_points.Length; k++)
                {
                    double err = Math.Abs(points[pointAmount - predictionPointAmount + k][parser.FunctionDimension] - new_points[k][parser.FunctionDimension]);
                    //Console.WriteLine(" \n " + (points[pointAmount - predictionPointAmount + k][parser.FunctionDimension] - new_points[k][parser.FunctionDimension]) + " " + points[pointAmount - predictionPointAmount + k][parser.FunctionDimension] + " " + new_points[k][parser.FunctionDimension] + " \n ");
                    if (err > tempErr)
                    {
                        tempErr = err;
                    }
                    //sConsole.WriteLine("f({0}) real val {1} predict val {2} err {3}", String.Join(", ", xx[k]), points[pointAmount - predictionPointAmount + k][parser.FunctionDimension], new_points[k][parser.FunctionDimension], err);
                }
                maxErr = tempErr;
                i++;
                totalPointsCount = points.Length;

            }
            testResult(parser.FunctionDimension, points, func);
            Console.WriteLine("Calc err avg " + calc_err(func, points.ToList(), parser));
            Console.WriteLine("{0} points was requested from the real function", totalPointsCount);

            return i;
        }


    }
}
