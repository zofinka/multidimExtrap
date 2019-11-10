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
            Tests.IFunction[] functions = new Tests.IFunction[1] { new Tests.SinXCosY() };
            collect_samples(functions);
            MLAlgorithms.IMLAlgorithm rg = get_rg();

            Tests.SinXCosXCosY SinXCosY = new Tests.SinXCosXCosY();
            Console.WriteLine(SinXCosY.name + " Test START");
            interAmount = test(rg, SinXCosY);
            Console.WriteLine(SinXCosY.name + " Test END in " + interAmount + " iterations");

            /*Tests.SinXCosXCosY SinXCosXCosY = new Tests.SinXCosXCosY();
            Console.WriteLine(SinXCosXCosY.name + " Test START");
            interAmount = test(cls, SinXCosXCosY);
            Console.WriteLine(SinXCosXCosY.name + " Test END in " + interAmount + " iterations");

            Tests.SinFromSumOnSum SinFromSumOnSum = new Tests.SinFromSumOnSum();
            Console.WriteLine(SinFromSumOnSum.name + " Test START");
            interAmount = test(cls, SinFromSumOnSum);
            Console.WriteLine(SinFromSumOnSum.name + " Test END in " + interAmount + " iterations");*/

        }

        public int test(MLAlgorithms.IMLAlgorithm rg, Tests.IFunction function)
        {

            Parser parserToLearn = new Parser(function.configFile, function.pointFile);
            Parser parser = new Parser(function.configFile, function.pointFile);
            int pointAmount = parser.PointAmount;

            double[][] points = new double[parser.PointAmount][];
            for (int j = 0; j < parser.PointAmount; j++)
            {
                points[j] = (double[])parser.Points[j].Clone();
            }

            int i = 0;
            double maxErr = 10;
            while (i < 1000 && maxErr > parser.Approximation)
            {
                Shepard model = new Shepard(parser.FunctionDimension, points);
                Analyzer analyzer = new Analyzer(model, points);
                analyzer.do_random_forest_analyse(rg, build_features);

                double[][] xx = analyzer.Result;
                int newPointsAmount = Math.Min(parser.PredictionPointAmount, xx.Length);
                pointAmount = pointAmount + newPointsAmount;
                List<double[]> newPoints = new List<double[]>();
                newPoints = points.ToList();
                for (int j = 0; j < newPointsAmount; j++)
                {
                    double[] new_point = (double[])xx[j].Clone();
                    new_point[parser.FunctionDimension] = function.func(new_point);
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
            testResult(parser.FunctionDimension, points, function.func);
            Console.WriteLine(" Avg err " + calc_err(function.func, points.ToList(), parser));
            return i;
        } 

        private MLAlgorithms.IMLAlgorithm get_rg()
        {
            MLAlgorithms.IMLAlgorithm ml = new MLAlgorithms.RandomForest();

            MLAlgorithms.RandomForestParams ps = new MLAlgorithms.RandomForestParams(ldata.ToArray(), ldata.Count   /* samples count */,
                                                                                     featureCount   /* features count */,
                                                                                     1   /* classes count */,
                                                                                     TreesCount   /* trees count */,
                                                                                     1   /* count of features to do split in a tree */,
                                                                                     0.7 /* percent of a training set of samples  */
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

        private void collect_samples(Tests.IFunction[] functions)
        {
            foreach (Tests.IFunction f in functions)
            {
                Parser p = new Parser(f.configFile, f.pointFile);
                int j = 0;
                while (j < 5)
                {
                    List<double[]> points = get_start_points(j, p, f.func);
                    int i = 0;
                    double err = 1000;
                    while (i < 25 && err > p.Approximation)
                    {
                        List<double[]> new_points = get_best_points(points.ToArray(), p);
                        update_samples(new_points, p);
                        //update_good_samples(new_points, p);

                        List<double[]> new_best_points = get_first_best_points(new_points.ToArray(), p.FunctionDimension);

                        for (int k = 0; k < new_best_points.Count; k++)
                        {
                            double[] new_point = new double[new_best_points[k].Length];
                            new_best_points[k].CopyTo(new_point, 0);
                            new_point[new_point.Length - 1] = 0;
                            points.Add(new_point);
                        }
                        err = calc_err(f.func, points, p);
                        i++;
                    }
                    //update_bad_samples(points, p);
                    j++;
                }
                
            }
        }

        private List<double[]> get_first_best_points(double[][] points, int functionDemention)
        {
            int[] sorted_index = new int[points.Length];
            for (int i = 0; i < points.Length; i++)
            {
                sorted_index[i] = i;
            }
            
            for (int i = 0; i < sorted_index.Length - 1; i++)
            {
                for (int j = i + 1; j < sorted_index.Length; j++)
                {
                    if (points[sorted_index[i]][functionDemention] < points[sorted_index[j]][functionDemention])
                    {
                        int temp = sorted_index[i];
                        sorted_index[i] = sorted_index[j];
                        sorted_index[j] = 0;
                    }
                }
            }
            List<double[]> new_points = new List<double[]>();
            for (int i = 0; i < sorted_index.Length; i++)
            {
                //if (points[sorted_index[i]][functionDemention] < THRESHOLD || i > 3)
                if (i > 9)
                {
                    break;
                }
                new_points.Add((double[])points[sorted_index[i]].Clone());
            }
            return new_points;
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

        

        private double[] build_features(double[] point, IFunction model, Grid grid, double[] distToKnownPoints, double[][] knownPoints = null, int index = -1)
        {
            featureCount = 3;
            double[] features = new double[featureCount];
            // min, max in locality
            double maxNeighboursVal = double.MinValue;
            double[] maxNeighbours = new double[point.Length];
            double minNeighboursVal = double.MaxValue;
            double[] minNeighbours = new double[point.Length];
            grid.ToIndex(point, out index);
            foreach (var neighbour in grid.Neighbours(index))
            {
                double[] calcNeighbour = (double[])grid.Node[neighbour].Clone();
                model.Calculate(calcNeighbour);
                if (calcNeighbour[calcNeighbour.Length - 1] < minNeighboursVal)
                {
                    minNeighboursVal = calcNeighbour[calcNeighbour.Length - 1];
                    maxNeighbours = (double[])calcNeighbour.Clone();
                }
                if (calcNeighbour[calcNeighbour.Length - 1] > maxNeighboursVal)
                {
                    maxNeighboursVal = calcNeighbour[calcNeighbour.Length - 1];
                    minNeighbours = (double[])calcNeighbour.Clone();
                }
            }
            // current val
            double[] curentNode = (double[])grid.Node[index].Clone();
            model.Calculate(curentNode);
            double curentNodeVal = curentNode[curentNode.Length - 1];
            if (curentNodeVal < minNeighboursVal)
            {
                minNeighboursVal = curentNodeVal;
            }
            if (curentNodeVal > maxNeighboursVal)
            {
                maxNeighboursVal = curentNodeVal;
            }

            
            features[0] = Math.Abs(curentNodeVal - maxNeighboursVal);
          //  features[1] = distanceX(curentNode, maxNeighbours, model.N);
            features[1] = Math.Abs(curentNodeVal - minNeighboursVal);
           // features[2] = distToKnownPoints[index];
            //  features[3] = distanceX(curentNode, maxNeighbours, model.N);

            return features;
        }

        private void update_good_samples(List<double[]> points, Parser paraser)
        {
            Shepard def_model = new Shepard(paraser.FunctionDimension, points.ToArray());

            int[] count = new int[def_model.N];
            for (int i = 0; i < def_model.N; i++) count[i] = (def_model.Min[i] == def_model.Max[i]) ? 1 : NGRID;
            Grid grid = new Grid(def_model.N, def_model.M, def_model.Min, def_model.Max, count);
            double[] dist = update_path_to_knowing_points(grid, points.ToArray(), paraser.FunctionDimension);
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
            Shepard def_model = new Shepard(parser.FunctionDimension, points.ToArray());

            int[] count = new int[def_model.N];
            for (int i = 0; i < def_model.N; i++) count[i] = (def_model.Min[i] == def_model.Max[i]) ? 1 : NGRID;
            Grid grid = new Grid(def_model.N, def_model.M, def_model.Min, def_model.Max, count);
            double[] dist = update_path_to_knowing_points(grid, points.ToArray(), parser.FunctionDimension);
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

        private void update_samples(List<double[]> points, Parser paraser)
        {
            Shepard def_model = new Shepard(paraser.FunctionDimension, points.ToArray());
            int bad_point = 0;
            int good_point = 0;

            int[] count = new int[def_model.N];
            for (int i = 0; i < def_model.N; i++) count[i] = (def_model.Min[i] == def_model.Max[i]) ? 1 : NGRID;
            Grid grid = new Grid(def_model.N, def_model.M, def_model.Min, def_model.Max, count);
            double[] dist = update_path_to_knowing_points(grid, points.ToArray(), paraser.FunctionDimension);
            foreach (double[] p in points)
            {
                if (p[p.Length - 1] < THRESHOLD)
                {
                    double[] feature = build_features(p, def_model, grid, dist);
                    ldata.Add(new MLAlgorithms.LabeledData(feature, 0));
                    good_point++;
                }
            }
            foreach (double[] p in points)
            {
                if (p[p.Length - 1] > THRESHOLD)
                {
                    double[] feature = build_features(p, def_model, grid, dist);
                    ldata.Add(new MLAlgorithms.LabeledData(feature, 1));
                    bad_point++;
                }
                if (bad_point == good_point)
                {
                    break;
                }
            }

        }

        // return all points with value of getting aproximation better in last index
        private List<double[]> get_best_points(double[][] points, Parser parser)
        {
            Shepard def_model = new Shepard(parser.FunctionDimension, points);
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
                double[] new_point = new double[parser.FunctionDimension + 1];
                grid.Node[i].CopyTo(new_point, 0);
                temp_points.Add(new_point);
                Shepard new_model = new Shepard(parser.FunctionDimension, temp_points.ToArray());
                temp_points.RemoveAt(temp_points.Count - 1);
                new_point[parser.FunctionDimension] = check_new_aproximation(def_model, new_model, grid, i);
                new_points.Add(new_point);
            }

            return new_points;
        }        
    }
}
