using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Approx
{
    public interface IConfig
    {
        ///// <summary>
        ///// количество точек которые было расчитано ранее и подается на вход
        ///// </summary>
        int PointAmount { get; set; }
        ///// <summary>
        ///// точность
        ///// </summary>
        float Approximation { get; set; }
        ///// <summary>
        ///// N - размерность функции
        ///// </summary>
        int FunctionDimension { get; set; }
        ///// <summary>
        ///// M - число зависимых переменных
        ///// </summary>
        int DependentVariablesNum { get; set; }
        /// <summary>
        /// количество точек которое мы готовы получить на выходе, чтобы расчитать в них точные значения
        /// </summary>
        int PredictionPointAmount { get; set; }
        /// <summary>
        /// N-мерный вектор минимальных значений входных парамеров 
        /// </summary>
        double[] Min { get; set; }
        /// <summary>
        /// N-мерный вектор максимальных значений входных парамеров 
        /// </summary>
        double[] Max { get; set; }
    }
    class Config : IConfig
    {
        public float Approximation { get; set; }
        public int FunctionDimension { get; set; }
        public int PredictionPointAmount { get; set; }
        public double[] Min { get; set; }
        public double[] Max { get; set; }
        public int PointAmount { get; set; }
        public int DependentVariablesNum { get; set; }
    }
}
