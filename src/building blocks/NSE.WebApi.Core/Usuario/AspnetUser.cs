﻿using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace NSE.WebApi.Core.Usuario;

public class AspnetUser: IAspnetUser
{
    private readonly IHttpContextAccessor _accessor;

        public AspnetUser(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public string Name => _accessor.HttpContext?.User.Identity?.Name;

        public Guid ObterUserId()
        {
            return EstaAutenticado() ? Guid.Parse(_accessor.HttpContext?.User.GetUserId() ?? throw new InvalidOperationException()) : Guid.Empty;
        }

        public string ObterUserEmail()
        {
            return EstaAutenticado() ? _accessor.HttpContext?.User.GetUserEmail() : string.Empty;
        }

        public string ObterUserToken()
        {
            return EstaAutenticado() ? _accessor.HttpContext?.User.GetUserToken() : string.Empty;
        }

        public bool EstaAutenticado()
        {
            return _accessor.HttpContext is { User.Identity.IsAuthenticated: true };
        }

        public bool PossuiRole(string role)
        {
            return _accessor.HttpContext != null && _accessor.HttpContext.User.IsInRole(role);
        }

        public IEnumerable<Claim> ObterClaims()
        {
            return _accessor.HttpContext?.User.Claims;
        }

        public HttpContext ObterHttpContext()
        {
            return _accessor.HttpContext;
        }

        public string ObterUserRefreshToken()
        {
            return _accessor.HttpContext?.User.GetUserRefreshToken();
        }
    }