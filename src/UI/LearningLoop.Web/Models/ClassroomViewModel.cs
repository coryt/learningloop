using ServiceStack;

namespace LearningLoop.Web.Models
{
    [Route("/classes", "POST")]
    [Route("/classes/{Id}", "PUT")]
    public class ClassroomViewModel : IReturn<ClassroomViewModel>
    {
        public long Id { get; set; }
    }
}