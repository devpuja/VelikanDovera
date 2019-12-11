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
    public class DoctorController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHostingEnvironment _host;

        public DoctorController(IMapper mapper, IUnitOfWork UnitOfWork, IHostingEnvironment host)
        {
            this.mapper = mapper;
            _unitOfWork = UnitOfWork;
            _host = host;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<MasterKeyValueResult<MasterKeyValue>> GetMastersForDoctor()
        {
            return await _unitOfWork.MasterKeyValues.GetMastersForDoctorAsync();
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> AddDoctor(DoctorViewModel doctorVM)
        {
            try
            {
                //Add Doctor
                string doctorTableID = Guid.NewGuid().ToString();
                var doctor = mapper.Map<Doctor>(doctorVM);
                doctor.Id = doctorTableID;
                _unitOfWork.Doctors.Add(doctor);

                //Add Comman
                CommonResourceViewModel commonVM = doctorVM.Common;
                commonVM.RefTableId = doctorTableID;
                commonVM.ID = Guid.NewGuid().ToString();
                commonVM.ContactPersonDateOfBirth = Utilities.FormatDateTimeByZone(doctorVM.Common.ContactPersonDateOfBirth.Value);
                commonVM.ContactPersonDateOfAnniversary = Utilities.FormatDateTimeByZone(doctorVM.Common.ContactPersonDateOfAnniversary.Value);
                var empCommon = mapper.Map<CommonResourceViewModel, Data.ChemistStockistResourse>(commonVM);
                _unitOfWork.CommonResourse.Add(empCommon);

                //Add Contact
                ContactResourseViewModel contactVM = doctorVM.Contact;
                contactVM.RefTableId = doctorTableID;
                contactVM.ID = Guid.NewGuid().ToString();
                var empContact = mapper.Map<ContactResourseViewModel, Data.ContactResourse>(contactVM);
                _unitOfWork.ContactResource.Add(empContact);

                //Add Auditable Entity
                AuditableEntityViewModel auditVM = doctorVM.AuditableEntity;
                auditVM.ID = Guid.NewGuid().ToString();
                auditVM.RefTableId = doctorTableID;
                auditVM.CreateDate = DateTime.Now;
                if (doctorVM.AuditableEntity.FoundationDay != null)
                    auditVM.FoundationDay = Utilities.FormatDateTimeByZone(doctorVM.AuditableEntity.FoundationDay.Value);
                else
                    auditVM.FoundationDay = null;
                var empAudit = mapper.Map<AuditableEntityViewModel, Data.AuditableEntity>(auditVM);
                _unitOfWork.AuditableEntity.Add(empAudit);

                await _unitOfWork.CompleteAsync();
            }
            catch (Exception Ex)
            {
                return new BadRequestObjectResult(Errors.AddError(Ex, ModelState));
            }
            return new OkObjectResult(doctorVM);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetAllDoctors(int pageIndex, int pageSize)
        {
            var results = await _unitOfWork.Doctors.GetAllDoctorAsync(pageIndex, pageSize);
            return Ok(results);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetAllDoctorsByUser(int pageIndex, int pageSize, string userName)
        {
            var results = await _unitOfWork.Doctors.GetAllDoctorsByUserAsync(pageIndex, pageSize, userName);
            return Ok(results);
        }

        [HttpPut]
        [Route("[action]")]
        public async Task<IActionResult> DeleteDoctor(DoctorViewModel docVM, string action)
        {
            try
            {
                var origionalData = _unitOfWork.AuditableEntity.GetSingleOrDefault(e => e.RefTableId == docVM.ID && e.RefTableName == ReferenceTableNames.DOCTOR);
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

            return Ok(docVM);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetDoctor(string ID)
        {
            var results = await _unitOfWork.Doctors.GetDoctorByIDAsync(ID);
            return Ok(results);
        }

        [HttpPut]
        [Route("[action]")]
        public async Task<IActionResult> EditDoctor(DoctorViewModel docVM)
        {
            try
            {
                //Update Doctor
                var doctorData = _unitOfWork.Doctors.GetSingleOrDefault(d => d.Id == docVM.ID);
                if (doctorData != null)
                {
                    doctorData.Qualification = docVM.Qualification;
                    doctorData.RegistrationNo = docVM.RegistrationNo;
                    doctorData.Speciality = docVM.Speciality;
                    doctorData.Gender = docVM.Gender;
                    doctorData.VisitFrequency = docVM.VisitFrequency;
                    doctorData.VisitPlan = docVM.VisitPlan;
                    doctorData.BestDayToMeet = docVM.BestDayToMeet;
                    doctorData.BestTimeToMeet = docVM.BestTimeToMeet;
                    doctorData.Brand = docVM.Brand;
                    doctorData.Class = docVM.Class;
                }
                _unitOfWork.Doctors.Update(doctorData);

                //Update Common
                var commonData = _unitOfWork.CommonResourse.GetSingleOrDefault(c => c.RefTableId == docVM.ID && c.RefTableName.Equals(ReferenceTableNames.DOCTOR));
                if (commonData != null)
                {
                    commonData.ContactPersonMobileNumber = docVM.Common.ContactPersonMobileNumber;
                    commonData.ContactPersonDateOfBirth = Utilities.FormatDateTimeByZone(docVM.Common.ContactPersonDateOfBirth.Value);
                    commonData.ContactPersonDateOfAnniversary = Utilities.FormatDateTimeByZone(docVM.Common.ContactPersonDateOfAnniversary.Value);
                }
                _unitOfWork.CommonResourse.Update(commonData);

                //Update Contact
                var contactData = _unitOfWork.ContactResource.GetSingleOrDefault(c => c.RefTableId == docVM.ID && c.RefTableName.Equals(ReferenceTableNames.DOCTOR));
                if (contactData != null)
                {
                    contactData.Address = docVM.Contact.Address;
                    contactData.State = docVM.Contact.State;
                    contactData.City = docVM.Contact.City;
                    contactData.PinCode = docVM.Contact.PinCode;
                    contactData.MobileNumber = docVM.Contact.MobileNumber;
                    contactData.ResidenceNumber = docVM.Contact.ResidenceNumber;
                }
                _unitOfWork.ContactResource.Update(contactData);

                //Update Auditable Entity
                var auditableData = _unitOfWork.AuditableEntity.GetSingleOrDefault(c => c.RefTableId == docVM.ID && c.RefTableName.Equals(ReferenceTableNames.DOCTOR));
                if (auditableData != null)
                {
                    if (docVM.AuditableEntity.FoundationDay != null)
                        auditableData.FoundationDay = Utilities.FormatDateTimeByZone(docVM.AuditableEntity.FoundationDay.Value);
                    else
                        auditableData.FoundationDay = null;
                    auditableData.CommunityId = docVM.AuditableEntity.CommunityID;
                    auditableData.CreateDate = DateTime.Now;
                    auditableData.CreatedBy = docVM.AuditableEntity.CreatedBy;
                }
                _unitOfWork.AuditableEntity.Update(auditableData);

                await _unitOfWork.CompleteAsync();
            }
            catch (Exception Ex)
            {
                return new BadRequestObjectResult(Errors.AddError(Ex, ModelState));
            }
            return new OkObjectResult(docVM);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> ExportDoctorsData()
        {
            var results = await _unitOfWork.Doctors.ExportDoctorsDataAsync(_host);
            return Ok(results);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> ExportDoctorsDataByUser(string userName)
        {
            var results = await _unitOfWork.Doctors.ExportDoctorsDataByUserAsync(_host, userName);
            return Ok(results);
        }
    }
}