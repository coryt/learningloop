using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.FluentValidation;

namespace LearningLoop.Web.Plugins
{
    public class CustomRegisterPlugin : IPlugin
    {
        public class CustomRegistrationValidator : RegistrationValidator
        {
            public CustomRegistrationValidator()
            {
                RuleSet(ApplyTo.Post, () =>
                {
                    RuleFor(x => x.UserName).Must(x => false)
                        .WithMessage("CustomRegistrationValidator is fired");
                });
            }
        }

        public void Register(IAppHost appHost)
        {
            appHost.RegisterAs<CustomRegistrationValidator, IValidator<Register>>();
        }
    }
}