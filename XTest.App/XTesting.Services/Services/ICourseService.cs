using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using XTest.Database.Models;
using XTesting.Services.Models;

namespace XTesting.Services.Services
{
    public interface ICourseService
    {
        void AddOrUpdate(CourseResponseModel entry);
        Task<IEnumerable<CourseResponseModel>> GetAsync();
        Task<CourseResponseModel> GetById(int id);
        void Remove(int id);
        IEnumerable<CourseResponseModel> Where(Expression<Func<Course, bool>> exp);
    }
}
