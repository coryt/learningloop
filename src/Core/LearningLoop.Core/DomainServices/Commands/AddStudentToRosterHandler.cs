using System.IO;
using LearningLoop.Core.Domain;
using LearningLoop.Core.Domain.Commands;
using LearningLoop.Core.Infrastructure;
using MediatR;
using Raven.Client;
using ServiceStack;

namespace LearningLoop.Core.DomainServices.Commands
{
    public class AddStudentToRosterHandler : RequestHandler<AddStudentToRosterCommand>
    {
        private readonly IDocumentSession _session;

        public AddStudentToRosterHandler(IDocumentSession session)
        {
            _session = session;
        }

        protected override void HandleCore(AddStudentToRosterCommand message)
        {
            var classroom = _session.Load<Classroom>(message.ClassId);
            var newStudent = new Student(
                message.WebRequest.FirstName,
                message.WebRequest.LastName,
                message.WebRequest.ProfileImagePath,
                message.WebRequest.Gender
                );

            if (message.WebRequest.ImageContent != null)
            {
                using (var ms = new MemoryStream())
                {
                    message.WebRequest.ImageContent.WriteTo(ms);
                    message.WebRequest.ProfileImagePath = ImageProcessing.WriteImage(ms);
                }
            }

            classroom.ClassRoster.Add(newStudent);
            _session.Store(classroom);
            _session.SaveChanges();
        }
    }
}