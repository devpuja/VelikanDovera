using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SmearAdmin.Auth;
using SmearAdmin.Data;
using SmearAdmin.Helpers;
using SmearAdmin.Models;
using SmearAdmin.ViewModels;

namespace SmearAdmin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtFactory _jwtFactory;
        private readonly JwtIssuerOptions _jwtOptions;
        private readonly SmearAdminDbContext _appDbContext;

        public AuthController(SmearAdminDbContext appDbContext, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IJwtFactory jwtFactory, IOptions<JwtIssuerOptions> jwtOptions)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtFactory = jwtFactory;
            _jwtOptions = jwtOptions.Value;
            _appDbContext = appDbContext;
        }

        // POST api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Post([FromBody]CredentialsViewModel credentials)
        {
            var jwt = "";
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var identity = await GetClaimsIdentity(credentials.UserName, credentials.Password);
                if (identity == null)
                {
                    //return BadRequest(Errors.AddErrorToModelState("login_failure",
                    //    "Invalid username or password. Please contact " + Permissions.Roles.ADMIN.ToUpper(),
                    //    ModelState));

                    var error = new
                    {
                        message = "Invalid username or password. Please contact " + Permissions.Roles.ADMIN.ToUpper(),
                        error = ModelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage))
                    };
                    return BadRequest(error);
                }

                jwt = await Tokens.GenerateJwt(identity, credentials.UserName, _jwtFactory, _jwtOptions, new JsonSerializerSettings { Formatting = Formatting.Indented });
                //return new OkObjectResult(jwt);
            }
            catch(Exception ex)
            {
                Errors.AddError(ex.Message.ToString(), ModelState);   
            }
            return new OkObjectResult(jwt);
        }


        private async Task<ClaimsIdentity> GetClaimsIdentity(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
                return await Task.FromResult<ClaimsIdentity>(null);

            // get the user to verifty
            var userToVerify = await _userManager.FindByNameAsync(userName);

            if (userToVerify == null) return await Task.FromResult<ClaimsIdentity>(null);

            if(!userToVerify.IsEnabled) return await Task.FromResult<ClaimsIdentity>(null);

            // check the credentials
            if (await _userManager.CheckPasswordAsync(userToVerify, password))
            {
                //var user = await _userManager.FindByEmailAsync(userName);
                var user = await _userManager.FindByEmailAsync(userToVerify.Email);
                var lstClaim = await GetValidClaims(user);

                return await Task.FromResult(_jwtFactory.GenerateClaimsIdentity(lstClaim));
            }

            // Credentials are invalid, or account doesn't exist
            return await Task.FromResult<ClaimsIdentity>(null);
        }

        private async Task<List<Claim>> GetValidClaims(AppUser user)
        {
            //IdentityOptions _options = new IdentityOptions();
            var claims = new List<Claim>
            {
            //new Claim(_options.ClaimsIdentity.UserIdClaimType, user.Id.ToString()),
            //new Claim(_options.ClaimsIdentity.UserNameClaimType, user.UserName),
            new Claim(Constants.Strings.JwtClaimIdentifiers.Id, user.Id),
            new Claim(CustomClaimTypes.Email, user.Email),
            //new Claim(CustomClaimTypes.Phone, user.ConcurrencyStamp),
            new Claim(CustomClaimTypes.FullName, string.Join(" ",user.FirstName,user.LastName)),
            new Claim(CustomClaimTypes.UserName, user.UserName),
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, await _jwtOptions.JtiGenerator()),
            new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(_jwtOptions.IssuedAt).ToString(), ClaimValueTypes.Integer64),
            };


            var userClaims = await _userManager.GetClaimsAsync(user);
            claims.AddRange(userClaims);

            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(Constants.Strings.JwtClaimIdentifiers.Roles, userRole));
                //claims.Add(new Claim(Constants.Strings.JwtClaimIdentifiers.Roles, "Usersss"));
                var role = await _roleManager.FindByNameAsync(userRole);
                if (role != null)
                {
                    var roleClaims = await _roleManager.GetClaimsAsync(role);
                    foreach (Claim roleClaim in roleClaims)
                    {
                        claims.Add(roleClaim);
                    }
                }
            }

            return claims;
        }

        private static long ToUnixEpochDate(DateTime date)
              => (long)Math.Round((date.ToUniversalTime() -
                                   new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
                                  .TotalSeconds);
    }
}