using System.Collections.Generic;

namespace TrafficReport.DAL
{
    interface IDataGateway<T> where T : class
    {
        IEnumerable<T> SelectAll();
        T SelectById(int? id);
        void Insert(T obj);
        void Update(T obj);
        T Delete(int? id);
        void Save();
    }
}
