using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SmearAdmin.Auth
{
    public interface IJwtFactory
    {
        Task<string> GenerateEncodedToken(ClaimsIdentity identity);
        ClaimsIdentity GenerateClaimsIdentity(List<Claim> LstClaims);
    }
}
