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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using LearningLoop.Core.Domain.Commands;
using MediatR;

namespace LearningLoop.Core.WebServices
{
    [Route("/home")]
    public class GetAccount
    { }

    public class AccountResponse
    {
        public AccountResponse()
        {
            Class = new Classroom();
        }
        public Classroom Class { get; set; }
    }

    [Route("/addstudent")]
    public class AddStudentRequest
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

        public object Get(GetAccount request)
        {
            return new AccountResponse()
            {
                Class =
                {
                    ClassRoster = new List<Student>
                    {
                        new Student()
                        {
                            DisplayName = "John",
                            Gender = "Male",
                        },
                        new Student()
                        {
                            DisplayName = "Mary",
                            Gender = "Female",
                        },
                        new Student()
                        {
                            DisplayName = "Jenny",
                            Gender = "Female",
                        }

                    }
                }
            };
        }

        public void Post(AddStudentRequest request)
        {
            request.ImageContent =
                  Request.Files.SingleOrDefault(
                      uploadedFile => uploadedFile.ContentLength > 0 && uploadedFile.ContentLength < MaxUploadSize);

            _mediator.Send(new AddStudentToRosterCommand(GetSession().UserAuthId, request));
        }
      
    }
}