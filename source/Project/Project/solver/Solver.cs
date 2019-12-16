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
        double calculate(Task task);
        void testResult(Func<double[], double[]> func);
    }
    public class Solver : ISolver
    {
        private IConfig config;
        private IAnalyzer analyzer;
        private IApprox approx;
        private IClassifier cls;

        public static Func<double[], double[]> func;
        public static Func<double[], double[]> derivativeFunc;

        public Solver()
        {
            func = TestGetter.getInstance(Approx.testFunction).GetFunc();
            derivativeFunc = TestGetter.getInstance(Approx.testFunction).GetDerivative();
        }

        public double calculate(Task task)
        {
            approx = new ShepardApprox(config.FunctionDimension, task.originPoints);
            analyzer = new Analyzer((IFunction)approx, task.originPoints);

            cls = analyzer.learn_random_forest_on_grid(func, derivativeFunc, config.Approximation);
            analyzer.do_random_forest_analyse(cls, config.Approximation, func, derivativeFunc);

            double[][] xx = analyzer.Result;
            MeasuredPoint[] xxMeasured = MeasuredPoint.getArrayFromDouble(xx, config.FunctionDimension);
            int newPointsAmount = Math.Min(config.PredictionPointAmount, xx.Length);
            config.PointAmount = config.PointAmount + newPointsAmount;
            task.extPoints = new MeasuredPoint[config.PointAmount];

            //double[][] newPoints = new double[pointAmount][];
            for (int j = 0; j < config.PointAmount; j++)
            {
                if (j < config.PointAmount - newPointsAmount)
                {
                    task.extPoints[j] = (MeasuredPoint)task.originPoints[j].Clone();
                    //newPoints[j] = (double[])points[j].Clone();
                }
                else
                {
                    task.extPoints[j] = xxMeasured[j - config.PointAmount + newPointsAmount];
                }
            }

            task.originPoints= task.extPoints;

            MeasuredPoint[] new_points = new MeasuredPoint[newPointsAmount];
            //double[][] new_points = new double[newPointsAmount][];
            for (int j = 0; j < newPointsAmount; j++)
            {
                new_points[j] = (MeasuredPoint)xxMeasured[j].Clone();
                //new_points[j] = new double[xx[j].Length];
                //Array.Copy(xx[j], new_points[j], xx[j].Length);
                approx.Calculate(new_points[j]);
            }

            double tempErr = 0;
            for (int k = 0; k < new_points.Length; k++)
            {
                //double err = Math.Abs(points[pointAmount - newPointsAmount + k][parser.FunctionDimension] - new_points[k][config.FunctionDimension]);
                double err = approx.GetError(task.originPoints[config.PointAmount - newPointsAmount + k].outputValues, new_points[k].outputValues);
                //Console.WriteLine(" \n " + (points[pointAmount - newPointsAmount + k][parser.FunctionDimension] - new_points[k][parser.FunctionDimension]) + " " + points[pointAmount - newPointsAmount + k][parser.FunctionDimension] + " " + new_points[k][parser.FunctionDimension] + " \n ");
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
    }
}
