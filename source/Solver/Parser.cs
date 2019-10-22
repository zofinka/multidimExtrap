using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Solver
{
    class Parser
    {
        /// <summary>
        /// количество точек которые было расчитано ранее и подается на вход
        /// </summary>
        public int PointAmount { get; protected set; }
        /// <summary>
        /// точность
        /// </summary>
        public float Approximation { get; protected set; }
        /// <summary>
        /// N - размерность функции
        /// </summary>
        public int FunctionDimension { get; protected set; }
        /// <summary>
        /// количество точек которое мы готовы получить на выходе, чтобы рачитать в них точные значения
        /// </summary>
        public int PredictionPointAmount { get; protected set; }
        /// <summary>
        /// путь до файла с уже расчитанными точками и значениями функций в этих точках
        /// формат точки: x1, x2, .. xn, y
        /// </summary>
        public double[][] Points { get; protected set; }
        /// <summary>
        /// путь до конфигурационного файла
        /// струтктура файла:
        /// N или functionDimension - размерность фунции 
        /// pointAmount - кpredictionPointAmountоличество уже расчитанных точек
        /// predictionPointAmount - количество точек которое нужно расчитать 
        /// approximation - точность
        /// min - N-мерный вектор минимальных значений входных парамеров 
        /// max - N-мерный вектор минимальных значений входных парамеров 
        /// </summary>
        private string configFile;
        /// <summary>
        /// путь до файла с уже расчитанными точками и значениями функций в этих точках
        /// формат точки: x1, x2, .. xn, y
        /// </summary>
        private string fileWithPoints;
        /// <summary>
        /// N-мерный вектор минимальных значений входных парамеров 
        /// </summary>
        public double[] Min { get; protected set; }
        /// <summary>
        /// N-мерный вектор максимальных значений входных парамеров 
        /// </summary>
        public double[] Max { get; protected set; }

        public Parser()
        {
            PointAmount = -1;
            Approximation = -1;
            FunctionDimension = -1;
            PredictionPointAmount = -1;
            Points = new double[0][];
            configFile = "";
            fileWithPoints = "";
            Min = new double[0];
            Max = new double[0];
            doParse();
        }

        public Parser(string configFile, string fileWithPoint)
        {
            this.configFile = configFile;
            this.fileWithPoints = fileWithPoint;
            PointAmount = -1;
            Approximation = -1;
            FunctionDimension = -1;
            PredictionPointAmount = -1;
            Points = new double[0][];
            Min = new double[0];
            Max = new double[0];
            doParse();
        }

        public void setConfigFile(string configFile)
        {
            this.configFile = configFile;
        }

        public void setFileWithPoints(string fileWithPoints)
        {
            this.fileWithPoints = fileWithPoints;
        }

        public int doParse()
        {
            if (this.configFile.Length == 0)
            {
                Console.WriteLine("Please, set config file path.");
                return 1;
            }
            if (this.fileWithPoints.Length == 0)
            {
                Console.WriteLine("Please, set file with calculated points path.");
                return 1;
            }
            if (!File.Exists(configFile))
            {
                Console.WriteLine("The config file is not exists: " + configFile);
                return 1;
            }
            if (!File.Exists(fileWithPoints))
            {
                Console.WriteLine("The file with calculated points is not exists: " + fileWithPoints);
                return 1;
            }

            using (StreamReader fs = new StreamReader(this.configFile))
            {
                FunctionDimension = Int32.Parse(fs.ReadLine());
                PointAmount = Int32.Parse(fs.ReadLine());
                PredictionPointAmount = Int32.Parse(fs.ReadLine());
                Approximation = float.Parse(fs.ReadLine());
                string temp = fs.ReadLine();
                string[] strItems = temp.Split(' ');
                Min = new double[FunctionDimension];
                if (strItems.Length != FunctionDimension)
                {
                    Console.WriteLine("The min vector " + temp + " length is not equel function demention " + FunctionDimension);
                } else
                {
                    for (int j = 0; j < FunctionDimension; j++)
                    {
                        Min[j] = double.Parse(strItems[j]);
                    }
                }

                temp = fs.ReadLine();
                strItems = temp.Split(' ');
                Max = new double[FunctionDimension];
                if (strItems.Length != FunctionDimension)
                {
                    Console.WriteLine("The max vector " + temp + " length is not equel function demention " + FunctionDimension);
                }
                else
                {
                    for (int j = 0; j < FunctionDimension; j++)
                    {
                        Max[j] = double.Parse(strItems[j]);
                    }
                }
            }


            using (StreamReader fs = new StreamReader(fileWithPoints))
            {
                string temp = "";
                Points = new double[PointAmount][];
                for (int i = 0; i < PointAmount; i++)
                {
                    temp = fs.ReadLine();
                    if (temp == null)
                    {
                        break;
                    }
                    Points[i] = new double[FunctionDimension + 1];
                    string[] strItems = temp.Split(' ');
                    if (strItems.Length != FunctionDimension + 1)
                    {
                        Console.WriteLine("The vector " + temp + " length is not equel function demention " + FunctionDimension);
                        continue;
                    }
                    for (int j = 0; j < FunctionDimension + 1; j++)
                    {
                        Points[i][j] = double.Parse(strItems[j]);
                    }
                }
            }
            return 0;
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
    }
}
