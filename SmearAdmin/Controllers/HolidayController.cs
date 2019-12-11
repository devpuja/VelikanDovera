using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SmearAdmin.Data;
using SmearAdmin.Helpers;
using SmearAdmin.Persistence;
using SmearAdmin.ViewModels;
using System;
using System.Threading.Tasks;

namespace SmearAdmin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HolidayController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IUnitOfWork _unitOfWork;

        public HolidayController(IMapper mapper, IUnitOfWork UnitOfWork)
        {
            this.mapper = mapper;
            _unitOfWork = UnitOfWork;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<MasterKeyValueResult<MasterKeyValue>> GetMastersForHoliday()
        {
            return await _unitOfWork.MasterKeyValues.GetMastersForHolidayAsync();
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> AddHoliday(HolidayViewModel holidayVM)
        {
            try
            {
                //Add Holiday
                holidayVM.FestivalDate = Utilities.FormatDateTimeByZone(holidayVM.FestivalDate);
                holidayVM.FestivalDay = Utilities.FormatDateTimeByZone(holidayVM.FestivalDate).ToString("dddd");
                var Holiday = mapper.Map<HolidayList>(holidayVM);                
                _unitOfWork.Holiday.Add(Holiday);

                await _unitOfWork.CompleteAsync();
            }
            catch (Exception Ex)
            {
                return new BadRequestObjectResult(Errors.AddError(Ex, ModelState));
            }
            return new OkObjectResult(holidayVM);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetAllHolidays(int pageIndex, int pageSize)
        {
            var results = await _unitOfWork.Holiday.GetAllHolidayAsync(pageIndex, pageSize);
            return Ok(results);
        }

        [HttpDelete]
        [Route("[action]")]
        public async Task<IActionResult> DeleteHoliday(int ID)
        {
            try
            {
                var holidayData = _unitOfWork.Holiday.GetSingleOrDefault(h => h.Id == ID);
                if (holidayData != null)
                {
                    _unitOfWork.Holiday.Remove(holidayData);
                    await _unitOfWork.CompleteAsync();
                }
                return Ok(holidayData);
            }
            catch (Exception Ex)
            {
                return new BadRequestObjectResult(Errors.AddError(Ex, ModelState));
            }
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetHoliday(string ID)
        {
            var results = await _unitOfWork.Holiday.GetHolidayByIDAsync(ID);
            return Ok(results);
        }

        [HttpPut]
        [Route("[action]")]
        public async Task<IActionResult> EditHoliday(HolidayViewModel holidayVM)
        {
            try
            {
                //Update Holiday
                var HolidayData = _unitOfWork.Holiday.GetSingleOrDefault(d => d.Id == holidayVM.ID);
                if (HolidayData != null)
                {
                    HolidayData.FestivalName = holidayVM.FestivalName;
                    HolidayData.FestivalDate = Utilities.FormatDateTimeByZone(holidayVM.FestivalDate);
                    HolidayData.FestivalDay = Utilities.FormatDateTimeByZone(holidayVM.FestivalDate).ToString("dddd");
                    HolidayData.FestivalDescription = holidayVM.FestivalDescription;
                    HolidayData.IsNationalFestival = holidayVM.IsNationalFestival;
                    HolidayData.BelongToCommunity = holidayVM.BelongToCommunity;
                }
                _unitOfWork.Holiday.Update(HolidayData);

                await _unitOfWork.CompleteAsync();
            }
            catch (Exception Ex)
            {
                return new BadRequestObjectResult(Errors.AddError(Ex, ModelState));
            }
            return new OkObjectResult(holidayVM);
        }
    }
}