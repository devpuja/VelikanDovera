using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmearAdmin.Helpers;
using SmearAdmin.Persistence;
using SmearAdmin.ViewModels;
using static SmearAdmin.Helpers.Constants;

namespace SmearAdmin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AdminDashboardController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IUnitOfWork _unitOfWork;

        private readonly IHostingEnvironment _host;
        private IConverter _converter;

        public AdminDashboardController(IMapper mapper, IUnitOfWork UnitOfWork, IConverter converter, IHostingEnvironment host)
        {
            this.mapper = mapper;
            _unitOfWork = UnitOfWork;
            _converter = converter;
            _host = host;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetAllSubmitNotification()
        {
            var results = await _unitOfWork.AdminDashboard.GetAllSubmitNotification();
            return Ok(results);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetAllUserName()
        {
            return Ok(await _unitOfWork.AdminDashboard.GetAllUserName());
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetEmployeeExpensesInMonth(string UserName, string MonthYear)
        {
            return Ok(await _unitOfWork.AdminDashboard.GetEmployeeExpensesInMonth(UserName, MonthYear));
        }

        [HttpPut]
        [Route("[action]")]
        public async Task<IActionResult> SaveEmployeeExpenses(EmployeeExpensesViewModel employeeExpensesVM)
        {
            try
            {
                //Find data
                var origionalData = _unitOfWork.EmployeeExpenses.GetSingleOrDefault(u => u.Id == employeeExpensesVM.ID);

                //update Raw Pwd
                if (origionalData != null)
                {
                    origionalData.ApprovedAmount = employeeExpensesVM.ApprovedAmount;
                    origionalData.ApproverRemark = employeeExpensesVM.ApproverRemark;
                    origionalData.ApprovedBy = employeeExpensesVM.ApprovedBy;
                    _unitOfWork.EmployeeExpenses.Update(origionalData);
                }

                await _unitOfWork.CompleteAsync();
            }
            catch (Exception ex) { throw ex; }
            return Ok(employeeExpensesVM);
        }

        [HttpPut]
        [Route("[action]")]
        public async Task<IActionResult> CloseEmployeeExpenses(EmployeeExpensesStatusViewModel empExpStatusVM)
        {
            var originalData = _unitOfWork.EmployeeExpensesStatus.GetSingleOrDefault(s => s.UserName == empExpStatusVM.UserName && s.ExpenseMonth == empExpStatusVM.ExpenseMonth);
            if (originalData != null)
            {
                originalData.Status = (int)EmployeeExpensesStatus.Approved;
                _unitOfWork.EmployeeExpensesStatus.Update(originalData);
                await _unitOfWork.CompleteAsync();
            }
            return Ok(originalData);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GeneratePdfEmployeeExpense(string UserName, string MonthYear)
        {
            byte[] file = null;
            string fileName = "", fullfilePath = "";
            PDFViewModel pdfVM = null;
            try
            {
                //Get PDF HTML
                var getPdfHtml = await _unitOfWork.AdminDashboard.GetEmployeeExpenseForPDF(UserName, MonthYear);

                //Set FileName
                fileName = $"{UserName}_{ MonthYear}_{DateTime.Now.ToString("ddmmyyyy")}{DateTime.Now.Millisecond}.pdf";

                //Generate PDF
                GeneratePDF generatePDF = new GeneratePDF(_converter, _host);
                file = generatePDF.GetPdfFile(fileName, getPdfHtml, "PDF Report", "For PSR", "Signature of PSR");

                fullfilePath = Path.Combine(_host.WebRootPath, "Downloads", fileName);

                pdfVM = new PDFViewModel()
                {
                    File = file.ToArray(),
                    FileName = fileName
                };
            }
            catch (Exception Ex) { throw Ex; }
            //return File(file.ToArray(), "application/pdf", fileName);
            return Ok(pdfVM);
        }
    }
}