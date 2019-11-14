  using System;

namespace Solver
{
    namespace MLAlgorithms
    {
        public class RandomForestParams : AMLAlgorithmParams
    {
        // Training set size, NPoints>=1
        public int NPoints { get; private set; }

        // Number of independent variables, NVars>=1
        public int NVars { get; private set; }

        // Number of classes
        public int NClasses { get; private set; }

        // Number of trees in a forest, NTrees>=1.
        // recommended values: 50-100.
        public int NTrees { get; private set; }

        // Number of variables used when choosing best split
        public int NRndvars { get; private set; }

        // Percent of a training set used to build
        //     individual trees. 0<R<=1.
        //     recommended values: 0.1 <= R <= 0.66.
        public double R { get; private set; }

        public RandomForestParams(LabeledData[] xy, int npoints, int nvars, int nclasses,
                                  int ntrees, int nrndvars, double r):
                                  base(xy)
        {
            NPoints = npoints;
            NVars = nvars;
            NClasses = nclasses;
            NTrees = ntrees;
            NRndvars = nrndvars;
            R = r;
        }
    }

    public class RandomForest : IMLAlgorithm
    {
        //use EVS!? (extended variable selection)
        public void train<T>(AMLAlgorithmParams p)
        {
            param = p as RandomForestParams;
            if (param == null)
            {
                 throw new ArgumentException("RandomForest::train accepts only RandomForestParams type for parameters object");
            }

            // XY - input table, where rows are samples and columns are features.
            //     Last column is responsible for the label.
            double[,] XY = new double[param.NPoints, param.NVars + 1];
            for (int i = 0; i < param.NPoints; i++)
            {
                for (int j = 0; j < param.NVars; j++)
                {
                    XY[i, j] = param.XY[i].data[j];
                }
                XY[i, param.NVars] = Convert.ToDouble((T)(param.XY[i].label));
            }

            alglib.dfbuildrandomdecisionforestx1(XY, param.NPoints, param.NVars, param.NClasses,
                                                 param.NTrees, param.NRndvars, param.R, out info, out df, out rep);
            if (info <= 0)
            {
                throw new Exception("Something went wrong during Random Forest model build");
            }
        }

        // Make this also generic
        public void infer(double[] x, out Object label) //where T is convertible from int
        {
            // alglib.dfprocess outputs vector of possibilities for the classes of given data.

            double[] yp  = new double[param.NClasses];
            alglib.dfprocess(df, x, ref yp);

            if (this.param.NClasses > 1)
            {
                double max = 0.0;
                int labelInd = 0;
                for (int i = 0; i < param.NClasses; i++)
                {
                    if (yp[i] > max)
                    {
                        max = yp[i];
                        labelInd = i;
                    }
                }

                label = labelInd;
            }
            else
            {
                 label = yp[0];    
            }

            // For debug purposes only
            //Console.WriteLine();
            //Console.WriteLine("Possibilities vector");
            //for (int i = 0; i < param.NClasses; i++)
            //{
            //    Console.Write(" " + yp[i]);
            //}
            //Console.WriteLine("\nEnd of possibilities vector");
            //Console.WriteLine();
        }

        public void validate<T>(LabeledData[] xy, out double modelPrecision) where T : IComparable
        {
            double error = 0;
            for (int i = 0; i < xy.Length; i++)
            {
                Object label;
                infer(xy[i].data, out label);
                if (param.NClasses > 1)
                {
                    Console.WriteLine("Predicted = {0}, expected = {1}", label, xy[i].label);
                    if (!((T)label).Equals((T)xy[i].label))
                    {
                        error++;
                    }
                }
                else
                {
                    Console.WriteLine("Predicted = {0}, expected = {1}", label, xy[i].label);
                    error += Math.Pow(Convert.ToDouble(label) - Convert.ToDouble(xy[i].label), 2);
                }
            }

            modelPrecision = error / (double)xy.Length;
        }

        RandomForestParams param = null;
        // Return code:
        //     * -2, if there is a point with class number
        //           outside of[0..NClasses - 1].
        //     * -1, if incorrect parameters was passed
        //           (NPoints<1, NVars<1, NClasses<1, NTrees<1, R<=0
        //           or R>1).
        //     *  1, if task has been solved
        int info = 0;
        // Model built
        alglib.decisionforest df = new alglib.decisionforest();
        // Training report, contains error on a training set
        // and out-of-bag estimates of generalization error.
        alglib.dfreport rep = new alglib.dfreport();
        }
}
}