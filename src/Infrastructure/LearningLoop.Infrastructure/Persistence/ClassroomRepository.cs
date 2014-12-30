using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using LearningLoop.Core.Domain;
using LearningLoop.Core.DomainServices;
using Raven.Client;

namespace LearningLoop.Infrastructure.Repositories
{
    public class ClassroomRepository : IClassroomRepository
    {
        private readonly IDocumentSession _session;

        public ClassroomRepository(IDocumentSession session)
        {
            _session = session;
        }

        public IEnumerable<Classroom> GetAll()
        {
            return _session.Query<Classroom>();
        }

        public IEnumerable<Classroom> GetByIds(IEnumerable<string> ids)
        {
            return _session.Query<Classroom>().Where(classes => ids.Any(id => id == classes.Id));
        }

        public Classroom GetById(string id)
        {
            return _session.Load<Classroom>(id);
        }

        public Classroom Save(Classroom classroom)
        {
            if (classroom == null)
                throw new ArgumentNullException("classroom", "classroom cannot be null");

            _session.Store(classroom);
            return classroom;
        }

        public void Update(Classroom updatedClassroom)
        {
            if (updatedClassroom == null)
                throw new ArgumentNullException("updatedClassroom", "updatedClassroom cannot be null");

            // Wrap this in a try catch so we can return a meaningful exception message
            try
            {
                var classroom = _session.Load<Classroom>(updatedClassroom.Id);
                if (classroom != null)
                    _session.Store(updatedClassroom);
                else
                    throw new NullReferenceException("This classroom does not exist");
            }
            catch (Exception)
            {
                throw new NullReferenceException("This classroom does not exist");
            }
        }

        public void Delete(string id)
        {
            _session.Delete(id);
        }
    }
}
