using LearningLoop.Core.Domain;
using LearningLoop.Core.DomainServices;
using LearningLoop.Core.WebServices.Types;
using ServiceStack;

namespace LearningLoop.Core.WebServices
{
    [Route("/home")]
    public class GetAccount
    {}

    public class AccountResponse
    {
        public Classroom Class { get; set; }
    }

    [Authenticate]
    [DefaultView("Home")]
    public class AccountWebService : Service
    {
        public object Get(GetAccount request)
        {
            return new AccountResponse();
        }
    }
}