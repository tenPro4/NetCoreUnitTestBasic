using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using XTest.Database.Models;

namespace XTesting.Services.Services
{
    public interface IBaseService<T> where T : BaseEntity
    {

        Task<IEnumerable<T>> GetAsync();

        Task<T> GetById(int id);

        IEnumerable<T> Where(Expression<Func<T, bool>> exp);

        void AddOrUpdate(T entry);

        void Remove(int id);
    }
}
