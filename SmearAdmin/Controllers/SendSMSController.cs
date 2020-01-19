using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SmearAdmin.Helpers;
using SmearAdmin.Persistence;
using SmearAdmin.ViewModels;

namespace SmearAdmin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SendSMSController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IUnitOfWork _unitOfWork;

        public SendSMSController(IMapper mapper, IUnitOfWork UnitOfWork)
        {
            this.mapper = mapper;
            _unitOfWork = UnitOfWork;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetAllDoctorSendSMS(int pageIndex, int pageSize)
        {
            var results = await _unitOfWork.SendSMS.GetAllDoctorSendSMSAsync(pageIndex, pageSize);
            return Ok(results);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetAllDoctorSendSMSByUser(int pageIndex, int pageSize, string userName)
        {
            var results = await _unitOfWork.SendSMS.GetAllDoctorSendSMSByUserAsync(pageIndex, pageSize, userName);
            return Ok(results);
        }


        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetAllDoctorSendSMSBySearch(int pageIndex, int pageSize, string searchValue)
        {
            var results = await _unitOfWork.SendSMS.GetAllDoctorSendSMSBySearchAsync(pageIndex, pageSize, searchValue);
            return Ok(results);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> SendSMSData([FromBody] List<SMSMessageMobileViewModel> objSMSMessageMobile)
        {
            bool Msg = false;
            if (objSMSMessageMobile.Count > 0)
            {
                for (int i = 0; i <= objSMSMessageMobile.Count - 1; i++)
                {
                    var result = await SMSUtility.SendSMS(objSMSMessageMobile[i].MessageText, objSMSMessageMobile[i].MobileNumber);

                    if (!Convert.ToString(result).Equals("fail"))
                    {
                        dynamic resultData = JsonConvert.DeserializeObject(Convert.ToString(result));

                        SMSLoggerViewModel objSMSLogger = new SMSLoggerViewModel
                        {
                            MobileNumber = objSMSMessageMobile[i].MobileNumber,
                            SMSText = objSMSMessageMobile[i].MessageText,
                            SendSMSTo = "Doctor",
                            Occasion = "Manual",
                            ErrorCode = resultData.ErrorCode,
                            ErrorMessage = resultData.ErrorMessage,
                            JobID = resultData.JobId,
                            SendSMSDate = DateTime.Now,
                            MessageData = Convert.ToString(resultData.MessageData)
                        };

                        var smslogger = mapper.Map<SMSLoggerViewModel, Data.Smslogger>(objSMSLogger);
                        _unitOfWork.SendSMS.Add(smslogger);

                        await _unitOfWork.CompleteAsync();

                        Msg = true;
                    }
                }
            }

            return Ok(Msg);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetSendSMSCount()
        {
            var results = await _unitOfWork.SendSMS.GetSendSMSCountAsync();
            return Ok(results);
        }
    }
}