using System;
using MediatR;

namespace LearningLoop.Core.Domain.Commands
{
    public class CreateClassroomCommand : IRequest<Classroom>
    {
       
        public string TeacherId { get; set; }
        public string Name { get; set; }

        public CreateClassroomCommand(string teacherId, string className)
        {
            TeacherId = teacherId;
            Name = className;
        }

        public override bool Equals(Object other)
        {
            if (other == null)
                return false;

            var otherCommand = other as CreateClassroomCommand;
            if (otherCommand == null)
                return false;

            return otherCommand.Name == Name &&
               otherCommand.TeacherId == TeacherId;
        }

        protected bool Equals(CreateClassroomCommand other)
        {
            return string.Equals(TeacherId, other.TeacherId) && string.Equals(Name, other.Name);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((TeacherId != null ? TeacherId.GetHashCode() : 0) * 397) ^ (Name != null ? Name.GetHashCode() : 0);
            }
        }

    }
}