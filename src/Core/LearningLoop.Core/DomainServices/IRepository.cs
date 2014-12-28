using System.Collections.Generic;

namespace LearningLoop.Core.DomainServices
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll();
        IEnumerable<T> GetByIds(IEnumerable<long> ids);

        T GetById(long id);
        T Save(T classroom);
        void Update(T updatedClassroom);
        void Delete(long id);
    }
}