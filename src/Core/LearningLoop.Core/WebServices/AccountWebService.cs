using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using LearningLoop.Core.Domain;
using LearningLoop.Core.DomainServices;
using LearningLoop.Core.WebServices.Types;
using ServiceStack;
using ServiceStack.Web;
using LearningLoop.Core.Domain.Commands;
using LearningLoop.Core.Domain.Queries;
using MediatR;

namespace LearningLoop.Core.WebServices
{
    [Route("/home")]
    public class GetAccount : IReturn<AccountResponse>
    { }

    public class AccountResponse
    {
        public Classroom Class { get; set; }
    }

    [Route("/class")]
    public class ClassRequest : IReturn<AccountResponse>
    {
        public string DisplayName { get; set; }
        public string OpenRegistration { get; set; }
    }

    [Route("/class/student")]
    public class StudentRequest : IReturn<AccountResponse>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfileImagePath { get; set; }
        public IHttpFile ImageContent { get; set; }
        public string Gender { get; set; }
    }

    [Authenticate]
    [DefaultView("Home")]
    public class AccountWebService : Service
    {
        private readonly IMediator _mediator;
        const int MaxUploadSize = 200000;

        public AccountWebService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public object Post(ClassRequest request)
        {
            var session = GetSession() as UserSession;

            var classroom = _mediator.Send(new CreateClassroomCommand(session.UserAuthRef, request.DisplayName));
            return new AccountResponse
            {
                Class = classroom
            };
        }

        public object Get(GetAccount request)
        {
            var session = GetSession() as UserSession;

            var response = _mediator.Send(new GetClassroomByTeacherIdQuery(session.UserAuthRef));

            return new AccountResponse
            {
                Class = response.Classoom
            };
        }

        public object Post(StudentRequest request)
        {
            var session = GetSession() as UserSession;

            request.ImageContent =
                  Request.Files.SingleOrDefault(
                      uploadedFile => uploadedFile.ContentLength > 0 && uploadedFile.ContentLength < MaxUploadSize);

            var classroom = _mediator.Send(new AddStudentToRosterCommand(session.UserAuthRef, request));
            return new AccountResponse
            {
                Class = classroom
            };
        }

        public object Put(StudentRequest request)
        {
            var session = GetSession() as UserSession;

            request.ImageContent =
                  Request.Files.SingleOrDefault(
                      uploadedFile => uploadedFile.ContentLength > 0 && uploadedFile.ContentLength < MaxUploadSize);

            var classroom = _mediator.Send(new AddStudentToRosterCommand(session.UserAuthRef, request));
            return new AccountResponse
            {
                Class = classroom
            };
        }
      
    }
}