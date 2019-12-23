using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Solver
{
    public class Parser
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
        public int N_Dimension { get; protected set; }
        /// <summary>
        /// M - количество выходных значений
        /// </summary>
        public int M_Dimension { get; protected set; }
        /// <summary>
        /// NM - количество входных + выходных значений
        /// </summary>
        public int NM_Dimension { get; protected set; }
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
        /// Путь до файла со всеми значениями функции на определенной таблице
        /// области определения
        /// формат точки: x1, x2, .. xn, y
        /// </summary>
        private string tableFile;
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
            N_Dimension = -1;
            M_Dimension = -1;
            NM_Dimension = -1;
            PredictionPointAmount = -1;
            Points = new double[0][];
            configFile = "";
            fileWithPoints = "";
            tableFile = "";
            Min = new double[0];
            Max = new double[0];
            doParse();
        }

        public Parser(string configFile, string fileWithPoint, string tableFile = null)
        {
            this.configFile = configFile;
            this.fileWithPoints = fileWithPoint;
            this.tableFile = tableFile;
            PointAmount = -1;
            Approximation = -1;
            N_Dimension = -1;
            M_Dimension = -1;
            NM_Dimension = -1;
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
                string dims = fs.ReadLine();
                string[] dimsItems = dims.Split(' ');
                N_Dimension = Int32.Parse(dimsItems[0]);
                M_Dimension = Int32.Parse(dimsItems[1]);
                NM_Dimension = N_Dimension + M_Dimension;

                PointAmount = Int32.Parse(fs.ReadLine());
                PredictionPointAmount = Int32.Parse(fs.ReadLine());
                Approximation = float.Parse(fs.ReadLine());
                string temp = fs.ReadLine();
                string[] strItems = temp.Split(' ');
                Min = new double[N_Dimension];
                if (strItems.Length != N_Dimension)
                {
                    Console.WriteLine("The min vector " + temp + " length is not equel function demention " + N_Dimension);
                } else
                {
                    for (int j = 0; j < N_Dimension; j++)
                    {
                        Min[j] = double.Parse(strItems[j]);
                    }
                }

                temp = fs.ReadLine();
                strItems = temp.Split(' ');
                Max = new double[N_Dimension];
                if (strItems.Length != N_Dimension)
                {
                    Console.WriteLine("The max vector " + temp + " length is not equel function demention " + N_Dimension);
                }
                else
                {
                    for (int j = 0; j < N_Dimension; j++)
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
                    Points[i] = new double[N_Dimension + M_Dimension];
                    string[] strItems = temp.Split(' ');
                    if (strItems.Length != NM_Dimension)
                    {
                        Console.WriteLine("The vector " + temp + " length is not equel function demention " + N_Dimension);
                        continue;
                    }
                    for (int j = 0; j < N_Dimension + M_Dimension; j++)
                    {
                        Points[i][j] = double.Parse(strItems[j]);
                    }
                }
            }

            if (tableFile != null)
            {
                using (StreamReader fs = new StreamReader(tableFile))
                {
                    string temp = "";
                    table = new Dictionary<double[], double[]>();

                    temp = fs.ReadLine();
                    while (temp != null)
                    {
                        var point = new double[N_Dimension];
                        var responce = new double[M_Dimension];

                        string[] strItems = temp.Split(';');

                        if (strItems.Length != NM_Dimension)
                        {
                            Console.WriteLine("The vector " + temp + " length is not equel function demention " + NM_Dimension);
                            continue;
                        }
                        for (int j = 0; j < N_Dimension; j++)
                        {
                            point[j] = double.Parse(strItems[j]);
                        }
                        for (int j = 0; j < M_Dimension; j++)
                        {
                            responce[j] = double.Parse(strItems[N_Dimension + j]);
                        }
                        table.Add(point, responce);

                        temp = fs.ReadLine();
                    }
                }
            }
            return 0;
        }


        public List<double[]> get_points(string pointFile, int demention)
        {
            List<double[]> points = new List<double[]>();
            
            using (StreamReader fs = new StreamReader(pointFile))
            {
                string temp = "";
                table = new Dictionary<double[], double[]>();

                temp = fs.ReadLine();
                while (temp != null)
                {
                    var point = new double[demention];

                    string[] strItems = temp.Split(';');
                    for (int j = 0; j < demention; j++)
                    {
                        point[j] = double.Parse(strItems[j]);
                    }
                    
                    points.Add(point);

                    temp = fs.ReadLine();
                }
            }
            
            return points;
        }


        public Dictionary<double[], double[]> getTable()
        {
            return table;
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

        private Dictionary<double[], double[]> table = null;
    }
}
