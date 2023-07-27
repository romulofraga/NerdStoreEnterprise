﻿using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace NSE.WebApi.Core.Usuario
{
    public interface IAspnetUser
    {
        string Name { get; }
        Guid ObterUserId();
        string ObterUserEmail();
        string ObterUserToken();
        bool EstaAutenticado();
        bool PossuiRole(string role);
        IEnumerable<Claim> ObterClaims();
        HttpContext ObterHttpContext();
        string ObterUserRefreshToken();
    }
}