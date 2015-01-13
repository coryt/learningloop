using System.IO;
using System.Linq;
using LearningLoop.Core.Domain;
using LearningLoop.Core.Domain.Commands;
using LearningLoop.Core.Infrastructure;
using MediatR;
using Raven.Client;
using Raven.Client.Linq;
using ServiceStack;

namespace LearningLoop.Core.DomainServices.Commands
{
    public class AddStudentToRosterHandler : IRequestHandler<AddStudentToRosterCommand, Classroom>
    {
        private readonly IDocumentSession _session;

        public AddStudentToRosterHandler(IDocumentSession session)
        {
            _session = session;
        }

        public Classroom Handle(AddStudentToRosterCommand message)
        {
            //loop up current class
            var classroom = _session.Query<Classroom>().Single(c => c.TeacherId.Equals(message.TeacherId));

            //save image if we have one
            if (message.WebRequest.ImageContent != null)
            {
                using (var ms = new MemoryStream())
                {
                    message.WebRequest.ImageContent.WriteTo(ms);
                    message.WebRequest.ProfileImagePath = ImageProcessing.WriteImage(ms);
                }
            }

            classroom.AddNewStudent(
                message.WebRequest.FirstName,
                message.WebRequest.LastName,
                message.WebRequest.ProfileImagePath,
                message.WebRequest.Gender
                );

            _session.Store(classroom);
            _session.SaveChanges();

            return classroom;
        }
    }
}