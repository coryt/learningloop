namespace LearningLoop.Core.Domain
{
    public class Student
    {
        public string Id { get; private set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfileImagePath { get; set; }
        public string Gender { get; set; }

        public Student(string firstName, string lastName, string profileImagePath, string gender)
        {
            FirstName = firstName;
            LastName = lastName;
            ProfileImagePath = profileImagePath;
            Gender = gender;
        }
    }
}