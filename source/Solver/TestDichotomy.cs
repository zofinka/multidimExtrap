using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solver
{
    class TestDichotomy: Test
    {
        override public void run()
        {
            Console.WriteLine("TESTS: Dichotomy way\n");
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
            /*Console.WriteLine("Test " + Tests.SquaresProducts.name + " START");
             interAmount = testDefWay(Tests.SquaresProducts.configFile, Tests.SquaresProducts.pointFile, Tests.SquaresProducts.func);
            Console.WriteLine("Test " + Tests.SquaresProducts.name + " END in " + interAmount + " iterations");*/

            // 212 iteration
            /*Console.WriteLine("Test " + Tests.SinXCosY.name + " START");
            interAmount = testDefWay(Tests.SinXCosY.configFile, Tests.SinXCosY.pointFile, Tests.SinXCosY.func);
            Console.WriteLine("Test " + Tests.SinXCosY.name + " END in " + interAmount + " iterations");*/

            //133 iteration
            /*Console.WriteLine(Tests.SinXCosXCosY.name  + " Test START");
            interAmount = testDefWay(Tests.SinXCosXCosY.configFile, Tests.SinXCosXCosY.pointFile, Tests.SinXCosXCosY.func);
            Console.WriteLine(Tests.SinXCosXCosY.name  + " Test END in " + interAmount + " iterations");*/

            //14 interation
            Tests.SinFromSumOnSum SinFromSumOnSum = new Tests.SinFromSumOnSum();
            Console.WriteLine(SinFromSumOnSum.name + " Test START");
            interAmount = test(SinFromSumOnSum.configFile, SinFromSumOnSum.pointFile, SinFromSumOnSum.func);
            Console.WriteLine(SinFromSumOnSum.name + " Test END in " + interAmount + " iterations");
        }

        private int test(string configFile, string pointFile, Func<double[], double> func)
        {
            int predictionPointAmount = 0;

            Parser parser = new Parser(configFile, pointFile);
            //int extraPointAmount = Convert.ToInt32(Math.Pow(parser.FunctionDimension, 2));

            List<double[]> startPoints = new List<double[]>();
            for (int j = 0; j < parser.PointAmount; j++)
            {
                startPoints.Add((double[])parser.Points[j].Clone());
            }

            //// If we have 2-dimension function, then we have two args: X and Y.
            //// And we have minimums and maximums for X and Y.
            //// To present all boundaries of X and Y with such minimums and maximums, 
            //// we shall create rectangle.
            //// From combinatory point we may say that we have 2 places: X and Y; and 2 possibilities: min and max.
            //// We shall do placement with repetitions: (MIN, MIN); (MIN, MAX); (MAX, MIN); (MAX, MAX).
            //// All placements with repetitions for such example: possibilities ^ places;
            //// So, we will have additionaly possibilities ^ places (2 ^ functionDimension) points
            //// to represent all boundaries.
            //// We can use 0 for denotion of the minimum and 1 for maximum.
            //// So we can enumerate all numbers from 0 to (2 ^ functionDimension) and represent this numbers in the 
            //// binary format, takes only last functionDimension bits and use maximum or minimum based on the bit number.
            //for (int j = 0; j < extraPointAmount; ++j)
            //{
            //    var point = new double[parser.FunctionDimension + 1];
            //    for (int shift = 0; shift < parser.FunctionDimension; ++shift)
            //    {
            //        int bitMask = 1 << shift;
            //        bool el = Convert.ToBoolean(j & bitMask);

            //        point[shift] = el ? parser.Max[shift] : parser.Min[shift];
            //    }

            //    double val = func(point);

            //    if (!Double.IsNaN(val) && !Double.IsInfinity(val))
            //    {
            //        point[parser.FunctionDimension] = val;
            //        startPoints.Add(point);
            //    }
            //}

            int pointAmount = startPoints.Count;

            double[][] points = startPoints.ToArray();

            int i = 0;
            double maxErr = 10;
            while (i < 20 && maxErr > parser.Approximation)
            {
                Shepard model = new Shepard(parser.FunctionDimension, points);
                Console.WriteLine("Max " + String.Join(", ", model.Max) + " Min " + String.Join(", ", model.Min));
                Analyzer analyzer = new Analyzer(model, points);
                analyzer.do_dichotomy_analyse();

                double[][] xx = analyzer.Result;
                predictionPointAmount = xx.Length;
                pointAmount = pointAmount + predictionPointAmount;
                points = getNewPoints(points, analyzer.Result, predictionPointAmount, parser.FunctionDimension, func);

                double[][] new_points = new double[predictionPointAmount][];
                for (int j = 0; j < predictionPointAmount; j++)
                {
                    new_points[j] = new double[xx[j].Length];
                    Array.Copy(xx[j], new_points[j], xx[j].Length);
                    model.Calculate(new_points[j]);
                }

                double tempErr = 0;
                for (int k = 0; k < new_points.Length; k++)
                {
                    double err = Math.Abs(points[pointAmount - predictionPointAmount + k][parser.FunctionDimension] - new_points[k][parser.FunctionDimension]);
                    Console.WriteLine(" \n " + (points[pointAmount - predictionPointAmount + k][parser.FunctionDimension] - new_points[k][parser.FunctionDimension]) + " " + points[pointAmount - predictionPointAmount + k][parser.FunctionDimension] + " " + new_points[k][parser.FunctionDimension] + " \n ");
                    if (err > tempErr)
                    {
                        tempErr = err;
                    }
                    Console.WriteLine("f({0}) real val {1} predict val {2} err {3}", String.Join(", ", xx[k]), points[pointAmount - predictionPointAmount + k][parser.FunctionDimension], new_points[k][parser.FunctionDimension], err);
                }
                maxErr = tempErr;
                i++;

            }
            testResult(parser.FunctionDimension, points, func);
            Console.WriteLine("Calc err avg " + calc_err(func, points.ToList(), parser));

            return i;
        }


    }
}
