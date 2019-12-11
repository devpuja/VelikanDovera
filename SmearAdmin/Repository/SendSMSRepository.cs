using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SmearAdmin.Data;
using SmearAdmin.Helpers;
using SmearAdmin.Interface;
using SmearAdmin.Persistence;
using SmearAdmin.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static SmearAdmin.Helpers.Constants;

namespace SmearAdmin.Repository
{
    public class SendSMSRepository : Repositories<Smslogger>, ISendSMSRepository
    {
        public SendSMSRepository(SmearAdminDbContext context) : base(context) { }

        private SmearAdminDbContext _appDbContext => (SmearAdminDbContext)_context;

        public async Task<PagingResult<DoctorViewModel>> GetAllDoctorSendSMSAsync(int pageIndex, int pageSize)
        {
            int totalCount = 0, totalPage = 0;
            pageIndex += 1;

            totalCount = _appDbContext.Doctor.Count();
            totalPage = (totalCount / pageSize) + ((totalCount % pageSize) > 0 ? 1 : 0);

            var dataUsers = await (from d in _appDbContext.Doctor
                                   join c in _appDbContext.ContactResourse
                                   on d.Id equals c.RefTableId
                                   where c.RefTableName.Equals(ReferenceTableNames.DOCTOR)
                                   select new DoctorViewModel
                                   {
                                       ID = Convert.ToString(d.Id),
                                       Name = d.Name,
                                       Speciality = d.Speciality,
                                       Class = d.Class,
                                       Brand = d.Brand,
                                       BrandName = (from b in _appDbContext.MasterKeyValue
                                                    where d.Brand.ToString().Contains(b.Id.ToString()) && b.Type.Equals(DoctorConstant.Brand)
                                                    select b.Value).ToList(),
                                       //BrandName = _appDbContext.MasterKeyValue.Where(c => d.Brand.ToString().Contains(c.Id.ToString()) && c.Type.Equals(DoctorConstant.Brand)).Select(v => v.Value).ToList(),
                                       Contact = new ContactResourseViewModel
                                       {
                                           MobileNumber = c.MobileNumber
                                       }
                                   })
                             .Skip((pageIndex - 1) * pageSize)
                             .Take(pageSize)
                             .ToListAsync().ConfigureAwait(false);

            var pagingData = new PagingResult<DoctorViewModel>
            {
                Items = dataUsers.AsEnumerable().OrderBy(f => f.Name),
                TotalCount = totalCount,
                TotalPage = totalPage
            };

            return await Task.FromResult(pagingData);
        }

        public async Task<AdminDashboardViewModelDTO> GetSendSMSCountAsync()
        {
            //Get Count
            var getSentToCount = _appDbContext.Smslogger.GroupBy(n => n.SendSmsto).
                     Select(group =>
                         new
                         {
                             SendSMSToName = group.Key,
                             //Notices = group.ToList(),
                             Count = group.Count()
                         }).ToList();

            List<AdminDashboardViewModel> objLst = new List<AdminDashboardViewModel>();
            foreach (var item in getSentToCount)
            {
                objLst.Add(new AdminDashboardViewModel
                {
                    SendSMSToName = item.SendSMSToName,
                    Count = item.Count
                });
            }

            //Get SMS Balance
            var getSMSBalance = await SMSUtility.GetSMSBalance();
            dynamic resultData = JsonConvert.DeserializeObject(Convert.ToString(getSMSBalance));
            string balaceData = resultData.Balance;
            int iBalace = (int)Math.Round(Convert.ToDecimal(Convert.ToString(balaceData.Split('|')[1].Substring(6))), 0);

            //Load Data in DTO class
            AdminDashboardViewModelDTO objDM = new AdminDashboardViewModelDTO
            {
                SendSMSToNameItems = objLst,
                SMSBalance = iBalace
            };

            return objDM;
        }
    }
}