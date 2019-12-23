using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Approx
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

        public static string configFile = @"d:\Study\Archive\project\config.cfg";
        public static string pointFile = @"d:\Study\Archive\project\points.txt";
        public static string[] learnFunctions;
        public static string testFunction;

        static void Main(string[] args)
        {
            learnFunctions = new string[] { SIN_X_COS_X_COS_Y };
            testFunction = SIN_X_COS_X_COS_Y;
            Console.WriteLine("Please write path to config file:");
            configFile = Console.ReadLine();
            Console.WriteLine("Please write path to points file:");
            pointFile = Console.ReadLine();
            Console.WriteLine(testFunction + " Test START");

            IParser parser = new Parser();
            IConfig config = parser.parseConfig(configFile);
            Task task = parser.parseTask(pointFile, config);
            task.function = TestFunctionGetter.getInstance(testFunction).GetFunc();

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
            Console.WriteLine(config.Approximation > 0.8);
            while (i < 100000000 && maxErr > config.Approximation)
            {
                maxErr = solver.calculate(task);
                Console.WriteLine("maxErr = " + maxErr);
                i++;
            }

            solver.testResult(Solver.func);
            Console.WriteLine(testFunction + " Test END in " + i + " iterations");
            Console.ReadKey();
        }
    }
}
