 using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using SmearAdmin.Data;
using SmearAdmin.Helpers;
using SmearAdmin.Interface;
using SmearAdmin.Models;
using SmearAdmin.Persistence;
using SmearAdmin.ViewModels;

namespace SmearAdmin.Repository
{
    public class EmployeeRepository : Repositories<AppUser>, IEmployeeRepository
    {
        public EmployeeRepository(SmearAdminDbContext context) : base(context) { }

        private SmearAdminDbContext _appDbContext => (SmearAdminDbContext)_context;

        public async Task<PagingResult<RegistrationViewModel>> GetAllEmployeeAsync(int pageIndex, int pageSize)
        {
            //int pageNum = Convert.ToInt32(newPage);
            int totalCount = 0, totalPage = 0;
            pageIndex += 1;

            totalCount = _appDbContext.Users.Count();
            totalPage = (totalCount / pageSize) + ((totalCount % pageSize) > 0 ? 1 : 0);

            var dataUsers = await (from u in _appDbContext.Users
                             join c in _appDbContext.ContactResourse
                             on u.Id equals c.RefTableId
                             join ur in _appDbContext.UserRoles
                             on u.Id equals ur.UserId
                             join r in _appDbContext.Roles
                             on ur.RoleId equals r.Id
                             where c.RefTableName.Equals(Constants.ReferenceTableNames.EMPLOYEE)
                             select new RegistrationViewModel
                             {
                                 ID = u.Id,
                                 FirstName = u.FirstName,
                                 MiddleName = u.MiddleName,
                                 LastName = u.LastName,
                                 PictureUrl = u.PictureUrl,
                                 Email = u.Email,
                                 DOJ = u.DOJ,
                                 DOB = u.DOB,
                                 Department = u.Department,
                                 Desigination = u.Desigination,
                                 Grade = u.Grade,
                                 HQ = u.HQ,
                                 HQName = (from m in _appDbContext.MasterKeyValue
                                           where m.Id == u.HQ
                                           select m.Value).FirstOrDefault(),

                                 RegionName = (from hqrgn in _appDbContext.Hqregion
                                               join mst in _appDbContext.MasterKeyValue
                                               on hqrgn.RegionId equals mst.Id
                                               where (hqrgn.Hqid.Equals(u.HQ) && hqrgn.UserId.Equals(u.Id))
                                               select mst.Value).ToList(),

                                 Pancard = u.Pancard,
                                 CustomUserName = u.UserName,
                                 Password = u.PasswordRaw,
                                 IsEnabled = u.IsEnabled,
                                 Contact = new ContactResourseViewModel
                                 {
                                     RefTableId = c.RefTableId,
                                     RefTableName = c.RefTableName,
                                     Address = c.Address,
                                     State = c.State,
                                     City = c.City,
                                     PinCode = c.PinCode,
                                     MobileNumber = c.MobileNumber,
                                     ResidenceNumber = c.ResidenceNumber
                                 },
                                 Roles = new RolesViewModel
                                 {
                                     RoleName = r.Name,
                                     UserClaimsOnRoles = (from usrClms in _appDbContext.UserClaims
                                                          where (usrClms.UserId.Equals(u.Id))
                                                          select usrClms.ClaimValue).ToList()
                                 }
                             })
                             .Skip((pageIndex - 1) * pageSize)
                             .Take(pageSize)
                             .ToListAsync().ConfigureAwait(false);

            var pagingData = new PagingResult<RegistrationViewModel>
            {
                Items = dataUsers.AsEnumerable().OrderBy(f => f.FirstName),
                TotalCount = totalCount,
                TotalPage = totalPage
            };

            return await Task.FromResult(pagingData);
        }

        //public async Task<IEnumerable<RegistrationViewModel>> GetEmployeeByIDAsync(string ID)
        //{
        //    var dataUsers = (from u in _appDbContext.Users
        //                     join c in _appDbContext.ContactResourse
        //                     on u.Id equals c.RefTableId
        //                     join ur in _appDbContext.UserRoles
        //                     on u.Id equals ur.UserId
        //                     join r in _appDbContext.Roles
        //                     on ur.RoleId equals r.Id
        //                     where c.RefTableName.Equals(Constants.ReferenceTableNames.EMPLOYEE)
        //                     select new RegistrationViewModel
        //                     {
        //                         ID = u.Id,
        //                         FirstName = u.FirstName,
        //                         MiddleName = u.MiddleName,
        //                         LastName = u.LastName,
        //                         PictureUrl = u.PictureUrl,
        //                         Email = u.Email,
        //                         DOJ = u.DOJ,
        //                         DOB = u.DOB,

        //                         Department = u.Department,
        //                         DepartmentName = (from depm in _appDbContext.MasterKeyValue
        //                                           where depm.Id == u.Department
        //                                           select depm.Value).FirstOrDefault(),

