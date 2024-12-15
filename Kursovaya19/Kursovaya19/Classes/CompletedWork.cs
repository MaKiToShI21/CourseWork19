using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kursovaya19.Classes
{
    public class CompletedWork
    {
        public string TypeOfWork {  get; set; }
        public double PercentOfCompletedWork { get; set; }

        public CompletedWork(string typeOfWork, double percentOfCompletedWork)
        {
            TypeOfWork = typeOfWork;
            PercentOfCompletedWork = percentOfCompletedWork;
        }

        public CompletedWork(CompletedWork other)
        {
            TypeOfWork = other.TypeOfWork;
            PercentOfCompletedWork = other.PercentOfCompletedWork;
        }

        public CompletedWork() { }
    }
}
