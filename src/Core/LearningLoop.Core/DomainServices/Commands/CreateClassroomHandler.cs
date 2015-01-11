using LearningLoop.Core.Domain.Commands;
using MediatR;
using Raven.Client;

namespace LearningLoop.Core.DomainServices.Commands
{
    public class CreateClassroomHandler : RequestHandler<CreateClassroomCommand>
    {
        private readonly IDocumentSession _session;

        public CreateClassroomHandler(IDocumentSession session)
        {
            _session = session;
        }

        protected override void HandleCore(CreateClassroomCommand message)
        {
           
        }
    }
}
