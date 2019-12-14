using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    public interface IParser
    {
        Task parseTask(string pathToTask);
        IConfig parseConfig(string pathToConfig);
    }

    public class Parser : IParser
    {
        private int pointAmount;
        private int functionDimension;

        public Parser()
        {
            pointAmount = -1;
            functionDimension = -1;
        }

        public static void keepSolution(string outputFile, double[][] points)
        {
            using (StreamWriter fs = new StreamWriter(outputFile))
            {

                for (int i = 0; i < points.Length; i++)
                {
                    // points[i][points[i].Length - 1] = 0;
                    fs.WriteLine(String.Join(" ", points[i]));
                }
            }
        }

        public Task parseTask(string pathToTask)
        {
            if (pathToTask.Length == 0)
            {
                Console.WriteLine("Please, set file with calculated points path.");
                return null;
            }
            if (!File.Exists(pathToTask))
            {
                Console.WriteLine("The file with calculated points is not exists: " + pathToTask);
                return null;
            }
            Task task = new Task();
            using (StreamReader fs = new StreamReader(pathToTask))
            {
                string temp = "";
                task.Points = new double[pointAmount][];
                for (int i = 0; i < pointAmount; i++)
                {
                    temp = fs.ReadLine();
                    if (temp == null)
                    {
                        break;
                    }
                    task.Points[i] = new double[functionDimension + 1];
                    string[] strItems = temp.Split(' ');
                    if (strItems.Length != functionDimension + 1)
                    {
                        Console.WriteLine("The vector " + temp + " length is not equel function demention " + functionDimension);
                        continue;
                    }
                    for (int j = 0; j < functionDimension + 1; j++)
                    {
                        task.Points[i][j] = double.Parse(strItems[j]);
                    }
                }
            }
            return task;
        }

        public IConfig parseConfig(string pathToConfig)
        {
            if (pathToConfig.Length == 0)
            {
                Console.WriteLine("Please, set config file path.");
                return null;
            }
            if (!File.Exists(pathToConfig))
            {
                Console.WriteLine("The config file is not exists: " + pathToConfig);
                return null;
            }
            Config config = new Config();
            using (StreamReader fs = new StreamReader(pathToConfig))
            {
                config.FunctionDimension = Int32.Parse(fs.ReadLine());
                config.PointAmount = Int32.Parse(fs.ReadLine());
                config.PredictionPointAmount = Int32.Parse(fs.ReadLine());
                config.Approximation = float.Parse(fs.ReadLine());
                pointAmount = config.PointAmount;
                functionDimension = config.FunctionDimension;
                string temp = fs.ReadLine();
                string[] strItems = temp.Split(' ');
                config.Min = new double[config.FunctionDimension];
                if (strItems.Length != config.FunctionDimension)
                {
                    Console.WriteLine("The min vector " + temp + " length is not equel function demention " + config.FunctionDimension);
                }
                else
                {
                    for (int j = 0; j < config.FunctionDimension; j++)
                    {
                        config.Min[j] = double.Parse(strItems[j]);
                    }
                }

                temp = fs.ReadLine();
                strItems = temp.Split(' ');
                config.Max = new double[config.FunctionDimension];
                if (strItems.Length != config.FunctionDimension)
                {
                    Console.WriteLine("The max vector " + temp + " length is not equel function demention " + config.FunctionDimension);
                }
                else
                {
                    for (int j = 0; j < config.FunctionDimension; j++)
                    {
                        config.Max[j] = double.Parse(strItems[j]);
                    }
                }
            }
            return config;
        }
    }
}
