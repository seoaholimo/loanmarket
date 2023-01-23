using AutoMapper;
using BeyondIT.MicroLoan.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BeyondIT.MicroLoan.Api.Infrastructure.Service
{
    public class UserContext: IUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IActionContextAccessor _actionContextAccessor;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;

        public UserContext(IHttpContextAccessor httpContextAccessor, IActionContextAccessor actionContextAccessor, IMapper mapper, IMemoryCache memoryCache)
        {
            _httpContextAccessor = httpContextAccessor;
            _actionContextAccessor = actionContextAccessor;
            _mapper = mapper;
            _memoryCache = memoryCache;
        }

        public new int UserId => _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated
            ? Convert.ToInt32(_httpContextAccessor.HttpContext.User.Claims
                .First(c => c.Type == Helpers.Constants.Strings.JwtClaimIdentifiers.Id).Value)
            : 0;

        public new bool IsSuperUser
        {
            get
            {
                if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
                {
                    Claim isSuperUserClaim = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(claim =>
                        claim.Type == Helpers.Constants.Strings.JwtClaimIdentifiers.IsSuperUser);

                    return isSuperUserClaim != null && Convert.ToBoolean(isSuperUserClaim.Value);
                }
                return false;
            }
        }

    }
}
