using System.Collections.Generic;
using LearningLoop.Core.Domain;

namespace LearningLoop.Core.DomainServices
{
    public interface IClassroomRepository : IRepository<Classroom>
    {
        IEnumerable<Classroom> GetAll();
        IEnumerable<Classroom> GetByIds(IEnumerable<long> ids);
        Classroom Save(Classroom classroom);
        Classroom GetById(long id);
        void Delete(long id);
        void Update(Classroom updatedClassroom);
    }
}
