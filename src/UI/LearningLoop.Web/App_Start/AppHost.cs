using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Net;
using Funq;
using LearningLoop.Infrastructure.Persistence;
using LearningLoop.Web.IoC;
using LearningLoop.Web.Services;
using Microsoft.Practices.ServiceLocation;
using Raven.Client;
using Raven.Client.Document;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Authentication.OAuth2;
using ServiceStack.Authentication.OpenId;
using ServiceStack.Authentication.RavenDb;
using ServiceStack.Configuration;
using ServiceStack.Logging;
using ServiceStack.Razor;
using ServiceStack.Text;
using ServiceStack.Web;

[assembly: WebActivator.PreApplicationStartMethod(typeof(LearningLoop.Web.App_Start.AppHost), "Start", Order = 2)]

namespace LearningLoop.Web.App_Start
{
    public class AppHost : AppHostBase
    {
        public static ILog Log = LogManager.GetLogger(typeof(AppHost));

        public AppHost() //Tell ServiceStack the name and where to find your web services
            : base("LearningLoop ASP.NET Host", typeof(ClassroomWebService).Assembly)
        {
        }

        public static void Start()
        {
            LogManager.LogFactory = new ConsoleLogFactory();
            new AppHost().Init();
            RouteConfig.Init();
        }

        public override void Configure(Container container)
        {
            //Set JSON web services to return idiomatic JSON camelCase properties
            JsConfig.EmitCamelCaseNames = true;

            Plugins.Add(new RazorFormat());

            var simpleContainer = new SimpleInjector.Container();
            DependencyRegistrar.RegisterServices(simpleContainer);
            container.Adapter = new SimpleInjectorAdapter(simpleContainer); // to integrate with ServiceStack
            var adapter =
                new CommonServiceLocator.SimpleInjectorAdapter.SimpleInjectorServiceLocatorAdapter(simpleContainer);
            ServiceLocator.SetLocatorProvider(() => adapter); // to integrate with service locator

            // Checkout ServiceStack wiki to extend upon this configuration, https://github.com/ServiceStack/ServiceStack/wiki/Config-API
            var appSettings = new AppSettings();
            if (appSettings.Get("populateMockData", false))
            {
                RavenDBBootstrap.PopulateMockData(container);
            }

            //Enable Authentication an Registration
            ConfigureAuth(container, appSettings);

            SetConfig(new HostConfig
            {
                DebugMode = true,
                AddRedirectParamsToQueryString = true,
            });

            CustomErrorHttpHandlers[HttpStatusCode.NotFound] = new RazorHandler("/notfound");
            CustomErrorHttpHandlers[HttpStatusCode.Unauthorized] = new RazorHandler("/login");
        }

        private void ConfigureAuth(Container container, AppSettings appSettings)
        {
            //Register all Authentication methods we want to enable            
            Plugins.Add(new AuthFeature(
                () => new CustomUserSession(),
                new IAuthProvider[]
                {
                    new CredentialsAuthProvider(), //HTML Form post of UserName/Password credentials
                    new TwitterAuthProvider(appSettings), //Sign-in with Twitter
                    new FacebookAuthProvider(appSettings), //Sign-in with Facebook
                    new GoogleOpenIdOAuthProvider(appSettings), //Sign-in with Google OpenId
                    new YahooOpenIdOAuthProvider(appSettings), //Sign-in with Yahoo OpenId
                    new OpenIdOAuthProvider(appSettings), //Sign-in with Custom OpenId
                    new GoogleOAuth2Provider(appSettings), //Sign-in with Google OAuth2 Provider
                    new LinkedInOAuth2Provider(appSettings), //Sign-in with LinkedIn OAuth2 Provider
                }));

            //Provide service for new users to register so they can login with supplied credentials.
            Plugins.Add(new RegistrationFeature());

            //override the default registration validation with your own custom implementation
            Plugins.Add(new CustomRegisterPlugin());

            Plugins.Add(new RequestLogsFeature());
        }
    }
}