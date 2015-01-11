using System.Collections.Generic;

namespace LearningLoop.Core.Domain
{
    public class Classroom
    {
        public Classroom()
        {
            ClassRoster = new List<Student>();
        }
        public string Id { get; private set; }
        public string UniqueName { get; set; }
        public string DisplayName { get; set; }
        public string InviteCode { get; set; }
        public bool PrivateRegistration { get; set; }

        public List<Student> ClassRoster { get; set; }
    }
}
