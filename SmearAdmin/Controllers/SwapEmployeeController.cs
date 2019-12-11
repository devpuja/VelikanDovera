using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmearAdmin.Persistence;
using SmearAdmin.ViewModels;

namespace SmearAdmin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SwapEmployeeController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IUnitOfWork _unitOfWork;

        public SwapEmployeeController(IMapper mapper, IUnitOfWork UnitOfWork)
        {
            this.mapper = mapper;
            _unitOfWork = UnitOfWork;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<List<SwapEmployeeViewModel>> GetSwapEmployeeList()
        {
            return await _unitOfWork.Employee.GetSwapEmployeeListAsync();
        }

        [HttpPut]
        [Route("[action]")]
        public async Task<IActionResult> SwapEmployeeUserName(SwapEmployeeViewModel swapEmp)
        {
            var response = await _unitOfWork.Employee.SwapEmployeeUserNamesAsync(swapEmp.SwapFrom, swapEmp.SwapTo);
            return Ok(response);
        }        
    }
}