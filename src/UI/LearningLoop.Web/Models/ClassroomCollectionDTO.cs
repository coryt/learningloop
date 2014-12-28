using System.Collections.Generic;
using ServiceStack;

namespace LearningLoop.Web.ViewModels
{
    [Route("/classes")]
    [Route("/classes/{Ids}")]
    public class ClassesViewModel : IReturn<List<ClassroomViewModel>>
    {
        public long[] Ids { get; set; }
        public ClassesViewModel(params long[] ids)
        {
            this.Ids = ids;
        }
    }
}
