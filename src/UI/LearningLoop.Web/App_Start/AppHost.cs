using System.Diagnostics;
using System.IO;
using System.Net;
using CommonServiceLocator.SimpleInjectorAdapter;
using Funq;
using LearningLoop.Core.IoC;
using LearningLoop.Core.WebServices;
using LearningLoop.Core.WebServices.Types;
using LearningLoop.Infrastructure.Persistence;
using LearningLoop.Web.Plugins;
using Microsoft.Practices.ServiceLocation;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Authentication.OAuth2;
using ServiceStack.Authentication.OpenId;
using ServiceStack.Configuration;
using ServiceStack.Logging;
using ServiceStack.Razor;
using ServiceStack.Text;

namespace LearningLoop.Web.App_Start
{
    public class AppHost : AppHostBase
    {
        public static ILog Log = LogManager.GetLogger(typeof(AppHost));

        /// <summary>
        /// Default constructor.
        /// Base constructor requires a name and assembly to locate web service classes. 
        /// </summary>
        public AppHost()
            : base("LearningLoop.Web", typeof(ClassroomWebService).Assembly)
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

            Plugins.Add(new RazorFormat());

            SetConfig(new HostConfig
            {
                DebugMode = AppSettings.Get("DebugMode", false),
                AddRedirectParamsToQueryString = true,
                DefaultContentType = MimeTypes.Json
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