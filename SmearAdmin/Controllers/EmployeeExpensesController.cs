using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmearAdmin.Data;
using SmearAdmin.Helpers;
using SmearAdmin.Models;
using SmearAdmin.Persistence;
using SmearAdmin.ViewModels;
using static SmearAdmin.Helpers.Constants;

namespace SmearAdmin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeExpensesController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IUnitOfWork _unitOfWork;

        private readonly UserManager<AppUser> _userManager;

        private readonly IHostingEnvironment _host;
        private IConverter _converter;

        public EmployeeExpensesController(IMapper mapper, IUnitOfWork UnitOfWork, UserManager<AppUser> userManager, IConverter converter, IHostingEnvironment host)
        {
            this.mapper = mapper;
            _unitOfWork = UnitOfWork;

            _userManager = userManager;

            _converter = converter;
            _host = host;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetEmployeeAllowance(string ID)
        {
            var results = await _unitOfWork.EmployeeExpenses.GetEmployeeAllowanceDetails(ID);
            return Ok(results);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> AddEmployeeExpenses(EmployeeExpensesViewModel empExpVM)
        {
            try
            {
                var newDate = empExpVM.Date.Value.AddDays(1);
                //var monthYear = $"{empExpVM.Date.Value.Month.ToString()}-{empExpVM.Date.Value.Year.ToString()}";
                var monthYear = $"{newDate.Month.ToString()}-{newDate.Year.ToString()}";
                var iExpenseMonth = _unitOfWork.EmployeeExpensesStatus.GetAll()
                    .Where(me => me.ExpenseMonth.Equals(monthYear) && me.UserName.Equals(empExpVM.UserName)).Count();

                if (iExpenseMonth <= 0)
                {
                    EmployeeExpensesStatusViewModel empExp = new EmployeeExpensesStatusViewModel()
                    {
                        ExpenseMonth = monthYear,
                        UserName = empExpVM.UserName,
                        Status = (int)EmployeeExpensesStatus.NotSubmitted
                    };

                    var expensesStatus = mapper.Map<ExpensesStatus>(empExp);
                    _unitOfWork.EmployeeExpensesStatus.Add(expensesStatus);
                }

                if (empExpVM.PresentType.Equals(PresentType.HALFDAY))
                {
                    empExpVM.BikeAllowance /= 2;
                    empExpVM.DailyAllowance /= 2;
                }
                else if (empExpVM.PresentType.Equals(PresentType.LEAVE))
                {
                    empExpVM.BikeAllowance = 0;
                    empExpVM.DailyAllowance = 0;
                    empExpVM.OtherAmount = 0;
                }

                var tempAmt = empExpVM.BikeAllowance + empExpVM.DailyAllowance + empExpVM.OtherAmount;
                empExpVM.ClaimAmount = tempAmt;
                empExpVM.ApprovedAmount = tempAmt;
                empExpVM.ExpenseMonth = monthYear;
                empExpVM.Date = newDate;
                var empExpenses = mapper.Map<Expenses>(empExpVM);
                _unitOfWork.EmployeeExpenses.Add(empExpenses);

                await _unitOfWork.CompleteAsync();
            }
            catch (Exception Ex) { throw Ex; }
            return Ok();
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetAllEmployeeExpenses(string UserName, string MonthYear)
        {
            var results = await _unitOfWork.EmployeeExpenses.GetAllEmployeeExpensesInMonth(UserName, MonthYear);
            return Ok(results);
        }

        [HttpDelete]
        [Route("[action]")]
        public async Task<IActionResult> DeleteEmployeeExpenses(int ID)
        {
            var data = _unitOfWork.EmployeeExpenses.GetSingleOrDefault(m => m.Id == ID);
            if (data != null)
            {
                _unitOfWork.EmployeeExpenses.Remove(data);
                await _unitOfWork.CompleteAsync();
            }
            return Ok(data);
        }

        [HttpPut]
        [Route("[action]")]
        public async Task<IActionResult> ChangeEmployeeExpenses(EmployeeExpensesStatusViewModel empExpStatusVM)
        {
            var originalData = _unitOfWork.EmployeeExpensesStatus.GetSingleOrDefault(s => s.UserName == empExpStatusVM.UserName && s.ExpenseMonth == empExpStatusVM.ExpenseMonth);
            if (originalData != null)
            {
                originalData.Status = (int)EmployeeExpensesStatus.Submitted;
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
            return Ok(pdfVM);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetAllNotification(string UserName)
        {
            return Ok(await _unitOfWork.EmployeeExpenses.GetAllNotification(UserName));
        }
    }
}