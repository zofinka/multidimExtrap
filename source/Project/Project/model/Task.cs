using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    public class Task
    {
        public MeasuredPoint[] originPoints;
        public MeasuredPoint[] extPoints;
        public IFunction function;

        public Task(MeasuredPoint[] originPoints)
        {
            this.originPoints = originPoints;
        }

        public Task(int pointAmount, int predictionPointAmount)
        {
            originPoints = new MeasuredPoint[pointAmount];
        }
    }
}
