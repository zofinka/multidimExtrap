using System;
using System.Collections.Generic;
using System.Text;

namespace Project
{
    class Task
    {
        private Function cheapFunction;
        private Configuration configuration;
        private ApproxPoint[] knownPoints;
        private ApproxPoint[] searchPoints;
        private int n;
        private int m;
        private double accuracy;


        public override string ToString()
        {
            return "";
        }

        public string ToJsonString()
        {
            return "";
        }
    }
}
