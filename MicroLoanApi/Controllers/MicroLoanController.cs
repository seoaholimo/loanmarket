using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using BeyondIT.MicroLoan.Application.Interfaces;
using BeyondIT.MicroLoan.Domain.BaseTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BeyondIT.MicroLoan.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MicroLoanController : ControllerBase
    {
        protected static int CurrentUserId
        {
            get
            {
                var userContext = Startup.Container.Resolve<IUserContext>();
                return userContext.UserId;
            }
        }

        protected static bool isSuperuser
        {
            get
            {
                var userContext = Startup.Container.Resolve<IUserContext>();
                return userContext.IsSuperUser;
            }
        }

        [AllowAnonymous]
        protected FileContentResult Document(Document document)
        {
            //var compressionService = new CompressionService();
            byte[] fileContents = document.File;
            return File(fileContents, document.MimeType, document.DocumentName);
        }
    }
}