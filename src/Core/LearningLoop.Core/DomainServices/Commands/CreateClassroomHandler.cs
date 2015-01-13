using LearningLoop.Core.Domain;
using LearningLoop.Core.Domain.Commands;
using MediatR;
using Raven.Client;

namespace LearningLoop.Core.DomainServices.Commands
{
    public class CreateClassroomHandler : IRequestHandler<CreateClassroomCommand, Classroom>
    {
        private readonly IDocumentSession _session;

        public CreateClassroomHandler(IDocumentSession session)
        {
            _session = session;
        }

        public Classroom Handle(CreateClassroomCommand message)
        {
            var classroom = new Classroom(message.TeacherId, message.Name, true);
           _session.Store(classroom);
            _session.SaveChanges();

            return classroom;
        }
    }
}
