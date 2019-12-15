using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    public class Task
    {
        /// <summary>
        /// путь до файла с уже расчитанными точками и значениями функций в этих точках
        /// формат точки: x1, x2, .. xn, y
        /// </summary>
        //public double[][] Points { get; set; }
        public MeasuredPoint[] originPoints;
        public MeasuredPoint[] extPoints;
        public IFunction function;
        /// <summary>
        /// количество точек которые было расчитано ранее и подается на вход
        /// </summary>
        //public int pointAmount;
        /// <summary>
        /// точность
        /// </summary>
        //public float approximation;
        /// <summary>
        /// N - размерность функции
        /// </summary>

        public Task(MeasuredPoint[] originPoints)
        {
            this.originPoints = originPoints;
        }

        public Task(int pointAmount, int predictionPointAmount)
        {
            originPoints = new MeasuredPoint[pointAmount];
            //extPoints = new MeasuredPoint[predictionPointAmount];
        }
    }
}
