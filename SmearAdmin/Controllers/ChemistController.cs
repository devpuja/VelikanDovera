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
    public class ChemistController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHostingEnvironment _host;

        public ChemistController(IMapper mapper, IUnitOfWork UnitOfWork, IHostingEnvironment host)
        {
            this.mapper = mapper;
            _unitOfWork = UnitOfWork;
            _host = host;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<MasterKeyValueResult<MasterKeyValue>> GetMastersForChemist()
        {
            return await _unitOfWork.MasterKeyValues.GetMastersForChemistAsync();
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> AddChemist(ChemistViewModel chemistVM)
        {
            try
            {
                //Add Chemist
                string chemistTableID = Guid.NewGuid().ToString();
                var chemist = mapper.Map<Chemist>(chemistVM);
                chemist.Id = chemistTableID;
                _unitOfWork.Chemist.Add(chemist);

                //Add Comman
                CommonResourceViewModel commonVM = chemistVM.Common;
                commonVM.ID = Guid.NewGuid().ToString();
                commonVM.RefTableId = chemistTableID;
                commonVM.ContactPersonDateOfBirth = Utilities.FormatDateTimeByZone(chemistVM.Common.ContactPersonDateOfBirth.Value);
                commonVM.ContactPersonDateOfAnniversary = Utilities.FormatDateTimeByZone(chemistVM.Common.ContactPersonDateOfAnniversary.Value);
                var chemCommon = mapper.Map<CommonResourceViewModel, Data.ChemistStockistResourse>(commonVM);
                _unitOfWork.CommonResourse.Add(chemCommon);

                //Add Contact
                ContactResourseViewModel contactVM = chemistVM.Contact;
                contactVM.ID = Guid.NewGuid().ToString();
                contactVM.RefTableId = chemistTableID;
                contactVM.Area = contactVM.Area;
                contactVM.EmailId = contactVM.EmailId;                
                var chemContact = mapper.Map<ContactResourseViewModel, Data.ContactResourse>(contactVM);
                _unitOfWork.ContactResource.Add(chemContact);

                //Add Auditable Entity
                AuditableEntityViewModel auditVM = chemistVM.AuditableEntity;
                auditVM.ID= Guid.NewGuid().ToString();
                auditVM.RefTableId = chemistTableID;
                auditVM.CreateDate = DateTime.Now;
                if (chemistVM.AuditableEntity.FoundationDay != null)
                    auditVM.FoundationDay = Utilities.FormatDateTimeByZone(chemistVM.AuditableEntity.FoundationDay.Value);
                else
                    auditVM.FoundationDay = null;
                var chemAudit = mapper.Map<AuditableEntityViewModel, Data.AuditableEntity>(auditVM);
                _unitOfWork.AuditableEntity.Add(chemAudit);

                await _unitOfWork.CompleteAsync();
            }
            catch (Exception Ex)
            {
                return new BadRequestObjectResult(Errors.AddError(Ex, ModelState));
            }
            return new OkObjectResult(chemistVM);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetAllChemists(int pageIndex, int pageSize)
        {
            var results = await _unitOfWork.Chemist.GetAllChemistAsync(pageIndex, pageSize);
            return Ok(results);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetAllChemistsByUser(int pageIndex, int pageSize, string userName)
        {
            var results = await _unitOfWork.Chemist.GetAllChemistByUserAsync(pageIndex, pageSize, userName);
            return Ok(results);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetAllChemistsBySearch(int pageIndex, int pageSize, string searchValue)
        {
            var results = await _unitOfWork.Chemist.GetAllChemistBySearchAsync(pageIndex, pageSize, searchValue);
            return Ok(results);
        }

        [HttpPut]
        [Route("[action]")]
        public async Task<IActionResult> DeleteChemist(ChemistViewModel chemistVM, string action)
        {
            try
            {
                var origionalData = _unitOfWork.AuditableEntity.GetSingleOrDefault(e => e.RefTableId == chemistVM.ID && e.RefTableName == ReferenceTableNames.CHEMIST);
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

            return Ok(chemistVM);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetChemist(string ID)
        {
            var results = await _unitOfWork.Chemist.GetChemistByIDAsync(ID);
            return Ok(results);
        }

        [HttpPut]
        [Route("[action]")]
        public async Task<IActionResult> EditChemist(ChemistViewModel chemistVM)
        {
            try
            {
                //Update Chemist
                var chemistData = _unitOfWork.Chemist.GetSingleOrDefault(d => d.Id == chemistVM.ID);
                if (chemistData != null)
                {
                    chemistData.MedicalName = chemistVM.MedicalName;
                    chemistData.Class = chemistVM.Class;
                }
                _unitOfWork.Chemist.Update(chemistData);

                //Update Common
                var commonData = _unitOfWork.CommonResourse.GetSingleOrDefault(c => c.RefTableId == chemistVM.ID && c.RefTableName.Equals(ReferenceTableNames.CHEMIST));
                if (commonData != null)
                {
                    commonData.DrugLicenseNo = chemistVM.Common.DrugLicenseNo;
                    commonData.FoodLicenseNo = chemistVM.Common.FoodLicenseNo;
                    commonData.Gstno = chemistVM.Common.GSTNo;
                    commonData.BestTimeToMeet = chemistVM.Common.BestTimeToMeet;
                    commonData.ContactPersonName = chemistVM.Common.ContactPersonName;
                    commonData.ContactPersonMobileNumber = chemistVM.Common.ContactPersonMobileNumber;
                    commonData.ContactPersonDateOfBirth = Utilities.FormatDateTimeByZone(chemistVM.Common.ContactPersonDateOfBirth.Value);
                    commonData.ContactPersonDateOfAnniversary = Utilities.FormatDateTimeByZone(chemistVM.Common.ContactPersonDateOfAnniversary.Value);
                }
                _unitOfWork.CommonResourse.Update(commonData);

                //Update Contact
                var contactData = _unitOfWork.ContactResource.GetSingleOrDefault(c => c.RefTableId == chemistVM.ID && c.RefTableName.Equals(ReferenceTableNames.CHEMIST));
                if (contactData != null)
                {
                    contactData.Address = chemistVM.Contact.Address;
                    contactData.State = chemistVM.Contact.State;
                    contactData.City = chemistVM.Contact.City;
                    contactData.PinCode = chemistVM.Contact.PinCode;
                    contactData.MobileNumber = chemistVM.Contact.MobileNumber;
                    contactData.ResidenceNumber = chemistVM.Contact.ResidenceNumber;
                    contactData.Area = chemistVM.Contact.Area;
                    contactData.EmailId = chemistVM.Contact.EmailId;
                }
                _unitOfWork.ContactResource.Update(contactData);

                //Update Auditable Entity
                var auditableData = _unitOfWork.AuditableEntity.GetSingleOrDefault(c => c.RefTableId == chemistVM.ID && c.RefTableName.Equals(ReferenceTableNames.CHEMIST));
                if (auditableData != null)
                {
                    if (chemistVM.AuditableEntity.FoundationDay != null)
                        auditableData.FoundationDay = Utilities.FormatDateTimeByZone(chemistVM.AuditableEntity.FoundationDay.Value);
                    else
                        auditableData.FoundationDay = null;
                    auditableData.FoundationDay = chemistVM.AuditableEntity.FoundationDay?.AddDays(1);
                    auditableData.CommunityId = chemistVM.AuditableEntity.CommunityID;
                    auditableData.CreateDate = DateTime.Now;
                    auditableData.CreatedBy = chemistVM.AuditableEntity.CreatedBy;
                }
                _unitOfWork.AuditableEntity.Update(auditableData);

                await _unitOfWork.CompleteAsync();
            }
            catch (Exception Ex)
            {
                return new BadRequestObjectResult(Errors.AddError(Ex, ModelState));
            }
            return new OkObjectResult(chemistVM);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> ExportChemistsData()
        {
            var results = await _unitOfWork.Chemist.ExportChemistsDataAsync(_host);
            return Ok(results);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> ExportChemistsDataByUser(string userName)
        {
            var results = await _unitOfWork.Chemist.ExportChemistsDataByUserAsync(_host, userName);
            return Ok(results);
        }
    }
}