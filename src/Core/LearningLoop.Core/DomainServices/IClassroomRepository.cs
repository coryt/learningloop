using System.Collections.Generic;
using LearningLoop.Core.Domain;

namespace LearningLoop.Core.DomainServices
{
    public interface IClassroomRepository : IRepository<Classroom>
    {
        IEnumerable<Classroom> GetAll();
        IEnumerable<Classroom> GetByIds(IEnumerable<string> ids);
        Classroom Save(Classroom classroom);
        Classroom GetById(string id);
        void Delete(string id);
        void Update(Classroom updatedClassroom);
    }
}
