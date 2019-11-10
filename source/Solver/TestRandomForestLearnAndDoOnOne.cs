using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solver
{
    class TestRandomForestLearnAndDoOnOne: Test
    {
        const int NGRID = 100;
        int featureCount = 0;

        public override void run()
        {
            int interAmount = 0;
            Tests.IFunction[] functions = new Tests.IFunction[1] { new Tests.SinXCosY() };
            Classifiers.IClassifier cls = get_cls(functions);

            /*Console.WriteLine("Square Area Function Test START");
            interAmount = testWithRandomForest(Tests.SquareArea.config, Tests.SquareArea.config, Tests.SquareArea.func, Tests.SquareArea.derivative);
            Console.WriteLine("Square Area Function Test END in " + interAmount + " iterations");*/

            /*Console.WriteLine("Test " + Tests.SquaresProducts.name + " START");
            interAmount = testWithRandomForest(Tests.SquaresProducts.configFile, Tests.SquaresProducts.pointFile, Tests.SquaresProducts.func, Tests.SquaresProducts.derivative);
            Console.WriteLine("Test " + Tests.SquaresProducts.name + " END in " + interAmount + " iterations");*/

            Tests.SinXCosY SinXCosY = new Tests.SinXCosY();
            Console.WriteLine("Test " + SinXCosY.name + " START");
            interAmount = test(cls, SinXCosY.configFile, SinXCosY.pointFile, SinXCosY.func, SinXCosY.derivative);
            Console.WriteLine("Test " + SinXCosY.name + " END in " + interAmount + " iterations");

            /*Console.WriteLine(Tests.SinXCosXCosY.name +  " Test START");
            interAmount = testWithRandomForest(Tests.SinXCosXCosY.configFile, Tests.SinXCosXCosY.pointFile, Tests.SinXCosXCosY.func, Tests.SinXCosXCosY.derivative);
            Console.WriteLine("17.sinXconYxonX Test END in " + interAmount + " iterations");*/

            /*Console.WriteLine(Tests.SinFromSumOnSum.name + " Test START");
            interAmount = testWithRandomForest(Tests.SinFromSumOnSum.configFile, Tests.SinFromSumOnSum.pointFile, Tests.SinFromSumOnSum.func, Tests.SinFromSumOnSum.derivative);
            Console.WriteLine(Tests.SinFromSumOnSum.name + " Test END in " + interAmount + " iterations");*/
        }

        private int test(Classifiers.IClassifier cls, string configFile, string pointFile, Func<double[], double> func, Func<double[], double[]> derivativeFunc)
        {
            Parser parser = new Parser(configFile, pointFile);
            double[][] pointsArray = parser.Points.ToList().ToArray();
            int pointAmount = pointsArray.Length;

            int i = 0;
            double maxErr = 10;
            while (i < 100000000 && maxErr > parser.Approximation)
            {
                Shepard model = new Shepard(parser.FunctionDimension, pointsArray);
                Analyzer analyzer = new Analyzer(model, pointsArray);
                analyzer.do_random_forest_analyse(cls, build_features);

                double[][] xx = analyzer.Result;
                int newPointsAmount = Math.Min(parser.PredictionPointAmount, xx.Length);
                pointAmount = pointAmount + newPointsAmount;
                List<double[]> newPoints = new List<double[]>();
                newPoints = pointsArray.ToList();
                for (int j = 0; j < newPointsAmount; j++)
                {
                    double[] new_point = (double[])xx[j].Clone();
                    new_point[parser.FunctionDimension] = func(new_point);
                    newPoints.Add(new_point);
                }
                pointsArray = newPoints.ToArray();


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
            Console.WriteLine(" Avg err " + calc_err(func, pointsArray.ToList(), parser));
            return i;
        }

        private Classifiers.IClassifier get_cls(Tests.IFunction[] functions)
        {
            Classifiers.LabeledData[] ldata = collect_samples(functions);
            Classifiers.IClassifier cls = new Classifiers.RandomForest();
            Classifiers.RandomForestParams ps = new Classifiers.RandomForestParams(ldata, ldata.Length   /* samples count */,
                                                                                          featureCount   /* features count */,
                                                                                          2   /* classes count */,
                                                                                          100   /* trees count */,
                                                                                          1   /* count of features to do split in a tree */,
                                                                                          0.7 /* percent of a training set of samples  */
                                                                                              /* used to build individual trees. */);

            cls.train(ps);
            return cls;
        }

        private double[] build_features(double[] point, IFunction model, Grid grid, double[] distToKnownPoints, double[][] knownPoints = null, int index = -1)
        {
            this.featureCount = 4;

            // min, max in locality
            double maxNeighbours = double.MinValue;
            double minNeighbours = double.MaxValue;
            foreach (var neighbour in grid.Neighbours(index))
            {
                double[] calcNeighbour = (double[])grid.Node[neighbour].Clone();
                model.Calculate(calcNeighbour);
                if (calcNeighbour[calcNeighbour.Length - 1] < minNeighbours)
                {
                    minNeighbours = calcNeighbour[calcNeighbour.Length - 1];
                }
                if (calcNeighbour[calcNeighbour.Length - 1] > maxNeighbours)
                {
                    maxNeighbours = calcNeighbour[calcNeighbour.Length - 1];
                }
            }
            // current val
            double[] cuurentNode = (double[])grid.Node[index].Clone();
            model.Calculate(cuurentNode);
            double cuurentNodeVal = cuurentNode[cuurentNode.Length - 1];
            if (cuurentNodeVal < minNeighbours)
            {
                minNeighbours = cuurentNodeVal;
            }
            if (cuurentNodeVal > maxNeighbours)
            {
                maxNeighbours = cuurentNodeVal;
            }

            List<double[]> temp_points = new List<double[]>();
            temp_points = knownPoints.ToList();
            temp_points.Add((double[])grid.Node[index].Clone());
            Shepard new_model = new Shepard(model.N, temp_points.ToArray());
            temp_points.RemoveAt(temp_points.Count - 1);

            // build features vector
            double[] features = new double[featureCount];
            //features[0] = Math.Abs(cuurentNodeVal - maxNeighbours);
            //features[1] = Math.Abs(minNeighbours - cuurentNodeVal);
            features[0] = maxNeighbours - cuurentNodeVal;
            features[1] = cuurentNodeVal - minNeighbours;
            features[2] = distToKnownPoints[index];
            features[3] = check_new_aproximation((Shepard)model, new_model, grid, index);

            return features;
        }

        private Classifiers.LabeledData[] collect_samples(Tests.IFunction[] functions)
        {
            List<Classifiers.LabeledData> ldata = new List<Classifiers.LabeledData>();
            foreach (Tests.IFunction f in functions)
            {
                Parser p = new Parser(f.configFile, f.pointFile);
                int j = 0;
                while (j < 1)
                {
                    List<double[]> points = get_start_points(j, p, f.func);

                    for (int k = 0; k < 1; k++)
                    {
                        points.AddRange(testDefWayUntilGood(p, points.ToArray(), f.func).ToList<double[]>()); 
                    }

                    Shepard def_model = new Shepard(p.FunctionDimension, points.ToArray());
                    int[] count = new int[p.FunctionDimension]; for (int i = 0; i < p.FunctionDimension; i++) count[i] = (def_model.Min[i] == def_model.Max[i]) ? 1 : NGRID;
                    Grid grid = new Grid(def_model.N, def_model.M, def_model.Min, def_model.Max, count);
                    double[] dist = update_path_to_knowing_points(grid, points.ToArray(), p.FunctionDimension);

                    int n = grid.Node.Length;
                    for (int i = 0; i < grid.Node.Length; i++)
                    {
                        double[] cuurentNode = (double[])grid.Node[i].Clone();
                        def_model.Calculate(cuurentNode);
                        double cuurentNodeVal = cuurentNode[cuurentNode.Length - 1];

                        int pointClass = 0;
                        if (Math.Abs(f.func(grid.Node[i]) - cuurentNodeVal) > p.Approximation)
                        {
                            pointClass = 1;
                        }

                        double[] features = build_features(grid.Node[i], def_model, grid, dist, points.ToArray(), i);
                        ldata.Add(new Classifiers.LabeledData(features, pointClass));
                        featureCount = features.Length;
                    }

                    j++;
                }

            }
            return ldata.ToArray();
        }

        private List<double[]> get_start_points(int i, Parser parser, Func<double[], double> func)
        {
            if (i == 0)
            {
                return parser.Points.ToList();
            }
            else
            {
                List<double[]> points = new List<double[]>();
                Random random = new Random();
                for (int j = 0; j < parser.Points.Length; j++)
                {
                    double[] point = new double[parser.FunctionDimension + 1];
                    for (int k = 0; k < parser.FunctionDimension; k++)
                    {
                        point[k] = random.NextDouble() * (parser.Max[k] - parser.Min[k]) + parser.Min[k];
                    }
                    point[parser.FunctionDimension] = func(point);
                    points.Add(point);
                }
                return points;
            }
        }

    }
}
