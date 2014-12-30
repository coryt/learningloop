using System.Collections.Generic;

namespace LearningLoop.Core.DomainServices
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll();
        IEnumerable<T> GetByIds(IEnumerable<string> ids);

        T GetById(string id);
        T Save(T classroom);
        void Update(T updatedClassroom);
        void Delete(string id);
    }
}