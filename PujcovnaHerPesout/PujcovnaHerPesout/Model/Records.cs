using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PujcovnaHerPesout.Model
{
    public class Records<T>
    {
        private LiteCollection<T> collection;
        public Type RecordType { get; }
        public Records(LiteCollection<T> collection)
        {
            this.collection = collection;
            RecordType = typeof(T);
        }

        public List<T> GetAll()
        {
            return collection.FindAll().ToList();
        }

        public void Add(T item)
        {
            collection.Insert(item);
        }

        public void Delete(int id)
        {
            collection.Delete(id);
        }

        public void Update(T item)
        {
            collection.Update(item);
        }
    }
}
