using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Solver
{
    class Test
    {
        public void runDefWayTests()
        {
            Console.WriteLine("TESTS: Default way\n");
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
            //Console.WriteLine("Test " + Tests.SquaresProducts.name + " START");
           // interAmount = testDefWay(Tests.SquaresProducts.configFile, Tests.SquaresProducts.pointFile, Tests.SquaresProducts.func);
            //Console.WriteLine("Test " + Tests.SquaresProducts.name + " END in " + interAmount + " iterations");

            // 212 iteration
            Console.WriteLine("Test " + Tests.SinXCosY.name + " START");
            interAmount = testDefWay(Tests.SinXCosY.configFile, Tests.SinXCosY.pointFile, Tests.SinXCosY.func);
            Console.WriteLine("Test " + Tests.SinXCosY.name + " END in " + interAmount + " iterations");

            //133 iteration
            /*Console.WriteLine(Tests.SinXCosXCosY.name  + " Test START");
            interAmount = testDefWay(Tests.SinXCosXCosY.configFile, Tests.SinXCosXCosY.pointFile, Tests.SinXCosXCosY.func);
            Console.WriteLine(Tests.SinXCosXCosY.name  + " Test END in " + interAmount + " iterations");*/

            //14 interation
            /*Console.WriteLine(Tests.SinFromSumOnSum.name + " Test START");
            interAmount = testDefWay(Tests.SinFromSumOnSum.configFile, Tests.SinFromSumOnSum.pointFile, Tests.SinFromSumOnSum.func);
            Console.WriteLine(Tests.SinFromSumOnSum.name + " Test END in " + interAmount + " iterations");*/
        }

        public void runRandomForestTestsLearnOnAll()
        {
            int interAmount;

            /*Console.WriteLine("Square Area Function Test START");
            interAmount = testWithRandomForest(Tests.SquareArea.config, Tests.SquareArea.config, Tests.SquareArea.func, Tests.SquareArea.derivative);
            Console.WriteLine("Square Area Function Test END in " + interAmount + " iterations");*/

           // Console.WriteLine("Test " + Tests.SquaresProducts.name + " START");
           // interAmount = testWithRandomForest(Tests.SquaresProducts.configFile, Tests.SquaresProducts.pointFile, Tests.SquaresProducts.func, Tests.SquaresProducts.derivative);
           // Console.WriteLine("Test " + Tests.SquaresProducts.name + " END in " + interAmount + " iterations");

            Console.WriteLine("Test " + Tests.SinXCosY.name + " START");
            interAmount = testWithRandomForest(Tests.SinXCosY.configFile, Tests.SinXCosY.pointFile, Tests.SinXCosY.func, Tests.SinXCosY.derivative);
            Console.WriteLine("Test " + Tests.SinXCosY.name + " END in " + interAmount + " iterations");

           /* Console.WriteLine(Tests.SinXCosXCosY.name +  " Test START");
            interAmount = testWithRandomForest(Tests.SinXCosXCosY.configFile, Tests.SinXCosXCosY.pointFile, Tests.SinXCosXCosY.func, Tests.SinXCosXCosY.derivative);
            Console.WriteLine("17.sinXconYxonX Test END in " + interAmount + " iterations");*/

            /*Console.WriteLine(Tests.SinFromSumOnSum.name + " Test START");
            interAmount = testWithRandomForest(Tests.SinFromSumOnSum.configFile, Tests.SinFromSumOnSum.pointFile, Tests.SinFromSumOnSum.func, Tests.SinFromSumOnSum.derivative);
            Console.WriteLine(Tests.SinFromSumOnSum.name + " Test END in " + interAmount + " iterations");*/
        }

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

            /*Console.WriteLine("Test " + Tests.SinXCosY.name + " START");
            interAmount = testWithRandomForest(Tests.SquaresProducts.configFile, Tests.SquaresProducts.pointFile, Tests.SinXCosY.configFile, Tests.SinXCosY.pointFile,
                                               Tests.SquaresProducts.func, Tests.SquaresProducts.derivative, Tests.SinXCosY.func, Tests.SinXCosY.derivative);
            Console.WriteLine("Test " + Tests.SinXCosY.name + " END in " + interAmount + " iterations");*/

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
            double[][] points = testDefWayUntilGood(configFile, pointFile, func);

            Parser parser = new Parser(configFile, pointFile);
            int pointAmount = points.Length;

            Shepard model = new Shepard(parser.FunctionDimension, points);
            Analyzer analyzer = new Analyzer(model, points);
            Classifiers.IClassifier cls = analyzer.learn_random_forest_on_grid(func, derivativeFunc, parser.Approximation);

            /*double[][] points = new double[parser.PointAmount][];
            for (int j = 0; j < parser.PointAmount; j++)
            {
                points[j] = (double[])parser.Points[j].Clone();
            }*/

            int i = 0;
            double maxErr = 10;
            while (i < 100000000 && maxErr > parser.Approximation)
            {
                model = new Shepard(parser.FunctionDimension, points);
                analyzer = new Analyzer(model, points);
                analyzer.do_random_forest_analyse(cls, parser.Approximation, func, derivativeFunc);

                double[][] xx = analyzer.Result;
                int newPointsAmount = Math.Min(parser.PredictionPointAmount, xx.Length);
                pointAmount += newPointsAmount;

                List<double[]> newPointsList = points.ToList<double[]>();

                for (int j = 0; j < newPointsAmount; j++)
                {
                    var newPoint = (double[])xx[j].Clone();
                    newPoint[parser.FunctionDimension] = func(newPoint);
                    newPointsList.Add(newPoint);                   
                }

                points = newPointsList.ToArray<double[]>();

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
                    var gtPoint = points[pointAmount - newPointsAmount + k][parser.FunctionDimension];
                    var approxPoint = new_points[k][parser.FunctionDimension];
                    var err = Math.Abs(gtPoint - approxPoint);
                    Console.WriteLine(" \n " + err + " " + gtPoint + " " + approxPoint + " \n ");
                    if (err > tempErr)
                    {
                        tempErr = err;
                    }
                    Console.WriteLine("f({0}) real val {1} predict val {2} err {3}", String.Join(", ", xx[k]), gtPoint, approxPoint, err);
                }
                maxErr = tempErr;
                i++;
            }
            testResult(parser.FunctionDimension, points, func);
            return i;
        }

        private double[][] testDefWayUntilGood(string configFile, string pointFile, Func<double[], double> func)
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

        private int testDefWay(string configFile, string pointFile, Func<double[], double> func)
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
        }

        private int testWithRandomForest(string configFileToLearn, string pointFileToLearn, string configFile, string pointFile, Func<double[], double> funcToLearn, Func<double[], double[]> derivativeFuncToLearn, Func<double[], double> func, Func<double[], double[]> derivativeFunc)
        {
            Parser parserToLearn = new Parser(configFile, pointFile);
            int pointAmountToLearn = parserToLearn.PointAmount;

            Shepard model = new Shepard(parserToLearn.FunctionDimension, parserToLearn.Points);
            Analyzer analyzer = new Analyzer(model, parserToLearn.Points);
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
                pointAmount = pointAmount + parser.PredictionPointAmount;
                double[][] newPoints = new double[pointAmount][];
                for (int j = 0; j < pointAmount; j++)
                {
                    if (j < pointAmount - parser.PredictionPointAmount)
                    {
                        newPoints[j] = (double[])points[j].Clone();
                    }
                    else
                    {
                        newPoints[j] = (double[])xx[j - pointAmount + parser.PredictionPointAmount].Clone();
                        newPoints[j][parser.FunctionDimension] = func(newPoints[j]);
                    }
                }
                points = newPoints;


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
                    Console.WriteLine(" \n " + (points[pointAmount - parser.PredictionPointAmount + k][parser.FunctionDimension] - new_points[k][parser.FunctionDimension]) + " " + points[pointAmount - parser.PredictionPointAmount + k][parser.FunctionDimension] + " " + new_points[k][parser.FunctionDimension] + " \n ");
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
        }


        private double[][] getNewPoints(double[][] oldPoints, double[][] allPointsToCalc, int pointToClacAmout, int functionDementsion, Func<double[], double> func)
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

        private void testResult(int functionDimension, double[][] points, Func<double[], double> func)
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
    }

    class ClassifierTest
    {
        public double runOnDAALDataset()
        {
            Func<string, int, Classifiers.LabeledData[]> parseDataSet = delegate (string path, int count)
            {
                Classifiers.LabeledData[] data = new Classifiers.LabeledData[count];

                using (StreamReader fs = new StreamReader(path))
                {
                    for (var i = 0; i < count; ++i)
                    {
                        string[] vals = fs.ReadLine().Split(',');
                        double[] features = { Double.Parse(vals[0]), Double.Parse(vals[1]), Double.Parse(vals[2]) };
                        int label = Int32.Parse(vals[3]);

                        data[i] = new Classifiers.LabeledData(features, label);
                    }
                }

                return data;
            };

            var trainDataCount = 100000;
            var trainDataSet = parseDataSet("..//..//..//test_classifier//df_classification_train.csv", trainDataCount);

            Classifiers.IClassifier cls = new Classifiers.RandomForest();

            Classifiers.RandomForestParams ps = new Classifiers.RandomForestParams(trainDataSet,
                                                                                   trainDataCount /* samples count */,
                                                                                   3              /* features count */,
                                                                                   5              /* classes count */,
                                                                                   100            /* trees count */,
                                                                                   2              /* count of features to do split in a tree */,
                                                                                   0.7            /* percent of a training set of samples  */
                                                                                                  /* used to build individual trees. */);

            cls.train(ps);

            double trainPrecision;
            cls.validate(trainDataSet, out trainPrecision);

            var testDataCount = 1000;
            var testDataSet = parseDataSet("..//..//..//test_classifier//df_classification_test.csv", testDataCount);

            double testPrecision;
            cls.validate(testDataSet, out testPrecision);

            Console.WriteLine("Model precision on train DAAL dataset is " + trainPrecision);
            Console.WriteLine("Model precision on test DAAL dataset is " + testPrecision);

            return testPrecision;
        }

        public bool runOnSimpleFunction()
        {
            return true;
        }
    }
}
