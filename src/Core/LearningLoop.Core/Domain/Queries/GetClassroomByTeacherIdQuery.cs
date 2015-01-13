using MediatR;

namespace LearningLoop.Core.Domain.Queries
{
    public class GetClassroomByTeacherIdQuery : IRequest<GetClassroomByTeacherIdQuery.Response>
    {
        public string TeacherId { get; set; }

        public GetClassroomByTeacherIdQuery(string teacherId)
        {
            TeacherId = teacherId;
        }

        public class Response
        {
            public Classroom Classoom { get; set; }
            public string Error { get; set; }
            public bool HasError { get { return !string.IsNullOrEmpty(Error); }}
        }
    }
}