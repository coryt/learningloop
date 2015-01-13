using System.Collections.Generic;

namespace LearningLoop.Core.Domain
{
    public class Classroom : Entity
    {
        private readonly IList<Student> _classRoster = new List<Student>();

        public Classroom(string teacherId, string name, bool privateRegistration = false)
        {
            TeacherId = teacherId;
            DisplayName = name;
            PrivateRegistration = privateRegistration;
        }

        public string UniqueName { get; set; }
        public string TeacherId { get; set; }
        public string DisplayName { get; set; }
        public string InviteCode { get; set; }
        public bool PrivateRegistration { get; set; }

        public IEnumerable<Student> ClassRoster
        {
            get { return _classRoster; }
        }

        public void AddNewStudent(string firstName, string lastName, string profileImagePath, string gender)
        {
            _classRoster.Add(new Student(firstName, lastName, profileImagePath, gender));
        }
    }
}