using System.Collections.Generic;
using ServiceStack;

namespace LearningLoop.Web.Models
{
    [Route("/classes")]
    [Route("/classes/{Ids}")]
    public class ClassesViewModel : IReturn<List<ClassroomViewModel>>
    {
        public string[] Ids { get; set; }
        public ClassesViewModel(params string[] ids)
        {
            this.Ids = ids;
        }
    }
}
