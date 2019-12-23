using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    class Approx
    {
        const string PYRAMYDE_VOLUME = "PyramidVolume";
        const string SIN_FROM_SUM_ON_SUM = "SinFromSumOnSum";
        const string SIN_X_COS_X_COS_Y = "SzinXCosXCosY";
        const string SIN_X_COS_Y = "SinXCosY";
        const string SQUARE_AREA = "SquareArea";
        const string SQUARE_PRODUCTS = "SquaresProducts";
        const string TRUNC_PYRAMID = "TruncPyramid";
        const string LG_FUNC = "LGFunc";
        const string LN_XY3 = "LnXY3";
        const string SQRT_X_SQRT_Y = "SqrtXSqrtY";

        public static string configFile = @"C:\Users\kiasu\Ikovaleva\Study\input\config.cfg";
        public static string pointFile = @"C:\Users\kiasu\Ikovaleva\Study\input\points.txt";
        public static string[] learnFunctions;
        public static string testFunction;
        
        static void Main(string[] args)
        {
            learnFunctions = new string[] { SIN_X_COS_X_COS_Y };
            testFunction = SIN_X_COS_X_COS_Y;

            Console.WriteLine(testFunction + " Test START");

            IParser parser = new Parser();
            IConfig config = parser.parseConfig(configFile);
            Task task = parser.parseTask(pointFile, config);

            foreach (string func in TestFunctionGetter.funcsWithTable.Keys)
            {
                if (learnFunctions.Contains(func) || testFunction == func)
                {
                    Console.WriteLine("Please write path to table file for function " + func);
                    string pathToTable = Console.ReadLine();
                    TestFunctionGetter.getInstance(func).SetTable(parser.parseTable(pathToTable, config));
                }
            }

            ISolver solver = new Solver();
            solver.setConfig(config);

            Func<double[], double[]>[] functions = new Func<double[], double[]>[learnFunctions.Length];
            for (int j = 0; j < learnFunctions.Length; j++)
            {
                functions[j] = TestFunctionGetter.getInstance(learnFunctions[j]).GetFunc();
            }
            solver.setCLassifier(functions, task);

            int i = 0;
            double maxErr = 10;
            while (i < 100000000 && maxErr > config.Approximation)
            {
                maxErr = solver.calculate(task);
                i++;
            }

            solver.testResult(Solver.func);
            Console.WriteLine(testFunction + " Test END in " + i + " iterations");
            Console.ReadKey();
        }
    }
}
