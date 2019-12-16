using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    public class TestGetter
    {
        private static TestGetter instance;
        private static bool IsInstanceCreated = false;
        private static Dictionary<string, Func<double[], double[]>> functions = new Dictionary<string, Func<double[], double[]>>()
        {
            { "PyramidVolume", PyramidVolume.func },
            { "SinFromSumOnSum", SinFromSumOnSum.func },
            { "SzinXCosXCosY", SzinXCosXCosY.func },
            { "SinXCosY", SinXCosY.func },
            { "SquareArea", SquareArea.func },
            { "SquaresProducts", SquaresProducts.func },
            { "TruncPyramid", TruncPyramid.func }
        };

        private static Dictionary<string, Func<double[], double[]>> derivatives = new Dictionary<string, Func<double[], double[]>>()
        {
            { "PyramidVolume", PyramidVolume.derivative },
            { "SinFromSumOnSum", SinFromSumOnSum.derivative },
            { "SzinXCosXCosY", SzinXCosXCosY.derivative },
            { "SinXCosY", SinXCosY.derivative },
            { "SquareArea", SquareArea.derivative },
            { "SquaresProducts", SquaresProducts.derivative },
            { "TruncPyramid", TruncPyramid.derivative }
        };

        protected TestGetter(string testName)
        {
            if (IsInstanceCreated)
            {
                throw new InvalidOperationException("Constructing a TestGetter" +
                    " manually is not allowed, use the Instance property.");
            }
            else
                this.testName = testName;
        }

        public static TestGetter getInstance(string testName)
        {
            if (instance == null)
                instance = new TestGetter(testName);
            return instance;
        }

        public Func<double[], double[]> GetFunc()
        {
            return functions[testName];
        }

        public Func<double[], double[]> GetDerivative()
        {
            return derivatives[testName];
        }

        private string testName;
    }

    public static class PyramidVolume
    {
        public static double[] derivative(double[] points)
        {
            double[] derivative = new double[points.Length - 1];
            for (int i = 0; i < points.Length - 1; i++)
            {
                double res = 1;

                for (int j = 0; j < points.Length - 1; j++)
                {
                    if (i != j)
                    {
                        res *= points[j];
                    }
                }
                derivative[i] = res / 3;
            }
            return derivative;
        }

        public static double[] func(double[] points)
        {

            double[] res = { 1 };
            for (int i = 0; i < points.Length - 1; i++)
            {
                res[0] *= points[i];
            }
            res[0] = res[0] / 3;
            return res;
        }
    }

    public static class SinFromSumOnSum
    {
        public static double[] derivative(double[] points)
        {
            return new double[2] { (Math.Cos(points[0] + points[1]) * (points[0] + points[1]) - Math.Sin(points[0] + points[1])) / (points[0] + points[1]),
                                   Math.Cos(points[0]) * (points[0] + points[1]) - Math.Sin(points[0] + points[1])  / (points[0] + points[1])};
        }

        // sin(x + y) / x + y
        public static double[] func(double[] points)
        {
            double[] result = { Math.Sin(points[0] + points[1]) / points[0] + points[1] };
            return result;
        }
    }

    public static class SzinXCosXCosY
    {
        public static double[] derivative(double[] points)
        {
            return new double[2] { Math.Cos(2 * points[0]) * Math.Cos(points[1]), -1 * Math.Sin(points[0]) * Math.Cos(points[0]) * Math.Sin(points[1]) };
        }

        // sin(x1)* cos(x1) * cos(x2)
        public static double[] func(double[] points)
        {
            double[] result = { Math.Sin(points[0]) * Math.Cos(points[0]) * Math.Cos(points[1]) };
            return result;
        }
    }

    public static class SinXCosY
    {
        public static double[] derivative(double[] points)
        {
            return new double[2] { Math.Cos(points[0]) * Math.Cos(points[1]), -1 * Math.Sin(points[0]) * Math.Sin(points[1]) };
        }

        public static double[] func(double[] points)
        {
            double[] result = { Math.Sin(points[0]) * Math.Cos(points[1]) };
            return result;
        }
    }

    public static class SquareArea
    {
        public static double[] derivative(double[] points)
        {
            double[] derivative = new double[points.Length - 1];
            for (int i = 0; i < points.Length - 1; i++)
            {
                double res = 1;

                for (int j = 0; j < points.Length - 1; j++)
                {
                    if (i != j)
                    {
                        res *= res;
                    }
                }
                derivative[i] = res;
            }
            return derivative;
        }

        public static double[] func(double[] points)
        {
            double[] res = { 1 };
            for (int i = 0; i < points.Length - 1; i++)
            {
                res[0] *= points[i];
            }
            return res;
        }
    }

    public static class SquaresProducts
    {
        public static double[] derivative(double[] points)
        {
            double[] derivative = new double[points.Length - 1];
            for (int i = 0; i < points.Length - 1; i++)
            {
                double res = points[i] * 2;

                for (int j = 0; j < points.Length - 1; j++)
                {
                    if (i != j)
                    {
                        res *= Math.Pow(points[i], 2);
                    }
                }
                derivative[i] = res;
            }
            return derivative;
        }

        // x^2*y^2 + 2
        public static double[] func(double[] points)
        {
            double[] res = { 1 };
            for (int i = 0; i < points.Length - 1; i++)
            {
                res[0] *= Math.Pow(points[i], 2);
            }
            res[0] += 2;
            return res;
        }
    }

    public static class TruncPyramid
    {
        public static double[] derivative(double[] points)
        {
            throw new NotImplementedException();
        }

        public static double[] func(double[] points)
        {
            double a = points[0] + points[1] + Math.Sqrt(points[0] * points[1]);
            double b = points[2] * a;
            double c = (1 / 3) * b;
            double v = b / 3;
            double[] result = { points[2] * (points[0] + points[1] + Math.Sqrt(points[0] * points[1])) / 3 };
            return result;
        }
    }
}
