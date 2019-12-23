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

        public static double[][] getArray(MeasuredPoint[] measuredPoints)
        {
            double[][] result = new double[measuredPoints.Length][];
            int N = measuredPoints[0].inputValues.Length;
            int M = measuredPoints[0].outputValues.Length;
            int NM = N + M;
            for (int i = 0; i < measuredPoints.Length; i++)
            {
                result[i] = new double[NM];
                for (int j = 0; j < NM; j++)
                {
                    if (j < N)
                        result[i][j] = measuredPoints[i].inputValues[j];
                    else
                        result[i][j] = measuredPoints[i].outputValues[j - N];
                }
            }
            return result;
        }
    }
}
