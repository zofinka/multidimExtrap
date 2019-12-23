using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    public interface ISolver
    {
        void setConfig(IConfig config);
        void setCLassifier(Func<double[], double[]>[] functions, Task task);
        double calculate(Task task);
        void testResult(Func<double[], double[]> func);
        void testResult(int nFunctionDimension, int mFunctionDimension, Func<double[], double[]> func);
    }
    public class Solver : ISolver
    {
        private IConfig config;
        private IAnalyzer analyzer;
        private IApprox approx;
        private IClassifierML cls;

        //public static Func<double[], double[]>[] studyFunc;
        public static Func<double[], double[]> func;
        //public static Func<double[], double[]> derivativeFunc;
        public static Func<double[], double[]> derivativeFunc;

        const int NGRID = 100;
        int featureCount = 0;
        int TreesCount = 100;

        public Solver()
        {
            func = TestFunctionGetter.getInstance(Approx.testFunction).GetFunc();
            derivativeFunc = TestFunctionGetter.getInstance(Approx.testFunction).GetDerivative();
        }

        public void setCLassifier(Func<double[], double[]>[] functions, Task task)
        {
            cls = get_cls(functions, task);
        }

        public double calculate(Task task)
        {
            approx = new ShepardApprox(config.FunctionDimension, task.originPoints);
            analyzer = new Analyzer((IFunction)approx, task.originPoints);
            analyzer.do_random_forest_analyse(cls, build_features);

            double[][] xx = analyzer.Result;
            MeasuredPoint[] xxMeasured = MeasuredPoint.getArrayFromDouble(xx, config.FunctionDimension);
            int newPointsAmount = Math.Min(config.PredictionPointAmount, xx.Length);
            config.PointAmount = config.PointAmount + newPointsAmount;
            task.extPoints = new MeasuredPoint[config.PointAmount];

            for (int j = 0; j < config.PointAmount; j++)
            {
                if (j < config.PointAmount - newPointsAmount)
                {
                    task.extPoints[j] = (MeasuredPoint)task.originPoints[j].Clone();
                }
                else
                {
                    task.extPoints[j] = xxMeasured[j - config.PointAmount + newPointsAmount];
                }
            }

            task.originPoints= task.extPoints;

            MeasuredPoint[] new_points = new MeasuredPoint[newPointsAmount];
            for (int j = 0; j < newPointsAmount; j++)
            {
                new_points[j] = (MeasuredPoint)xxMeasured[j].Clone();
                approx.Calculate(new_points[j]);
            }

            double tempErr = 0;
            for (int k = 0; k < new_points.Length; k++)
            {
                double err = approx.GetError(task.originPoints[config.PointAmount - newPointsAmount + k].outputValues, new_points[k].outputValues);
                if (err > tempErr)
                {
                    tempErr = err;
                }
                Console.WriteLine("f({0}) real val {1} predict val {2} err {3}", String.Join(", ", xxMeasured[k].inputValues),
                    String.Join(", ", task.originPoints[config.PointAmount - newPointsAmount + k].outputValues), String.Join(", ", new_points[k].outputValues), err);
            }
            return tempErr;
        }

        public void setConfig(IConfig config)
        {
            this.config = config;
        }



        protected double[][] testDefWayUntilGood(IConfig config, MeasuredPoint[] measuredPoints, Func<double[], double[]> func)
        {

            int pointAmount = config.PointAmount;

            int i = 0;
            double maxErr = 10;
            int goodPr = 0;
            double[][] points = null;
            while (i < 100000000 && maxErr > config.Approximation && goodPr < 50)
            {
                ShepardApprox model = new ShepardApprox(config.FunctionDimension, measuredPoints);
                Analyzer analyzer = new Analyzer(model, measuredPoints);
                analyzer.do_default_analyse();
                goodPr = analyzer.getGoodPr(func, config.Approximation);
                Console.WriteLine("Good pr " + goodPr);

                double[][] xx = analyzer.Result;
                MeasuredPoint[] xxMeasured = MeasuredPoint.getArrayFromDouble(xx, config.FunctionDimension);
                int newPointsAmount = Math.Min(config.PredictionPointAmount, xx.Length);
                points = Task.getArray(measuredPoints);

                pointAmount = pointAmount + newPointsAmount;
                points = getNewPoints(points, analyzer.Result, newPointsAmount, config.FunctionDimension, func);

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
                    double err = Math.Abs(points[pointAmount - newPointsAmount + k][config.FunctionDimension] - new_points[k][config.FunctionDimension]);
                    if (err > tempErr)
                    {
                        tempErr = err;
                    }
                }
                maxErr = tempErr;
                i++;
            }
            measuredPoints = MeasuredPoint.getArrayFromDouble(points, config.FunctionDimension);
            ShepardApprox model1 = new ShepardApprox(config.FunctionDimension, measuredPoints);
            Analyzer analyzer1 = new Analyzer(model1, measuredPoints);
            analyzer1.do_default_analyse();

            return points;
        }
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

                    for (int k = functionDementsion, l = 0; k < newPoints[j].Length; ++k, ++l)
                    {
                        newPoints[j][k] = response[l];
                    }
                }
            }
            return newPoints;
        }

        //protected void testResult(int nFunctionDimension, int mFunctionDimension, MeasuredPoint[] measuredPoints, Func<double[], double[]> func)
        public void testResult(int nFunctionDimension, int mFunctionDimension, Func<double[], double[]> func)
        {
            IFunction model = (IFunction)approx;
            Console.WriteLine("\n\nTest result approximation");
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

        public void testResult(Func<double[], double[]> func)
        {
            Console.WriteLine("\n\nTest result approximation");
            //Shepard model = new Shepard(functionDimension, points);
            IFunction model = (IFunction)approx;

            for (int i = 1; i < 10; i += 2)
            {
                double[] a = new double[config.FunctionDimension + config.DependentVariablesNum];
                double[] b = new double[config.FunctionDimension + config.DependentVariablesNum];
                for (int j = 0; j < config.FunctionDimension; j++)
                {
                    a[j] = model.Min[j] + ((model.Max[j] - model.Min[j]) * i / 10);
                    b[j] = a[j];
                }
                double[] results = new double[config.DependentVariablesNum];
                for (int j = 0; j < config.DependentVariablesNum; j++)
                {
                    results[j] = a[j + config.FunctionDimension];
                }
                model.Calculate(a);
                //Console.WriteLine("Vector " + String.Join(", ", b) + " Approximation result " + a[functionDimension] + " real " + func(b) + " error " + Math.Abs(a[functionDimension] - func(b)));
                Console.WriteLine("Vector " + String.Join(", ", b) + " Approximation result " + String.Join(", ", results) + " real " + String.Join(", ", func(b)) + " error " + approx.GetError(results, func(b)));
            }
        }

        private IClassifierML get_cls(Func<double[], double[]>[] functions, Task task)
        {
            LabeledData[] ldata = collect_samples(functions, task);
            IClassifierML cls = new RandomForest();
            RandomForestParams ps = new RandomForestParams(ldata, ldata.Length   /* samples count */,
                                                           featureCount   /* features count */,
                                                           2   /* classes count */,
                                                           100   /* trees count */,
                                                           1   /* count of features to do split in a tree */,
                                                           0.7 /* percent of a training set of samples  */
                                                           /* used to build individual trees. */);

            cls.train<int>(ps);
            return cls;
        }

        private IClassifierML get_rg(Func<double[], double[]>[] functions, Task task)
        {
            LabeledData[] ldata = collect_samples(functions, task);
            IClassifierML ml = new RandomForest();
            RandomForestParams ps = new RandomForestParams(ldata.ToArray(),
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

        private double calcVal(int M, int N, double[] point)
        {
            double val = 0;

            for (int l = 0; l < M; ++l)
            {
                val = point[N + l];
            }
            val = val / M;

            //return val;
            return point[N + M - 1];
        }

        private double[] build_features(double[] point, IFunction model, Grid grid, double[] distToKnownPoints, double[][] knownPoints = null, int index = -1)
        {
            this.featureCount = 4;
            MeasuredPoint[] measuredPoint = null;
            if (knownPoints != null)
            {
                measuredPoint = MeasuredPoint.getArrayFromDouble(knownPoints, config.FunctionDimension);
            }

            Analyzer analyzer = new Analyzer(model, measuredPoint);
            analyzer.do_some_analyse();

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
            temp_points.RemoveAt(analyzer.Domain(grid.Node[index]));
            measuredPoint = MeasuredPoint.getArrayFromDouble(temp_points.ToArray(), config.FunctionDimension);
            ShepardApprox new_model = new ShepardApprox(model.N, measuredPoint);
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

            features[2] = distToKnownPoints[index];
            features[3] = err;
            //features[4] = monotonicNode[0];

            return features;
        }

        private LabeledData[] collect_samples(Func<double[], double[]>[] functions, Task task)
        {
            List<LabeledData> ldata = new List<LabeledData>();
            foreach (Func<double[], double[]> f in functions)
            {
                //Parser p = new Parser(f.configFile, f.pointFile, f.tableFile);
                //if (f.tableFile != null)
                //{
                //    f.table = p.getTable();
                //}
                int j = 0;
                while (j < 1)
                {
                    List<double[]> points = get_start_points(j, task, f);
                    MeasuredPoint[] measuredPoints = MeasuredPoint.getArrayFromDouble(points.ToArray(), config.FunctionDimension);
                    points.AddRange(testDefWayUntilGood(config, measuredPoints, f).ToList<double[]>());

                    ShepardApprox def_model = new ShepardApprox(config.FunctionDimension, task.originPoints);
                    int[] count = new int[config.FunctionDimension];
                    for (int i = 0; i < config.FunctionDimension; i++)
                        count[i] = (def_model.Min[i] == def_model.Max[i]) ? 1 : NGRID;
                    Grid grid = new Grid(def_model.N, def_model.M, def_model.Min, def_model.Max, count);
                    double[] dist = update_path_to_knowing_points(grid, points.ToArray(), config.FunctionDimension);

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

                        var realFunctionVal = f(grid.Node[i]);

                        double[] diffs = realFunctionVal.Zip(approxFunctionVal, (d1, d2) => Math.Abs(d1 - d2)).ToArray();

                        double err = (diffs.Sum() / diffs.Length);

                        int pointClass = 0;
                        if (err > config.Approximation)
                        {
                            pointClass = 1;
                        }
                        //double pointClass = err;

                        double[] features = build_features(grid.Node[i], def_model, grid, dist, points.ToArray(), i);
                        ldata.Add(new LabeledData(features, pointClass));
                        featureCount = features.Length;
                    }

                    j++;
                }

            }
            return ldata.ToArray();
        }

        private List<double[]> get_start_points(int i, Task task, Func<double[], double[]> func)
        {
            double[][] origPoints = Task.getArray(task.originPoints);
            if (i == 0)
            {
                return origPoints.ToList();
            }
            else
            {
                List<double[]> points = new List<double[]>();
                Random random = new Random();
                for (int j = 0; j < origPoints.Length; j++)
                {
                    double[] point = new double[config.FunctionDimension + config.DependentVariablesNum];
                    for (int k = 0; k < config.FunctionDimension; k++)
                    {
                        point[k] = random.NextDouble() * (config.Max[k] - config.Min[k]) + config.Min[k];
                    }

                    var response = func(point);
                    for (int l = 0; l < config.DependentVariablesNum; ++l)
                    {
                        point[config.FunctionDimension + l] = response[l];
                    }
                    points.Add(point);
                }
                return points;
            }
        }
    }
}
