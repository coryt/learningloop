using System;
using System.Net;
using System.Net.Sockets;
using LearningLoop.Infrastructure.Persistence;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;
using ServiceStack.MiniProfiler;

namespace LearningLoop.Web
{
    using App_Start;
    public class Global : System.Web.HttpApplication
    {
        public Global()
        {
            BeginRequest += (sender, args) =>
            {
                //Context.Items["CurrentRequestRavenSession"] = RavenController.DocumentStore.OpenSession();
            };
            EndRequest += (sender, args) =>
            {
                using (var session = (IDocumentSession)Context.Items["CurrentRequestRavenSession"])
                {
                    if (session == null)
                        return;

                    if (Server.GetLastError() != null)
                        return;

                    session.SaveChanges();
                }
            };
        }

        protected void Application_Start(object sender, EventArgs e)
        {
            RavenDBBootstrap.InitializeDocumentStore();
        }

        protected void Application_BeginRequest(object src, EventArgs e)
        {
            if (Request.IsLocal)
                Profiler.Start();
        }

        protected void Application_EndRequest(object src, EventArgs e)
        {
            Profiler.Stop();
        }
    }
}