        //                         Desigination = u.Desigination,
        //                         DesiginationName = (from desm in _appDbContext.MasterKeyValue
        //                                             where desm.Id == u.Desigination
        //                                             select desm.Value).FirstOrDefault(),

        //                         Grade = u.Grade,
        //                         GradeName = (from gm in _appDbContext.MasterKeyValue
        //                                      where gm.Id == u.Grade
        //                                      select gm.Value).FirstOrDefault(),

        //                         HQ = u.HQ,
        //                         HQName = (from m in _appDbContext.MasterKeyValue
        //                                   where m.Id == u.HQ
        //                                   select m.Value).FirstOrDefault(),

        //                         RegionName = (from hqrgn in _appDbContext.Hqregion
        //                                       join mst in _appDbContext.MasterKeyValue
        //                                       on hqrgn.RegionId equals mst.Id
        //                                       where (hqrgn.Hqid.Equals(u.HQ) && hqrgn.UserId.Equals(u.Id))
        //                                       select mst.Value).ToList(),

        //                         Pancard = u.Pancard,
        //                         CustomUserName = u.UserName,
        //                         Password = u.PasswordRaw,
        //                         IsEnabled = u.IsEnabled,
        //                         Contact = new ContactResourseViewModel
        //                         {
        //                             RefTableId = c.RefTableId,
        //                             RefTableName = c.RefTableName,
        //                             Address = c.Address,
        //                             State = c.State,
        //                             City = c.City,
        //                             PinCode = c.PinCode,
        //                             MobileNumber = c.MobileNumber,
        //                             ResidenceNumber = c.ResidenceNumber
        //                         },
        //                         Roles = new RolesViewModel
        //                         {
        //                             RoleName = r.Name,
        //                             UserClaimsOnRoles = (from usrClms in _appDbContext.UserClaims
        //                                                  where (usrClms.UserId.Equals(u.Id))
        //                                                  select usrClms.ClaimValue).ToList()
        //                         }
        //                     })
        //                     .Where(x => x.ID.Equals(ID));

        //    var dataDaily = await (from mst in _appDbContext.MasterKeyValue
        //                           where mst.Type.Equals(Constants.EmployeeConstant.Daily)
        //                           select mst).FirstOrDefaultAsync();

        //    var dataBike = await (from mst in _appDbContext.MasterKeyValue
        //                          where mst.Type.Equals(Constants.EmployeeConstant.Bike)
        //                          select mst.Value).FirstOrDefaultAsync();

        //    var dataMobile = await (from mst in _appDbContext.MasterKeyValue
        //                          where mst.Type.Equals(Constants.EmployeeConstant.Mobile)
        //                          select mst.Value).FirstOrDefaultAsync();

        //    var dataFare = await (from mst in _appDbContext.MasterKeyValue
        //                          where mst.Type.Equals(Constants.EmployeeConstant.Fare)
        //                          select mst.Value).FirstOrDefaultAsync();

        //    var dataStationery = await (from mst in _appDbContext.MasterKeyValue
        //                          where mst.Type.Equals(Constants.EmployeeConstant.Stationery)
        //                          select mst.Value).FirstOrDefaultAsync();

        //    var dataCyber = await (from mst in _appDbContext.MasterKeyValue
        //                          where mst.Type.Equals(Constants.EmployeeConstant.Cyber)
        //                          select mst.Value).FirstOrDefaultAsync();

        //    return await Task.FromResult(dataUsers);
        //}

