using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Funq;
using LearningLoop.Core.Domain;
using LearningLoop.Core.WebServices.Types;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Linq;
using ServiceStack.Auth;

[assembly: WebActivator.PreApplicationStartMethod(typeof(LearningLoop.Infrastructure.Persistence.RavenDBBootstrap), "InitializeDocumentStore", Order = 1)]

namespace LearningLoop.Infrastructure.Persistence
{
    public static class RavenDBBootstrap
    {
        public static IDocumentStore DocumentStore { get; private set; }

        public static void InitializeDocumentStore()
        {
            if (DocumentStore != null)
                return; // prevent misuse

            DocumentStore = new DocumentStore
            {
                ConnectionStringName = "RavenHQ"
            }.Initialize();

            TryCreatingIndexesOrRedirectToErrorPage();
        }

        private static void TryCreatingIndexesOrRedirectToErrorPage()
        {
            try
            {
                //TODO: Create some indexes
                //IndexCreation.CreateIndexes(GetType().Assembly, DocumentStore);
            }
            catch (WebException e)
            {
                var socketException = e.InnerException as SocketException;
                if (socketException == null)
                    throw;

                switch (socketException.SocketErrorCode)
                {
                    case SocketError.AddressNotAvailable:
                    case SocketError.NetworkDown:
                    case SocketError.NetworkUnreachable:
                    case SocketError.ConnectionAborted:
                    case SocketError.ConnectionReset:
                    case SocketError.TimedOut:
                    case SocketError.ConnectionRefused:
                    case SocketError.HostDown:
                    case SocketError.HostUnreachable:
                    case SocketError.HostNotFound:
                        //TODO: Find a way to handle this in ServiceStack
                        //HttpContext.Response.Redirect("~/RavenNotReachable.htm");
                        break;
                    default:
                        throw;
                }
            }
        }

        public static void PopulateMockData(Container container)
        {
            //var classes = new List<Classroom>()
            //{
            //    new Classroom("Classroom 1", true),
            //    new Classroom("Classroom 2", true),
            //    new Classroom("Classroom 3", true)
            //};

            //using (var session = DocumentStore.OpenSession())
            //{
            //    classes.ForEach(c =>
            //    {
            //        var result = session.Query<Classroom>().Where(cr => cr.DisplayName.Equals(c.DisplayName));
            //        if (result == null)
            //            session.Store(c);
            //    });
            //    session.SaveChanges();
            //}
        }
    }
}
