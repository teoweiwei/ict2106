using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficReport.DAL
{
    interface IDataGateway<T> where T : class
    {
        IEnumerable<T> SelectAll();
        T SelectById(int? id);
        void Insert(T obj);
        void Update(T obj);
        T Delete(int? id);
        int MonthDifference(DateTime lValue, DateTime rValue);
        void Save();
    }
}
