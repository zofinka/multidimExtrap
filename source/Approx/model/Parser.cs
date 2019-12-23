using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Approx
{
    public interface IParser
    {
        Task parseTask(string pathToTask, IConfig config);
        IConfig parseConfig(string pathToConfig);
        Dictionary<double[], double[]> parseTable(string pathToTable, IConfig config);
    }

    public class Parser : IParser
    {
        public Task parseTask(string pathToTask, IConfig config)
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
            Task task = new Task(config.PointAmount, config.FunctionDimension, config.DependentVariablesNum);
            using (StreamReader fs = new StreamReader(pathToTask))
            {
                string temp = "";
                for (int i = 0; i < config.PointAmount; i++)
                {
                    temp = fs.ReadLine();
                    if (temp == null)
                    {
                        break;
                    }
                    string[] strItems = temp.Split(' ');
                    if (strItems.Length != config.FunctionDimension + config.DependentVariablesNum)
                    {
                        Console.WriteLine("The vector " + temp + " length is not equel function demention " + config.FunctionDimension);
                        continue;
                    }
                    for (int j = 0; j < config.FunctionDimension + config.DependentVariablesNum; j++)
                    {
                        task.points[i][j] = double.Parse(strItems[j]);
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
                string temp = fs.ReadLine();
                string[] strItems = temp.Split(' ');
                if (strItems.Length != 2)
                {
                    Console.WriteLine("The first line of config file has incorrect format. It should be 'FunctionDimension DependentVariablesNum'");
                }
                else
                {
                    config.FunctionDimension = Int32.Parse(strItems[0]);
                    config.DependentVariablesNum = Int32.Parse(strItems[1]);
                }
                config.PointAmount = Int32.Parse(fs.ReadLine());
                config.PredictionPointAmount = Int32.Parse(fs.ReadLine());
                config.Approximation = float.Parse(fs.ReadLine());
                temp = fs.ReadLine();
                strItems = temp.Split(' ');
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

        public Dictionary<double[], double[]> parseTable(string pathToTable, IConfig config)
        {
            Dictionary<double[], double[]> table = null;
            if (pathToTable.Length == 0)
            {
                Console.WriteLine("Please, set table file path.");
                return null;
            }
            if (!File.Exists(pathToTable))
            {
                Console.WriteLine("The table file is not exists: " + pathToTable);
                return null;
            }

            using (StreamReader fs = new StreamReader(pathToTable))
            {
                string temp = "";
                table = new Dictionary<double[], double[]>();

                temp = fs.ReadLine();
                while (temp != null)
                {
                    var point = new double[config.FunctionDimension];
                    var responce = new double[config.DependentVariablesNum];

                    string[] strItems = temp.Split(';');

                    if (strItems.Length != config.FunctionDimension + config.DependentVariablesNum)
                    {
                        Console.WriteLine("The vector " + temp + " length is not equel function demention " + config.FunctionDimension);
                        continue;
                    }
                    for (int j = 0; j < config.FunctionDimension; j++)
                    {
                        point[j] = double.Parse(strItems[j]);
                    }
                    for (int j = 0; j < config.DependentVariablesNum; j++)
                    {
                        responce[j] = double.Parse(strItems[config.FunctionDimension + j]);
                    }
                    table.Add(point, responce);

                    temp = fs.ReadLine();
                }
            }
            return table;
        }
    }
}
