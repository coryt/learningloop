using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using LearningLoop.Core.Domain;
using LearningLoop.Core.DomainServices;

namespace LearningLoop.Infrastructure.Repositories
{
    public class ClassroomRepository : IClassroomRepository
    {
        private IDictionary<long, Classroom> _context;

        public ClassroomRepository()
        {
            _context = new ConcurrentDictionary<long, Classroom>();
        }

        public IEnumerable<Classroom> GetAll()
        {
            return _context.Values;
        }

        public IEnumerable<Classroom> GetByIds(IEnumerable<long> ids)
        {
            return _context.Values.Where(classes => ids.Any(id => id == classes.Id));
        }

        public Classroom GetById(long id)
        {
            return _context[id];
        }

        public Classroom Save(Classroom classroom)
        {
            if (classroom == null)
                throw new ArgumentNullException("classroom", "classroom cannot be null");

            _context.Add(classroom.Id, classroom);
            return classroom;
        }

        public void Update(Classroom updatedClassroom)
        {
            if (updatedClassroom == null)
                throw new ArgumentNullException("updatedClassroom", "updatedClassroom cannot be null");

            // Wrap this in a try catch so we can return a meaningful exception message
            try
            {
                if (_context.ContainsKey(updatedClassroom.Id))
                    _context[updatedClassroom.Id] = updatedClassroom;
                else
                    throw new NullReferenceException("This classroom does not exist");
            }
            catch (Exception)
            {
                throw new NullReferenceException("This classroom does not exist");
            }
        }

        public void Delete(long id)
        {
            _context.Remove(id);
        }
    }
}
