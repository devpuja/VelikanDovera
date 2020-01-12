using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using SmearAdmin.Data;
using SmearAdmin.Helpers;
using SmearAdmin.Interface;
using SmearAdmin.Persistence;
using SmearAdmin.ViewModels;
using static SmearAdmin.Helpers.Constants;

namespace SmearAdmin.Repository
{
    public class DoctorRepository : Repositories<Doctor>, IDoctorRepository
    {
        public DoctorRepository(SmearAdminDbContext context) : base(context) { }

        private SmearAdminDbContext _appDbContext => (SmearAdminDbContext)_context;

        public async Task<PagingResult<DoctorViewModel>> GetAllDoctorAsync(int pageIndex, int pageSize)
        {
            int totalCount = 0, totalPage = 0;
            pageIndex += 1;

            totalCount = _appDbContext.Doctor.Count();
            totalPage = (totalCount / pageSize) + ((totalCount % pageSize) > 0 ? 1 : 0);

            var dataUsers = await (from d in _appDbContext.Doctor
                                   join c in _appDbContext.ContactResourse
                                   on d.Id equals c.RefTableId
                                   join cs in _appDbContext.ChemistStockistResourse
                                   on d.Id equals cs.RefTableId
                                   join a in _appDbContext.AuditableEntity
                                   on d.Id equals a.RefTableId
                                   where c.RefTableName.Equals(ReferenceTableNames.DOCTOR)
                                   select new DoctorViewModel
                                   {
                                       ID = Convert.ToString(d.Id),
                                       Name = d.Name,
                                       Qualification = d.Qualification,
                                       RegistrationNo = d.RegistrationNo,
                                       Speciality = d.Speciality,
                                       Gender = d.Gender,
                                       VisitFrequency = d.VisitFrequency,
                                       VisitPlan = d.VisitPlan,
                                       BestDayToMeet = d.BestDayToMeet,
                                       BestTimeToMeet = d.BestTimeToMeet,
                                       Class = d.Class,
                                       Brand = d.Brand,
                                       BrandName = (from b in _appDbContext.MasterKeyValue
                                                    where d.Brand.ToString().Contains(b.Id.ToString()) && b.Type.Equals(DoctorConstant.Brand)
                                                    select b.Value).ToList(),
                                       //BrandName = _appDbContext.MasterKeyValue.Where(c => d.Brand.ToString().Contains(c.Id.ToString()) && c.Type.Equals(DoctorConstant.Brand)).Select(v => v.Value).ToList(),
                                       Contact = new ContactResourseViewModel
                                       {
                                           ID = c.Id,
                                           RefTableId = c.RefTableId,
                                           RefTableName = c.RefTableName,
                                           Address = c.Address,
                                           State = c.State,
                                           City = c.City,
                                           PinCode = c.PinCode,
                                           MobileNumber = c.MobileNumber,
                                           ResidenceNumber = c.ResidenceNumber
                                       },
                                       Common = new CommonResourceViewModel
                                       {
                                           ID = cs.Id,
                                           ContactPersonMobileNumber = cs.ContactPersonMobileNumber,
                                           ContactPersonDateOfBirth = cs.ContactPersonDateOfBirth,
                                           ContactPersonDateOfAnniversary = cs.ContactPersonDateOfAnniversary,
                                           RefTableId = Convert.ToString(cs.RefTableId),
                                           RefTableName = cs.RefTableName
                                       },
                                       AuditableEntity = new AuditableEntityViewModel
                                       {
                                           ID = a.Id,
                                           RefTableId = a.RefTableId,
                                           RefTableName = a.RefTableName,
                                           FoundationDay = a.FoundationDay,
                                           CommunityID = (int)a.CommunityId,
                                           CommunityName = (from c in _appDbContext.MasterKeyValue
                                                            where c.Id == a.CommunityId && c.Type.Equals(EmployeeConstant.Community)
                                                            select c.Value).FirstOrDefault(),
                                           IsActive = Convert.ToBoolean(a.IsActive),
                                           CreateDate = a.CreateDate,
                                           CreatedBy = a.CreatedBy
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

        public async Task<PagingResult<DoctorViewModel>> GetAllDoctorsByUserAsync(int pageIndex, int pageSize, string userName)
        {
            int totalCount = 0, totalPage = 0;
            pageIndex += 1;

            //totalCount = _appDbContext.Doctor.Count();
            totalCount = await (from d in _appDbContext.Doctor
                                join c in _appDbContext.ContactResourse
                                on d.Id equals c.RefTableId
                                join a in _appDbContext.AuditableEntity
                                on d.Id equals a.RefTableId
                                where c.RefTableName.Equals(ReferenceTableNames.DOCTOR) && a.CreatedBy.Equals(userName)
                                select d).CountAsync().ConfigureAwait(false);

            totalPage = (totalCount / pageSize) + ((totalCount % pageSize) > 0 ? 1 : 0);

            var dataUsers = await (from d in _appDbContext.Doctor
                                   join c in _appDbContext.ContactResourse
                                   on d.Id equals c.RefTableId
                                   join cs in _appDbContext.ChemistStockistResourse
                                   on d.Id equals cs.RefTableId
                                   join a in _appDbContext.AuditableEntity
                                   on d.Id equals a.RefTableId
                                   where c.RefTableName.Equals(ReferenceTableNames.DOCTOR) && a.CreatedBy.Equals(userName)
                                   select new DoctorViewModel
                                   {
                                       ID = Convert.ToString(d.Id),
                                       Name = d.Name,
                                       Qualification = d.Qualification,
                                       RegistrationNo = d.RegistrationNo,
                                       Speciality = d.Speciality,
                                       Gender = d.Gender,
                                       VisitFrequency = d.VisitFrequency,
                                       VisitPlan = d.VisitPlan,
                                       BestDayToMeet = d.BestDayToMeet,
                                       BestTimeToMeet = d.BestTimeToMeet,
                                       Class = d.Class,
                                       Brand = d.Brand,
                                       BrandName = (from b in _appDbContext.MasterKeyValue
                                                    where d.Brand.ToString().Contains(b.Id.ToString()) && b.Type.Equals(DoctorConstant.Brand)
                                                    select b.Value).ToList(),
                                       //BrandName = _appDbContext.MasterKeyValue.Where(c => d.Brand.ToString().Contains(c.Id.ToString()) && c.Type.Equals(DoctorConstant.Brand)).Select(v => v.Value).ToList(),
                                       Contact = new ContactResourseViewModel
                                       {
                                           ID = c.Id,
                                           RefTableId = c.RefTableId,
                                           RefTableName = c.RefTableName,
                                           Address = c.Address,
                                           State = c.State,
                                           City = c.City,
                                           PinCode = c.PinCode,
                                           MobileNumber = c.MobileNumber,
                                           ResidenceNumber = c.ResidenceNumber
                                       },
                                       Common = new CommonResourceViewModel
                                       {
                                           ID = cs.Id,
                                           ContactPersonMobileNumber = cs.ContactPersonMobileNumber,
                                           ContactPersonDateOfBirth = cs.ContactPersonDateOfBirth,
                                           ContactPersonDateOfAnniversary = cs.ContactPersonDateOfAnniversary,
                                           RefTableId = Convert.ToString(cs.RefTableId),
                                           RefTableName = cs.RefTableName
                                       },
                                       AuditableEntity = new AuditableEntityViewModel
                                       {
                                           ID = a.Id,
                                           RefTableId = a.RefTableId,
                                           RefTableName = a.RefTableName,
                                           FoundationDay = a.FoundationDay,
                                           CommunityID = (int)a.CommunityId,
                                           CommunityName = (from c in _appDbContext.MasterKeyValue
                                                            where c.Id == a.CommunityId && c.Type.Equals(EmployeeConstant.Community)
                                                            select c.Value).FirstOrDefault(),
                                           IsActive = Convert.ToBoolean(a.IsActive),
                                           CreateDate = a.CreateDate,
                                           CreatedBy = a.CreatedBy
                                       }
                                   })
                             .Skip((pageIndex - 1) * pageSize)
                             .Take(pageSize)
                             .ToListAsync().ConfigureAwait(false);

            var pagingData = new PagingResult<DoctorViewModel>
            {
                Items = dataUsers.AsEnumerable().OrderBy(f=>f.Name),
                TotalCount = totalCount,
                TotalPage = totalPage
            };

            return await Task.FromResult(pagingData);
        }

        public async Task<DoctorViewModel> GetDoctorByIDAsync(string ID)
        {
            var dataDoctor = await (from d in _appDbContext.Doctor
                                    join c in _appDbContext.ContactResourse
                                    on d.Id equals c.RefTableId
                                    join cs in _appDbContext.ChemistStockistResourse
                                    on d.Id equals cs.RefTableId
                                    join a in _appDbContext.AuditableEntity
                                    on d.Id equals a.RefTableId
                                    where c.RefTableName.Equals(ReferenceTableNames.DOCTOR) && cs.RefTableName.Equals(ReferenceTableNames.DOCTOR) && 
                                    a.RefTableName.Equals(ReferenceTableNames.DOCTOR)
                                    select new DoctorViewModel
                                    {
                                        ID = d.Id,
                                        Name = d.Name,
                                        Qualification = d.Qualification,
                                        RegistrationNo = d.RegistrationNo,
                                        Speciality = d.Speciality,
                                        Gender = d.Gender,
                                        VisitFrequency = d.VisitFrequency,
                                        VisitPlan = d.VisitPlan,
                                        BestDayToMeet = d.BestDayToMeet,
                                        BestTimeToMeet = d.BestTimeToMeet,
                                        Class = d.Class,
                                        Brand = d.Brand,
                                        BrandName = (from b in _appDbContext.MasterKeyValue
                                                     where d.Brand.ToString().Contains(b.Id.ToString()) && b.Type.Equals(DoctorConstant.Brand)
                                                     select b.Value).ToList(),
                                        Contact = new ContactResourseViewModel
                                        {
                                            ID = c.Id,
                                            RefTableId = c.RefTableId,
                                            RefTableName = c.RefTableName,
                                            Address = c.Address,
                                            State = c.State,
                                            City = c.City,
                                            PinCode = c.PinCode,
                                            MobileNumber = c.MobileNumber,
                                            ResidenceNumber = c.ResidenceNumber
                                        },
                                        Common = new CommonResourceViewModel
                                        {
                                            ID = cs.Id,
                                            ContactPersonMobileNumber = cs.ContactPersonMobileNumber,
                                            ContactPersonDateOfBirth = cs.ContactPersonDateOfBirth,
                                            ContactPersonDateOfAnniversary = cs.ContactPersonDateOfAnniversary,
                                            RefTableId = Convert.ToString(cs.RefTableId),
                                            RefTableName = cs.RefTableName
                                        },
                                        AuditableEntity = new AuditableEntityViewModel
                                        {
                                            ID = a.Id,
                                            RefTableId = a.RefTableId,
                                            RefTableName = a.RefTableName,
                                            FoundationDay = a.FoundationDay,
                                            CommunityID = (int)a.CommunityId,
                                            CommunityName = (from c in _appDbContext.MasterKeyValue
                                                             where c.Id.ToString().Equals(a.CommunityId.ToString())
                                                             select c.Value).FirstOrDefault(),
                                            IsActive = Convert.ToBoolean(a.IsActive),
                                            CreateDate = a.CreateDate,
                                            CreatedBy = a.CreatedBy
                                        }
                                    })
                             .Where(d => d.ID.Equals(Convert.ToString(ID))).FirstOrDefaultAsync().ConfigureAwait(false);

            return dataDoctor;
        }

        public async Task<ExportExcel> ExportDoctorsDataAsync(IHostingEnvironment hostingEnvironment)
        {
            string fileName = "";
            string _Prefix = "Doctor_";
            fileName = string.Concat(_Prefix + DateTime.Now.ToString("ddMMyyyyHHmmssffff") + ".xlsx");
            string filePath = Path.Combine(hostingEnvironment.WebRootPath, "Downloads", fileName);

            var doctorsList = await (from d in _appDbContext.Doctor
                               join c in _appDbContext.ContactResourse
                               on d.Id equals c.RefTableId
                               join cs in _appDbContext.ChemistStockistResourse
                               on d.Id equals cs.RefTableId
                               join a in _appDbContext.AuditableEntity
                               on d.Id equals a.RefTableId
                               where c.RefTableName.Equals(ReferenceTableNames.DOCTOR)
                               select new DoctorViewModel
                               {
                                   ID = Convert.ToString(d.Id),
                                   Name = d.Name,
                                   Qualification = d.Qualification,
                                   RegistrationNo = d.RegistrationNo,
                                   Speciality = d.Speciality,
                                   Gender = d.Gender,
                                   VisitFrequency = d.VisitFrequency,
                                   VisitPlan = d.VisitPlan,
                                   BestDayToMeet = d.BestDayToMeet,
                                   BestTimeToMeet = d.BestTimeToMeet,
                                   Class = d.Class,
                                   Brand = d.Brand,
                                   BrandName = (from b in _appDbContext.MasterKeyValue
                                                where d.Brand.ToString().Contains(b.Id.ToString()) && b.Type.Equals(DoctorConstant.Brand)
                                                select b.Value).ToList(),
                                   Contact = new ContactResourseViewModel
                                   {
                                       ID = c.Id,
                                       RefTableId = c.RefTableId,
                                       RefTableName = c.RefTableName,
                                       Address = c.Address,
                                       State = c.State,
                                       City = c.City,
                                       PinCode = c.PinCode,
                                       MobileNumber = c.MobileNumber,
                                       ResidenceNumber = c.ResidenceNumber
                                   },
                                   Common = new CommonResourceViewModel
                                   {
                                       ID = cs.Id,
                                       ContactPersonMobileNumber = cs.ContactPersonMobileNumber,
                                       ContactPersonDateOfBirth = cs.ContactPersonDateOfBirth,
                                       ContactPersonDateOfAnniversary = cs.ContactPersonDateOfAnniversary,
                                       RefTableId = Convert.ToString(cs.RefTableId),
                                       RefTableName = cs.RefTableName
                                   },
                                   AuditableEntity = new AuditableEntityViewModel
                                   {
                                       ID = a.Id,
                                       RefTableId = a.RefTableId,
                                       RefTableName = a.RefTableName,
                                       FoundationDay = a.FoundationDay,
                                       CommunityID = (int)a.CommunityId,
                                       CommunityName = (from c in _appDbContext.MasterKeyValue
                                                        where c.Id == a.CommunityId && c.Type.Equals(EmployeeConstant.Community)
                                                        select c.Value).FirstOrDefault(),
                                       IsActive = Convert.ToBoolean(a.IsActive),
                                       CreateDate = a.CreateDate,
                                       CreatedBy = a.CreatedBy
                                   }
                               })
                             .ToListAsync().ConfigureAwait(false);

            doctorsList.OrderBy(n => n.Name);

            await GenerateXLS(doctorsList, filePath);

            ExportExcel exportExcel = new ExportExcel()
            {
                FilePath = filePath,
                FileName = fileName
            };

            return await Task.FromResult(exportExcel);
        }

        public async Task<ExportExcel> ExportDoctorsDataByUserAsync(IHostingEnvironment hostingEnvironment, string userName)
        {
            string fileName = "";
            string _Prefix = "Doctor_";
            fileName = string.Concat(_Prefix + DateTime.Now.ToString("ddMMyyyyHHmmssffff") + ".xlsx");
            string filePath = Path.Combine(hostingEnvironment.WebRootPath, "Downloads", fileName);

            var doctorsList = await (from d in _appDbContext.Doctor
                               join c in _appDbContext.ContactResourse
                               on d.Id equals c.RefTableId
                               join cs in _appDbContext.ChemistStockistResourse
                               on d.Id equals cs.RefTableId
                               join a in _appDbContext.AuditableEntity
                               on d.Id equals a.RefTableId
                               where c.RefTableName.Equals(ReferenceTableNames.DOCTOR) && a.CreatedBy.Equals(userName)
                               select new DoctorViewModel
                               {
                                   ID = Convert.ToString(d.Id),
                                   Name = d.Name,
                                   Qualification = d.Qualification,
                                   RegistrationNo = d.RegistrationNo,
                                   Speciality = d.Speciality,
                                   Gender = d.Gender,
                                   VisitFrequency = d.VisitFrequency,
                                   VisitPlan = d.VisitPlan,
                                   BestDayToMeet = d.BestDayToMeet,
                                   BestTimeToMeet = d.BestTimeToMeet,
                                   Class = d.Class,
                                   Brand = d.Brand,
                                   BrandName = (from b in _appDbContext.MasterKeyValue
                                                where d.Brand.ToString().Contains(b.Id.ToString()) && b.Type.Equals(DoctorConstant.Brand)
                                                select b.Value).ToList(),
                                   Contact = new ContactResourseViewModel
                                   {
                                       ID = c.Id,
                                       RefTableId = c.RefTableId,
                                       RefTableName = c.RefTableName,
                                       Address = c.Address,
                                       State = c.State,
                                       City = c.City,
                                       PinCode = c.PinCode,
                                       MobileNumber = c.MobileNumber,
                                       ResidenceNumber = c.ResidenceNumber
                                   },
                                   Common = new CommonResourceViewModel
                                   {
                                       ID = cs.Id,
                                       ContactPersonMobileNumber = cs.ContactPersonMobileNumber,
                                       ContactPersonDateOfBirth = cs.ContactPersonDateOfBirth,
                                       ContactPersonDateOfAnniversary = cs.ContactPersonDateOfAnniversary,
                                       RefTableId = Convert.ToString(cs.RefTableId),
                                       RefTableName = cs.RefTableName
                                   },
                                   AuditableEntity = new AuditableEntityViewModel
                                   {
                                       ID = a.Id,
                                       RefTableId = a.RefTableId,
                                       RefTableName = a.RefTableName,
                                       FoundationDay = a.FoundationDay,
                                       CommunityID = (int)a.CommunityId,
                                       CommunityName = (from c in _appDbContext.MasterKeyValue
                                                        where c.Id == a.CommunityId && c.Type.Equals(EmployeeConstant.Community)
                                                        select c.Value).FirstOrDefault(),
                                       IsActive = Convert.ToBoolean(a.IsActive),
                                       CreateDate = a.CreateDate,
                                       CreatedBy = a.CreatedBy
                                   }
                               })
                             .ToListAsync().ConfigureAwait(false);

            doctorsList.OrderBy(n => n.Name);

            await GenerateXLS(doctorsList, filePath);

            ExportExcel exportExcel = new ExportExcel()
            {
                FilePath = filePath,
                FileName = fileName
            };

            return await Task.FromResult(exportExcel);
        }

        public static Task GenerateXLS(List<DoctorViewModel> datasource, string filePath)
        {
            return Task.Run(() =>
            {
                using (ExcelPackage pck = new ExcelPackage())
                {
                    //Create the worksheet 
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Doctor");

                    ws.Cells[1, 1].Value = "Sr No";
                    ws.Cells[1, 1].Style.Font.Bold = true;

                    ws.Cells[1, 2].Value = "Name";
                    ws.Cells[1, 2].Style.Font.Bold = true;

                    ws.Cells[1, 3].Value = "Qualification";
                    ws.Cells[1, 3].Style.Font.Bold = true;

                    ws.Cells[1, 4].Value = "Registration No";
                    ws.Cells[1, 4].Style.Font.Bold = true;

                    ws.Cells[1, 5].Value = "Speciality";
                    ws.Cells[1, 5].Style.Font.Bold = true;

                    ws.Cells[1, 6].Value = "Gender";
                    ws.Cells[1, 6].Style.Font.Bold = true;

                    ws.Cells[1, 7].Value = "Visit Frequency";
                    ws.Cells[1, 7].Style.Font.Bold = true;

                    ws.Cells[1, 8].Value = "Visit Plan";
                    ws.Cells[1, 8].Style.Font.Bold = true;

                    ws.Cells[1, 9].Value = "Best Day To Meet";
                    ws.Cells[1, 9].Style.Font.Bold = true;

                    ws.Cells[1, 10].Value = "Best Time To Meet";
                    ws.Cells[1, 10].Style.Font.Bold = true;

                    ws.Cells[1, 11].Value = "Brand";
                    ws.Cells[1, 11].Style.Font.Bold = true;

                    ws.Cells[1, 12].Value = "Class";
                    ws.Cells[1, 12].Style.Font.Bold = true;

                    ws.Cells[1, 13].Value = "Address";
                    ws.Cells[1, 13].Style.Font.Bold = true;

                    ws.Cells[1, 14].Value = "State";
                    ws.Cells[1, 14].Style.Font.Bold = true;

                    ws.Cells[1, 15].Value = "City";
                    ws.Cells[1, 15].Style.Font.Bold = true;

                    ws.Cells[1, 16].Value = "PinCode";
                    ws.Cells[1, 16].Style.Font.Bold = true;

                    ws.Cells[1, 17].Value = "EmailID";
                    ws.Cells[1, 17].Style.Font.Bold = true;

                    ws.Cells[1, 18].Value = "Mobile Number";
                    ws.Cells[1, 18].Style.Font.Bold = true;

                    ws.Cells[1, 19].Value = "Residence Number";
                    ws.Cells[1, 19].Style.Font.Bold = true;

                    ws.Cells[1, 20].Value = "Contact Person Mobile";
                    ws.Cells[1, 20].Style.Font.Bold = true;

                    ws.Cells[1, 21].Value = "Date Of Birth";
                    ws.Cells[1, 21].Style.Font.Bold = true;

                    ws.Cells[1, 22].Value = "Date Of Anniversary";
                    ws.Cells[1, 22].Style.Font.Bold = true;

                    ws.Cells[1, 23].Value = "Foundation Day";
                    ws.Cells[1, 23].Style.Font.Bold = true;

                    ws.Cells[1, 24].Value = "Community";
                    ws.Cells[1, 24].Style.Font.Bold = true;

                    ws.Cells[1, 25].Value = "Is Active";
                    ws.Cells[1, 25].Style.Font.Bold = true;

                    int iRowNum = 1;
                    for (int i = 0; i < datasource.Count(); i++)
                    {
                        ws.Cells[i + 2, 1].Value = iRowNum.ToString();
                        ws.Cells[i + 2, 2].Value = datasource.ElementAt(i).Name;
                        ws.Cells[i + 2, 3].Value = datasource.ElementAt(i).Qualification;
                        ws.Cells[i + 2, 4].Value = datasource.ElementAt(i).RegistrationNo;
                        ws.Cells[i + 2, 5].Value = datasource.ElementAt(i).Speciality;
                        ws.Cells[i + 2, 6].Value = datasource.ElementAt(i).Gender;
                        ws.Cells[i + 2, 7].Value = datasource.ElementAt(i).VisitFrequency;
                        ws.Cells[i + 2, 8].Value = datasource.ElementAt(i).VisitPlan;
                        ws.Cells[i + 2, 9].Value = datasource.ElementAt(i).BestDayToMeet;
                        ws.Cells[i + 2, 10].Value = datasource.ElementAt(i).BestTimeToMeet;
                        ws.Cells[i + 2, 11].Value = datasource.ElementAt(i).Brand;
                        ws.Cells[i + 2, 12].Value = datasource.ElementAt(i).Class;
                        ws.Cells[i + 2, 13].Value = datasource.ElementAt(i).Contact.Address;
                        ws.Cells[i + 2, 14].Value = datasource.ElementAt(i).Contact.State;
                        ws.Cells[i + 2, 15].Value = datasource.ElementAt(i).Contact.City;
                        ws.Cells[i + 2, 16].Value = datasource.ElementAt(i).Contact.PinCode;
                        ws.Cells[i + 2, 17].Value = datasource.ElementAt(i).Contact.EmailId;
                        ws.Cells[i + 2, 18].Value = datasource.ElementAt(i).Contact.MobileNumber;
                        ws.Cells[i + 2, 19].Value = datasource.ElementAt(i).Contact.ResidenceNumber;
                        ws.Cells[i + 2, 20].Value = datasource.ElementAt(i).Common.ContactPersonMobileNumber;

                        ws.Cells[i + 2, 21].Value = datasource.ElementAt(i).Common.ContactPersonDateOfBirth;
                        ws.Cells[i + 2, 21].Style.Numberformat.Format = "dd-MMM-yyyy";

                        ws.Cells[i + 2, 22].Value = datasource.ElementAt(i).Common.ContactPersonDateOfAnniversary;
                        ws.Cells[i + 2, 22].Style.Numberformat.Format = "dd-MMM-yyyy";

                        ws.Cells[i + 2, 23].Value = datasource.ElementAt(i).AuditableEntity.FoundationDay;
                        ws.Cells[i + 2, 23].Style.Numberformat.Format = "dd-MMM-yyyy";

                        ws.Cells[i + 2, 24].Value = datasource.ElementAt(i).AuditableEntity.CommunityName;
                        ws.Cells[i + 2, 25].Value = datasource.ElementAt(i).AuditableEntity.IsActive;
                        iRowNum++;
                    }

                    using (ExcelRange rng = ws.Cells["A1:A" + iRowNum])
                    {
                        rng.Style.Font.Bold = true;
                        rng.Style.Fill.PatternType = ExcelFillStyle.Solid;        //Set Pattern for the background to Solid 
                        rng.Style.Fill.BackgroundColor.SetColor(Color.DarkGray);  //Set color to DarkGray 
                        rng.Style.Font.Color.SetColor(Color.Black);
                    }

                    pck.SaveAs(new FileInfo(filePath));
                }
            });
        }

        
    }
}
