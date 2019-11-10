using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solver
{
    class TestDichotomy: Test
    {
        override public void run()
        {
            Console.WriteLine("TESTS: Dichotomy way\n");
            int interAmount;

            // 1 iteration on def algorithm 
            /*Console.WriteLine(Tests.SquareArea.name + " START \n");
            interAmount = testDefWay(Tests.SquareArea.config, Tests.SquareArea.pointFile, Tests.SquareArea.func);
            Console.WriteLine(Tests.SquareArea.name +  " END in " + interAmount + " iterations\n");*/

            // 1 interation with def algorithm 
            /*Console.WriteLine(Tests.PyramidVolume.name + " Test START");
            interAmount = testDefWay(Tests.PyramidVolume.config, Tests.PyramidVolume.pointFile, Tests.PyramidVolume.func);
            Console.WriteLine(Tests.PyramidVolume.name  + " Test END in " + interAmount + " iterations");*/

            /*Console.WriteLine(Tests.TruncPyramid.name + " Test START");
            interAmount = testDefWay(Tests.TruncPyramid.configFile, Tests.TruncPyramid.pointFile, Tests.TruncPyramid.func);
            Console.WriteLine(Tests.TruncPyramid.name  + " Test END in " + interAmount + " iterations");*/

            //126 interation
            /*Console.WriteLine("Test " + Tests.SquaresProducts.name + " START");
             interAmount = testDefWay(Tests.SquaresProducts.configFile, Tests.SquaresProducts.pointFile, Tests.SquaresProducts.func);
            Console.WriteLine("Test " + Tests.SquaresProducts.name + " END in " + interAmount + " iterations");*/

            // 212 iteration
            /*Console.WriteLine("Test " + Tests.SinXCosY.name + " START");
            interAmount = testDefWay(Tests.SinXCosY.configFile, Tests.SinXCosY.pointFile, Tests.SinXCosY.func);
            Console.WriteLine("Test " + Tests.SinXCosY.name + " END in " + interAmount + " iterations");*/

            //133 iteration
            /*Console.WriteLine(Tests.SinXCosXCosY.name  + " Test START");
            interAmount = testDefWay(Tests.SinXCosXCosY.configFile, Tests.SinXCosXCosY.pointFile, Tests.SinXCosXCosY.func);
            Console.WriteLine(Tests.SinXCosXCosY.name  + " Test END in " + interAmount + " iterations");*/

            //14 interation
            Tests.SinFromSumOnSum SinFromSumOnSum = new Tests.SinFromSumOnSum();
            Console.WriteLine(SinFromSumOnSum.name + " Test START");
            interAmount = test(SinFromSumOnSum.configFile, SinFromSumOnSum.pointFile, SinFromSumOnSum.func);
            Console.WriteLine(SinFromSumOnSum.name + " Test END in " + interAmount + " iterations");
        }

        private int test(string configFile, string pointFile, Func<double[], double> func)
        {
            int predictionPointAmount = 0;

            Parser parser = new Parser(configFile, pointFile);
            int pointAmount = parser.PointAmount;
            double[][] points = new double[parser.PointAmount][];
            for (int j = 0; j < parser.PointAmount; j++)
            {
                points[j] = (double[])parser.Points[j].Clone();

            }

            int i = 0;
            double maxErr = 10;
            while (i < 100000000 && maxErr > parser.Approximation)
            {
                Shepard model = new Shepard(parser.FunctionDimension, points);
                Console.WriteLine("Max " + String.Join(", ", model.Max) + " Min " + String.Join(", ", model.Min));
                Analyzer analyzer = new Analyzer(model, points);
                analyzer.do_dichotomy_analyse();

                double[][] xx = analyzer.Result;
                predictionPointAmount = xx.Length;
                pointAmount = pointAmount + predictionPointAmount;
                points = getNewPoints(points, analyzer.Result, predictionPointAmount, parser.FunctionDimension, func);

                double[][] new_points = new double[predictionPointAmount][];
                for (int j = 0; j < predictionPointAmount; j++)
                {
                    new_points[j] = new double[xx[j].Length];
                    Array.Copy(xx[j], new_points[j], xx[j].Length);
                    model.Calculate(new_points[j]);
                }

                double tempErr = 0;
                for (int k = 0; k < new_points.Length; k++)
                {
                    double err = Math.Abs(points[pointAmount - predictionPointAmount + k][parser.FunctionDimension] - new_points[k][parser.FunctionDimension]);
                    Console.WriteLine(" \n " + (points[pointAmount - predictionPointAmount + k][parser.FunctionDimension] - new_points[k][parser.FunctionDimension]) + " " + points[pointAmount - predictionPointAmount + k][parser.FunctionDimension] + " " + new_points[k][parser.FunctionDimension] + " \n ");
                    if (err > tempErr)
                    {
                        tempErr = err;
                    }
                    Console.WriteLine("f({0}) real val {1} predict val {2} err {3}", String.Join(", ", xx[k]), points[pointAmount - predictionPointAmount + k][parser.FunctionDimension], new_points[k][parser.FunctionDimension], err);
                }
                maxErr = tempErr;
                i++;

            }
            testResult(parser.FunctionDimension, points, func);
            Console.WriteLine("Calc err avg " + calc_err(func, points.ToList(), parser));

            return i;
        }


    }
}
