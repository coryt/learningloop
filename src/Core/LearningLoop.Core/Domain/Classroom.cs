using System.Collections.Generic;

namespace LearningLoop.Core.Domain
{
    public class Classroom
    {
        public Classroom()
        {
            Roster = new List<Student>();
        }
        public string Id { get; private set; }
        public string UniqueName { get; set; }
        public string DisplayName { get; set; }

        public List<Student> Roster { get; set; }
    }

    public class Student
    {
        public string Id { get; private set; }
        public string UniqueName { get; set; }
        public string DisplayName { get; set; }
        public string Gender { get; set; }
    }
}
