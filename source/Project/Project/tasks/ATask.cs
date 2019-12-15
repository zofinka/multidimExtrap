using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    public abstract class BaseTask<T> where T : BaseTask<T>, new()
    {
        //private static ATask instance;
        private static bool IsInstanceCreated = false;

        private static readonly Lazy<T> LazyInstance = new Lazy<T>(() =>
        {
            IsInstanceCreated = true;
            T instance = new T();
            return instance;
        });

        protected BaseTask()
        {
            if (IsInstanceCreated)
            {
                throw new InvalidOperationException("Constructing a " + typeof(T).Name +
                    " manually is not allowed, use the Instance property.");
            }
        }

        public static T Instance
        {
            get
            {
                return LazyInstance.Value;
            }
        }

        //private ATask() {}

        //public static ATask getInstance()
        //{
        //    if (instance == null)
        //        instance = new ATask();
        //    return instance;
        //}

        //protected string configFile;
        //protected string pointFile;
        //private string name;

        //public string Config { get => config; set => config = value; }

        public abstract double[] func(double[] points);
        public abstract double[] derivative(double[] points);
    }
}
