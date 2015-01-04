using ServiceStack;

namespace LearningLoop.Core.WebServices.Types
{
    [Route("/classes", "POST")]
    [Route("/classes/{Id}", "PUT")]
    public class ClassroomViewModel : IReturn<ClassroomViewModel>
    {
        public long Id { get; set; }
    }
}