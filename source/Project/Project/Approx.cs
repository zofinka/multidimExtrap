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

        public static string configFile = @"d:\Study\Archive\project\config.cfg";
        public static string pointFile = @"d:\Study\Archive\project\points.txt";
        public static string testFunction;
        static void Main(string[] args)
        {
            testFunction = SIN_X_COS_X_COS_Y;
            Console.WriteLine(testFunction + " Test START");

            IParser parser = new Parser();
            IConfig config = parser.parseConfig(configFile);
            Task task = parser.parseTask(pointFile, config);

            ISolver solver = new Solver();
            solver.setConfig(config);
            
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
