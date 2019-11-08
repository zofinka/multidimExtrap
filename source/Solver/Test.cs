using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Solver
{
    abstract class Test
    {
        const int NGRID = 100;

        public abstract void run();
        
        public void runRandomForestTestsLearnOnOneUseForAnother()
        {
            Console.WriteLine("TESTS: Learn on one func use for another");
            int interAmount;

            /*Console.WriteLine("Test " + Tests.SinXCosY.name + " START");
            interAmount = testWithRandomForest(Tests.SquaresProducts.configFile, Tests.SquaresProducts.pointFile, Tests.SinXCosY.configFile, Tests.SinXCosY.pointFile, 
                                               Tests.SquaresProducts.func, Tests.SquaresProducts.derivative, Tests.SinXCosY.func, Tests.SinXCosY.derivative);
            Console.WriteLine("Test " + Tests.SinXCosY.name + " END in " + interAmount + " iterations");*/

            /*Console.WriteLine("Test " + Tests.SquaresProducts.name + " START");
            interAmount = testWithRandomForest(Tests.SquareArea.config, Tests.SquareArea.pointFile, Tests.SquaresProducts.configFile, Tests.SquaresProducts.pointFile,
                                               Tests.SquareArea.func, Tests.SquareArea.derivative, Tests.SquaresProducts.func, Tests.SquaresProducts.derivative);
            Console.WriteLine("Test " + Tests.SquaresProducts.name + " END in " + interAmount + " iterations");*/

            Tests.IFunction SinXCosY = new Tests.SinXCosY();
            Tests.IFunction SquaresProducts = new Tests.SquaresProducts();
            Console.WriteLine("Test " + SinXCosY.name + " START");
            interAmount = testWithRandomForest(SquaresProducts.configFile, SquaresProducts.pointFile, SinXCosY.configFile, SinXCosY.pointFile,
                                               SquaresProducts.func, SquaresProducts.derivative, SinXCosY.func, SinXCosY.derivative);
            Console.WriteLine("Test " + SinXCosY.name + " END in " + interAmount + " iterations");

            /*Console.WriteLine(Tests.SinXCosXCosY.name + " Test START");
            interAmount = testWithRandomForest(Tests.SinXCosY.configFile, Tests.SinXCosY.pointFile, Tests.SinXCosXCosY.configFile, Tests.SinXCosXCosY.pointFile,
                                               Tests.SinXCosY.func, Tests.SinXCosY.derivative, Tests.SinXCosXCosY.func, Tests.SinXCosXCosY.derivative);
            Console.WriteLine(Tests.SinXCosXCosY.name  + " Test END in " + interAmount + " iterations");*/

            /*Console.WriteLine(Tests.SinFromSumOnSum.name + " Test START");
            interAmount = testWithRandomForest(Tests.SinXCosXCosY.configFile, Tests.SinXCosXCosY.pointFile, Tests.SinFromSumOnSum.configFile, Tests.SinFromSumOnSum.pointFile,
                                               Tests.SinXCosXCosY.func, Tests.SinXCosXCosY.derivative, Tests.SinFromSumOnSum.func, Tests.SinFromSumOnSum.derivative);
            Console.WriteLine(Tests.SinFromSumOnSum.name + " Test END in " + interAmount + " iterations");*/

        }

        private int testWithRandomForest(string configFile, string pointFile, Func<double[], double> func, Func<double[], double[]> derivativeFunc)
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
            return i;
        }

        protected double[][] testDefWayUntilGood(Parser parser, double[][] points, Func<double[], double> func)
        {

            int pointAmount = parser.PointAmount;

            int i = 0;
            double maxErr = 10;
            int goodPr = 0;
            while (i < 100000000 && maxErr > parser.Approximation && goodPr < 50)
            {
                Shepard model = new Shepard(parser.FunctionDimension, points);
                Analyzer analyzer = new Analyzer(model, points);
                analyzer.do_default_analyse();
                goodPr = analyzer.getGoodPr(func, parser.Approximation);
                Console.WriteLine("Good pr " + goodPr);

                double[][] xx = analyzer.Result;
                pointAmount = pointAmount + parser.PredictionPointAmount;
                points = getNewPoints(points, analyzer.Result, parser.PredictionPointAmount, parser.FunctionDimension, func);


                double[][] new_points = new double[parser.PredictionPointAmount][];
                for (int j = 0; j < parser.PredictionPointAmount; j++)
                {
                    new_points[j] = new double[xx[j].Length];
                    Array.Copy(xx[j], new_points[j], xx[j].Length);
                    model.Calculate(new_points[j]);
                }

                double tempErr = 0;
                for (int k = 0; k < new_points.Length; k++)
                {
                    double err = Math.Abs(points[pointAmount - parser.PredictionPointAmount + k][parser.FunctionDimension] - new_points[k][parser.FunctionDimension]);
                    //Console.WriteLine(" \n " + (points[pointAmount - parser.PredictionPointAmount + k][parser.FunctionDimension] - new_points[k][parser.FunctionDimension]) + " " + points[pointAmount - parser.PredictionPointAmount + k][parser.FunctionDimension] + " " + new_points[k][parser.FunctionDimension] + " \n ");
                    if (err > tempErr)
                    {
                        tempErr = err;
                    }
                   // Console.WriteLine("f({0}) real val {1} predict val {2} err {3}", String.Join(", ", xx[k]), points[pointAmount - parser.PredictionPointAmount + k][parser.FunctionDimension], new_points[k][parser.FunctionDimension], err);
                }
                maxErr = tempErr;
                i++;
                //Console.WriteLine("aaa " + i < 100000000 && maxErr > parser.Approximation && goodPr < 50);

            }
            //testResult(parser.FunctionDimension, points, func);
            Shepard model1 = new Shepard(parser.FunctionDimension, points);
            Analyzer analyzer1 = new Analyzer(model1, points);
            analyzer1.do_default_analyse();


            //return analyzer1.getGoodSamples(func, parser.Approximation, goodPr);
            return points;
        }

        /*private int testDefWay(string configFile, string pointFile, Func<double[], double> func)
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
            while (i < 100000000 && maxErr > parser.Approximation)
            {
                Shepard model = new Shepard(parser.FunctionDimension, points);
                Console.WriteLine("Max " + String.Join(", ", model.Max) + " Min " + String.Join(", ", model.Min));
                Analyzer analyzer = new Analyzer(model, points);
                analyzer.do_default_analyse();

                double[][] xx = analyzer.Result;
                pointAmount = pointAmount + parser.PredictionPointAmount;
                points = getNewPoints(points, analyzer.Result, parser.PredictionPointAmount, parser.FunctionDimension, func);


                double[][] new_points = new double[parser.PredictionPointAmount][];
                for (int j = 0; j < parser.PredictionPointAmount; j++)
                {
                    new_points[j] = new double[xx[j].Length];
                    Array.Copy(xx[j], new_points[j], xx[j].Length);
                    model.Calculate(new_points[j]);
                }

                double tempErr = 0;
                for (int k = 0; k < new_points.Length; k++)
                {
                    double err = Math.Abs(points[pointAmount - parser.PredictionPointAmount + k][parser.FunctionDimension] - new_points[k][parser.FunctionDimension]);
                    Console.WriteLine(" \n " + (points[pointAmount - parser.PredictionPointAmount + k][parser.FunctionDimension] - new_points[k][parser.FunctionDimension]) + " " + points[pointAmount - parser.PredictionPointAmount + k][parser.FunctionDimension] + " " +  new_points[k][parser.FunctionDimension] + " \n ");
                    if (err > tempErr)
                    {
                        tempErr = err;
                    }
                    Console.WriteLine("f({0}) real val {1} predict val {2} err {3}", String.Join(", ", xx[k]), points[pointAmount - parser.PredictionPointAmount + k][parser.FunctionDimension], new_points[k][parser.FunctionDimension], err);
                }
                maxErr = tempErr;
                i++;

            }
            testResult(parser.FunctionDimension, points, func);

            return i;
        }*/

        private int testWithRandomForest(string configFileToLearn, string pointFileToLearn, string configFile, string pointFile, Func<double[], double> funcToLearn, Func<double[], double[]> derivativeFuncToLearn, Func<double[], double> func, Func<double[], double[]> derivativeFunc)
        {
            Parser parserToLearn = new Parser(configFile, pointFile);
            

            List<double[]> pointsToLearn = new List<double[]>();
            pointsToLearn = parserToLearn.Points.ToList();

            for (int j = 0; j < 1; j++)
            {
                pointsToLearn.AddRange(testDefWayUntilGood(parserToLearn, pointsToLearn.ToArray(), func).ToList<double[]>());
            }

            int pointAmountToLearn = pointsToLearn.Count;

            Shepard model = new Shepard(parserToLearn.FunctionDimension, pointsToLearn.ToArray());
            Analyzer analyzer = new Analyzer(model, pointsToLearn.ToArray());
            Classifiers.IClassifier cls = analyzer.learn_random_forest_on_grid(funcToLearn, derivativeFuncToLearn, parserToLearn.Approximation);

            Parser parser = new Parser(configFile, pointFile);
            int pointAmount = parser.PointAmount;

            double[][] points = new double[parser.PointAmount][];
            for (int j = 0; j < parser.PointAmount; j++)
            {
                points[j] = (double[])parser.Points[j].Clone();
            }

            int i = 0;
            double maxErr = 10;
            while (i < 10 && maxErr > parser.Approximation)
            {
                model = new Shepard(parser.FunctionDimension, points);
                analyzer = new Analyzer(model, points);
                analyzer.do_random_forest_analyse(cls, parser.Approximation, func, derivativeFunc);

                double[][] xx = analyzer.Result;
                int newPointsAmount = Math.Min(parser.PredictionPointAmount, xx.Length);
                pointAmount = pointAmount + newPointsAmount;
                double[][] newPoints = new double[pointAmount][];
                for (int j = 0; j < pointAmount; j++)
                {
                    if (j < pointAmount - newPointsAmount)
                    {
                        newPoints[j] = (double[])points[j].Clone();
                    }
                    else
                    {
                        newPoints[j] = (double[])xx[j - pointAmount + newPointsAmount].Clone();
                        newPoints[j][parser.FunctionDimension] = func(newPoints[j]);
                    }
                }
                points = newPoints;


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
                    double err = Math.Abs(points[pointAmount - newPointsAmount + k][parser.FunctionDimension] - new_points[k][parser.FunctionDimension]);
                    Console.WriteLine(" \n " + (points[pointAmount - newPointsAmount + k][parser.FunctionDimension] - new_points[k][parser.FunctionDimension]) + " " + points[pointAmount - newPointsAmount + k][parser.FunctionDimension] + " " + new_points[k][parser.FunctionDimension] + " \n ");
                    if (err > tempErr)
                    {
                        tempErr = err;
                    }
                    Console.WriteLine("f({0}) real val {1} predict val {2} err {3}", String.Join(", ", xx[k]), points[pointAmount - newPointsAmount + k][parser.FunctionDimension], new_points[k][parser.FunctionDimension], err);
                }
                maxErr = tempErr;
                i++;
            }
            testResult(parser.FunctionDimension, points, func);
            return i;
        }


        protected double[][] getNewPoints(double[][] oldPoints, double[][] allPointsToCalc, int pointToClacAmout, int functionDementsion, Func<double[], double> func)
        {
            int newPointAmount = oldPoints.Length + pointToClacAmout;
            double[][] newPoints = new double[newPointAmount][];
            for (int j = 0; j < newPointAmount; j++)
            {
                if (j < newPointAmount - pointToClacAmout)
                {
                    newPoints[j] = (double[])oldPoints[j].Clone();
                }
                else
                {
                    newPoints[j] = (double[])allPointsToCalc[j - newPointAmount + pointToClacAmout].Clone();
                    newPoints[j][functionDementsion] = func(newPoints[j]);
                }
            }
            return newPoints;
        }

        protected void testResult(int functionDimension, double[][] points, Func<double[], double> func)
        {
            Console.WriteLine("\n\nTest result approximation");
            Shepard model = new Shepard(functionDimension, points);
            for (int i = 1; i < 10; i += 2)
            {
                double[] a = new double[functionDimension + 1];
                double[] b = new double[functionDimension + 1];
                for (int j = 0; j < functionDimension; j++)
                {
                    a[j] = model.Min[j] + ((model.Max[j] - model.Min[j]) * i / 10);
                    b[j] = a[j];
                }
                model.Calculate(a);
                Console.WriteLine("Vector " + String.Join(", ", b) + " Approximation result " + a[functionDimension] + " real " + func(b) + " error " + Math.Abs(a[functionDimension] - func(b)));
            }
        }

        /*private void testResult(int functionDimension, double err, double[][] points, Func<double[], double> func)
        {
            Console.WriteLine("\n\nTest result approximation");
            Shepard model = new Shepard(functionDimension, points);

            for (int i = 1; i < 10; i += 2)
            {
                double[] a = new double[functionDimension + 1];
                double[] b = new double[functionDimension + 1];
                for (int j = 0; j < functionDimension; j++)
                {
                    a[j] = model.Min[j] + ((model.Max[j] - model.Min[j]) * i / 10);
                    b[j] = a[j];
                }
                model.Calculate(a);
                Console.WriteLine("Vector " + String.Join(", ", b) + " Approximation result " + a[functionDimension] + " real " + func(b) + " error " + Math.Abs(a[functionDimension] - func(b)));
            }
        }*/

        protected double calc_err(Func<double[], double> func, List<double[]> points, Parser parser)
        {
            // TODO add calc by integrall
            Shepard def_model = new Shepard(parser.FunctionDimension, points.ToArray());
            int[] count = new int[def_model.N];
            for (int i = 0; i < def_model.N; i++) count[i] = (def_model.Min[i] == def_model.Max[i]) ? 1 : NGRID;
            Grid grid = new Grid(def_model.N, def_model.M, def_model.Min, def_model.Max, count);
            double sum = 0;
            int numInSum = 0;
            for (int i = 0; i < grid.Node.Length; i++)
            {
                bool in_range = true;
                for (int j = 0; j < grid.Node[i].Length - 1; j++)
                {
                    if (grid.Node[i][j] > def_model.Max[j] || grid.Node[i][j] < def_model.Min[j])
                    {
                        in_range = false;
                    }
                }
                if (in_range)
                {
                    double[] grid_node = (double[])grid.Node[i].Clone();
                    def_model.Calculate(grid_node);
                    double realFunctionVal = func(grid.Node[i]);
                    sum += Math.Abs(grid_node[grid_node.Length - 1] - realFunctionVal);
                    numInSum++;
                }
            }

            return (double)sum / numInSum;
        }
    }
}
