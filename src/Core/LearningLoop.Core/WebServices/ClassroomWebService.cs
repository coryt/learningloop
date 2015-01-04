using LearningLoop.Core.Domain;
using LearningLoop.Core.DomainServices;
using LearningLoop.Core.WebServices.Types;
using ServiceStack;

namespace LearningLoop.Core.WebServices
{
    [DefaultView("default")]
    public class ClassroomWebService : Service
    {
        public IClassroomRepository Repository { get; set; }  //Injected by IOC

        public object Get()
        {
            return null;
        }

        public object Get(ClassesViewModel request)
        {
            return request.Ids.IsEmpty()
                ? Repository.GetAll()
                : Repository.GetByIds(request.Ids);
        }

        public object Post(ClassroomViewModel classroomVM)
        {
            var classroom = new Classroom();
            return Repository.Save(classroom);
        }

        public object Put(ClassroomViewModel classroomVM)
        {
            var classroom = new Classroom();
            return Repository.Save(classroom);
        }

        public void Delete(ClassesViewModel request)
        {
            request.Ids.Each(id=>Repository.Delete(id));
        }
    }
}