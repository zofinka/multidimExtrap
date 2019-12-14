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
        public double[][] Points { get; set; }
    }
}
