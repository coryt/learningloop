using System;
using LearningLoop.Core.WebServices;
using MediatR;

namespace LearningLoop.Core.Domain.Commands
{
    public class AddStudentToRosterCommand : IRequest<Classroom>
    {
        public string TeacherId { get; set; }
        public StudentRequest WebRequest { get; set; }

        public AddStudentToRosterCommand(string teacherId, StudentRequest webRequest)
        {
            TeacherId = teacherId;
            WebRequest = webRequest;
        }

        public override bool Equals(Object other)
        {
            if (other == null)
                return false;

            var otherCommand = other as AddStudentToRosterCommand;
            if (otherCommand == null)
                return false;

            return otherCommand.TeacherId == TeacherId &&
                   otherCommand.WebRequest == WebRequest;
        }

        protected bool Equals(AddStudentToRosterCommand other)
        {
            return string.Equals(TeacherId, other.TeacherId)
                   && string.Equals(WebRequest, other.WebRequest);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((TeacherId != null ? TeacherId.GetHashCode() : 0) * 397)
                       ^ (WebRequest != null ? WebRequest.GetHashCode() : 0);
            }
        }

    }
}