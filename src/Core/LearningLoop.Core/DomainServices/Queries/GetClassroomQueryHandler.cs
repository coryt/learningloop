using System.Linq;
using LearningLoop.Core.Domain;
using LearningLoop.Core.Domain.Queries;
using MediatR;
using Raven.Client;

namespace LearningLoop.Core.DomainServices.Queries
{
    public class GetClassroomQueryHandler : IRequestHandler<GetClassroomByTeacherIdQuery, GetClassroomByTeacherIdQuery.Response>
    {
        private readonly IDocumentSession _session;

        public GetClassroomQueryHandler(IDocumentSession session)
        {
            _session = session;
        }

        public GetClassroomByTeacherIdQuery.Response Handle(GetClassroomByTeacherIdQuery message)
        {
            //TODO: convert to index when model is stable
            var query = _session.Query<Classroom>().Where(c => c.TeacherId.Equals(message.TeacherId));
            return new GetClassroomByTeacherIdQuery.Response
            {
                Classoom = query.SingleOrDefault(),
                Error = !query.Any() ? "Classroom not found" : string.Empty
            };
        }
    }
}
