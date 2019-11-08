using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solver
{
    class TestRandomForestLearnAndDoOnOne: Test
    {
        public override void run()
        {
            int interAmount;

            /*Console.WriteLine("Square Area Function Test START");
            interAmount = testWithRandomForest(Tests.SquareArea.config, Tests.SquareArea.config, Tests.SquareArea.func, Tests.SquareArea.derivative);
            Console.WriteLine("Square Area Function Test END in " + interAmount + " iterations");*/

            /*Console.WriteLine("Test " + Tests.SquaresProducts.name + " START");
            interAmount = testWithRandomForest(Tests.SquaresProducts.configFile, Tests.SquaresProducts.pointFile, Tests.SquaresProducts.func, Tests.SquaresProducts.derivative);
            Console.WriteLine("Test " + Tests.SquaresProducts.name + " END in " + interAmount + " iterations");*/

            Tests.SinXCosY SinXCosY = new Tests.SinXCosY();
            Console.WriteLine("Test " + SinXCosY.name + " START");
            interAmount = test(SinXCosY.configFile, SinXCosY.pointFile, SinXCosY.func, SinXCosY.derivative);
            Console.WriteLine("Test " + SinXCosY.name + " END in " + interAmount + " iterations");

            /*Console.WriteLine(Tests.SinXCosXCosY.name +  " Test START");
            interAmount = testWithRandomForest(Tests.SinXCosXCosY.configFile, Tests.SinXCosXCosY.pointFile, Tests.SinXCosXCosY.func, Tests.SinXCosXCosY.derivative);
            Console.WriteLine("17.sinXconYxonX Test END in " + interAmount + " iterations");*/

            /*Console.WriteLine(Tests.SinFromSumOnSum.name + " Test START");
            interAmount = testWithRandomForest(Tests.SinFromSumOnSum.configFile, Tests.SinFromSumOnSum.pointFile, Tests.SinFromSumOnSum.func, Tests.SinFromSumOnSum.derivative);
            Console.WriteLine(Tests.SinFromSumOnSum.name + " Test END in " + interAmount + " iterations");*/
        }

        private int test(string configFile, string pointFile, Func<double[], double> func, Func<double[], double[]> derivativeFunc)
        {
            Parser parser = new Parser(configFile, pointFile);
            List<double[]> points = new List<double[]>();
            points = parser.Points.ToList();

            for (int j = 0; j < 1; j++)
            {
                points.AddRange(testDefWayUntilGood(parser, points.ToArray(), func).ToList<double[]>());
            }


            int pointAmount = points.Count;

            Shepard model = new Shepard(parser.FunctionDimension, points.ToArray());
            Analyzer analyzer = new Analyzer(model, points.ToArray());
            Classifiers.IClassifier cls = analyzer.learn_random_forest_on_grid(func, derivativeFunc, parser.Approximation);

            double[][] pointsArray = new double[parser.PointAmount][];
            pointAmount = pointsArray.Length;
            for (int j = 0; j < parser.PointAmount; j++)
            {
                pointsArray[j] = (double[])parser.Points[j].Clone();
            }

            int i = 0;
            double maxErr = 10;
            while (i < 100000000 && maxErr > parser.Approximation)
            {
                model = new Shepard(parser.FunctionDimension, pointsArray);
                analyzer = new Analyzer(model, pointsArray);
                analyzer.do_random_forest_analyse(cls, parser.Approximation, func, derivativeFunc);

                double[][] xx = analyzer.Result;
                int newPointsAmount = Math.Min(parser.PredictionPointAmount, xx.Length);
                pointAmount = pointAmount + newPointsAmount;
                double[][] newPoints = new double[pointAmount][];
                for (int j = 0; j < pointAmount; j++)
                {
                    if (j < pointAmount - newPointsAmount)
                    {
                        newPoints[j] = (double[])pointsArray[j].Clone();
                    }
                    else
                    {
                        newPoints[j] = (double[])xx[j - pointAmount + newPointsAmount].Clone();
                        newPoints[j][parser.FunctionDimension] = func(newPoints[j]);
                    }
                }
                pointsArray = newPoints;


                double[][] new_points = new double[newPointsAmount][];
                for (int j = 0; j < newPointsAmount; j++)
                {
                    new_points[j] = new double[xx[j].Length];
                    Array.Copy(xx[j], new_points[j], xx[j].Length);
                    model.Calculate(new_points[j]);
                }

                double tempErr = 0;
                for (int k = 0; k < new_points.Length; k++)
                {
                    double err = Math.Abs(pointsArray[pointAmount - newPointsAmount + k][parser.FunctionDimension] - new_points[k][parser.FunctionDimension]);
                    Console.WriteLine(" \n " + (pointsArray[pointAmount - newPointsAmount + k][parser.FunctionDimension] - new_points[k][parser.FunctionDimension]) + " " + pointsArray[pointAmount - newPointsAmount + k][parser.FunctionDimension] + " " + new_points[k][parser.FunctionDimension] + " \n ");
                    if (err > tempErr)
                    {
                        tempErr = err;
                    }
                    Console.WriteLine("f({0}) real val {1} predict val {2} err {3}", String.Join(", ", xx[k]), pointsArray[pointAmount - newPointsAmount + k][parser.FunctionDimension], new_points[k][parser.FunctionDimension], err);
                }
                maxErr = tempErr;
                i++;
            }
            testResult(parser.FunctionDimension, pointsArray, func);
            Console.WriteLine(" Avg err " + calc_err(func, points.ToList(), parser));
            return i;
        }

    }
}
