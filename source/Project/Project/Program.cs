using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    class Program
    {
        public static string configFile = @"d:\Study\Archive\project\config.cfg";
        public static string pointFile = @"d:\Study\Archive\project\points.txt";
        static void Main(string[] args)
        {
            //Console.WriteLine(Tests.SinXCosXCosY.name + " Test START");

            IParser parser = new Parser();
            IConfig config = parser.parseConfig(configFile);
            Task task = parser.parseTask(pointFile, config);

            ISolver solver = new Solver();
            solver.setConfig(config);
            
            int i = 0;
            double maxErr = 10;
            while (i < 100000000 && maxErr > config.Approximation)
            {
                maxErr = solver.calculate(task);
                i++;
            }

            solver.testResult(Solver.func);
            Console.WriteLine("sinXconYxonX Test END in " + i + " iterations");
            Console.ReadKey();
            //    Shepard model = new Shepard(parser.FunctionDimension, parser.Points);
            //Analyzer analyzer = new Analyzer(model, parser.Points);
            //Classifiers.IClassifier cls = analyzer.learn_random_forest_on_grid(func, derivativeFunc, parser.Approximation);

            //double[][] points = new double[parser.PointAmount][];
            //for (int j = 0; j < parser.PointAmount; j++)
            //{
            //    points[j] = (double[])parser.Points[j].Clone();
            //}

            //int i = 0;
            //double maxErr = 10;
            //while (i < 100000000 && maxErr > parser.Approximation)
            //{
            //    model = new Shepard(parser.FunctionDimension, points);
            //    analyzer = new Analyzer(model, points);
            //    analyzer.do_random_forest_analyse(cls, parser.Approximation, func, derivativeFunc);

            //    double[][] xx = analyzer.Result;
            //    int newPointsAmount = Math.Min(parser.PredictionPointAmount, xx.Length);
            //    pointAmount = pointAmount + newPointsAmount;
            //    double[][] newPoints = new double[pointAmount][];
            //    for (int j = 0; j < pointAmount; j++)
            //    {
            //        if (j < pointAmount - newPointsAmount)
            //        {
            //            newPoints[j] = (double[])points[j].Clone();
            //        }
            //        else
            //        {
            //            newPoints[j] = (double[])xx[j - pointAmount + newPointsAmount].Clone();
            //            newPoints[j][parser.FunctionDimension] = func(newPoints[j]);
            //        }
            //    }
            //    points = newPoints;


            //    double[][] new_points = new double[newPointsAmount][];
            //    for (int j = 0; j < newPointsAmount; j++)
            //    {
            //        new_points[j] = new double[xx[j].Length];
            //        Array.Copy(xx[j], new_points[j], xx[j].Length);
            //        model.Calculate(new_points[j]);
            //    }

            //    double tempErr = 0;
            //    for (int k = 0; k < new_points.Length; k++)
            //    {
            //        double err = Math.Abs(points[pointAmount - newPointsAmount + k][parser.FunctionDimension] - new_points[k][parser.FunctionDimension]);
            //        Console.WriteLine(" \n " + (points[pointAmount - newPointsAmount + k][parser.FunctionDimension] - new_points[k][parser.FunctionDimension]) + " " + points[pointAmount - newPointsAmount + k][parser.FunctionDimension] + " " + new_points[k][parser.FunctionDimension] + " \n ");
            //        if (err > tempErr)
            //        {
            //            tempErr = err;
            //        }
            //        Console.WriteLine("f({0}) real val {1} predict val {2} err {3}", String.Join(", ", xx[k]), points[pointAmount - newPointsAmount + k][parser.FunctionDimension], new_points[k][parser.FunctionDimension], err);
            //    }
            //    maxErr = tempErr;
            //    i++;
            //}

        }
    }
}
