using System;
using System.IO;
using System.Net;
using Funq;
using LearningLoop.Core.WebServices;
using ServiceStack;
using ServiceStack.Configuration;
using ServiceStack.Razor;

namespace LearningLoop.App
{
    public class AppHost : AppHostBase
    {
        /// <summary>
        /// Default constructor.
        /// Base constructor requires a name and assembly to locate web service classes. 
        /// </summary>
        public AppHost()
            : base("LearningLoop.App", typeof(ClassroomWebService).Assembly)
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
            //Return default.cshtml home page for all 404 requests so we can handle routing on the client
            base.CustomErrorHttpHandlers[HttpStatusCode.NotFound] = new RazorHandler("/default.cshtml");

            SetConfig(new HostConfig
            {
                DebugMode = AppSettings.Get("DebugMode", false),
                AddRedirectParamsToQueryString = true
            });

            this.Plugins.Add(new RazorFormat());
        }
    }
}