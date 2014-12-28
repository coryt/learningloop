using System.Configuration;
using System.Net;
using Funq;
using LearningLoop.DependencyResolution;
using LearningLoop.Web.Services;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.Data;
using ServiceStack.Logging;
using ServiceStack.OrmLite;
using ServiceStack.Razor;

[assembly: WebActivator.PreApplicationStartMethod(typeof(LearningLoop.Web.App_Start.AppHost), "Start")]

namespace LearningLoop.Web.App_Start
{
	public class AppHost : AppHostBase
	{		
		public AppHost() //Tell ServiceStack the name and where to find your web services
			: base("LearningLoop ASP.NET Host", typeof(ClassroomWebService).Assembly) { }
        public static void Start()
        {
            new AppHost().Init();
        }

		public override void Configure(Container container)
		{
            //Dependencies should be registered in the DependencyRegistrar which is invoked during PreApplicationStartup
            container.Adapter = new SimpleInjectorAdapter(DependencyRegistrar.Container);

            LogManager.LogFactory = new ConsoleLogFactory();

            ConfigurePlugins();

		    RouteConfig.Init();  
		}

        private void ConfigurePlugins()
        {
            //Set JSON web services to return idiomatic JSON camelCase properties
            ServiceStack.Text.JsConfig.EmitCamelCaseNames = true;

            Plugins.Add(new RazorFormat());

            //Uncomment to change the default ServiceStack configuration
            //SetConfig(new HostConfig {
            //});

            CustomErrorHttpHandlers[HttpStatusCode.NotFound] = new RazorHandler("/notfound");
            CustomErrorHttpHandlers[HttpStatusCode.Unauthorized] = new RazorHandler("/login");
        }
	}
}