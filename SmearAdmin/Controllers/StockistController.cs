using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SmearAdmin.Data;
using SmearAdmin.Helpers;
using SmearAdmin.Persistence;
using SmearAdmin.ViewModels;
using System;
using System.Threading.Tasks;
using static SmearAdmin.Helpers.Constants;

namespace SmearAdmin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockistController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHostingEnvironment _host;

        public StockistController(IMapper mapper, IUnitOfWork UnitOfWork, IHostingEnvironment host)
        {
            this.mapper = mapper;
            _unitOfWork = UnitOfWork;
            _host = host;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<MasterKeyValueResult<MasterKeyValue>> GetMastersForStockist()
        {
            return await _unitOfWork.MasterKeyValues.GetMastersForStockistAsync();
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> AddStockist(StockistViewModel stockistVM)
        {
            try
            {
                //Add Stockist
                string stockistTableID = Guid.NewGuid().ToString();
                var stockist = mapper.Map<Stockist>(stockistVM);
                stockist.Id = stockistTableID;
                _unitOfWork.Stockist.Add(stockist);

                //Add Comman
                CommonResourceViewModel commonVM = stockistVM.Common;
                commonVM.ID = Guid.NewGuid().ToString();
                commonVM.RefTableId = stockistTableID;
                commonVM.ContactPersonDateOfBirth = Utilities.FormatDateTimeByZone(stockistVM.Common.ContactPersonDateOfBirth.Value);
                commonVM.ContactPersonDateOfAnniversary = Utilities.FormatDateTimeByZone(stockistVM.Common.ContactPersonDateOfAnniversary.Value);
                var stockCommon = mapper.Map<CommonResourceViewModel, Data.ChemistStockistResourse>(commonVM);
                _unitOfWork.CommonResourse.Add(stockCommon);

                //Add Contact
                ContactResourseViewModel contactVM = stockistVM.Contact;
                contactVM.ID = Guid.NewGuid().ToString();
                contactVM.RefTableId = stockistTableID;
                contactVM.Area = contactVM.Area;
                contactVM.EmailId = contactVM.EmailId;
                var stockContact = mapper.Map<ContactResourseViewModel, Data.ContactResourse>(contactVM);
                _unitOfWork.ContactResource.Add(stockContact);

                //Add Auditable Entity
                AuditableEntityViewModel auditVM = stockistVM.AuditableEntity;
                auditVM.ID = Guid.NewGuid().ToString();
                auditVM.RefTableId = stockistTableID;
                auditVM.CreateDate = DateTime.Now;
                if (stockistVM.AuditableEntity.FoundationDay != null)
                    auditVM.FoundationDay = Utilities.FormatDateTimeByZone(stockistVM.AuditableEntity.FoundationDay.Value);
                else
                    auditVM.FoundationDay = null;
                var stockAudit = mapper.Map<AuditableEntityViewModel, Data.AuditableEntity>(auditVM);
                _unitOfWork.AuditableEntity.Add(stockAudit);

                await _unitOfWork.CompleteAsync();
            }
            catch (Exception Ex)
            {
                return new BadRequestObjectResult(Errors.AddError(Ex, ModelState));
            }
            return new OkObjectResult(stockistVM);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetAllStockists(int pageIndex, int pageSize)
        {
            var results = await _unitOfWork.Stockist.GetAllStockistAsync(pageIndex, pageSize);
            return Ok(results);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetAllStockistsByUser(int pageIndex, int pageSize, string userName)
        {
            var results = await _unitOfWork.Stockist.GetAllStockistByUserAsync(pageIndex, pageSize, userName);
            return Ok(results);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetAllStockistsBySearch(int pageIndex, int pageSize, string searchValue)
        {
            var results = await _unitOfWork.Stockist.GetAllStockistBySearchAsync(pageIndex, pageSize, searchValue);
            return Ok(results);
        }

        [HttpPut]
        [Route("[action]")]
        public async Task<IActionResult> DeleteStockist(StockistViewModel stockistVM, string action)
        {
            try
            {
                var origionalData = _unitOfWork.AuditableEntity.GetSingleOrDefault(e => e.RefTableId == stockistVM.ID && e.RefTableName == ReferenceTableNames.STOCKIST);
                if (origionalData != null)
                {
                    if (action.Equals("delete"))
                        origionalData.IsActive = false;
                    else
                        origionalData.IsActive = true;

                    _unitOfWork.AuditableEntity.Update(origionalData);
                    await _unitOfWork.CompleteAsync();
                }
            }
            catch (Exception Ex)
            {
                return new BadRequestObjectResult(Errors.AddError(Ex, ModelState));
            }

            return Ok(stockistVM);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetStockist(string ID)
        {
            var results = await _unitOfWork.Stockist.GetStockistByIDAsync(ID);
            return Ok(results);
        }

        [HttpPut]
        [Route("[action]")]
        public async Task<IActionResult> EditStockist(StockistViewModel stockistVM)
        {
            string dtOld = string.Empty;
            string dtNew = string.Empty;
            try
            {
                //Update Stockist
                var stockistData = _unitOfWork.Stockist.GetSingleOrDefault(d => d.Id == stockistVM.ID);
                if (stockistData != null)
                {
                    stockistData.StockistName = stockistVM.StockistName;
                }
                _unitOfWork.Stockist.Update(stockistData);

                //Update Common
                var commonData = _unitOfWork.CommonResourse.GetSingleOrDefault(c => c.RefTableId == stockistVM.ID && c.RefTableName.Equals(ReferenceTableNames.STOCKIST));
                if (commonData != null)
                {
                    commonData.DrugLicenseNo = stockistVM.Common.DrugLicenseNo;
                    commonData.FoodLicenseNo = stockistVM.Common.FoodLicenseNo;
                    commonData.Gstno = stockistVM.Common.GSTNo;
                    commonData.BestTimeToMeet = stockistVM.Common.BestTimeToMeet;
                    commonData.ContactPersonName = stockistVM.Common.ContactPersonName;
                    commonData.ContactPersonMobileNumber = stockistVM.Common.ContactPersonMobileNumber;
                    commonData.ContactPersonDateOfBirth = Utilities.FormatDateTimeByZone(stockistVM.Common.ContactPersonDateOfBirth.Value);
                    commonData.ContactPersonDateOfAnniversary = Utilities.FormatDateTimeByZone(stockistVM.Common.ContactPersonDateOfAnniversary.Value);
                }

                _unitOfWork.CommonResourse.Update(commonData);

                //Update Contact
                var contactData = _unitOfWork.ContactResource.GetSingleOrDefault(c => c.RefTableId == stockistVM.ID && c.RefTableName.Equals(ReferenceTableNames.STOCKIST));
                if (contactData != null)
                {
                    contactData.Address = stockistVM.Contact.Address;
                    contactData.State = stockistVM.Contact.State;
                    contactData.City = stockistVM.Contact.City;
                    contactData.PinCode = stockistVM.Contact.PinCode;
                    contactData.MobileNumber = stockistVM.Contact.MobileNumber;
                    contactData.ResidenceNumber = stockistVM.Contact.ResidenceNumber;
                    contactData.Area = stockistVM.Contact.Area;
                    contactData.EmailId = stockistVM.Contact.EmailId;
                }
                _unitOfWork.ContactResource.Update(contactData);

                //Update Auditable Entity
                var auditableData = _unitOfWork.AuditableEntity.GetSingleOrDefault(c => c.RefTableId == stockistVM.ID && c.RefTableName.Equals(ReferenceTableNames.STOCKIST));
                if (auditableData != null)
                {
                    if (stockistVM.AuditableEntity.FoundationDay != null)
                        auditableData.FoundationDay = Utilities.FormatDateTimeByZone(stockistVM.AuditableEntity.FoundationDay.Value);
                    else
                        stockistVM.AuditableEntity.FoundationDay = null;
                    auditableData.CommunityId = stockistVM.AuditableEntity.CommunityID;
                    auditableData.CreateDate = DateTime.Now;
                    auditableData.CreatedBy = stockistVM.AuditableEntity.CreatedBy;
                }
                _unitOfWork.AuditableEntity.Update(auditableData);

                await _unitOfWork.CompleteAsync();
            }
            catch (Exception Ex)
            {
                return new BadRequestObjectResult(Errors.AddError(Ex, ModelState));
            }
            return new OkObjectResult(stockistVM);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> ExportStockistsData()
        {
            var results = await _unitOfWork.Stockist.ExportStockistsDataAsync(_host);
            return Ok(results);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> ExportStockistsDataByUser(string userName)
        {
            var results = await _unitOfWork.Stockist.ExportStockistsDataByUserAsync(_host, userName);
            return Ok(results);
        }
    }
}