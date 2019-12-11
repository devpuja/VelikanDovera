using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SmearAdmin.Data;
using SmearAdmin.Persistence;
using SmearAdmin.ViewModels;

namespace SmearAdmin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MastersController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IUnitOfWork _unitOfWork;

        public MastersController(IMapper mapper, IUnitOfWork UnitOfWork)
        {
            this.mapper = mapper;
            _unitOfWork = UnitOfWork;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<MasterKeyValue> AddMaster(MasterKeyValueViewModel masterKeyValueVM)
        {
            var mstKeyVal = mapper.Map<MasterKeyValueViewModel, MasterKeyValue>(masterKeyValueVM);
            _unitOfWork.MasterKeyValues.Add(mstKeyVal);
            await _unitOfWork.CompleteAsync();
            return await Task.FromResult(mstKeyVal);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetAllMasters(string masterFor, int pageIndex, int pageSize)
        {
            var results = await _unitOfWork.MasterKeyValues.GetAllMastersAsync(masterFor, pageIndex, pageSize);
            return Ok(results);
        }

        [HttpDelete]
        [Route("[action]")]
        //public async Task<IActionResult> DeleteMaster(int ID) => (await _unitOfWork.MasterKeyValues.Remove(ID); ) ? (IActionResult)Ok() : NoContent();
        public async Task<IActionResult> DeleteMaster(int ID)
        {
            var data = _unitOfWork.MasterKeyValues.GetSingleOrDefault(m => m.Id == ID);
            if (data != null)
            {
                _unitOfWork.MasterKeyValues.Remove(data);
                await _unitOfWork.CompleteAsync();
            }
            return Ok(data);
        }

        [HttpPut]
        [Route("[action]")]
        public async Task<MasterKeyValue> EditMaster(MasterKeyValueViewModel masterKeyValueVM)
        {
            MasterKeyValue mstKeyVal = new MasterKeyValue();
            try
            {
                var origionalData = _unitOfWork.MasterKeyValues.GetSingleOrDefault(m => m.Id == masterKeyValueVM.Id);
                if (origionalData != null)
                {
                    mstKeyVal = mapper.Map<MasterKeyValueViewModel, MasterKeyValue>(masterKeyValueVM);
                    _unitOfWork.MasterKeyValues.Update(origionalData, mstKeyVal);
                    await _unitOfWork.CompleteAsync();
                }
            }
            catch (Exception Ex) { throw Ex; }

            return await Task.FromResult(mstKeyVal);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<MasterKeyValue> GetMastersByID(int ID)
        {
            var data = _unitOfWork.MasterKeyValues.GetSingleOrDefault(m => m.Id == ID);
            return await Task.FromResult(data);
        }

        //[HttpGet]
        //[Route("[action]")]
        //public async Task<(List<MasterKeyValue> dep, List<MasterKeyValue> des, List<MasterKeyValue> grade, List<MasterKeyValue> region)> GetMastersForEmployee()
        //{
        //    return await _unitOfWork.MasterKeyValues.GetMastersForEmployeeAsync();            
        //}
    }
}