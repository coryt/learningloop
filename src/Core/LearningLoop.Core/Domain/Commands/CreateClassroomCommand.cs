using System;
using MediatR;

namespace LearningLoop.Core.Domain.Commands
{
    public class CreateClassroomCommand : IRequest
    {
       
        public string UserId { get; set; }
        public string Name { get; set; }

        public CreateClassroomCommand(string userId, string className)
        {
            UserId = userId;
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
               otherCommand.UserId == UserId;
        }

        protected bool Equals(CreateClassroomCommand other)
        {
            return string.Equals(UserId, other.UserId) && string.Equals(Name, other.Name);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((UserId != null ? UserId.GetHashCode() : 0) * 397) ^ (Name != null ? Name.GetHashCode() : 0);
            }
        }

    }
}