using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Storage;
using SmearAdmin.Data;
using SmearAdmin.Helpers;
using SmearAdmin.Models;
using SmearAdmin.Persistence;
using SmearAdmin.ViewModels;

namespace SmearAdmin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IUnitOfWork _unitOfWork;

        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IHostingEnvironment _host;

        private readonly int MAX_BYTES = 2 * 1024 * 1024; //2 MB (2097152)
        private readonly string[] ACCEPTED_FILE_TYPES = new[] { ".jpg", ".jpeg", ".png"};

        public EmployeeController(IMapper mapper, IUnitOfWork UnitOfWork, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager,
            IHostingEnvironment host)
        {
            this.mapper = mapper;
            _unitOfWork = UnitOfWork;

            _userManager = userManager;
            _roleManager = roleManager;
            _host = host;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<MasterKeyValueResult<MasterKeyValue>> GetMastersForEmployee()
        {
            return await _unitOfWork.MasterKeyValues.GetMastersForEmployeeAsync();
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> AddEmployee(RegistrationViewModel userRegVM)
        {
            try
            {
                //Check Email first
                var emailToCheck = userRegVM.Email;
                var emailVal = _unitOfWork.Employee.GetSingleOrDefault(e => e.Email == emailToCheck);
                if (emailVal == null)
                {
                    string tempPassword = DateTime.Now.ToString("ffff");
                    string userName = userRegVM.FirstName.Substring(0, 1).ToLower() + userRegVM.MiddleName.Substring(0, 1).ToLower() + userRegVM.LastName.Replace(" ", "").ToLower();
                    int iUserNameCount = _unitOfWork.Employee.Find(u => u.UserName.Contains(userName)).ToList().Count();
                    if (iUserNameCount > 0)
                    {
                        iUserNameCount += 1;
                        userName += iUserNameCount.ToString();
                    }

                    tempPassword = userName + tempPassword;

                    userRegVM.CustomUserName = userName;
                    userRegVM.Password = tempPassword;
                    userRegVM.PasswordRaw = tempPassword;
                    userRegVM.IsEnabled = true;

                    //Create User
                    userRegVM.DOB = Utilities.FormatDateTimeByZone(userRegVM.DOB.Value);
                    userRegVM.DOJ = Utilities.FormatDateTimeByZone(userRegVM.DOJ.Value);
                    var userIdentity = mapper.Map<RegistrationViewModel, AppUser>(userRegVM);
                    userIdentity.Id = Guid.NewGuid().ToString();
                    var result = await _userManager.CreateAsync(userIdentity, tempPassword);

                    if (!result.Succeeded) return new BadRequestObjectResult(Errors.AddError(result.ToString(), ModelState));

                    //Get user to add roles to user
                    var user = await _userManager.FindByIdAsync(userIdentity.Id);

                    //Add Roles to user
                    IdentityRole adminUserRole = null;
                    if (userRegVM.Roles.RoleName.ToLower().Equals(Permissions.Roles.USER))
                    {
                        //User: Data in Roles Table
                        adminUserRole = await _roleManager.FindByNameAsync(Permissions.Roles.USER);
                        if (adminUserRole == null)
                        {
                            adminUserRole = new IdentityRole(Permissions.Roles.USER);
                            await _roleManager.CreateAsync(adminUserRole);
                        }

                        //Add Claims to user
                        for (int i = 0; i <= userRegVM.Roles.RoleClaims.Length - 1; i++)
                        {
                            string roleClaims = userRegVM.Roles.RoleClaims[i].ToString();
                            switch (roleClaims)
                            {
                                case Permissions.RolePermission.Add:
                                    {
                                        await _userManager.AddClaimAsync(user, new Claim(CustomClaimTypes.Permission, Permissions.Users.Add));
                                        break;
                                    }
                                case Permissions.RolePermission.Edit:
                                    {
                                        await _userManager.AddClaimAsync(user, new Claim(CustomClaimTypes.Permission, Permissions.Users.Edit));
                                        break;
                                    }
                                case Permissions.RolePermission.View:
                                    {
                                        await _userManager.AddClaimAsync(user, new Claim(CustomClaimTypes.Permission, Permissions.Users.View));
                                        break;
                                    }
                                case Permissions.RolePermission.Delete:
                                    {
                                        await _userManager.AddClaimAsync(user, new Claim(CustomClaimTypes.Permission, Permissions.Users.Delete));
                                        break;
                                    }
                                default:
                                    {
                                        break;
                                    }
                            }
                        }
                    }
                    else
                    {
                        //Admin: Data in Roles Table
                        adminUserRole = await _roleManager.FindByNameAsync(Permissions.Roles.ADMIN);
                        if (adminUserRole == null)
                        {
                            adminUserRole = new IdentityRole(Permissions.Roles.ADMIN);
                            await _roleManager.CreateAsync(adminUserRole);
                        }

                        //Add Claims to Admin
                        await _userManager.AddClaimAsync(user, new Claim(CustomClaimTypes.Permission, Permissions.Admins.CRUD));
                    }

                    //Add Roles to user                
                    if (!await _userManager.IsInRoleAsync(user, adminUserRole.Name))
                    {
                        await _userManager.AddToRoleAsync(user, adminUserRole.Name);
                    }

                    //Add Contact
                    ContactResourseViewModel contactVM = userRegVM.Contact;                    
                    var empContact = mapper.Map<ContactResourseViewModel, Data.ContactResourse>(contactVM);
                    empContact.RefTableId = userIdentity.Id;                    
                    _unitOfWork.ContactResource.Add(empContact);

                    //Add Region according to UserID and HQ
                    HQRegionViewModel HQRegionVM = null;
                    foreach (var rgn in userRegVM.Region)
                    {
                        HQRegionVM = new HQRegionViewModel
                        {
                            HQID = userRegVM.HQ,
                            UserID = userIdentity.Id,
                            RegionID = rgn
                        };

                        var empHQRegion = mapper.Map<HQRegionViewModel, Data.Hqregion>(HQRegionVM);
                        _unitOfWork.HQRegion.Add(empHQRegion);
                    }

                    await _unitOfWork.CompleteAsync();
                }
                else
                {
                    return BadRequest(Errors.AddError("Email Already Exists.", ModelState));
                }
            }
            catch (Exception Ex)
            {
                return new BadRequestObjectResult(Errors.AddError(Ex, ModelState));
            }
            return new OkObjectResult(userRegVM);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetAllEmployee(int pageIndex, int pageSize)
        {
            var results = await _unitOfWork.Employee.GetAllEmployeeAsync(pageIndex, pageSize);
            return Ok(results);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetEmployee(string ID)
        {
            var results = await _unitOfWork.Employee.GetEmployeeByIDAsync(ID);
            return Ok(results);
        }

        [HttpPut]
        [Route("[action]")]
        public async Task<IActionResult> DeleteEmployee(RegistrationViewModel userRegVM, string action)
        {
            try
            {
                var origionalData = _unitOfWork.Employee.GetSingleOrDefault(e => e.Id == userRegVM.ID);
                if (origionalData != null)
                {
                    if (action.Equals("delete"))
                        origionalData.IsEnabled = false;
                    else
                        origionalData.IsEnabled = true;

                    _unitOfWork.Employee.Update(origionalData);
                    await _unitOfWork.CompleteAsync();
                }
            }
            catch (Exception Ex) { throw Ex; }

            return Ok(userRegVM);
        }
        
        [HttpPut]
        [Route("[action]")]
        public async Task<IActionResult> EditEmployee(RegistrationViewModel userRegVM)
        {
            try
            {
                var origionalData = _unitOfWork.Employee.GetSingleOrDefault(e => e.Id == userRegVM.ID);
                if (origionalData != null)
                {
                    //update AspNetusers Table
                    origionalData.DOB = Utilities.FormatDateTimeByZone(userRegVM.DOB.Value);
                    origionalData.DOJ = Utilities.FormatDateTimeByZone(userRegVM.DOJ.Value);
                    origionalData.Department = (int)userRegVM.Department;
                    origionalData.Desigination = (int)userRegVM.Desigination;
                    origionalData.Grade = (int)userRegVM.Grade;
                    origionalData.HQ = (int)userRegVM.HQ;
                    origionalData.Pancard = userRegVM.Pancard;

                    _unitOfWork.Employee.Update(origionalData);

                    //Get user
                    var user = await _userManager.FindByIdAsync(userRegVM.ID);

                    //Get claims & Remove it
                    var userClaims = await _userManager.GetClaimsAsync(user);
                    foreach (Claim claim in userClaims)
                    {
                        await _userManager.RemoveClaimAsync(user, claim);
                    }

                    //Get Roles & Remove it
                    var userRole = await _userManager.GetRolesAsync(user);
                    foreach (var role in userRole)
                    {
                        await _userManager.RemoveFromRoleAsync(user, role);
                    }


                    //Add Roles to user
                    IdentityRole adminUserRole = null;
                    if (userRegVM.Roles.RoleName.ToLower().Equals(Permissions.Roles.USER))
                    {
                        //User: Data in Roles Table
                        adminUserRole = await _roleManager.FindByNameAsync(Permissions.Roles.USER);
                        if (adminUserRole == null)
                        {
                            adminUserRole = new IdentityRole(Permissions.Roles.USER);
                            await _roleManager.CreateAsync(adminUserRole);
                        }

                        //Add Claims to user
                        for (int i = 0; i <= userRegVM.Roles.RoleClaims.Length - 1; i++)
                        {
                            string roleClaims = userRegVM.Roles.RoleClaims[i].ToString();
                            switch (roleClaims)
                            {
                                case Permissions.RolePermission.Add:
                                    {
                                        await _userManager.AddClaimAsync(user, new Claim(CustomClaimTypes.Permission, Permissions.Users.Add));
                                        break;
                                    }
                                case Permissions.RolePermission.Edit:
                                    {
                                        await _userManager.AddClaimAsync(user, new Claim(CustomClaimTypes.Permission, Permissions.Users.Edit));
                                        break;
                                    }
                                case Permissions.RolePermission.View:
                                    {
                                        await _userManager.AddClaimAsync(user, new Claim(CustomClaimTypes.Permission, Permissions.Users.View));
                                        break;
                                    }
                                case Permissions.RolePermission.Delete:
                                    {
                                        await _userManager.AddClaimAsync(user, new Claim(CustomClaimTypes.Permission, Permissions.Users.Delete));
                                        break;
                                    }
                                default:
                                    {
                                        break;
                                    }
                            }
                        }
                    }
                    else
                    {
                        //Admin: Data in Roles Table
                        adminUserRole = await _roleManager.FindByNameAsync(Permissions.Roles.ADMIN);
                        if (adminUserRole == null)
                        {
                            adminUserRole = new IdentityRole(Permissions.Roles.ADMIN);
                            await _roleManager.CreateAsync(adminUserRole);
                        }

                        //Add Claims to Admin
                        await _userManager.AddClaimAsync(user, new Claim(CustomClaimTypes.Permission, Permissions.Admins.CRUD));
                    }

                    //Add Roles to user                
                    if (!await _userManager.IsInRoleAsync(user, adminUserRole.Name))
                    {
                        await _userManager.AddToRoleAsync(user, adminUserRole.Name);
                    }

                    //Update Contact
                    var origionalContactData = _unitOfWork.ContactResource.GetSingleOrDefault(
                        c => c.RefTableId == userRegVM.ID &&
                        c.RefTableName == Constants.ReferenceTableNames.EMPLOYEE);

                    ContactResourseViewModel contactVM = userRegVM.Contact;

                    origionalContactData.RefTableId = contactVM.RefTableId;
                    origionalContactData.RefTableName = contactVM.RefTableName;
                    origionalContactData.Address = contactVM.Address;
                    origionalContactData.State = contactVM.State;
                    origionalContactData.City = contactVM.City;
                    origionalContactData.PinCode = contactVM.PinCode;
                    origionalContactData.MobileNumber = contactVM.MobileNumber;
                    origionalContactData.ResidenceNumber = contactVM.ResidenceNumber;

                    _unitOfWork.ContactResource.Update(origionalContactData);

                    //Delete Existing Region 
                    var HQRegionData = _unitOfWork.HQRegion.GetAll()
                        .Where(r => r.Hqid == userRegVM.HQ && r.UserId == userRegVM.ID);

                    if (HQRegionData != null)
                    {
                        _unitOfWork.HQRegion.RemoveRange(HQRegionData);
                    }

                    // Add Region according to UserID and HQ
                    HQRegionViewModel HQRegionVM = null;
                    foreach (var rgn in userRegVM.Region)
                    {
                        HQRegionVM = new HQRegionViewModel
                        {
                            HQID = userRegVM.HQ,
                            UserID = userRegVM.ID,
                            RegionID = rgn
                        };

                        var empHQRegion = mapper.Map<HQRegionViewModel, Data.Hqregion>(HQRegionVM);
                        _unitOfWork.HQRegion.Add(empHQRegion);
                    }

                    await _unitOfWork.CompleteAsync();
                }
            }
            catch (Exception Ex)
            {
                return new BadRequestObjectResult(Errors.AddError(Ex, ModelState));
            }

            return Ok(userRegVM);
        }

        [HttpPut]
        [Route("[action]")]
        public async Task<IActionResult> ChangePassword(RegistrationViewModel userRegVM)
        {
            try
            {
                //Find user
                var user = await _userManager.FindByIdAsync(userRegVM.ID);
                string oldPassword = userRegVM.Password;
                string newPassword = userRegVM.PasswordRaw;
                IdentityResult result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);

                //update Raw Pwd
                var origionalData = _unitOfWork.Employee.GetSingleOrDefault(u => u.Id == user.Id);
                if (origionalData != null)
                {
                    origionalData.PasswordRaw = newPassword;
                    _unitOfWork.Employee.Update(origionalData);
                }

                await _unitOfWork.CompleteAsync();
            }
            catch (Exception Ex)
            {
                return new BadRequestObjectResult(Errors.AddError(Ex, ModelState));
            }
            return Ok(userRegVM);
        }

        [HttpPost]
        [Route("/api/Employee/{userID}/UploadPhoto")]
        public async Task<IActionResult> UploadPhoto(string userID, IFormFile formFile)
        {
            //Fetch User
            var userData = _unitOfWork.Employee.GetSingleOrDefault(u => u.Id == userID);

            try
            {
                if (formFile.Length > MAX_BYTES) return BadRequest(Errors.AddError("Max file size is 2 MB", ModelState));
                if (!ACCEPTED_FILE_TYPES.Any(s => s == Path.GetExtension(formFile.FileName)))
                    return BadRequest(Errors.AddError("Only *.jpg,*.jpeg & *.png type files are allowed", ModelState));

                //UploadFile                
                if (userData != null)
                {
                    var uploadFolderPath = Path.Combine(_host.WebRootPath, "Uploads");
                    if (!Directory.Exists(uploadFolderPath))
                        Directory.CreateDirectory(uploadFolderPath);

                    //var fileName = userData.UserName + "_" + Guid.NewGuid().ToString() + Path.GetExtension(formFile.FileName);
                    var fileName = userData.UserName + Path.GetExtension(formFile.FileName);
                    var filePath = Path.Combine(uploadFolderPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }

                    //Update URL to User                    
                    userData.PictureUrl = $"/Uploads/{ fileName}";
                    _unitOfWork.Employee.Update(userData);

                    await _unitOfWork.CompleteAsync();
                }
            }
            catch (Exception Ex)
            {
                return new BadRequestObjectResult(Errors.AddError(Ex, ModelState));
            }
            return Ok(userData);
        }
    }
}