        public async Task<(RegistrationViewModel, string, string, string, string, string, string)> GetEmployeeByIDAsync(string ID)
        {
            var dataUsers = await (from u in _appDbContext.Users
                                   join c in _appDbContext.ContactResourse
                                   on u.Id equals c.RefTableId
                                   join ur in _appDbContext.UserRoles
                                   on u.Id equals ur.UserId
                                   join r in _appDbContext.Roles
                                   on ur.RoleId equals r.Id
                                   where c.RefTableName.Equals(Constants.ReferenceTableNames.EMPLOYEE)
                                   select new RegistrationViewModel
                                   {
                                       ID = u.Id,
                                       FirstName = u.FirstName,
                                       MiddleName = u.MiddleName,
                                       LastName = u.LastName,
                                       PictureUrl = u.PictureUrl,
                                       Email = u.Email,
                                       DOJ = u.DOJ,
                                       DOB = u.DOB,

                                       Department = u.Department,
                                       DepartmentName = (from depm in _appDbContext.MasterKeyValue
                                                         where depm.Id == u.Department
                                                         select depm.Value).FirstOrDefault(),

                                       Desigination = u.Desigination,
                                       DesiginationName = (from desm in _appDbContext.MasterKeyValue
                                                           where desm.Id == u.Desigination
                                                           select desm.Value).FirstOrDefault(),

                                       Grade = u.Grade,
                                       GradeName = (from gm in _appDbContext.MasterKeyValue
                                                    where gm.Id == u.Grade
                                                    select gm.Value).FirstOrDefault(),

                                       HQ = u.HQ,
                                       HQName = (from m in _appDbContext.MasterKeyValue
                                                 where m.Id == u.HQ
                                                 select m.Value).FirstOrDefault(),

                                       Region = (from hqrgn in _appDbContext.Hqregion
                                                 join mst in _appDbContext.MasterKeyValue
                                                 on hqrgn.RegionId equals mst.Id
                                                 where (hqrgn.Hqid.Equals(u.HQ) && hqrgn.UserId.Equals(u.Id))
                                                 select mst.Id).ToList(),

                                       RegionName = (from hqrgn in _appDbContext.Hqregion
                                                     join mst in _appDbContext.MasterKeyValue
                                                     on hqrgn.RegionId equals mst.Id
                                                     where (hqrgn.Hqid.Equals(u.HQ) && hqrgn.UserId.Equals(u.Id))
                                                     select mst.Value).ToList(),

                                       Pancard = u.Pancard,
                                       CustomUserName = u.UserName,
                                       Password = u.PasswordRaw,
                                       IsEnabled = u.IsEnabled,
                                       Contact = new ContactResourseViewModel
                                       {
                                           RefTableId = c.RefTableId,
                                           RefTableName = c.RefTableName,
                                           Address = c.Address,
                                           State = c.State,
                                           City = c.City,
                                           PinCode = c.PinCode,
                                           MobileNumber = c.MobileNumber,
                                           ResidenceNumber = c.ResidenceNumber
                                       },
                                       Roles = new RolesViewModel
                                       {
                                           RoleName = r.Name,
                                           UserClaimsOnRoles = (from usrClms in _appDbContext.UserClaims
                                                                where (usrClms.UserId.Equals(u.Id))
                                                                select usrClms.ClaimValue).ToList()
                                       }
                                   })
                             .Where(x => x.ID.Equals(ID)).ToListAsync().ConfigureAwait(false);

            var dataDaily = await (from mst in _appDbContext.MasterKeyValue
                                   where mst.Type.Equals(Constants.EmployeeConstant.Daily)
                                   select mst.Value).FirstOrDefaultAsync().ConfigureAwait(false);

            var dataBike = await (from mst in _appDbContext.MasterKeyValue
                                  where mst.Type.Equals(Constants.EmployeeConstant.Bike)
                                  select mst.Value).FirstOrDefaultAsync().ConfigureAwait(false);

            var dataMobile = await (from mst in _appDbContext.MasterKeyValue
                                    where mst.Type.Equals(Constants.EmployeeConstant.Mobile)
                                    select mst.Value).FirstOrDefaultAsync().ConfigureAwait(false);

            var dataFare = await (from mst in _appDbContext.MasterKeyValue
                                  where mst.Type.Equals(Constants.EmployeeConstant.Fare)
                                  select mst.Value).FirstOrDefaultAsync().ConfigureAwait(false);

            var dataStationery = await (from mst in _appDbContext.MasterKeyValue
                                        where mst.Type.Equals(Constants.EmployeeConstant.Stationery)
                                        select mst.Value).FirstOrDefaultAsync().ConfigureAwait(false);

            var dataCyber = await (from mst in _appDbContext.MasterKeyValue
                                   where mst.Type.Equals(Constants.EmployeeConstant.Cyber)
                                   select mst.Value).FirstOrDefaultAsync().ConfigureAwait(false);

            return (dataUsers.ToList()[0], dataDaily, dataBike, dataMobile, dataFare, dataStationery, dataCyber);
        }

        public async Task<List<SwapEmployeeViewModel>> GetSwapEmployeeListAsync()
        {
            List<SwapEmployeeViewModel> dataUserNames = await (from u in _appDbContext.Users
                                                               select new SwapEmployeeViewModel
                                                               {
                                                                   ID = u.Id,
                                                                   FullName = string.Join(" ", u.FirstName, u.MiddleName, u.LastName, "(" + u.UserName + ")"),
                                                                   UserName = u.UserName,
                                                               }).ToListAsync().ConfigureAwait(false);
            return dataUserNames;
        }

        public async Task<string> SwapEmployeeUserNamesAsync(string userNameFrom, string userNameTo)
        {
            string msg = "No records found to update.";
            var userNameList = await _appDbContext.AuditableEntity.Where(a => a.CreatedBy.Equals(userNameFrom)).ToListAsync().ConfigureAwait(false);
            if (userNameList.Count > 0)
            {
                userNameList.ForEach(c => c.CreatedBy = userNameTo);
                _appDbContext.SaveChanges();
                msg = "ok";
            }

            return JsonConvert.SerializeObject(new { status = msg });
        }
    }
}
