using Raven.Client;
using Raven.Client.Document;

namespace LearningLoop.Infrastructure.Repositories
{
    public class RavenMembershipRebootDatabase
    {
        public IDocumentStore DocumentStore { get; private set; }

        public RavenMembershipRebootDatabase(string connectionStringName)
        {
            DocumentStore = new DocumentStore
            {
                ConnectionStringName = connectionStringName
            }.Initialize();
        }

        public RavenMembershipRebootDatabase(IDocumentStore documentStore)
        {
            DocumentStore = documentStore;
        }
    }
}