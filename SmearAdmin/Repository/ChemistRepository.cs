using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using SmearAdmin.Data;
using SmearAdmin.Helpers;
using SmearAdmin.Interface;
using SmearAdmin.Persistence;
using SmearAdmin.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static SmearAdmin.Helpers.Constants;

namespace SmearAdmin.Repository
{
    public class ChemistRepository : Repositories<Chemist>, IChemistRepository
    {
        public ChemistRepository(SmearAdminDbContext context) : base(context) { }

        private SmearAdminDbContext _appDbContext => (SmearAdminDbContext)_context;

        public async Task<PagingResult<ChemistViewModel>> GetAllChemistAsync(int pageIndex, int pageSize)
        {
            int totalCount = 0, totalPage = 0;
            pageIndex += 1;

            totalCount = _appDbContext.Chemist.Count();
            totalPage = (totalCount / pageSize) + ((totalCount % pageSize) > 0 ? 1 : 0);

            var dataUsers = await (from d in _appDbContext.Chemist
                             join c in _appDbContext.ContactResourse
                             on d.Id equals c.RefTableId
                             join cs in _appDbContext.ChemistStockistResourse
                             on d.Id equals cs.RefTableId
                             join a in _appDbContext.AuditableEntity
                             on d.Id equals a.RefTableId
                             where c.RefTableName.Equals(ReferenceTableNames.CHEMIST)
                             select new ChemistViewModel
                             {
                                 ID = d.Id,
                                 MedicalName = d.MedicalName,
                                 Class = d.Class,
                                 Contact = new ContactResourseViewModel
                                 {
                                     ID = c.Id,
                                     RefTableId = c.RefTableId,
                                     RefTableName = c.RefTableName,
                                     Address = c.Address,
                                     State = c.State,
                                     City = c.City,
                                     PinCode = c.PinCode,
                                     Area = c.Area,
                                     EmailId = c.EmailId,
                                     MobileNumber = c.MobileNumber,
                                     ResidenceNumber = c.ResidenceNumber
                                 },
                                 Common = new CommonResourceViewModel
                                 {
                                     ID = cs.Id,
                                     DrugLicenseNo = cs.DrugLicenseNo,
                                     FoodLicenseNo = cs.FoodLicenseNo,
                                     GSTNo = cs.Gstno,
                                     BestTimeToMeet = cs.BestTimeToMeet,
                                     ContactPersonName = cs.ContactPersonName,
                                     ContactPersonMobileNumber = cs.ContactPersonMobileNumber,
                                     ContactPersonDateOfBirth = cs.ContactPersonDateOfBirth,
                                     ContactPersonDateOfAnniversary = cs.ContactPersonDateOfAnniversary,
                                     RefTableId = cs.RefTableId,
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

            var pagingData = new PagingResult<ChemistViewModel>
            {
                Items = dataUsers.AsEnumerable().OrderBy(f => f.MedicalName),
                TotalCount = totalCount,
                TotalPage = totalPage
            };

            return await Task.FromResult(pagingData);
        }

        public async Task<PagingResult<ChemistViewModel>> GetAllChemistByUserAsync(int pageIndex, int pageSize, string userName)
        {
            int totalCount = 0, totalPage = 0;
            pageIndex += 1;

            //totalCount = _appDbContext.Chemist.Count();
            totalCount = await (from d in _appDbContext.Chemist
                                join c in _appDbContext.ContactResourse
                                on d.Id equals c.RefTableId
                                join a in _appDbContext.AuditableEntity
                                on d.Id equals a.RefTableId
                                where c.RefTableName.Equals(ReferenceTableNames.CHEMIST) && a.CreatedBy.Equals(userName)
                                select d).CountAsync().ConfigureAwait(false);

            totalPage = (totalCount / pageSize) + ((totalCount % pageSize) > 0 ? 1 : 0);

            var dataUsers = await (from d in _appDbContext.Chemist
                             join c in _appDbContext.ContactResourse
                             on d.Id equals c.RefTableId
                             join cs in _appDbContext.ChemistStockistResourse
                             on d.Id equals cs.RefTableId
                             join a in _appDbContext.AuditableEntity
                             on d.Id equals a.RefTableId
                             where c.RefTableName.Equals(ReferenceTableNames.CHEMIST) && a.CreatedBy.Equals(userName)
                             select new ChemistViewModel
                             {
                                 ID = d.Id,
                                 MedicalName = d.MedicalName,
                                 Class = d.Class,
                                 Contact = new ContactResourseViewModel
                                 {
                                     ID = c.Id,
                                     RefTableId = c.RefTableId,
                                     RefTableName = c.RefTableName,
                                     Address = c.Address,
                                     State = c.State,
                                     City = c.City,
                                     PinCode = c.PinCode,
                                     Area = c.Area,
                                     EmailId = c.EmailId,
                                     MobileNumber = c.MobileNumber,
                                     ResidenceNumber = c.ResidenceNumber
                                 },
                                 Common = new CommonResourceViewModel
                                 {
                                     ID = cs.Id,
                                     DrugLicenseNo = cs.DrugLicenseNo,
                                     FoodLicenseNo = cs.FoodLicenseNo,
                                     GSTNo = cs.Gstno,
                                     BestTimeToMeet = cs.BestTimeToMeet,
                                     ContactPersonName = cs.ContactPersonName,
                                     ContactPersonMobileNumber = cs.ContactPersonMobileNumber,
                                     ContactPersonDateOfBirth = cs.ContactPersonDateOfBirth,
                                     ContactPersonDateOfAnniversary = cs.ContactPersonDateOfAnniversary,
                                     RefTableId = cs.RefTableId,
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

            var pagingData = new PagingResult<ChemistViewModel>
            {
                Items = dataUsers.AsEnumerable().OrderBy(f => f.MedicalName),
                TotalCount = totalCount,
                TotalPage = totalPage
            };

            return await Task.FromResult(pagingData);
        }
        
        public async Task<PagingResult<ChemistViewModel>> GetAllChemistBySearchAsync(int pageIndex, int pageSize, string searchValue)
        {
            int totalCount = 0, totalPage = 0;
            pageIndex += 1;

            //totalCount = _appDbContext.Chemist.Count();
            totalCount = await (from d in _appDbContext.Chemist
                                join c in _appDbContext.ContactResourse
                                on d.Id equals c.RefTableId
                                join a in _appDbContext.AuditableEntity
                                on d.Id equals a.RefTableId
                                where c.RefTableName.Equals(ReferenceTableNames.CHEMIST) && d.MedicalName.Contains(searchValue)
                                select d).CountAsync().ConfigureAwait(false);

            totalPage = (totalCount / pageSize) + ((totalCount % pageSize) > 0 ? 1 : 0);

            var dataUsers = await (from d in _appDbContext.Chemist
                                   join c in _appDbContext.ContactResourse
                                   on d.Id equals c.RefTableId
                                   join cs in _appDbContext.ChemistStockistResourse
                                   on d.Id equals cs.RefTableId
                                   join a in _appDbContext.AuditableEntity
                                   on d.Id equals a.RefTableId
                                   where c.RefTableName.Equals(ReferenceTableNames.CHEMIST) && d.MedicalName.Contains(searchValue)
                                   select new ChemistViewModel
                                   {
                                       ID = d.Id,
                                       MedicalName = d.MedicalName,
                                       Class = d.Class,
                                       Contact = new ContactResourseViewModel
                                       {
                                           ID = c.Id,
                                           RefTableId = c.RefTableId,
                                           RefTableName = c.RefTableName,
                                           Address = c.Address,
                                           State = c.State,
                                           City = c.City,
                                           PinCode = c.PinCode,
                                           Area = c.Area,
                                           EmailId = c.EmailId,
                                           MobileNumber = c.MobileNumber,
                                           ResidenceNumber = c.ResidenceNumber
                                       },
                                       Common = new CommonResourceViewModel
                                       {
                                           ID = cs.Id,
                                           DrugLicenseNo = cs.DrugLicenseNo,
                                           FoodLicenseNo = cs.FoodLicenseNo,
                                           GSTNo = cs.Gstno,
                                           BestTimeToMeet = cs.BestTimeToMeet,
                                           ContactPersonName = cs.ContactPersonName,
                                           ContactPersonMobileNumber = cs.ContactPersonMobileNumber,
                                           ContactPersonDateOfBirth = cs.ContactPersonDateOfBirth,
                                           ContactPersonDateOfAnniversary = cs.ContactPersonDateOfAnniversary,
                                           RefTableId = cs.RefTableId,
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

            var pagingData = new PagingResult<ChemistViewModel>
            {
                Items = dataUsers.AsEnumerable().OrderBy(f => f.MedicalName),
                TotalCount = totalCount,
                TotalPage = totalPage
            };

            return await Task.FromResult(pagingData);
        }
        
        public async Task<ChemistViewModel> GetChemistByIDAsync(string ID)
        {
            var dataChemist = await (from d in _appDbContext.Chemist
                                    join c in _appDbContext.ContactResourse
                                    on d.Id equals c.RefTableId
                                    join cs in _appDbContext.ChemistStockistResourse
                                    on d.Id equals cs.RefTableId
                                    join a in _appDbContext.AuditableEntity
                                    on d.Id equals a.RefTableId
                                    where c.RefTableName.Equals(ReferenceTableNames.CHEMIST)
                                    select new ChemistViewModel
                                    {
                                        ID = d.Id,
                                        MedicalName = d.MedicalName,
                                        Class = d.Class,
                                        Contact = new ContactResourseViewModel
                                        {
                                            ID = c.Id,
                                            RefTableId = c.RefTableId,
                                            RefTableName = c.RefTableName,
                                            Address = c.Address,
                                            State = c.State,
                                            City = c.City,
                                            PinCode = c.PinCode,
                                            Area = c.Area,
                                            EmailId = c.EmailId,
                                            MobileNumber = c.MobileNumber,
                                            ResidenceNumber = c.ResidenceNumber
                                        },
                                        Common = new CommonResourceViewModel
                                        {
                                            ID = cs.Id,
                                            DrugLicenseNo = cs.DrugLicenseNo,
                                            FoodLicenseNo = cs.FoodLicenseNo,
                                            GSTNo = cs.Gstno,
                                            BestTimeToMeet = cs.BestTimeToMeet,                                            
                                            ContactPersonName = cs.ContactPersonName,
                                            ContactPersonMobileNumber = cs.ContactPersonMobileNumber,
                                            ContactPersonDateOfBirth = cs.ContactPersonDateOfBirth,
                                            ContactPersonDateOfAnniversary = cs.ContactPersonDateOfAnniversary,
                                            RefTableId = cs.RefTableId,
                                            RefTableName = cs.RefTableName
                                        },
                                        AuditableEntity = new AuditableEntityViewModel
                                        {
                                            ID = a.Id,
                                            RefTableId = a.RefTableId,
                                            RefTableName = a.RefTableName,
                                            FoundationDay = a.FoundationDay,
                                            CommunityID = (int)a.CommunityId,
                                            IsActive = Convert.ToBoolean(a.IsActive),
                                            CreateDate = a.CreateDate,
                                            CreatedBy = a.CreatedBy
                                        }
                                    })
                             .Where(d => d.ID.Equals(Convert.ToString(ID))).FirstOrDefaultAsync().ConfigureAwait(false);

            return dataChemist;
        }

        public async Task<ExportExcel> ExportChemistsDataAsync(IHostingEnvironment hostingEnvironment)
        {
            string _Prefix = "Chemist_";
            string fileName = string.Concat(_Prefix + DateTime.Now.ToString("ddMMyyyyHHmmssffff") + ".xlsx");
            string filePath = Path.Combine(hostingEnvironment.WebRootPath, "Downloads", fileName);

            var chemistList = await (from d in _appDbContext.Chemist
                               join c in _appDbContext.ContactResourse
                               on d.Id equals c.RefTableId
                               join cs in _appDbContext.ChemistStockistResourse
                               on d.Id equals cs.RefTableId
                               join a in _appDbContext.AuditableEntity
                               on d.Id equals a.RefTableId
                               where c.RefTableName.Equals(ReferenceTableNames.CHEMIST)
                               select new ChemistViewModel
                               {
                                   ID = d.Id,
                                   MedicalName = d.MedicalName,
                                   Class = d.Class,
                                   Contact = new ContactResourseViewModel
                                   {
                                       ID = c.Id,
                                       RefTableId = c.RefTableId,
                                       RefTableName = c.RefTableName,
                                       Address = c.Address,
                                       State = c.State,
                                       City = c.City,
                                       PinCode = c.PinCode,
                                       Area = c.Area,
                                       EmailId = c.EmailId,
                                       MobileNumber = c.MobileNumber,
                                       ResidenceNumber = c.ResidenceNumber
                                   },
                                   Common = new CommonResourceViewModel
                                   {
                                       ID = cs.Id,
                                       DrugLicenseNo = cs.DrugLicenseNo,
                                       FoodLicenseNo = cs.FoodLicenseNo,
                                       GSTNo = cs.Gstno,
                                       BestTimeToMeet = cs.BestTimeToMeet,
                                       ContactPersonName = cs.ContactPersonName,
                                       ContactPersonMobileNumber = cs.ContactPersonMobileNumber,
                                       ContactPersonDateOfBirth = cs.ContactPersonDateOfBirth,
                                       ContactPersonDateOfAnniversary = cs.ContactPersonDateOfAnniversary,
                                       RefTableId = cs.RefTableId,
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

            chemistList.OrderBy(n => n.MedicalName);

            await GenerateXLS(chemistList, filePath);

            ExportExcel exportExcel = new ExportExcel()
            {
                FilePath = filePath,
                FileName = fileName
            };

            return await Task.FromResult(exportExcel);
        }

        public async Task<ExportExcel> ExportChemistsDataByUserAsync(IHostingEnvironment hostingEnvironment, string userName)
        {
            string _Prefix = "Chemist_";
            string fileName = string.Concat(_Prefix + DateTime.Now.ToString("ddMMyyyyHHmmssffff") + ".xlsx");
            string filePath = Path.Combine(hostingEnvironment.WebRootPath, "Downloads", fileName);

            var chemistList = await (from d in _appDbContext.Chemist
                               join c in _appDbContext.ContactResourse
                               on d.Id equals c.RefTableId
                               join cs in _appDbContext.ChemistStockistResourse
                               on d.Id equals cs.RefTableId
                               join a in _appDbContext.AuditableEntity
                               on d.Id equals a.RefTableId
                               where c.RefTableName.Equals(ReferenceTableNames.CHEMIST) && a.CreatedBy.Equals(userName)
                               select new ChemistViewModel
                               {
                                   ID = d.Id,
                                   MedicalName = d.MedicalName,
                                   Class = d.Class,
                                   Contact = new ContactResourseViewModel
                                   {
                                       ID = c.Id,
                                       RefTableId = c.RefTableId,
                                       RefTableName = c.RefTableName,
                                       Address = c.Address,
                                       State = c.State,
                                       City = c.City,
                                       PinCode = c.PinCode,
                                       Area = c.Area,
                                       EmailId = c.EmailId,
                                       MobileNumber = c.MobileNumber,
                                       ResidenceNumber = c.ResidenceNumber
                                   },
                                   Common = new CommonResourceViewModel
                                   {
                                       ID = cs.Id,
                                       DrugLicenseNo = cs.DrugLicenseNo,
                                       FoodLicenseNo = cs.FoodLicenseNo,
                                       GSTNo = cs.Gstno,
                                       BestTimeToMeet = cs.BestTimeToMeet,
                                       ContactPersonName = cs.ContactPersonName,
                                       ContactPersonMobileNumber = cs.ContactPersonMobileNumber,
                                       ContactPersonDateOfBirth = cs.ContactPersonDateOfBirth,
                                       ContactPersonDateOfAnniversary = cs.ContactPersonDateOfAnniversary,
                                       RefTableId = cs.RefTableId,
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

            chemistList.OrderBy(n => n.MedicalName);

            await GenerateXLS(chemistList, filePath);

            ExportExcel exportExcel = new ExportExcel()
            {
                FilePath = filePath,
                FileName = fileName
            };

            return await Task.FromResult(exportExcel);
        }

        public static Task GenerateXLS(List<ChemistViewModel> datasource, string filePath)
        {
            return Task.Run(() =>
            {
                using (ExcelPackage pck = new ExcelPackage())
                {
                    //Create the worksheet 
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Chemists");

                    ws.Cells[1, 1].Value = "Sr No";
                    ws.Cells[1, 1].Style.Font.Bold = true;

                    ws.Cells[1, 2].Value = "Medical Name";
                    ws.Cells[1, 2].Style.Font.Bold = true;

                    ws.Cells[1, 3].Value = "Class";
                    ws.Cells[1, 3].Style.Font.Bold = true;

                    ws.Cells[1, 4].Value = "Address";
                    ws.Cells[1, 4].Style.Font.Bold = true;

                    ws.Cells[1, 5].Value = "State";
                    ws.Cells[1, 5].Style.Font.Bold = true;

                    ws.Cells[1, 6].Value = "City";
                    ws.Cells[1, 6].Style.Font.Bold = true;

                    ws.Cells[1, 7].Value = "PinCode";
                    ws.Cells[1, 7].Style.Font.Bold = true;

                    ws.Cells[1, 8].Value = "Area";
                    ws.Cells[1, 8].Style.Font.Bold = true;

                    ws.Cells[1, 9].Value = "EmailID";
                    ws.Cells[1, 9].Style.Font.Bold = true;

                    ws.Cells[1, 10].Value = "Mobile Number";
                    ws.Cells[1, 10].Style.Font.Bold = true;

                    ws.Cells[1, 11].Value = "Residence Number";
                    ws.Cells[1, 11].Style.Font.Bold = true;

                    ws.Cells[1, 12].Value = "Drug License No";
                    ws.Cells[1, 12].Style.Font.Bold = true;

                    ws.Cells[1, 13].Value = "Food License No";
                    ws.Cells[1, 13].Style.Font.Bold = true;

                    ws.Cells[1, 14].Value = "GST No";
                    ws.Cells[1, 14].Style.Font.Bold = true;

                    ws.Cells[1, 15].Value = "Best Time To Meet";
                    ws.Cells[1, 15].Style.Font.Bold = true;

                    ws.Cells[1, 16].Value = "Contact Person Name";
                    ws.Cells[1, 16].Style.Font.Bold = true;

                    ws.Cells[1, 17].Value = "Contact Person Mobile";
                    ws.Cells[1, 17].Style.Font.Bold = true;

                    ws.Cells[1, 18].Value = "Date Of Birth";
                    ws.Cells[1, 18].Style.Font.Bold = true;

                    ws.Cells[1, 19].Value = "Date Of Anniversary";
                    ws.Cells[1, 19].Style.Font.Bold = true;

                    ws.Cells[1, 20].Value = "Foundation Day";
                    ws.Cells[1, 20].Style.Font.Bold = true;

                    ws.Cells[1, 21].Value = "Community";
                    ws.Cells[1, 21].Style.Font.Bold = true;

                    ws.Cells[1, 22].Value = "Is Active";
                    ws.Cells[1, 22].Style.Font.Bold = true;

                    int iRowNum = 1;
                    for (int i = 0; i < datasource.Count(); i++)
                    {
                        ws.Cells[i + 2, 1].Value = iRowNum.ToString();
                        ws.Cells[i + 2, 2].Value = datasource.ElementAt(i).MedicalName;
                        ws.Cells[i + 2, 3].Value = datasource.ElementAt(i).Class;
                        ws.Cells[i + 2, 4].Value = datasource.ElementAt(i).Contact.Address;
                        ws.Cells[i + 2, 5].Value = datasource.ElementAt(i).Contact.State;
                        ws.Cells[i + 2, 6].Value = datasource.ElementAt(i).Contact.City;
                        ws.Cells[i + 2, 7].Value = datasource.ElementAt(i).Contact.PinCode;
                        ws.Cells[i + 2, 8].Value = datasource.ElementAt(i).Contact.Area;
                        ws.Cells[i + 2, 9].Value = datasource.ElementAt(i).Contact.EmailId;
                        ws.Cells[i + 2, 10].Value = datasource.ElementAt(i).Contact.MobileNumber;
                        ws.Cells[i + 2, 11].Value = datasource.ElementAt(i).Contact.ResidenceNumber;
                        ws.Cells[i + 2, 12].Value = datasource.ElementAt(i).Common.DrugLicenseNo;
                        ws.Cells[i + 2, 13].Value = datasource.ElementAt(i).Common.FoodLicenseNo;
                        ws.Cells[i + 2, 14].Value = datasource.ElementAt(i).Common.GSTNo;
                        ws.Cells[i + 2, 15].Value = datasource.ElementAt(i).Common.BestTimeToMeet;
                        ws.Cells[i + 2, 16].Value = datasource.ElementAt(i).Common.ContactPersonName;
                        ws.Cells[i + 2, 17].Value = datasource.ElementAt(i).Common.ContactPersonMobileNumber;
                        
                        ws.Cells[i + 2, 18].Value = datasource.ElementAt(i).Common.ContactPersonDateOfBirth;
                        ws.Cells[i + 2, 18].Style.Numberformat.Format = "dd-MMM-yyyy";

                        ws.Cells[i + 2, 19].Value = datasource.ElementAt(i).Common.ContactPersonDateOfAnniversary;
                        ws.Cells[i + 2, 19].Style.Numberformat.Format = "dd-MMM-yyyy";

                        ws.Cells[i + 2, 20].Value = datasource.ElementAt(i).AuditableEntity.FoundationDay;
                        ws.Cells[i + 2, 20].Style.Numberformat.Format = "dd-MMM-yyyy";

                        ws.Cells[i + 2, 21].Value = datasource.ElementAt(i).AuditableEntity.CommunityName;
                        ws.Cells[i + 2, 22].Value = datasource.ElementAt(i).AuditableEntity.IsActive;
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
