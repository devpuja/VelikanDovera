using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SmearAdmin.Data;
using SmearAdmin.Helpers;
using SmearAdmin.Models;
using SmearAdmin.ViewModels;

namespace SmearAdmin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly SmearAdminDbContext _appDbContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        //private readonly IdentityRole<AppUser> identityRole;
        private readonly IMapper _mapper;

        public AccountsController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper, SmearAdminDbContext appDbContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _appDbContext = appDbContext;
        }

        [HttpPost("PostSeed")]
        //public async Task<IActionResult> PostSeed([FromBody]RegistrationViewModel model)
        public async Task<IActionResult> PostSeed()
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            await InBuildAdminAsync();
            await InBuildUserAsync();

            #region "Create User, Roles & Add User Claims"
            //Admin: Data in Roles Table
            //var adminRole = await _roleManager.FindByNameAsync("Admin");
            //if (adminRole == null)
            //{
            //  adminRole = new IdentityRole("Admin");
            //  await _roleManager.CreateAsync(adminRole);
            //}

            ////Add UserClaims
            //await _userManager.AddClaimAsync(user, new Claim(CustomClaimTypes.Permission, Permissions.Admins.CRUD));
            //await _userManager.AddClaimAsync(user, new Claim(CustomClaimTypes.Permission, Permissions.Admins.Add));
            //await _userManager.AddClaimAsync(user, new Claim(CustomClaimTypes.Permission, Permissions.Admins.Edit));
            //await _userManager.AddClaimAsync(user, new Claim(CustomClaimTypes.Permission, Permissions.Admins.Delete));
            //await _userManager.AddClaimAsync(user, new Claim(CustomClaimTypes.Permission, Permissions.Admins.View));

            //if (!await _userManager.IsInRoleAsync(user, adminRole.Name))
            //{
            //  await _userManager.AddToRoleAsync(user, adminRole.Name);
            //}

            //User: Add Role & UserClaims
            //var accountManagerRole = await _roleManager.FindByNameAsync("User");
            //if (accountManagerRole == null)
            //{
            //    accountManagerRole = new IdentityRole("User");
            //    await _roleManager.CreateAsync(accountManagerRole);
            //}

            //// Create customized claim 
            //await _userManager.AddClaimAsync(user, new Claim(CustomClaimTypes.Permission, Permissions.Users.Add));
            //await _userManager.AddClaimAsync(user, new Claim(CustomClaimTypes.Permission, Permissions.Users.Edit));

            //if (!await _userManager.IsInRoleAsync(user, accountManagerRole.Name))
            //{
            //    await _userManager.AddToRoleAsync(user, accountManagerRole.Name);
            //}
            #endregion

            //if (!result.Succeeded) return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));

            //await _appDbContext.Customers.AddAsync(new Customer { IdentityId = userIdentity.Id, Location = model.Location });
            //await _appDbContext.SaveChangesAsync();

            return new OkObjectResult("Account created");
        }

        private async Task InBuildUserAsync()
        {
            RegistrationViewModel model = new RegistrationViewModel();
            model.FirstName = "Dev";
            model.MiddleName = "Vikram";
            model.LastName = "Sharma";
            model.Email = "devuser@smear.com";
            model.Password = "dev@123";
            model.Department = 0;
            model.Grade = 0;
            model.HQ = 0;
            model.Pancard = "Dummy";
            model.DOJ = DateTime.Now;
            model.DOB = DateTime.Now;
            model.Desigination = 0;
            model.CustomUserName = "Dummy";
            model.PasswordRaw = "dev@123";
            model.IsEnabled = true;

            string userName = model.FirstName.Substring(0, 1).ToLower() + model.MiddleName.Substring(0, 1).ToLower() + model.LastName.Replace(" ", "").ToLower();
            int iUserNameCount = _appDbContext.Users.Where(u => u.UserName.Contains(userName)).ToList().Count();
            if (iUserNameCount > 0)
            {
                iUserNameCount += 1;
                userName = userName + iUserNameCount.ToString();
            }

            model.CustomUserName = userName;
            model.PasswordRaw = model.Password;

            var userIdentity = _mapper.Map<AppUser>(model);
            var result = await _userManager.CreateAsync(userIdentity, model.Password);
            var user = await _userManager.FindByIdAsync(userIdentity.Id);

            #region "Create User, Roles & Add Role Claims"
            //User: Data in Roles Table
            var accountManagerRole = await _roleManager.FindByNameAsync(Permissions.Roles.USER);
            if (accountManagerRole == null)
            {
                accountManagerRole = new IdentityRole(Permissions.Roles.USER);
                await _roleManager.CreateAsync(accountManagerRole);                
            }

            ////Add UserClaims
            //await _userManager.AddClaimAsync(user, new Claim(CustomClaimTypes.Permission, Permissions.Admins.CRUD));
            await _userManager.AddClaimAsync(user, new Claim(CustomClaimTypes.Permission, Permissions.Users.Add));
            await _userManager.AddClaimAsync(user, new Claim(CustomClaimTypes.Permission, Permissions.Users.Edit));
            await _userManager.AddClaimAsync(user, new Claim(CustomClaimTypes.Permission, Permissions.Users.Delete));
            await _userManager.AddClaimAsync(user, new Claim(CustomClaimTypes.Permission, Permissions.Users.View));

            //await _roleManager.AddClaimAsync(accountManagerRole, new Claim(CustomClaimTypes.Permission, Permissions.Users.Add));
            //await _roleManager.AddClaimAsync(accountManagerRole, new Claim(CustomClaimTypes.Permission, Permissions.Users.Edit));
            //await _roleManager.AddClaimAsync(accountManagerRole, new Claim(CustomClaimTypes.Permission, Permissions.Users.View));

            if (!await _userManager.IsInRoleAsync(user, accountManagerRole.Name))
            {
                await _userManager.AddToRoleAsync(user, accountManagerRole.Name);
            }
            #endregion

            //if (!result.Succeeded) return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));

            //Extra Tables
            ContactResourseViewModel contact = new ContactResourseViewModel();
            contact.RefTableId = user.Id;
            contact.RefTableName = Constants.ReferenceTableNames.EMPLOYEE;
            contact.Address = "Dev User Address";
            contact.State = "Dev User State";
            contact.City = "Dev User City";
            contact.PinCode = "400086";
            contact.MobileNumber = "9029718964";
            contact.ResidenceNumber = "9090909090";

            var contactData = _mapper.Map<ContactResourseViewModel, Data.ContactResourse>(contact);
            _appDbContext.ContactResourse.Add(contactData);

            await _appDbContext.SaveChangesAsync();
        }

        private async Task InBuildAdminAsync()
        {
            RegistrationViewModel model = new RegistrationViewModel();
            model.FirstName = "Dev";
            model.MiddleName = "Vikram";
            model.LastName = "Sharma";
            model.Email = "devadmin@smear.com";
            model.Password = "dev@123";
            model.Department = 0;
            model.Grade = 0;
            model.HQ = 0;
            model.Pancard = "Dummy";
            model.DOJ = DateTime.Now;
            model.DOB = DateTime.Now;
            model.Desigination = 0;
            model.CustomUserName = "Dummy";
            model.PasswordRaw = "dev@123";
            model.IsEnabled = true;

            string userName = model.FirstName.Substring(0, 1).ToLower() + model.MiddleName.Substring(0, 1).ToLower() + model.LastName.Replace(" ", "").ToLower();
            int iUserNameCount = _appDbContext.Users.Where(u => u.UserName.Contains(userName)).ToList().Count();
            if (iUserNameCount > 0)
            {
                iUserNameCount += 1;
                userName = userName + iUserNameCount.ToString();
            }

            model.CustomUserName = userName;
            model.PasswordRaw = model.Password;

            var userIdentity = _mapper.Map<AppUser>(model);
            var result = await _userManager.CreateAsync(userIdentity, model.Password);
            var user = await _userManager.FindByIdAsync(userIdentity.Id);

            #region "Create User, Roles & Add Role Claims"
            //Admin: Data in Roles Table
            var adminRole = await _roleManager.FindByNameAsync(Permissions.Roles.ADMIN);
            if (adminRole == null)
            {
                adminRole = new IdentityRole(Permissions.Roles.ADMIN);
                await _roleManager.CreateAsync(adminRole);                
            }

            //await _roleManager.AddClaimAsync(adminRole, new Claim(CustomClaimTypes.Permission, Permissions.Admins.CRUD));

            ////Add UserClaims
            await _userManager.AddClaimAsync(user, new Claim(CustomClaimTypes.Permission, Permissions.Admins.CRUD));

            if (!await _userManager.IsInRoleAsync(user, adminRole.Name))
            {
                await _userManager.AddToRoleAsync(user, adminRole.Name);
            }
            #endregion

            //Extra Tables
            ContactResourseViewModel contact = new ContactResourseViewModel();
            contact.RefTableId = user.Id;
            contact.RefTableName = Constants.ReferenceTableNames.EMPLOYEE;
            contact.Address = "Dev Admin Address";
            contact.State = "Dev Admin State";
            contact.City = "Dev Admin City";
            contact.PinCode = "400086";
            contact.MobileNumber = "9029718964";
            contact.ResidenceNumber = "9090909090";

            var contactData = _mapper.Map<ContactResourseViewModel,Data.ContactResourse>(contact);
            _appDbContext.ContactResourse.Add(contactData);

            await _appDbContext.SaveChangesAsync();

        }

        [HttpPost("ChangePwd")]
        public async Task ChangePassword()
        {
            //IdentityResult result = await _userManager.ChangePasswordAsync(User.Identity.GetUserId(), oldPassword, newPassword);
            var usrID = "cc3677bd-d279-4439-ad29-d237a185003f";
            var oldPwd = "mynewpwd";
            var newPwd = "avsanjay1325";

            //update Raw Pwd
            //var origionalData = _unitOfWork.MasterKeyValues.GetSingleOrDefault(m => m.Id == masterKeyValueVM.Id);
            //if (origionalData != null)
            //{
            //    mstKeyVal = mapper.Map<MasterKeyValueViewModel, MasterKeyValue>(masterKeyValueVM);
            //    _unitOfWork.MasterKeyValues.Update(origionalData, mstKeyVal);
            //    await _unitOfWork.CompleteAsync();
            //}

            var user = await _userManager.FindByIdAsync(usrID);
            IdentityResult result = await _userManager.ChangePasswordAsync(user, oldPwd, newPwd);

            await _appDbContext.SaveChangesAsync();

            //Find user
            var userNewPwd = await _userManager.FindByIdAsync(usrID);
        }
    }
}