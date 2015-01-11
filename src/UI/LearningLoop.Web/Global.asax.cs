using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using CommonServiceLocator.SimpleInjectorAdapter;
using Funq;
using LearningLoop.Core.Infrastructure.IoC;
using LearningLoop.Core.WebServices;
using LearningLoop.Core.WebServices.Types;
using LearningLoop.Infrastructure.Persistence;
using Microsoft.Practices.ServiceLocation;
using Raven.Client;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Authentication.OpenId;
using ServiceStack.Configuration;
using ServiceStack.Logging;
using ServiceStack.MiniProfiler;
using ServiceStack.Razor;
using ServiceStack.Text;
using ServiceStack.Validation;

namespace LearningLoop.Web
{
    public class AppHost : AppHostBase
    {
        public static ILog Log = LogManager.GetLogger(typeof(AppHost));

        /// <summary>
        /// Default constructor.
        /// Base constructor requires a name and assembly to locate web service classes. 
        /// </summary>
        public AppHost()
            : base("LearningLoop.Web", typeof(AccountWebService).Assembly)
        {
            var customSettings = new FileInfo(@"~/appsettings.txt".MapHostAbsolutePath());
            AppSettings = customSettings.Exists
                ? (IAppSettings)new TextFileSettings(customSettings.FullName)
                : new AppSettings();
        }

        /// <summary>
        /// Application specific configuration
        /// This method should initialize any IoC resources utilized by your web service classes.
        /// </summary>
        /// <param name="container"></param>
        public override void Configure(Container container)
        {
            var simpleContainer = DependencyRegistrar.RegisterServices();
            container.Adapter = new SimpleInjectorAdapter(simpleContainer); // to integrate with ServiceStack
            var adapter = new SimpleInjectorServiceLocatorAdapter(simpleContainer);
            ServiceLocator.SetLocatorProvider(() => adapter); // to integrate with service locator

            SetupDbMockData(container, AppSettings);

            //Enable Authentication an Registration
            ConfigureAuth(container, AppSettings);
            
            //Set JSON web services to return idiomatic JSON camelCase properties
            JsConfig.EmitCamelCaseNames = true;
            JsConfig.DateHandler = DateHandler.ISO8601;

            Plugins.Add(new RazorFormat());
            Plugins.Add(new RequestLogsFeature());
            Plugins.Add(new PostmanFeature());
            Plugins.Add(new CorsFeature());
            Plugins.Add(new AutoQueryFeature());
            Plugins.Add(new ValidationFeature());
            //container.RegisterValidators(typeof(AccountWebService).Assembly);

            SetConfig(new HostConfig
            {
                DebugMode = AppSettings.Get("DebugMode", true),
                AddRedirectParamsToQueryString = true,
            });

            this.GlobalResponseFilters.Add((httpReq, httpRes, requestDto) =>
             {
                 using (var documentSession = Container.Resolve<IDocumentSession>())
                 {
                     if (documentSession == null)
                         return;

                     if (httpRes.StatusCode >= 400 && httpRes.StatusCode < 600)
                         return;

                     documentSession.SaveChanges();
                 }
             });

            //Return default.cshtml home page for all 404 requests so we can handle routing on the client
            base.CustomErrorHttpHandlers[HttpStatusCode.NotFound] = new RazorHandler("/default.cshtml");
        }

        [Conditional("DEBUG")]
        private void SetupDbMockData(Container container, IAppSettings appSettings)
        {
            // Checkout ServiceStack wiki to extend upon this configuration, https://github.com/ServiceStack/ServiceStack/wiki/Config-API
            if (AppSettings.Get("populateMockData", false))
            {
                RavenDBBootstrap.PopulateMockData(container);
            }
        }

        private void ConfigureAuth(Container container, IAppSettings appSettings)
        {
            //Register all Authentication methods we want to enable            
            Plugins.Add(new AuthFeature(
                () => new UserSession(),
                new IAuthProvider[]
                {
                    new CredentialsAuthProvider(appSettings), //HTML Form post of UserName/Password credentials
                    new TwitterAuthProvider(appSettings), //Sign-in with Twitter
                    new FacebookAuthProvider(appSettings), //Sign-in with Facebook
                    new GoogleOpenIdOAuthProvider(appSettings), //Sign-in with Google OpenId

                    //TODO: add support for these 
                    //new YahooOpenIdOAuthProvider(appSettings), //Sign-in with Yahoo OpenId
                    //new OpenIdOAuthProvider(appSettings), //Sign-in with Custom OpenId
                    //new GoogleOAuth2Provider(appSettings), //Sign-in with Google OAuth2 Provider
                    //new LinkedInOAuth2Provider(appSettings), //Sign-in with LinkedIn OAuth2 Provider
                })
            {
                HtmlRedirect = "~/",
                IncludeRegistrationService = true,
            });
        }
    }

    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            log4net.Config.XmlConfigurator.Configure();
            LogManager.LogFactory = new ConsoleLogFactory();
            RavenDBBootstrap.InitializeDocumentStore();
            new AppHost().Init();
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