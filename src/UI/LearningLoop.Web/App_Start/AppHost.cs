using System.Net;
using Funq;
using LearningLoop.Infrastructure.Persistence;
using LearningLoop.Web.IoC;
using LearningLoop.Web.Services;
using Microsoft.Practices.ServiceLocation;
using ServiceStack;
using ServiceStack.Configuration;
using ServiceStack.Logging;
using ServiceStack.Razor;

[assembly: WebActivator.PreApplicationStartMethod(typeof(LearningLoop.Web.App_Start.AppHost), "Start", Order = 2)]

namespace LearningLoop.Web.App_Start
{
	public class AppHost : AppHostBase
	{		
		public AppHost() //Tell ServiceStack the name and where to find your web services
			: base("LearningLoop ASP.NET Host", typeof(ClassroomWebService).Assembly) { }
        public static void Start()
        {
            LogManager.LogFactory = new ConsoleLogFactory();
            new AppHost().Init();
            RouteConfig.Init();  
        }

		public override void Configure(Container container)
		{
		    var simpleContainer = new SimpleInjector.Container();
            DependencyRegistrar.RegisterServices(simpleContainer);
            container.Adapter = new SimpleInjectorAdapter(simpleContainer); // to integrate with ServiceStack
		    var adapter = new CommonServiceLocator.SimpleInjectorAdapter.SimpleInjectorServiceLocatorAdapter(simpleContainer);
            ServiceLocator.SetLocatorProvider(() => adapter); // to integrate with service locator

            ConfigurePlugins();

            var appSettings = new AppSettings();
		    if (appSettings.Get("populateMockData", false))
		    {
		        RavenDBBootstrap.PopulateMockData();
		    }
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