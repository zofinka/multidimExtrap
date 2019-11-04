using System;
using System.Collections.Generic;
using System.Linq;

namespace Solver
{
    public class Analyzer : IFunction
    {
        IFunction func;
        double[][] xf;
        double[][] xfcandidates;
        Grid grid;
        int[] domain;
        int[][] graph;
        double[] borderdist;
        int[] bordernear;
        double[] error;
        int[] candidates;

        const int NGRID = 100;

        public Analyzer(IFunction func, double[][] xf)
        {
            this.func = func;
            this.xf = xf;

            int[] count = new int[N]; for (int i = 0; i < N; i++) count[i] = (Min[i] == Max[i]) ? 1 : NGRID;

            //create_grid(count);
            //analyse_voronoi();
            //analyse_error();

           // random_forest_learn();

        }

        public void do_default_analyse()
        {
            int[] count = new int[N]; for (int i = 0; i < N; i++) count[i] = (Min[i] == Max[i]) ? 1 : NGRID;
            create_grid(count);
            analyse_voronoi();
            analyse_error();
        }

        public int getGoodPr(Func<double[], double> meFunc, float allowErr)
        {
            int prGoodRes = 0;
            int n = grid.Node.Length;
            for (int i = 0; i < n; i++)
            {
                double[] cuurentNode = (double[])grid.Node[i].Clone();
                this.func.Calculate(cuurentNode);
                double cuurentNodeVal = cuurentNode[cuurentNode.Length - 1];
                if (Math.Abs(meFunc(grid.Node[i]) - cuurentNodeVal) < allowErr)
                {
                    prGoodRes += 1;
                }
            }
            return prGoodRes * 100 / n;
        }

        public double[][] getGoodSamples(Func<double[], double> meFunc, float allowErr, int goodPr)
        {
            int n = grid.Node.Length;
            int badPr = 0;
            int nextSample = 0;
            int i = 0;
            double[][] samples = new double[goodPr * n / 100 * 2][];
            while(i < n && nextSample < samples.Length)
            {
                double[] cuurentNode = (double[])grid.Node[i].Clone();
                this.func.Calculate(cuurentNode);
                double cuurentNodeVal = cuurentNode[cuurentNode.Length - 1];
                if (Math.Abs(meFunc(grid.Node[i]) - cuurentNodeVal) < allowErr)
                {
                    //samples[nextSample] = new double[grid.Node[i].Length];
                    samples[nextSample] = grid.Node[i];
                    nextSample += 1;
                } else
                {
                    if (badPr  < samples.Length / 2)
                    {
                        //samples[nextSample] = new double[grid.Node[i].Length];
                        samples[nextSample] = grid.Node[i];
                        nextSample += 1;
                        badPr += 1;
                    }
                }
                i += 1;
            }
            return samples;
        }



        public void do_default_analyse(int[] count)
        {
            create_grid(count);
            analyse_voronoi();
            analyse_error();
        }

