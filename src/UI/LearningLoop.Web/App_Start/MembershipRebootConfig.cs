using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Hierarchical;

namespace LearningLoop.Web.App_Start
{
    public class MembershipRebootConfig
    {
        public static MembershipRebootConfiguration<HierarchicalUserAccount> Create()
        {
            var config = new MembershipRebootConfiguration<HierarchicalUserAccount>();
            //config.RequireAccountVerification = false;

            config.AddEventHandler(new DebuggerEventHandler<HierarchicalUserAccount>());

            //var appinfo = new AspNetApplicationInformation("Test", "Test Email Signature",
            //    "UserAccount/Login",
            //    "UserAccount/ChangeEmail/Confirm/",
            //    "UserAccount/Register/Cancel/",
            //    "UserAccount/PasswordReset/Confirm/");

            //var emailFormatter = new EmailMessageFormatter<HierarchicalUserAccount>(appinfo);
            // uncomment if you want email notifications -- also update smtp settings in web.config
            //config.AddEventHandler(new EmailAccountEventsHandler<HierarchicalUserAccount>(emailFormatter));

            // uncomment to enable SMS notifications -- also update TwilloSmsEventHandler class below
            //config.AddEventHandler(new TwilloSmsEventHandler(appinfo));

            // uncomment to ensure proper password complexity
            //config.ConfigurePasswordComplexity();

            var debugging = false;
#if DEBUG
            debugging = true;
#endif
            // this config enables cookies to be issued once user logs in with mobile code
            //config.ConfigureTwoFactorAuthenticationCookies(debugging);

            return config;
        }
    }
}