using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solver
{
    class TestRandomForestBestAprxClass: Test
    {
        int featureCount = 0;
        List<MLAlgorithms.LabeledData> ldata = new List<MLAlgorithms.LabeledData>();
        int TreesCount = 100;
        double THRESHOLD = 1;

        const int NGRID = 100;

        override public void run()
        {
            int interAmount = 0;
            Tests.AFunction[] functions = new Tests.AFunction[1] { new Tests.SinXCosY() };
            collect_samples(functions);
            MLAlgorithms.IMLAlgorithm rg = get_rg();

            //Tests.SinXCosY SinXCosY = new Tests.SinXCosY();
            //Console.WriteLine(SinXCosY.name + " Test START");
            //interAmount = test(rg, SinXCosY);
            //Console.WriteLine(SinXCosY.name + " Test END in " + interAmount + " iterations");

            /*Tests.SinXCosXCosY SinXCosXCosY = new Tests.SinXCosXCosY();
            Console.WriteLine(SinXCosXCosY.name + " Test START");
            interAmount = test(cls, SinXCosXCosY);
            Console.WriteLine(SinXCosXCosY.name + " Test END in " + interAmount + " iterations");

            Tests.SinFromSumOnSum SinFromSumOnSum = new Tests.SinFromSumOnSum();
            Console.WriteLine(SinFromSumOnSum.name + " Test START");
            interAmount = test(cls, SinFromSumOnSum);
            Console.WriteLine(SinFromSumOnSum.name + " Test END in " + interAmount + " iterations");*/

            // interation
            Tests.LGFunc LGFunc = new Tests.LGFunc();
            Console.WriteLine(LGFunc.name + " Test START");
            interAmount = test(rg, LGFunc, LGFunc.tableFile);
            Console.WriteLine(LGFunc.name + " Test END in " + interAmount + " iterations");

        }

        public int test(MLAlgorithms.IMLAlgorithm rg, Tests.AFunction function, string tableFile = null)
        {
            Parser parser = new Parser(function.configFile, function.pointFile, tableFile);
            if (tableFile != null)
            {
                function.table = parser.getTable();
            }

            int pointAmount = parser.PointAmount;

            double[][] points = new double[parser.PointAmount][];
            for (int j = 0; j < parser.PointAmount; j++)
            {
                points[j] = (double[])parser.Points[j].Clone();
            }

            int i = 0;
            double maxErr = 10;
            //while (i < 1000 && maxErr > parser.Approximation)
            int totalPointsCount = 0;

            double oldIntegAccuracy = 10000000;
            double IntegAccuracy = 10000000 - 100;

            //while (maxErr > parser.Approximation)
            while (IntegAccuracy > parser.Approximation)
            {
                Shepard model = new Shepard(parser.N_Dimension, points);
                Analyzer analyzer = new Analyzer(model, points);
                analyzer.do_random_forest_analyse(rg, build_features);

                //this code to dichotomy
                double[][] xx = analyzer.Result;
                int newPointsAmount = Math.Min(parser.PredictionPointAmount, xx.Length);
                pointAmount = pointAmount + newPointsAmount;
                List<double[]> newPoints = new List<double[]>();
                newPoints = points.ToList();
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
                points = newPoints.ToArray();


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
                    var realPoint = points[pointAmount - parser.PredictionPointAmount + k];
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
                    if (err > tempErr)
                    {
                        tempErr = err;
                    }
                    //Console.WriteLine("f({0}) real val {1} predict val {2} err {3}", String.Join(", ", xx[k]), String.Join(", ", realFunctionVal), String.Join(", ", approxFunctionVal), err);
                    Console.WriteLine("{0};{1}", String.Join(";", xx[k]), String.Join(";", realFunctionVal));
                }
                maxErr = tempErr;
                i++;
                //maxErr = totalErr / new_points.Length;
                totalPointsCount = points.Length;
                i++;
                oldIntegAccuracy = IntegAccuracy;
                IntegAccuracy = calc_err(function.func, points.ToList(), parser);
                Console.WriteLine("Calc err avg " + IntegAccuracy);
            }
            //   testResult(parser.N_Dimension, parser.M_Dimension, points, function.func);
            //   Console.WriteLine(" Avg err " + calc_err(function.func, points.ToList(), parser));
            testResult(parser.N_Dimension, parser.M_Dimension, points, function.func);
            Console.WriteLine("Calc err avg " + calc_err(function.func, points.ToList(), parser));
            Console.WriteLine("{0} points was requested from the real function", totalPointsCount);
            return i;
        } 

        private MLAlgorithms.IMLAlgorithm get_rg()
        {
            MLAlgorithms.IMLAlgorithm ml = new MLAlgorithms.RandomForest();

            MLAlgorithms.RandomForestParams ps = new MLAlgorithms.RandomForestParams(ldata.ToArray(),
                                                                                     ldata.Count    /* samples count */,
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

        private void analyze_points()
        {

        }

        private void collect_samples(Tests.AFunction[] functions)
        {
            foreach (Tests.AFunction f in functions)
            {
                Parser p = new Parser(f.configFile, f.pointFile);
                if (f.tableFile != null)
                {
                    f.table = p.getTable();
                }
                int j = 0;
                while (j < 5)
                {
                    List<double[]> knowing_points = get_start_points(j, p, f.func);
                    int i = 0;
                    double err = 1000;
                    while (i < 25 && err > p.Approximation)
                    {
                        List<double[]> new_points = get_best_points(knowing_points.ToArray(), p);
                        update_samples(new_points, knowing_points.ToArray(), p);
                        //update_good_samples(new_points, p);

                        List<double[]> new_best_points = get_first_best_points(new_points.ToArray(), p.N_Dimension);

                        for (int k = 0; k < new_best_points.Count; k++)
                        {
                            double[] new_point = new double[new_best_points[k].Length];
                            new_best_points[k].CopyTo(new_point, 0);
                            new_point[new_point.Length - 1] = 0;
                            knowing_points.Add(new_point);
                        }
                        err = calc_err(f.func, knowing_points, p);
                        i++;
                    }
                    //update_bad_samples(points, p);
                    j++;
                }
                
            }
        }

        private List<double[]> get_first_best_points(double[][] points, int functionDemention)
        {
            //int[] sorted_index = new int[points.Length];
            //for (int i = 0; i < points.Length; i++)
            //{
            //    sorted_index[i] = i;
            //}

            //for (int i = 0; i < sorted_index.Length - 1; i++)
            //{
            //    for (int j = i + 1; j < sorted_index.Length; j++)
            //    {
            //        if (points[sorted_index[i]][functionDemention] < points[sorted_index[j]][functionDemention])
            //        {
            //            int temp = sorted_index[i];
            //            sorted_index[i] = sorted_index[j];
            //            sorted_index[j] = 0;
            //        }
            //    }
            //}

            List<Tuple<double, int>> improveDiffs = new List<Tuple<double, int>>();

            for(int i = 0; i < points.Length; ++i)
            {
                improveDiffs.Add(new Tuple<double, int>(points[i][functionDemention], i));
            }

            var sortedImproveDiffs = improveDiffs.OrderByDescending((t) => t.Item1).ToList();

            List<double[]> new_points = new List<double[]>();
            for (int i = 0; i < 9; i++)
            {
                new_points.Add((double[])(points[sortedImproveDiffs[i].Item2]).Clone());
            }

            return new_points;
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



        //private double[] build_features(double[] point, IFunction model, Grid grid, double[] distToKnownPoints, double[][] knownPoints = null, int index = -1)
        //{ 
        //    // на сколько образующая домен точка близка
        //    // сколько до и после монотонно 
        //    // расстояние до известной точки 
        //    featureCount = 3 + (point.Length - 1);
        //    double[] features = new double[featureCount];
        //    // min, max in locality
        //    double maxNeighboursVal = double.MinValue;
        //    double[] maxNeighbours = new double[point.Length];
        //    double minNeighboursVal = double.MaxValue;
        //    double[] minNeighbours = new double[point.Length];
        //    grid.ToIndex(point, out index);
        //    foreach (var neighbour in grid.Neighbours(index))
        //    {
        //        double[] calcNeighbour = (double[])grid.Node[neighbour].Clone();
        //        model.Calculate(calcNeighbour);
        //        if (calcNeighbour[calcNeighbour.Length - 1] < minNeighboursVal)
        //        {
        //            minNeighboursVal = calcNeighbour[calcNeighbour.Length - 1];
        //            minNeighbours = (double[])calcNeighbour.Clone();
        //        }
        //        if (calcNeighbour[calcNeighbour.Length - 1] > maxNeighboursVal)
        //        {
        //            maxNeighboursVal = calcNeighbour[calcNeighbour.Length - 1];
        //            maxNeighbours = (double[])calcNeighbour.Clone();
        //        }
        //    }
        //    // current val
        //    double[] curentNode = (double[])grid.Node[index].Clone();
        //    model.Calculate(curentNode);
        //    double curentNodeVal = curentNode[curentNode.Length - 1];
        //    if (curentNodeVal < minNeighboursVal)
        //    {
        //        minNeighboursVal = curentNodeVal;
        //    }
        //    if (curentNodeVal > maxNeighboursVal)
        //    {
        //        maxNeighboursVal = curentNodeVal;
        //    }


        //    features[0] = Math.Abs(curentNodeVal - maxNeighboursVal);
        //  //  features[1] = distanceX(curentNode, maxNeighbours, model.N);
        //    features[1] = Math.Abs(curentNodeVal - minNeighboursVal);
        //    features[2] = distToKnownPoints[index];

        //    for (int i = 0; i < point.Length - 1; ++i)
        //    {
        //        features[3 + i] = point[i];
        //    }
        //    //  features[3] = distanceX(curentNode, maxNeighbours, model.N);

        //    return features;
        //}

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
            Analyzer analyzer = new Analyzer(model, knownPoints);
            analyzer.do_some_analyse();
            //double[] monotonicNode1 = getMonotonicNode(grid);
            int[] monotonicNode = new int[model.N];
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
                        bool isDown = (calcVal(model.M, model.N, knownPoints[analyzer.Domain(grid.Node[nextIndex])]) <
                                      calcVal(model.M, model.N, knownPoints[analyzer.Domain(grid.Node[prevNextIndex])]));
                        if (first)
                        {
                            isUpMonotone = !isDown;
                            isDownMonotone = isDown;
                            monotonicNode[i]++;
                            first = false;
                        }
                        else
                        {
                            if ((isDown && !isDownMonotone) || (!isDown && !isUpMonotone))
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


                int prevIndex = index;
                int prevPrevIndex = grid.prevNeighbours(index, i);
                monotone = true;
                first = true;
                while (monotone && prevPrevIndex != -1)
                {
                    if (analyzer.Domain(grid.Node[prevIndex]) != analyzer.Domain(grid.Node[prevPrevIndex]))
                    {
                        bool isDown = (calcVal(model.M, model.N, knownPoints[analyzer.Domain(grid.Node[prevIndex])]) >
                                       calcVal(model.M, model.N, knownPoints[analyzer.Domain(grid.Node[prevPrevIndex])]));

                        if ((isDown && !isDownMonotone) || (!isDown && !isUpMonotone))
                        {
                            monotone = false;
                            break;
                        }
                        monotonicNode[i]++;
                    }
                    prevIndex = prevPrevIndex;
                    prevPrevIndex = grid.prevNeighbours(prevPrevIndex, i);
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
                double newErr = Math.Abs(old_model_point[model.N + k] - new_model_point[model.N + k]);
                if (newErr > err)
                {
                    err = newErr;
                }
            }

            //err = err / model.M;

            // build features vector

            this.featureCount = 4 + monotonicNode.Length;
            double[] features = new double[featureCount];
            //features[0] = Math.Abs(curentNodeVal - maxNeighbours);
            //features[1] = Math.Abs(minNeighbours - curentNodeVal);
            features[0] = maxNeighbours - curentNodeVal;
            features[1] = curentNodeVal - minNeighbours;
            //ok


            //for (int i = 0; i < model.N; i++)
            //{
            //    features[i] = monotonicNode[i];
            //}
            features[2] = distToKnownPoints[index];
            features[3] = err;
            for (int i = 0; i < monotonicNode.Length; i++)
            {
                features[4 + i] = monotonicNode[i];
            }

            return features;
        }



        //private double[] build_features_new(double[] point, IFunction model, Grid grid, double[] distToKnownPoints, double[][] knownPoints = null, int index = -1)
        //{
        //    // на сколько образующая домен точка близка
        //    // сколько до и после монотонно 
        //    // расстояние до известной точки 

        //    featureCount = 3 + (point.Length - 1);
        //    double[] features = new double[featureCount];
        //    // min, max in locality
        //    double maxNeighboursVal = double.MinValue;
        //    double[] maxNeighbours = new double[point.Length];
        //    double minNeighboursVal = double.MaxValue;
        //    double[] minNeighbours = new double[point.Length];
        //    grid.ToIndex(point, out index);
        //    foreach (var neighbour in grid.Neighbours(index))
        //    {
        //        double[] calcNeighbour = (double[])grid.Node[neighbour].Clone();
        //        model.Calculate(calcNeighbour);
        //        if (calcNeighbour[calcNeighbour.Length - 1] < minNeighboursVal)
        //        {
        //            minNeighboursVal = calcNeighbour[calcNeighbour.Length - 1];
        //            minNeighbours = (double[])calcNeighbour.Clone();
        //        }
        //        if (calcNeighbour[calcNeighbour.Length - 1] > maxNeighboursVal)
        //        {
        //            maxNeighboursVal = calcNeighbour[calcNeighbour.Length - 1];
        //            maxNeighbours = (double[])calcNeighbour.Clone();
        //        }
        //    }
        //    // current val
        //    double[] curentNode = (double[])grid.Node[index].Clone();
        //    model.Calculate(curentNode);
        //    double curentNodeVal = curentNode[curentNode.Length - 1];
        //    if (curentNodeVal < minNeighboursVal)
        //    {
        //        minNeighboursVal = curentNodeVal;
        //    }
        //    if (curentNodeVal > maxNeighboursVal)
        //    {
        //        maxNeighboursVal = curentNodeVal;
        //    }


        //    features[0] = Math.Abs(curentNodeVal - maxNeighboursVal);
        //    //  features[1] = distanceX(curentNode, maxNeighbours, model.N);
        //    features[1] = Math.Abs(curentNodeVal - minNeighboursVal);
        //    features[2] = distToKnownPoints[index];

        //    for (int i = 0; i < point.Length - 1; ++i)
        //    {
        //        features[3 + i] = point[i];
        //    }
        //    //  features[3] = distanceX(curentNode, maxNeighbours, model.N);

        //    return features;
        //}

        private void update_good_samples(List<double[]> points, Parser paraser)
        {
            Shepard def_model = new Shepard(paraser.N_Dimension, points.ToArray());

            int[] count = new int[def_model.N];
            for (int i = 0; i < def_model.N; i++) count[i] = (def_model.Min[i] == def_model.Max[i]) ? 1 : NGRID;
            Grid grid = new Grid(def_model.N, def_model.M, def_model.Min, def_model.Max, count);
            double[] dist = update_path_to_knowing_points(grid, points.ToArray(), paraser.N_Dimension);
            foreach (double[] p in points)
            {
                if (p[p.Length - 1] < THRESHOLD)
                {
                    double[] feature = build_features(p, def_model, grid, dist);
                    ldata.Add(new MLAlgorithms.LabeledData(feature, 0));
                }
            }
        }

        private void update_bad_samples(List<double[]> points, Parser parser)
        {
            Shepard def_model = new Shepard(parser.N_Dimension, points.ToArray());

            int[] count = new int[def_model.N];
            for (int i = 0; i < def_model.N; i++) count[i] = (def_model.Min[i] == def_model.Max[i]) ? 1 : NGRID;
            Grid grid = new Grid(def_model.N, def_model.M, def_model.Min, def_model.Max, count);
            double[] dist = update_path_to_knowing_points(grid, points.ToArray(), parser.N_Dimension);
            for (int i = 0; i < grid.Node.Length; i++)
            {
                bool eql = true;
                foreach (double[] p in points)
                {
                    for (int j = 0; j < grid.Node[i].Length - 1; j++)
                    {
                        if (grid.Node[i][j] != p[j])
                        {
                            eql = false;
                            break;
                        }
                    }
                }
                if (!eql)
                {
                    double[] feature = build_features(grid.Node[i], def_model, grid, dist);
                    ldata.Add(new MLAlgorithms.LabeledData(feature, 0));
                }
            }
            
        }

        private void update_samples(List<double[]> points, double[][] knowingPoints, Parser paraser)
        {
            Shepard def_model = new Shepard(paraser.N_Dimension, points.ToArray());
            int bad_point = 0;
            int good_point = 0;

            int[] count = new int[def_model.N];
            for (int i = 0; i < def_model.N; i++) count[i] = (def_model.Min[i] == def_model.Max[i]) ? 1 : NGRID;
   
            Grid grid = new Grid(def_model.N, def_model.M, def_model.Min, def_model.Max, count);

            double[] dist = update_path_to_knowing_points(grid, knowingPoints, paraser.N_Dimension);

            foreach (double[] p in points)
            {
                int index;
                grid.ToIndex(p, out index);
                double[] feature = build_features(p, def_model, grid, dist, knowingPoints, index);
                ldata.Add(new MLAlgorithms.LabeledData(feature, p[p.Length - 1]));
            }
        }

        // return all points with value of getting aproximation better in last index
        private List<double[]> get_best_points(double[][] points, Parser parser)
        {
            Shepard def_model = new Shepard(parser.N_Dimension, points);
            List<double[]> new_points = new List<double[]>();
            List<double[]> temp_points = new List<double[]>();
            temp_points = points.ToList();

            int[] count = new int[def_model.N];
            for (int i = 0; i < def_model.N; i++)
            {
                count[i] = (def_model.Min[i] == def_model.Max[i]) ? 1 : NGRID;
            }

            Grid grid = new Grid(def_model.N, def_model.M, def_model.Min, def_model.Max, count);
            for (int i = 0; i < grid.Node.Length; i++)
            {
                double[] new_point = new double[parser.NM_Dimension];
                grid.Node[i].CopyTo(new_point, 0);
                temp_points.Add(new_point);
                Shepard new_model = new Shepard(parser.N_Dimension, temp_points.ToArray());
                temp_points.Remove(new_point);
                new_point[parser.N_Dimension] = check_new_aproximation(def_model, new_model, grid, i);
                new_points.Add(new_point);
            }

            return new_points;
        }        
    }
}