        public void do_random_forest_analyse(Classifiers.IClassifier cls, double allowErr, Func<double[], double> meFunc, Func<double[], double[]> calcDerivative)
        {
            int[] count = new int[N]; for (int i = 0; i < N; i++) count[i] = (Min[i] == Max[i]) ? 1 : NGRID;
            create_grid(count);
            analyse_voronoi();
            analyse_all_error();


            //int n = candidates.Length;
            int n = grid.Node.Length;
            Console.WriteLine(candidates.Length);
            //int n = grid.Node.Length;
            Classifiers.LabeledData[] ldata = new Classifiers.LabeledData[n];
            int featureCount = 0;
            for (int i = 0; i < n; i++)
            {
                // min, max in locality
                double maxNeighbours = double.MinValue;
                double minNeighbours = double.MaxValue;
                foreach (var neighbour in grid.Neighbours(i))
                //foreach (var neighbour in grid.Neighbours(i))
                {
                    double[] calcNeighbour = (double[])grid.Node[neighbour].Clone();
                    this.func.Calculate(calcNeighbour);
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
                double[] cuurentNode = (double[])grid.Node[i].Clone();
               // double[] cuurentNode = (double[])grid.Node[i].Clone();
                this.func.Calculate(cuurentNode);
                double cuurentNodeVal = cuurentNode[cuurentNode.Length - 1];
                if (cuurentNodeVal < minNeighbours)
                {
                    minNeighbours = cuurentNodeVal;
                }
                if (cuurentNodeVal > maxNeighbours)
                {
                    maxNeighbours = cuurentNodeVal;
                }

                // is real function and approximation are equal, class for point

                //derivative 
                //double[] derivative = calcDerivative(grid.Node[candidates[i]]);
                double[] derivative = calcDerivative(grid.Node[i]);

                // build features vector
                double[] features = new double[5 + derivative.Length];
                //features[0] = borderdist[i];
                features[0] = error[i];
                //features[2] = maxNeighbours;
                //features[3] = minNeighbours;
                //features[4] = cuurentNodeVal;
                //for (int k = 0; k < derivative.Length; k++)
                //{
                //    features[5 + k] = derivative[k];
                //}

                ldata[i] = new Classifiers.LabeledData(features, 0);
                featureCount = features.Length;
            }
            List<int> newCandidates = new List<int>();
            int[] y = new int[ldata.Length];
            for (int i = 0; i < ldata.Length; i++)
            {
                cls.infer(ldata[i].data, out y[i]);
                if (y[i] == 1)
                {
                    //newCandidates.Add(candidates[i]);
                    newCandidates.Add(i);
                    //Console.WriteLine("err " + ldata[i].data[1]);
                }
            }
            candidates = newCandidates.ToArray();
           // Console.WriteLine(candidates.Length);

            xfcandidates = Tools.Sub(grid.Node, candidates);
        }

        private double[] build_fetures_from_existing_points(int i, Func<double[], double[]> calcDerivative) 
        {
            // min, max in locality
            int index;
            grid.ToIndex(xf[i], out index);
            double maxNeighbours = double.MinValue;
            double minNeighbours = double.MaxValue;
            foreach (var neighbour in grid.Neighbours(index))
            {
                double[] calcNeighbour = (double[])grid.Node[neighbour].Clone();
                this.func.Calculate(calcNeighbour);
                if (calcNeighbour[calcNeighbour.Length - 1] < minNeighbours)
                {
                    minNeighbours = calcNeighbour[calcNeighbour.Length - 1];
                }
                if (calcNeighbour[calcNeighbour.Length - 1] > maxNeighbours)
                {
                    maxNeighbours = calcNeighbour[calcNeighbour.Length - 1];
                }
            }
            double[] nearestNeighbour = (double[])grid.Node[index].Clone();
            this.func.Calculate(nearestNeighbour);
            if (nearestNeighbour[nearestNeighbour.Length - 1] < minNeighbours)
            {
                minNeighbours = nearestNeighbour[nearestNeighbour.Length - 1];
            }
            if (nearestNeighbour[nearestNeighbour.Length - 1] > maxNeighbours)
            {
                maxNeighbours = nearestNeighbour[nearestNeighbour.Length - 1];
            }

            // current val
            double[] cuurentNode = (double[])xf[i].Clone();
            this.func.Calculate(cuurentNode);
            double cuurentNodeVal = cuurentNode[cuurentNode.Length - 1];

            //derivative 
            double[] derivative = calcDerivative(xf[i]);

            // build features vector
            double[] features = new double[5 + derivative.Length];
            features[0] = borderdist[index];
            features[1] = 0;
            features[2] = maxNeighbours;
            features[3] = minNeighbours;
            features[4] = cuurentNodeVal;
            for (int k = 0; k < derivative.Length; k++)
            {
                features[5 + k] = derivative[k];
            }
            return features;
        }

        public Classifiers.IClassifier learn_random_forest_on_known_points(Func<double[], double> meFunc, Func<double[], double[]> calcDerivative, double allowErr)
        {
            int[] count = new int[N]; for (int i = 0; i < N; i++) count[i] = (Min[i] == Max[i]) ? 1 : NGRID;
            create_grid(count);
            analyse_voronoi();
            analyse_error();

            int n = xf.Length;
            Classifiers.LabeledData[] ldata = new Classifiers.LabeledData[n];
            int featureCount = 0;
            for (int i = 0; i < n; i++)
            {
                double[] feature = build_fetures_from_existing_points(i, calcDerivative);
                ldata[i] = new Classifiers.LabeledData(feature, 1);
                featureCount = feature.Length;
            }

        
           Classifiers.IClassifier cls = new Classifiers.RandomForest();
           
           Classifiers.RandomForestParams ps = new Classifiers.RandomForestParams(ldata, n   /* samples count */,
                                                                                        featureCount   /* features count */,
                                                                                        2   /* classes count */,
                                                                                       n / 10   /* trees count */,
                                                                                       5   /* count of features to do split in a tree */,
                                                                                       0.7 /* percent of a training set of samples  */
                                                                                              /* used to build individual trees. */);

            cls.train(ps);

            double trainModelPrecision;
            cls.validate(ldata, out trainModelPrecision);

            Console.WriteLine("Model precision on training dataset: " + trainModelPrecision);
            return cls;

        }

        public Classifiers.IClassifier learn_random_forest_on_grid(Func<double[], double> meFunc, Func<double[], double[]> calcDerivative, double allowErr)
        {

            Console.WriteLine("MN " + M + " " + N );
            int[] count = new int[N]; for (int i = 0; i < N; i++) count[i] = (Min[i] == Max[i]) ? 1 : NGRID;
            create_grid(count);
            analyse_voronoi();
            analyse_all_error();

            int n = grid.Node.Length + xf.Length;
            // int n = grid.Node.Length;
            Classifiers.LabeledData[] ldata = new Classifiers.LabeledData[n];
            int featureCount = 0;
            for (int i = 0; i < grid.Node.Length; i++)
            {
                // min, max in locality
                double maxNeighbours = double.MinValue;
                double minNeighbours = double.MaxValue;
                foreach (var neighbour in grid.Neighbours(i))
                {
                    double[] calcNeighbour = (double[])grid.Node[neighbour].Clone();
                    this.func.Calculate(calcNeighbour);
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
                double[] cuurentNode = (double[])grid.Node[i].Clone();
                this.func.Calculate(cuurentNode);
                double cuurentNodeVal = cuurentNode[cuurentNode.Length - 1];
                if (cuurentNodeVal < minNeighbours)
                {
                    minNeighbours = cuurentNodeVal;
                }
                if (cuurentNodeVal > maxNeighbours)
                {
                    maxNeighbours = cuurentNodeVal;
                }

                // is real function and approximation are equal, class for point
                int pointClass = 0;
                if (Math.Abs(meFunc(grid.Node[i]) - cuurentNodeVal) > allowErr)
                {
                    pointClass = 1;
                }

                //derivative 
                double[] derivative = calcDerivative(grid.Node[i]);

                // build features vector
                double[] features = new double[5 + derivative.Length];
                //features[0] = borderdist[i];
                features[0] = error[i];
                //features[2] = maxNeighbours;
                //features[3] = minNeighbours;
                //features[4] = cuurentNodeVal;
                //for (int k = 0; k < derivative.Length; k++)
                //{
                //    features[5 + k] = derivative[k];
                //}
               
                ldata[i] = new Classifiers.LabeledData(features, pointClass);
                featureCount = features.Length;
            }
            for(int i = 0; i < xf.Length; i++)
            {
                double[] feature = build_fetures_from_existing_points(i, calcDerivative);
                ldata[grid.Node.Length + i] = new Classifiers.LabeledData(feature, 0);
                featureCount = feature.Length;
            }


            Classifiers.IClassifier cls = new Classifiers.RandomForest();
            Classifiers.RandomForestParams ps = new Classifiers.RandomForestParams(ldata, n   /* samples count */,
                                                                                          1   /* features count */,
                                                                                          2   /* classes count */,
                                                                                          100   /* trees count */,
                                                                                          1   /* count of features to do split in a tree */,
                                                                                          0.7 /* percent of a training set of samples  */
                                                                                              /* used to build individual trees. */);

            cls.train(ps);
            double trainModelPrecision;
            cls.validate(ldata, out trainModelPrecision);
            for (int i = 0; i < 10; i++)
            {
                int y;
                cls.infer(ldata[i].data, out y);
                double[] cuurentNode = (double[])grid.Node[i].Clone();
                this.func.Calculate(cuurentNode);
                int pointClass = 0;
                if (Math.Abs(meFunc(grid.Node[i]) - cuurentNode[cuurentNode.Length - 1]) > allowErr)
                {
                    pointClass = 1;
                }

              //  Console.WriteLine(" pointClass " + pointClass + " y " + y);
            }

            //Console.WriteLine("Model precision on training dataset: " + trainModelPrecision);
            return cls;
        }

        public Analyzer(IFunction func, double[][] xf, int[] count)
        {
            this.func = func;
            this.xf = xf;

            //create_grid(count);
            //analyse_voronoi();
            //analyse_error();
        }

        private void create_grid(int[] count)
        {
            //Console.WriteLine("Построение сетки ({0})", count.Str());
            grid = new Grid(N, M, Min, Max, count);
            //Console.WriteLine("Сетка на {0} узлах построена успешно", grid.Node.Length);
        }

        private void analyse_voronoi()
        {
            //вычисляю принадлежность узлов сетки доменам (графовый алгоритм на базе структуры уровней смежности)
            SortedSet<int>[] adjncy = new SortedSet<int>[xf.Length];
            for (int i = 0; i < xf.Length; i++)
                adjncy[i] = new SortedSet<int>();

           // Console.WriteLine("Разбиение пространства на домены");
            Queue<int> queue = new Queue<int>();
            domain = new int[grid.Node.Length];
            double[] dist = new double[grid.Node.Length];
            for (int i = 0; i < domain.Length; i++)
            {
                domain[i] = -1;
                dist[i] = double.PositiveInfinity;
            }
            
            for (int i = 0; i < xf.Length; i++)
            {
                int index;
                grid.ToIndex(xf[i], out index);
                dist[index] = distanceX(grid.Node[index], xf[i]);
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
                    double d = distanceX(grid.Node[adj], xf[i]);
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

            //Console.WriteLine("dist " + String.Join(", ", dist));
            //Console.WriteLine("domen " + String.Join(", ", domain));

            //Console.WriteLine("Построение графа доменов");
            //строю граф соседства доменов
            graph = new int[xf.Length][];
            for (int i = 0; i < xf.Length; i++)
            {
                adjncy[i].Add(i);
                graph[i] = adjncy[i].ToArray();
               // Console.WriteLine("dist " + String.Join(", ", adjncy[i]));
            }

            //Console.WriteLine("Построение диграммы Вороного на сетке");
            //уточняю домены (диаграмма вороного на сетке)
            for (int i = 0; i < grid.Node.Length; i++)
            {
                double[] xy = grid.Node[i];
                int[] adj = graph[domain[i]];
                double min = double.PositiveInfinity;
                for (int j = 0; j < adj.Length; j++)
                {
                    double d = distanceX(xy, xf[adj[j]]);
                    if (min > d) { min = d; domain[i] = adj[j]; }
                }
            }

            //Console.WriteLine("Вычисление границ доменов");
            //вычисляю границы доменов
            borderdist = new double[grid.Node.Length];
            bordernear = new int[grid.Node.Length];
            for (int i = 0; i < grid.Node.Length; i++)
            {
                borderdist[i] = double.PositiveInfinity;
                bordernear[i] = -1;
                int dom = domain[i];
                foreach (var adj in grid.Neighbours(i))
                    if (domain[adj] != dom)
                    {
                        borderdist[i] = 0;
                        bordernear[i] = i;
                        queue.Enqueue(i);
                        break;
                    }
            }
            candidates = queue.ToArray();

            //Console.WriteLine("Построение функции расстояний до границ доменов");
            //вычисляю расстояния от границ
            while (queue.Count > 0)
            {
                int index = queue.Dequeue();
                int dom = domain[index];
                int brd = bordernear[index];
                foreach (var adj in grid.Neighbours(index))
                {
                    if (domain[adj] != dom) continue;
                    double d = distanceX(grid.Node[adj], grid.Node[brd]);
                    if (bordernear[adj] >= 0)
                    {
                        if (d < borderdist[adj])
                        {
                            bordernear[adj] = brd;
                            borderdist[adj] = d;
                        }
                        continue;
                    }
                    bordernear[adj] = brd;
                    borderdist[adj] = d;
                    queue.Enqueue(adj);
                }
            }

            //Console.WriteLine("Нормировка функции расстояний до границ доменов");
            //нормирую расстояния от границ
            for (int i = 0; i < grid.Node.Length; i++)
            {
                int dom = domain[i];
                int brd = bordernear[i];
                double a = distanceX(grid.Node[i], xf[dom]);
                double b = distanceX(grid.Node[i], grid.Node[brd]);
                double c = a + b;
                borderdist[i] = (c == 0) ? 0 : b / c;
            }
        }

        private double square_area(double[] x)
        {
            double res = 1;
            for (int i = 0; i < x.Length - 1; i++)
            {
                res = res * x[i];
            }
            return res;
        }

        private void analyse_error()
        {
            //Console.WriteLine("Вычисление локальных экстраполянтов");
            //вычисляю экстраполянты
            Shepard[] sh = new Shepard[xf.Length];
            for (int i = 0; i < xf.Length; i++)
                sh[i] = new Shepard(N, xf, graph[i]);

            //Console.WriteLine("Аппроксимация значений исходной функции на границах доменов");
            //пересчитываю значения в узлах решетки только на границах доменов
            for (int i = 0; i < grid.Node.Length; i++)
            {
                if (borderdist[i] > 0) continue;
                sh[domain[i]].Calculate(grid.Node[i]);
            }

            //Console.WriteLine("Вычисление ошибки аппроксимации функции на границах доменов");
            //вычисляю ошибку на границах доменов
            error = new double[grid.Node.Length];
            for (int i = 0; i < grid.Node.Length; i++)
            {
                if (borderdist[i] > 0) continue;
                error[i] = 0;
                foreach (var adj in grid.Neighbours(i))
                {
                    double d = distanceF(grid.Node[i], grid.Node[adj]);
                    if (error[i] < d) error[i] = d;
                }
            }

            //Console.WriteLine("Интерполяция ошибки аппроксимации функции на области доменов");
            //интерполирую ошибку
            double max = 0;
            for (int i = 0; i < grid.Node.Length; i++)
            {
                int brd = bordernear[i];
                double err = error[brd];
                error[i] = err * (1 - borderdist[i]);
                if (max < error[i]) max = error[i];
            }

            //Console.WriteLine("Нормировка функции ошибки");
            //нормирую ошибку
            if (max > 0) 
                for (int i = 0; i < grid.Node.Length; i++)
                    error[i] = error[i] / max;

            int maxcandidates = Math.Min(candidates.Length, 1000);
            //if (candidates.Length > maxcandidates)
            //    Console.WriteLine("Предварительный отбор {1} из {0} кандидатов", candidates.Length, maxcandidates);

            //Console.WriteLine("Упорядочение {0} потенциальных кандидатов", maxcandidates);
            //сортировка потенциальных кандидатов
            for (int i = 0; i < maxcandidates - 1; i++)
                for (int j = i + 1; j < candidates.Length; j++)
                    if (error[candidates[i]] < error[candidates[j]])
                    {
                        int temp = candidates[i];
                        candidates[i] = candidates[j];
                        candidates[j] = temp;
                    }

            //Console.WriteLine("Формирование списка кандидатов", maxcandidates);
            candidates = Tools.Sub(candidates, 0, maxcandidates);
            xfcandidates = Tools.Sub(grid.Node, candidates);
            for (int i = 0; i < xfcandidates.Length; i++)
            {
                //func.Calculate(xfcandidates[i]);
                for (int j = N; j < N + M; j++) xfcandidates[i][j] = 0;
            }
            Console.WriteLine("Процесс расчета закончен успешно");
        }

        private void analyse_all_error()
        {
            //Console.WriteLine("Вычисление локальных экстраполянтов");
            //вычисляю экстраполянты
            Shepard[] sh = new Shepard[xf.Length];
            for (int i = 0; i < xf.Length; i++)
                sh[i] = new Shepard(N, xf, graph[i]);

            //Console.WriteLine("Аппроксимация значений исходной функции на границах доменов");
            //пересчитываю значения в узлах решетки только на границах доменов
            for (int i = 0; i < grid.Node.Length; i++)
            {
                //if (borderdist[i] > 0) continue;
                sh[domain[i]].Calculate(grid.Node[i]);
            }

            //Console.WriteLine("Вычисление ошибки аппроксимации функции на границах доменов");
            //вычисляю ошибку на границах доменов
            error = new double[grid.Node.Length];
            for (int i = 0; i < grid.Node.Length; i++)
            {
                //if (borderdist[i] > 0) continue;
                error[i] = 0;
                foreach (var adj in grid.Neighbours(i))
                {
                    double d = distanceF(grid.Node[i], grid.Node[adj]);
                    if (error[i] < d) error[i] = d;
                }
            }

            //Console.WriteLine("Интерполяция ошибки аппроксимации функции на области доменов");
            //интерполирую ошибку
            double max = 0;
            for (int i = 0; i < grid.Node.Length; i++)
            {
                int brd = bordernear[i];
                double err = error[brd];
                error[i] = err * (1 - borderdist[i]);
                if (max < error[i]) max = error[i];
            }

            //Console.WriteLine("Нормировка функции ошибки");
            //нормирую ошибку
            if (max > 0)
                for (int i = 0; i < grid.Node.Length; i++)
                    error[i] = error[i] / max;

            int maxcandidates = Math.Min(candidates.Length, 1000);
            //if (candidates.Length > maxcandidates)
            //    Console.WriteLine("Предварительный отбор {1} из {0} кандидатов", candidates.Length, maxcandidates);

            //Console.WriteLine("Упорядочение {0} потенциальных кандидатов", maxcandidates);
            //сортировка потенциальных кандидатов
            for (int i = 0; i < maxcandidates - 1; i++)
                for (int j = i + 1; j < candidates.Length; j++)
                    if (error[candidates[i]] < error[candidates[j]])
                    {
                        int temp = candidates[i];
                        candidates[i] = candidates[j];
                        candidates[j] = temp;
                    }

            //Console.WriteLine("Формирование списка кандидатов", maxcandidates);
            candidates = Tools.Sub(candidates, 0, maxcandidates);
            xfcandidates = Tools.Sub(grid.Node, candidates);
            for (int i = 0; i < xfcandidates.Length; i++)
            {
                //func.Calculate(xfcandidates[i]);
                for (int j = N; j < N + M; j++) xfcandidates[i][j] = 0;
            }
            Console.WriteLine("Процесс расчета закончен успешно");
        }

        private double distanceX(double[] a, double[] b)
        {
            double dist = 0;
            for (int i = 0; i < N; i++) dist += (a[i] - b[i]) * (a[i] - b[i]);
            return Math.Sqrt(dist);
        }

        private double distanceF(double[] a, double[] b)
        {
            double dist = 0;
            for (int i = N; i < N+M; i++) dist += (a[i] - b[i]) * (a[i] - b[i]);
            return Math.Sqrt(dist);
        }


        private void setvalue(int index, double[] value)
        {
            for (int i = N; i < value.Length; i++)
                grid.Node[index][i] = value[i];
        }

        public int M { get { return func.M; } }

        public double[] Max { get { return func.Max; } }

        public double[] Min { get { return func.Min; } }

        public int N { get { return func.N; } }

        public void Calculate(double[] xy)
        {
            func.Calculate(xy);
        }

        public IFunction Function { get { return func; } }

        public double[][] Source { get { return xf; } }

        public double[][] Result { get { return xfcandidates; } }
   
        public int Error2Nodes(double err)
        {
            for (int i = 0; i < candidates.Length; i++)
                if (error[candidates[i]] < err) return i + 1;
            return candidates.Length;
        }

        public double Nodes2Error(int nodes)
        {
            if (nodes < 0) nodes = 0;
            if (nodes >= candidates.Length - 1) nodes = candidates.Length - 1;
            return error[candidates[nodes]];
        }

        public int Domain(double[] xy)
        {
            int id;
            grid.ToIndex(xy, out id);
            id = domain[id];
            return id;
        }

        public double Border(double[] xy)
        {
            int id;
            grid.ToIndex(xy, out id);
            return borderdist[id];
        }

        public int Voronoi(double[] xy)
        {
            int id;
            grid.ToIndex(xy, out id);
            int dom = domain[id];
            //if (borderdist[id] > 0) return dom;  //это оптимизация для скорости вычисления (потеря точности)
            int[] adj = graph[dom];
            double min = double.PositiveInfinity;
            for (int i = 0; i < adj.Length; i++)
            {
                double d = distanceX(xy, xf[adj[i]]);
                if (min > d) { min = d; dom = adj[i]; }
            }
            return dom;
        }

        public double Error(double[] xy)
        {
            int id;
            grid.ToIndex(xy, out id);
            return error[id];
        }

        public double Error(double[] xy, int compX, int compY)
        {
            int id;
            grid.ToIndex(xy, out id);
            updateError(xy, compX, compY);
            if (errmax == errmin) return error[id];
            return (error[id] - errmin) / (errmax - errmin);
        }

        double errmin = 0, errmax = 0;
        int errX = -1, errY = -1;
        double[] errXY = null;

        private void updateError(double[] xy, int compX, int compY)
        {
            if (errX == compX && errY == compY && errXY != null)
            {
                bool equal = true;
                for (int i = 0; i < xy.Length; i++)
                {
                    if (i == compX || i == compY) continue;
                    if (xy[i] != errXY[i]) { equal = false; break; }
                }
                if (equal) return;
            }
            int id;
            grid.ToIndex(xy, out id);
            int[] counter = new int[N];
            grid.ToCounter(id, counter);
            counter[compX] = 0;
            counter[compY] = 0;
            int errID;
            grid.ToIndex(counter, out errID);
            errXY = (double[])xy.Clone();
            errX = compX;
            errY = compY;
            errmin = double.PositiveInfinity;
            errmax = 0;
            for (int i = 0; i < grid.count[errX]; i++)
                for (int j = 0; j < grid.count[errY]; j++)
                {
                    double err = error[errID + grid.rount[errX] * i + grid.rount[errY] * j];
                    if (errmin > err) errmin = err;
                    if (errmax < err) errmax = err;
                }
        }
    }
}
