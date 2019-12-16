using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    public class MeasuredPoint : ICloneable
    {
        public double[] inputValues;
        public double[] outputValues;

        public MeasuredPoint(int functionDimension, int dependentVariablesNum)
        {
            inputValues = new double[functionDimension];
            outputValues = new double[dependentVariablesNum];
        }

        public object Clone()
        {
            MeasuredPoint measuredPoint = new MeasuredPoint(this.inputValues.Length, this.outputValues.Length);
            measuredPoint.inputValues = (double[])this.inputValues.Clone();
            measuredPoint.outputValues = (double[])this.outputValues.Clone();
            return measuredPoint;
        }

        public static MeasuredPoint getPointFromDouble(double[] input, int N)
        {
            MeasuredPoint point = new MeasuredPoint(N, input.Length - N);
            for (int i = 0; i < input.Length; i++)
            {
                if (i < N)
                    point.inputValues[i] = input[i];
                else
                    point.outputValues[i - N] = input[i];
            }
            return point;
        }

        public static MeasuredPoint[] getArrayFromDouble(double[][] input, int N)
        {
            MeasuredPoint[] points = new MeasuredPoint[input.Length];
            for (int i = 0; i < points.Length; i++)
            {
                points[i] = getPointFromDouble(input[i], N);
            }
            return points;
        }
    }
}
