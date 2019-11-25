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
        int TreesCount = 100;

        public override void run()
        {
            int interAmount = 0;
            Tests.AFunction[] functions = new Tests.AFunction[1] { new Tests.LGFunc() };
            //MLAlgorithms.IMLAlgorithm cls = get_cls(functions);

            MLAlgorithms.IMLAlgorithm rg = get_rg(functions);

            /*Console.WriteLine("Square Area Function Test START");
            interAmount = testWithRandomForest(Tests.SquareArea.config, Tests.SquareArea.config, Tests.SquareArea.func, Tests.SquareArea.derivative);
            Console.WriteLine("Square Area Function Test END in " + interAmount + " iterations");*/

            //126 interation
            Tests.SquaresProducts SquaresProducts = new Tests.SquaresProducts();
            Console.WriteLine("Test " + SquaresProducts.name + " START");
            interAmount = test(rg, SquaresProducts.configFile, SquaresProducts.pointFile, SquaresProducts);
            Console.WriteLine("Test " + SquaresProducts.name + " END in " + interAmount + " iterations");

            //Tests.SinXCosY SinXCosY = new Tests.SinXCosY();
            //Console.WriteLine("Test " + SinXCosY.name + " START");
            //interAmount = test(rg, SinXCosY.configFile, SinXCosY.pointFile, SinXCosY);
            //Console.WriteLine("Test " + SinXCosY.name + " END in " + interAmount + " iterations");

            /*Console.WriteLine(Tests.SinXCosXCosY.name +  " Test START");
            interAmount = testWithRandomForest(Tests.SinXCosXCosY.configFile, Tests.SinXCosXCosY.pointFile, Tests.SinXCosXCosY.func, Tests.SinXCosXCosY.derivative);
            Console.WriteLine("17.sinXconYxonX Test END in " + interAmount + " iterations");*/

            /*Console.WriteLine(Tests.SinFromSumOnSum.name + " Test START");
            interAmount = testWithRandomForest(Tests.SinFromSumOnSum.configFile, Tests.SinFromSumOnSum.pointFile, Tests.SinFromSumOnSum.func, Tests.SinFromSumOnSum.derivative);
            Console.WriteLine(Tests.SinFromSumOnSum.name + " Test END in " + interAmount + " iterations");*/

            //14 interation
            //Tests.SinFromSumOnSum SinFromSumOnSum = new Tests.SinFromSumOnSum();
            //Console.WriteLine(SinFromSumOnSum.name + " Test START");
            //interAmount = test(cls, SinFromSumOnSum.configFile, SinFromSumOnSum.pointFile, SinFromSumOnSum);
            //Console.WriteLine(SinFromSumOnSum.name + " Test END in " + interAmount + " iterations");

            // interation
            //Tests.LGFunc LGFunc = new Tests.LGFunc();
            //Console.WriteLine(LGFunc.name + " Test START");
            //interAmount = test(rg, LGFunc.configFile, LGFunc.pointFile, LGFunc, LGFunc.tableFile);
            //Console.WriteLine(LGFunc.name + " Test END in " + interAmount + " iterations");
        }

        private int test(MLAlgorithms.IMLAlgorithm cls, string configFile, string pointFile, Tests.AFunction function, string tableFile = null)
        {
            Parser parser = new Parser(configFile, pointFile, tableFile);
            if (tableFile != null)
            {
                function.table = parser.getTable();
            }
            double[][] pointsArray = parser.Points.ToList().ToArray();
            int pointAmount = pointsArray.Length;

            int i = 0;
            double maxErr = 100000000;
            int totalPointsCount = 0;
            while (i < 100000000 && maxErr > parser.Approximation)
            {
                Shepard model = new Shepard(parser.N_Dimension, pointsArray);
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

                    var response = function.func(new_point);
                    for (int k = 0; k < parser.M_Dimension; ++k)
                    {
                        new_point[parser.N_Dimension + k] = response[k];
                    }
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

                //Нужно не максимальную ошибку считать, а среднюю.
                double tempErr = 0;
                double totalErr = 0.0;
                for (int k = 0; k < new_points.Length; k++)
                {
                    var realPoint = newPoints[pointAmount - parser.PredictionPointAmount + k];
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

                    //Console.WriteLine(" \n " + err + " " + String.Join(", ", realFunctionVal) + " " + String.Join(", ", approxFunctionVal) + " \n ");
                    //if (err > tempErr)
                    //{
                    //    tempErr = err;
                    //}
                    totalErr += err;
                    //Console.WriteLine("f({0}) real val {1} predict val {2} err {3}", String.Join(", ", xx[k]), String.Join(", ", realFunctionVal), String.Join(", ", approxFunctionVal), err);
                    Console.WriteLine("{0};{1}", String.Join(";", xx[k]), String.Join(";", realFunctionVal));
                }
                maxErr = totalErr;
                //maxErr = totalErr / new_points.Length;
                totalPointsCount = pointsArray.Length;
                i++;
            }

       
            testResult(parser.N_Dimension, parser.M_Dimension, pointsArray, function.func);
            Console.WriteLine("Calc err avg " + calc_err(function.func, pointsArray.ToList(), parser));
            Console.WriteLine("{0} points was requested from the real function", totalPointsCount);

            return i;
        }

        private MLAlgorithms.IMLAlgorithm get_cls(Tests.AFunction[] functions)
        {
            MLAlgorithms.LabeledData[] ldata = collect_samples(functions);
            MLAlgorithms.IMLAlgorithm cls = new MLAlgorithms.RandomForest();
            MLAlgorithms.RandomForestParams ps = new MLAlgorithms.RandomForestParams(ldata, ldata.Length   /* samples count */,
                                                                                          featureCount   /* features count */,
                                                                                          2   /* classes count */,
                                                                                          100   /* trees count */,
                                                                                          1   /* count of features to do split in a tree */,
                                                                                          0.7 /* percent of a training set of samples  */
                                                                                              /* used to build individual trees. */);

            cls.train<int>(ps);
            return cls;
        }

        private MLAlgorithms.IMLAlgorithm get_rg(Tests.AFunction[] functions)
        {
            MLAlgorithms.LabeledData[] ldata = collect_samples(functions);
            MLAlgorithms.IMLAlgorithm ml = new MLAlgorithms.RandomForest();
            MLAlgorithms.RandomForestParams ps = new MLAlgorithms.RandomForestParams(ldata.ToArray(),
                                                                                     ldata.Length    /* samples count */,
                                                                                     featureCount   /* features count */,
                                                                                     1              /* classes count */,
                                                                                     TreesCount     /* trees count */,
                                                                                     3              /* count of features to do split in a tree */,
                                                                                     0.7            /* percent of a training set of samples  */
                                                                                                    /* used to build individual trees. */);
            

            ml.train<double>(ps);

            double trainModelPrecision;
            ml.validate<double>(ldata.ToArray(), out trainModelPrecision);

            Console.WriteLine("Model precision on training dataset: " + trainModelPrecision);

            return ml;
        }

        private double[] build_features_old(double[] point, IFunction model, Grid grid, double[] distToKnownPoints, double[][] knownPoints = null, int index = -1)
        {
            this.featureCount = 5;

            // min, max in locality
            double maxNeighbours = double.MinValue;
            double minNeighbours = double.MaxValue;
            foreach (var neighbour in grid.Neighbours(index))
            {
                double[] calcNeighbour = (double[])grid.Node[neighbour].Clone();
                model.Calculate(calcNeighbour);

                double calcNeighbourVal = 0;

                for (int l = 0; l < model.M; ++l)
                {
                    calcNeighbourVal = calcNeighbour[model.N + l];
                }
                calcNeighbourVal = calcNeighbourVal / model.M;

                if (calcNeighbour[calcNeighbour.Length - 1] < minNeighbours)
                {
                    //minNeighbours = calcNeighbour[calcNeighbour.Length - 1];
                    minNeighbours = calcNeighbourVal;
                }
                if (calcNeighbour[calcNeighbour.Length - 1] > maxNeighbours)
                {
                    //maxNeighbours = calcNeighbour[calcNeighbour.Length - 1];
                    maxNeighbours = calcNeighbourVal;
                }
            }
            // current val
            double[] curentNode = (double[])grid.Node[index].Clone();
            model.Calculate(curentNode);
            double curentNodeVal = 0;

            for (int l = 0; l < model.M; ++l)
            {
                curentNodeVal = curentNode[model.N + l];
            }
            curentNodeVal = curentNodeVal / model.M;
            
            if (curentNodeVal < minNeighbours)
            {
                minNeighbours = curentNodeVal;
            }
            if (curentNodeVal > maxNeighbours)
            {
                maxNeighbours = curentNodeVal;
            }

            List<double[]> temp_points = new List<double[]>();
            temp_points = knownPoints.ToList();
            temp_points.Add((double[])grid.Node[index].Clone());
            Shepard new_model = new Shepard(model.N, temp_points.ToArray());
            temp_points.RemoveAt(temp_points.Count - 1);

            // build features vector
            double[] features = new double[featureCount];
            //features[0] = Math.Abs(cuurentNodeVal - maxNeighbours);
            //features[1] = Math.Abs(minNeighbours - cuurentNodeVal);a
            features[0] = maxNeighbours - curentNodeVal;
            features[1] = curentNodeVal - minNeighbours;
            features[2] = distToKnownPoints[index];
            features[3] = check_new_aproximation((Shepard)model, new_model, grid, index);

            return features;
        }

        private double calcVal(int M, int N, double[] point)
        {
            //double val = 0;

            //for (int l = 0; l < M; ++l)
            //{
            //    val += point[N + l];
            //}
            //val = val / M;

            double val = 0;

            for (int l = 0; l < M; ++l)
            {
                val += Math.Pow(point[N + l], 2);
            }

            //return val;
            return Math.Sqrt(val);
            //return point[N + M - 1];
        }

        private double[] build_features(double[] point, IFunction model, Grid grid, double[] distToKnownPoints, double[][] knownPoints = null, int index = -1)
        {
            this.featureCount = 4;

            Analyzer analyzer = new Analyzer(model, knownPoints);
            analyzer.do_some_analyse();

            double[] monotonicNode = new double[model.N];
            for (int i = 0; i < model.N; i++)
            {
                int nextIndex = grid.nextNeighbours(index, i);
                int prevNextIndex = index;
                bool isUpMonotone = true;
                bool isDownMonotone = true;
                bool monotone = true;
                bool first = true;
                while (monotone && nextIndex != -1)
                {
                    if (analyzer.Domain(grid.Node[nextIndex]) != analyzer.Domain(grid.Node[prevNextIndex]))
                    {
                        if (first)
                        {
                            first = false;

                            if (calcVal(model.M, model.N, knownPoints[analyzer.Domain(grid.Node[nextIndex])]) <
                                calcVal(model.M, model.N, knownPoints[analyzer.Domain(grid.Node[prevNextIndex])]))
                            {
                                isUpMonotone = false;
                                isDownMonotone = true;
                            }
                            else
                            {
                                isUpMonotone = true;
                                isDownMonotone = false;
                            }

                        }
                        else
                        {
                            if (calcVal(model.M, model.N, knownPoints[analyzer.Domain(grid.Node[nextIndex])]) <
                                calcVal(model.M, model.N, knownPoints[analyzer.Domain(grid.Node[prevNextIndex])]) && !isDownMonotone)
                            {
                                monotone = false;
                                break;
                            }
                            if (calcVal(model.M, model.N, knownPoints[analyzer.Domain(grid.Node[nextIndex])]) >
                                calcVal(model.M, model.N, knownPoints[analyzer.Domain(grid.Node[prevNextIndex])]) && !isUpMonotone)
                            {
                                monotone = false;
                                break;
                            }
                            monotonicNode[i]++;
                        }
                    }
                    prevNextIndex = nextIndex;
                    nextIndex = grid.nextNeighbours(prevNextIndex, i);
                }


                int prevIndex = grid.prevNeighbours(index, i);
                int prevPrevIndex = index;
                monotone = true;
                first = true;
                while (monotone && prevIndex != -1)
                {
                    if (analyzer.Domain(grid.Node[prevIndex]) != analyzer.Domain(grid.Node[prevPrevIndex]))
                    {

                        if (calcVal(model.M, model.N, knownPoints[analyzer.Domain(grid.Node[prevIndex])]) >
                            calcVal(model.M, model.N, knownPoints[analyzer.Domain(grid.Node[prevPrevIndex])]) && !isDownMonotone)
                        {
                            monotone = false;
                            break;
                        }
                        if (calcVal(model.M, model.N, knownPoints[analyzer.Domain(grid.Node[prevIndex])]) <
                            calcVal(model.M, model.N, knownPoints[analyzer.Domain(grid.Node[prevPrevIndex])]) && !isUpMonotone)
                        {
                            monotone = false;
                            break;
                        }
                        monotonicNode[i]++;
                    }
                    prevPrevIndex = prevIndex;
                    prevIndex = grid.prevNeighbours(prevPrevIndex, i);
                }
            }


            // min, max in locality
            double maxNeighbours = double.MinValue;
            double minNeighbours = double.MaxValue;
            foreach (var neighbour in grid.Neighbours(index))
            {
                double[] calcNeighbour = (double[])grid.Node[neighbour].Clone();
                model.Calculate(calcNeighbour);

                double calcNeighbourVal = calcVal(model.M, model.N, calcNeighbour);

                if (calcNeighbour[calcNeighbour.Length - 1] < minNeighbours)
                {
                    //minNeighbours = calcNeighbour[calcNeighbour.Length - 1];
                    minNeighbours = calcNeighbourVal;
                }
                if (calcNeighbour[calcNeighbour.Length - 1] > maxNeighbours)
                {
                    //maxNeighbours = calcNeighbour[calcNeighbour.Length - 1];
                    maxNeighbours = calcNeighbourVal;
                }
            }
            // current val
            double[] curentNode = (double[])grid.Node[index].Clone();
            model.Calculate(curentNode);
            double curentNodeVal = calcVal(model.M, model.N, curentNode);

            if (curentNodeVal < minNeighbours)
            {
                minNeighbours = curentNodeVal;
            }
            if (curentNodeVal > maxNeighbours)
            {
                maxNeighbours = curentNodeVal;
            }

            List<double[]> temp_points = new List<double[]>();
            temp_points = knownPoints.ToList();
            temp_points.RemoveAt(analyzer.Domain(grid.Node[index]));
            Shepard new_model = new Shepard(model.N, temp_points.ToArray());
            double[] old_model_point = (double[])grid.Node[index].Clone();
            model.Calculate(old_model_point);
            double[] new_model_point = (double[])grid.Node[index].Clone();
            new_model.Calculate(new_model_point);

            double err = 0;
            for (int k = 0; k < model.M; ++k)
            {
                err = Math.Abs(old_model_point[model.N + k] - new_model_point[model.N + k]);
            }

            err = err / model.M;

            // build features vector

            double[] features = new double[featureCount];
            features[0] = Math.Abs(curentNodeVal - maxNeighbours);
            features[1] = Math.Abs(minNeighbours - curentNodeVal);
            //features[0] = maxNeighbours - curentNodeVal;
            //features[1] = curentNodeVal - minNeighbours;
            //ok


            //for (int i = 0; i < model.N; i++)
            //{
            //    features[i] = monotonicNode[i];
            //}
            features[2] = distToKnownPoints[index];
            features[3] = err;
            //features[4] = monotonicNode[0];

            return features;
        }

        private MLAlgorithms.LabeledData[] collect_samples(Tests.AFunction[] functions)
        {
            List<MLAlgorithms.LabeledData> ldata = new List<MLAlgorithms.LabeledData>();
            foreach (Tests.AFunction f in functions)
            {
                Parser p = new Parser(f.configFile, f.pointFile, f.tableFile);
                if (f.tableFile != null)
                {
                    f.table = p.getTable();
                }
                int j = 0;
                while (j < 1)
                {
                    List<double[]> points = get_start_points(j, p, f.func);

                    for (int k = 0; k < 1; k++)
                    {
                        points.AddRange(testDefWayUntilGood(p, points.ToArray(), f.func).ToList<double[]>()); 
                    }

                    Shepard def_model = new Shepard(p.N_Dimension, points.ToArray());
                    int[] count = new int[p.N_Dimension]; for (int i = 0; i < p.N_Dimension; i++) count[i] = (def_model.Min[i] == def_model.Max[i]) ? 1 : NGRID;
                    Grid grid = new Grid(def_model.N, def_model.M, def_model.Min, def_model.Max, count);
                    double[] dist = update_path_to_knowing_points(grid, points.ToArray(), p.N_Dimension);

                    int n = grid.Node.Length;
                    for (int i = 0; i < grid.Node.Length; i++)
                    {
                        double[] cuurentNode = (double[])grid.Node[i].Clone();
                        def_model.Calculate(cuurentNode);

                        double[] approxFunctionVal = new double[def_model.M];
                        for (int k = 0; k < def_model.M; ++k)
                        {
                            approxFunctionVal[k] = cuurentNode[def_model.N + k];
                        }

                        var realFunctionVal = f.func(grid.Node[i]);

                        double[] diffs = realFunctionVal.Zip(approxFunctionVal, (d1, d2) => Math.Abs(d1 - d2)).ToArray();

                        double err = (diffs.Sum() / diffs.Length);

                        //int pointClass = 0;
                        //if (err > p.Approximation)
                        //{
                        //    pointClass = 1;
                        //}
                        double pointClass = err;

                        double[] features = build_features(grid.Node[i], def_model, grid, dist, points.ToArray(), i);
                        ldata.Add(new MLAlgorithms.LabeledData(features, pointClass));
                        featureCount = features.Length;
                    }

                    j++;
                }

            }
            return ldata.ToArray();
        }

        private List<double[]> get_start_points(int i, Parser parser, Func<double[], double[]> func)
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
                    double[] point = new double[parser.NM_Dimension];
                    for (int k = 0; k < parser.N_Dimension; k++)
                    {
                        point[k] = random.NextDouble() * (parser.Max[k] - parser.Min[k]) + parser.Min[k];
                    }

                    var response = func(point);
                    for (int l = 0; l < parser.M_Dimension; ++l)
                    {
                        point[parser.N_Dimension + l] = response[l];
                    }
                    points.Add(point);
                }
                return points;
            }
        }

    }
}
