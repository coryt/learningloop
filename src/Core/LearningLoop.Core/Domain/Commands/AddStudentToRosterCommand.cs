using System;
using LearningLoop.Core.WebServices;
using MediatR;

namespace LearningLoop.Core.Domain.Commands
{
    public class AddStudentToRosterCommand : IRequest
    {
        public string ClassId { get; set; }
        public AddStudentRequest WebRequest { get; set; }

        public AddStudentToRosterCommand(string classId, AddStudentRequest webRequest)
        {
            ClassId = classId;
            WebRequest = webRequest;
        }

        public override bool Equals(Object other)
        {
            if (other == null)
                return false;

            var otherCommand = other as AddStudentToRosterCommand;
            if (otherCommand == null)
                return false;

            return otherCommand.ClassId == ClassId &&
                   otherCommand.WebRequest == WebRequest;
        }

        protected bool Equals(AddStudentToRosterCommand other)
        {
            return string.Equals(ClassId, other.ClassId)
                   && string.Equals(WebRequest, other.WebRequest);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((ClassId != null ? ClassId.GetHashCode() : 0) * 397)
                       ^ (WebRequest != null ? WebRequest.GetHashCode() : 0);
            }
        }

    }
}