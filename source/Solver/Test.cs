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

            //Tests.IFunction SinXCosY = new Tests.SinXCosY();
            //Tests.IFunction SquaresProducts = new Tests.SquaresProducts();
            //Console.WriteLine("Test " + SinXCosY.name + " START");
            //interAmount = testWithRandomForest(SquaresProducts.configFile, SquaresProducts.pointFile, SinXCosY.configFile, SinXCosY.pointFile,
            //                                   SquaresProducts.func, SquaresProducts.derivative, SinXCosY.func, SinXCosY.derivative);
            //Console.WriteLine("Test " + SinXCosY.name + " END in " + interAmount + " iterations");

            /*Console.WriteLine(Tests.SinXCosXCosY.name + " Test START");
            interAmount = testWithRandomForest(Tests.SinXCosY.configFile, Tests.SinXCosY.pointFile, Tests.SinXCosXCosY.configFile, Tests.SinXCosXCosY.pointFile,
                                               Tests.SinXCosY.func, Tests.SinXCosY.derivative, Tests.SinXCosXCosY.func, Tests.SinXCosXCosY.derivative);
            Console.WriteLine(Tests.SinXCosXCosY.name  + " Test END in " + interAmount + " iterations");*/

            /*Console.WriteLine(Tests.SinFromSumOnSum.name + " Test START");
            interAmount = testWithRandomForest(Tests.SinXCosXCosY.configFile, Tests.SinXCosXCosY.pointFile, Tests.SinFromSumOnSum.configFile, Tests.SinFromSumOnSum.pointFile,
                                               Tests.SinXCosXCosY.func, Tests.SinXCosXCosY.derivative, Tests.SinFromSumOnSum.func, Tests.SinFromSumOnSum.derivative);
            Console.WriteLine(Tests.SinFromSumOnSum.name + " Test END in " + interAmount + " iterations");*/

        }

       /* private int testWithRandomForest(string configFile, string pointFile, Func<double[], double> func, Func<double[], double[]> derivativeFunc)
        {
            Parser parser = new Parser(configFile, pointFile);
            List<double[]> points = new List<double[]>();
            points = parser.Points.ToList();

            for (int j = 0; j < 1; j++)
            {
                points.AddRange(testDefWayUntilGood(parser, points.ToArray(), func).ToList<double[]>());
            }


            int pointAmount = points.Count;

            Shepard model = new Shepard(parser.N_Dimension, points.ToArray());
            Analyzer analyzer = new Analyzer(model, points.ToArray());
            MLAlgorithms.IMLAlgorithm cls = analyzer.learn_random_forest_on_grid(func, derivativeFunc, parser.Approximation);

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
                model = new Shepard(parser.N_Dimension, pointsArray);
                analyzer = new Analyzer(model, pointsArray);
                analyzer.do_random_forest_analyse(cls, parser.Approximation, func, derivativeFunc);

                double[][] xx = analyzer.Result;
                int newPointsAmount = Math.Min(parser.PredictionPointAmount, xx.Length);
                pointAmount += newPointsAmount;

                List<double[]> newPointsList = points.ToList<double[]>();

                for (int j = 0; j < newPointsAmount; j++)
                {
                    var newPoint = (double[])xx[j].Clone();
                    newPoint[parser.N_Dimension] = func(newPoint);
                    newPointsList.Add(newPoint);                   
                }

                points = newPointsList;

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
                    var gtPoint = points[pointAmount - newPointsAmount + k][parser.N_Dimension];
                    var approxPoint = new_points[k][parser.N_Dimension];
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
            testResult(parser.N_Dimension, pointsArray, func);
            return i;
        }*/

        protected double[][] testDefWayUntilGood(Parser parser, double[][] points, Func<double[], double[]> func)
        {

            int pointAmount = parser.PointAmount;

            int i = 0;
            double maxErr = 10;
            int goodPr = 0;
            while (i < 100000000 && maxErr > parser.Approximation && goodPr < 50)
            {
                Shepard model = new Shepard(parser.N_Dimension, points);
                Analyzer analyzer = new Analyzer(model, points);
                analyzer.do_default_analyse();
                goodPr = analyzer.getGoodPr(func, parser.Approximation);
                Console.WriteLine("Good pr " + goodPr);

                double[][] xx = analyzer.Result;
                pointAmount = pointAmount + parser.PredictionPointAmount;
                points = getNewPoints(points, analyzer.Result, parser.PredictionPointAmount, parser.N_Dimension, func);


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
                    double err = Math.Abs(points[pointAmount - parser.PredictionPointAmount + k][parser.N_Dimension] - new_points[k][parser.N_Dimension]);
                    //Console.WriteLine(" \n " + (points[pointAmount - parser.PredictionPointAmount + k][parser.N_Dimension] - new_points[k][parser.N_Dimension]) + " " + points[pointAmount - parser.PredictionPointAmount + k][parser.N_Dimension] + " " + new_points[k][parser.N_Dimension] + " \n ");
                    if (err > tempErr)
                    {
                        tempErr = err;
                    }
                   // Console.WriteLine("f({0}) real val {1} predict val {2} err {3}", String.Join(", ", xx[k]), points[pointAmount - parser.PredictionPointAmount + k][parser.N_Dimension], new_points[k][parser.N_Dimension], err);
                }
                maxErr = tempErr;
                i++;
                //Console.WriteLine("aaa " + i < 100000000 && maxErr > parser.Approximation && goodPr < 50);

            }
            //testResult(parser.N_Dimension, points, func);
            Shepard model1 = new Shepard(parser.N_Dimension, points);
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
                Shepard model = new Shepard(parser.N_Dimension, points);
                Console.WriteLine("Max " + String.Join(", ", model.Max) + " Min " + String.Join(", ", model.Min));
                Analyzer analyzer = new Analyzer(model, points);
                analyzer.do_default_analyse();

                double[][] xx = analyzer.Result;
                pointAmount = pointAmount + parser.PredictionPointAmount;
                points = getNewPoints(points, analyzer.Result, parser.PredictionPointAmount, parser.N_Dimension, func);


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
                    double err = Math.Abs(points[pointAmount - parser.PredictionPointAmount + k][parser.N_Dimension] - new_points[k][parser.N_Dimension]);
                    Console.WriteLine(" \n " + (points[pointAmount - parser.PredictionPointAmount + k][parser.N_Dimension] - new_points[k][parser.N_Dimension]) + " " + points[pointAmount - parser.PredictionPointAmount + k][parser.N_Dimension] + " " +  new_points[k][parser.N_Dimension] + " \n ");
                    if (err > tempErr)
                    {
                        tempErr = err;
                    }
                    Console.WriteLine("f({0}) real val {1} predict val {2} err {3}", String.Join(", ", xx[k]), points[pointAmount - parser.PredictionPointAmount + k][parser.N_Dimension], new_points[k][parser.N_Dimension], err);
                }
                maxErr = tempErr;
                i++;

            }
            testResult(parser.N_Dimension, points, func);

            return i;
        }*/

        /*private int testWithRandomForest(string configFileToLearn, string pointFileToLearn, string configFile, string pointFile, Func<double[], double> funcToLearn, Func<double[], double[]> derivativeFuncToLearn, Func<double[], double> func, Func<double[], double[]> derivativeFunc)
        {
            Parser parserToLearn = new Parser(configFile, pointFile);
            

            List<double[]> pointsToLearn = new List<double[]>();
            pointsToLearn = parserToLearn.Points.ToList();

            for (int j = 0; j < 1; j++)
            {
                pointsToLearn.AddRange(testDefWayUntilGood(parserToLearn, pointsToLearn.ToArray(), func).ToList<double[]>());
            }

            int pointAmountToLearn = pointsToLearn.Count;

            Shepard model = new Shepard(parserToLearn.N_Dimension, pointsToLearn.ToArray());
            Analyzer analyzer = new Analyzer(model, pointsToLearn.ToArray());
            MLAlgorithms.IMLAlgorithm cls = analyzer.learn_random_forest_on_grid(funcToLearn, derivativeFuncToLearn, parserToLearn.Approximation);

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
                model = new Shepard(parser.N_Dimension, points);
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
                        newPoints[j][parser.N_Dimension] = func(newPoints[j]);
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
                    double err = Math.Abs(points[pointAmount - newPointsAmount + k][parser.N_Dimension] - new_points[k][parser.N_Dimension]);
                    Console.WriteLine(" \n " + (points[pointAmount - newPointsAmount + k][parser.N_Dimension] - new_points[k][parser.N_Dimension]) + " " + points[pointAmount - newPointsAmount + k][parser.N_Dimension] + " " + new_points[k][parser.N_Dimension] + " \n ");
                    if (err > tempErr)
                    {
                        tempErr = err;
                    }
                    Console.WriteLine("f({0}) real val {1} predict val {2} err {3}", String.Join(", ", xx[k]), points[pointAmount - newPointsAmount + k][parser.N_Dimension], new_points[k][parser.N_Dimension], err);
                }
                maxErr = tempErr;
                i++;
            }
            testResult(parser.N_Dimension, points, func);
            return i;
        }*/


        protected double[][] getNewPoints(double[][] oldPoints, double[][] allPointsToCalc, int pointToClacAmout, int functionDementsion, Func<double[], double[]> func)
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
                    var response = func(newPoints[j]);

                    for (int k = functionDementsion , l = 0; k < newPoints[j].Length; ++k, ++l)
                    {
                        newPoints[j][k] = response[l];
                    }
                }
            }
            return newPoints;
        }

        protected void testResult(int nFunctionDimension, int mFunctionDimension, double[][] points, Func<double[], double[]> func)
        {
            Console.WriteLine("\n\nTest result approximation");
            Shepard model = new Shepard(nFunctionDimension, points);
            for (int i = 1; i < 10; i += 2)
            {
                double[] point = new double[nFunctionDimension + mFunctionDimension];
                double[] pointX = new double[nFunctionDimension];
                double[] pointY = new double[mFunctionDimension];

                for (int j = 0; j < nFunctionDimension; j++)
                {
                    point[j] = model.Min[j] + ((model.Max[j] - model.Min[j]) * i / 10);
                    pointX[j] = point[j];
                }
                model.Calculate(point);

                for (int k = 0; k < mFunctionDimension; ++k)
                {
                    pointY[k] = point[nFunctionDimension + k];
                }

                double[] realPointY = func(point);

                double absErr = realPointY.Zip(pointY, (d1, d2) => Math.Abs(d1 - d2)).ToArray().Sum() / realPointY.Length;

                Console.WriteLine("Vector " + String.Join(", ", pointX) + " Approximation result: " + String.Join(", ", pointY) + 
                    ", real: " + String.Join(", ", realPointY) + ", err: " + absErr);
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

        protected double calc_err(Func<double[], double[]> func, List<double[]> points, Parser parser)
        {
            // TODO add calc by integrall
            Shepard def_model = new Shepard(parser.N_Dimension, points.ToArray());
            int[] count = new int[def_model.N];
            for (int i = 0; i < def_model.N; i++) count[i] = (def_model.Min[i] == def_model.Max[i]) ? 1 : NGRID;
            Grid grid = new Grid(def_model.N, def_model.M, def_model.Min, def_model.Max, count);
            double sum = 0;
            int numInSum = 0;
            for (int i = 0; i < grid.Node.Length; i++)
            {
                bool in_range = true;
                for (int j = 0; j < def_model.N; j++)
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
                    double[] approxFunctionVal = new double[def_model.N];
                    
                    for (int j = 0; j < def_model.M; ++j)
                    {
                        approxFunctionVal[j] = grid_node[def_model.N + j];
                    }

                    double[] realFunctionVal = func(grid.Node[i]);

                    double[] diffs = realFunctionVal.Zip(approxFunctionVal, (d1, d2) => Math.Abs(d1 - d2)).ToArray();
                     
                    sum += (diffs.Sum() / diffs.Length);
                    numInSum++;
                }
            }

            return (double)sum / numInSum;
        }

        public double distanceX(double[] a, double[] b, int N)
        {
            double dist = 0;
            for (int i = 0; i < N; i++) dist += (a[i] - b[i]) * (a[i] - b[i]);
            return Math.Sqrt(dist);
        }

        public double distanceF(double[] a, double[] b, int N, int M)
        {
            double dist = 0;
            for (int i = N; i < N + M; i++) dist += (a[i] - b[i]) * (a[i] - b[i]);
            return Math.Sqrt(dist);
        }

        // It is wrong.
        protected double[] update_path_to_knowing_points(Grid grid, double[][] points, int functionDemention)
        {
            //вычисляю принадлежность узлов сетки доменам (графовый алгоритм на базе структуры уровней смежности)
            SortedSet<int>[] adjncy = new SortedSet<int>[points.Length];
            for (int i = 0; i < points.Length; i++)
                adjncy[i] = new SortedSet<int>();

            // Console.WriteLine("Разбиение пространства на домены");
            Queue<int> queue = new Queue<int>();
            int[] domain = new int[grid.Node.Length];
            double[] dist = new double[grid.Node.Length];
            for (int i = 0; i < domain.Length; i++)
            {
                domain[i] = -1;
                dist[i] = double.PositiveInfinity;
            }

            for (int i = 0; i < points.Length; i++)
            {
                int index;
                grid.ToIndex(points[i], out index);
                dist[index] = distanceX(grid.Node[index], points[i], functionDemention);
                domain[index] = i;
                //setvalue(index, xf[i]);
                //Console.WriteLine("index " + index + "domain " + domain[index] + " dist " + dist[index] );
                queue.Enqueue(index);
            }
            Console.WriteLine("queue ");
            while (queue.Count > 0)
            {
                int index = queue.Dequeue();
                int i = domain[index];
                //Console.WriteLine("index " + index + "domain " + domain[index]);
                foreach (var adj in grid.Neighbours(index))
                {
                    double d = distanceX(grid.Node[adj], points[i], functionDemention);
                    // Console.WriteLine("adj " + adj + "distanceX(grid.Node[adj], xf[i]) " + d);
                    if (domain[adj] >= 0)
                    {
                        adjncy[domain[adj]].Add(i);
                        adjncy[i].Add(domain[adj]);
                        if (d < dist[adj])
                        {
                            domain[adj] = i;
                            dist[adj] = d;
                        }
                        continue;
                    }
                    domain[adj] = i;
                    dist[adj] = d;
                    //Console.WriteLine("adj " + adj + "domain " + domain[adj] + " dist " + dist[adj]);
                    //setvalue(adj, xf[i]);
                    queue.Enqueue(adj);
                }
            }

            return dist;
        }

        protected double check_new_aproximation(Shepard old_model, Shepard new_model, Grid grid, int nodeNum)
        {
            SortedSet<int> neighbours = new SortedSet<int>();
            foreach (var n in grid.Neighbours(nodeNum))
            {
                neighbours.Add(n);
                foreach (var nn in grid.Neighbours(n))
                {
                    neighbours.Add(nn);
                }
            }

            double diff = 0.0;
            int n_count = 0;

            neighbours.ToList<int>().ForEach(
            (int node_num) =>
            {
                bool in_range = true;

                for (int j = 0; j < grid.Node[node_num].Length - 1; j++)
                {
                    if (grid.Node[node_num][j] > old_model.Max[j] || grid.Node[node_num][j] < old_model.Min[j])
                    {
                        in_range = false;
                    }
                }

                if (in_range)
                {
                    double[] old_grid_node = (double[])grid.Node[node_num].Clone();
                    old_model.Calculate(old_grid_node);

                    double[] new_grid_node = (double[])grid.Node[node_num].Clone();
                    new_model.Calculate(new_grid_node);

                    diff += Math.Abs(old_grid_node[old_grid_node.Length - 1] - new_grid_node[new_grid_node.Length - 1]);
                    n_count++;
                }
            });

            return n_count == 0 ? 0.0 : diff / (double)n_count;
        }
    }

    class ClassifierTest
    { 
        public double runOnDAALDataset()
        {
            Func<string, int, MLAlgorithms.LabeledData[]> parseDataSet = delegate (string path, int count)
            {
                MLAlgorithms.LabeledData[] data = new MLAlgorithms.LabeledData[count];

                using (StreamReader fs = new StreamReader(path))
                {
                    for (var i = 0; i < count; ++i)
                    {
                        string[] vals = fs.ReadLine().Split(',');
                        double[] features = { Double.Parse(vals[0]), Double.Parse(vals[1]), Double.Parse(vals[2]) };
                        Int32 label = Int32.Parse(vals[3]);

                        data[i] = new MLAlgorithms.LabeledData(features, label);
                    }
                }

                return data;
            };

            var trainDataCount = 100000;
            var trainDataSet = parseDataSet("..//..//..//test_df//df_classification_train.csv", trainDataCount);

            MLAlgorithms.IMLAlgorithm cls = new MLAlgorithms.RandomForest();

            MLAlgorithms.RandomForestParams ps = new MLAlgorithms.RandomForestParams(trainDataSet,
                                                                                   trainDataCount /* samples count */,
                                                                                   3              /* features count */,
                                                                                   5              /* classes count */,
                                                                                   100            /* trees count */,
                                                                                   2              /* count of features to do split in a tree */,
                                                                                   0.7            /* percent of a training set of samples  */
                                                                                                  /* used to build individual trees. */);

            cls.train<int>(ps);

            double trainPrecision;
            cls.validate<int>(trainDataSet, out trainPrecision);

            var testDataCount = 1000;
            var testDataSet = parseDataSet("..//..//..//test_df//df_classification_test.csv", testDataCount);

            double testPrecision;
            cls.validate<int>(testDataSet, out testPrecision);

            Console.WriteLine("Model precision on train DAAL dataset is " + trainPrecision);
            Console.WriteLine("Model precision on test DAAL dataset is " + testPrecision);

            return testPrecision;
        }
    }

    class RegressorTest
    {
        public double runOnDAALDataset()
        {
            Func<string, int, MLAlgorithms.LabeledData[]> parseDataSet = delegate (string path, int count)
            {
                MLAlgorithms.LabeledData[] data = new MLAlgorithms.LabeledData[count];

                using (StreamReader fs = new StreamReader(path))
                {
                    for (var i = 0; i < count; ++i)
                    {
                        string[] vals = fs.ReadLine().Split(',');
                        double[] features = { Double.Parse(vals[0]), Double.Parse(vals[1]), Double.Parse(vals[2]),
                                            Double.Parse(vals[3]), Double.Parse(vals[4]), Double.Parse(vals[5]),
                                            Double.Parse(vals[6]), Double.Parse(vals[7]), Double.Parse(vals[8]),
                                            Double.Parse(vals[9]), Double.Parse(vals[10]), Double.Parse(vals[11])};
                        Double label = Double.Parse(vals[12]);

                        data[i] = new MLAlgorithms.LabeledData(features, label);
                    }
                }

                return data;
            };

            var trainDataCount = 380;
            var trainDataSet = parseDataSet("..//..//..//test_df//df_regression_train.csv", trainDataCount);

            MLAlgorithms.IMLAlgorithm rg = new MLAlgorithms.RandomForest();

            MLAlgorithms.RandomForestParams ps = new MLAlgorithms.RandomForestParams(trainDataSet,
                                                                                   trainDataCount /* samples count */,
                                                                                   12             /* features count */,
                                                                                   1              /* classes count */,
                                                                                   50             /* trees count */,
                                                                                   7              /* count of features to do split in a tree */,
                                                                                   0.65           /* percent of a training set of samples  */
                                                                                                  /* used to build individual trees. */);

            rg.train<double>(ps);

            double trainPrecision;
            rg.validate<double>(trainDataSet, out trainPrecision);

            var testDataCount = 127;
            var testDataSet = parseDataSet("..//..//..//test_df//df_regression_test.csv", testDataCount);

            double testPrecision;
            rg.validate<double>(testDataSet, out testPrecision);

            Console.WriteLine("Model precision on train DAAL dataset is " + trainPrecision);
            Console.WriteLine("Model precision on test DAAL dataset is " + testPrecision);

            return testPrecision;
        }
    }
}
