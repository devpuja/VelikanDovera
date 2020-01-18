using Humanizer;
using SmearAdmin.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmearAdmin.Helpers
{
    public class GenerateHTML
    {
        GenerateHTML() { }

        public static string GetHTMLEmployeeExpense(RegistrationViewModel dataUsers, List<EmployeeExpensesViewModel> dataExpenses, int dataMobile, int dataFare,
            int dataStationery, int dataCyber)
        {
            int iExp = 1;
            string FullName = $"{dataUsers.FirstName} {dataUsers.MiddleName} {dataUsers.LastName}";
            string ApprovedBy = dataExpenses[0].ApprovedBy;

            DateTime curDt = DateTime.Now;
            var currentDt = $"{curDt.Day}-{curDt.Month}-{curDt.Year}";

            int dailyTot = 0;
            int bikeTot = 0;
            int otherTot = 0;

            int totalB = dataMobile + dataFare + dataStationery + dataCyber;
            int grandTotal = 0;

            var sb = new StringBuilder();

            sb.AppendFormat(@"<html>
<head>
    <meta charset='utf-8' />
    <title></title>    
</head>
<body style='font-family:Arial; font-size:12px;'>
    <table style='width:100%; margin-top:2px;'>
        <tr>
            <td colspan='3' style='text-align:center;'><h1>VELIKAN DOVERA INDIA PRIVATE LIMITED</h1></td>            
        </tr>
        <tr>
            <td colspan='3' style='text-align:center;'><h2>EXPENSE STATEMENT</h2></td>
        </tr>
        <tr>
            <td><b>Name:</b> {0}</td>
            <td><b>Department:</b> {1}</td>
            <td><b>Desigination:</b> {2}</td>
        </tr>
        <tr>
            <td><b>HQ:</b> {3}</td>
            <td><b>Expense Month:</b> {4}</td>
            <td><b>Date:</b> {5}</td>
        </tr>
    </table>

    <table style='width:100%;'>
        <tr>
            <td style='width:25px'>#</td>
            <td style='width:100px'>DATE</td>
            <td style='width:300px'>TOWN</td>
            <td style='width:100px'>DAILY</td>
            <td style='width:100px'>BIKE</td>
            <td style='width:100px'>OTHER</td>
            <td style='width:250px'>REMARKS</td>
        </tr>
    </table>", FullName, dataUsers.DepartmentName, dataUsers.DesiginationName, dataExpenses[0].HQName, dataExpenses[0].ExpenseMonth, currentDt);

            sb.Append(@"<table style='width: 100%'>");

            foreach (var empExp in dataExpenses)
            {
                DateTime dt = DateTime.Parse(empExp.Date.ToString());
                var dayName = dt.ToString("dddd");
                var expDt = $"{dt.Day}-{dt.Month}-{dt.Year} ({dayName})";

                sb.AppendFormat(@"
                    <tr>
                    <td style='width:25px'>{0}</td>
                    <td style='width:100px'>{1}</td>
                    <td style='width:300px'>{2}</td>
                    <td style='width:100px'>{3}</td>
                    <td style='width:100px'>{4}</td>
                    <td style='width:100px'>{5}</td>
                    <td style='width:250px'>{6}</td>
                    </tr>",
                    iExp, expDt, empExp.Region, empExp.DailyAllowance, empExp.BikeAllowance, empExp.OtherAmount, empExp.ApproverRemark
                );

                dailyTot += (int)empExp.DailyAllowance;
                bikeTot += (int)empExp.BikeAllowance;
                otherTot += (int)empExp.OtherAmount;

                iExp++;
            }

            sb.AppendFormat(@"<tr>
                    <td colspan='3'><b>TOTAL (A)</b></td>
                    <td><b>{0}</b></td>
                    <td><b>{1}</b></td>
                    <td><b>{2}</b></td>
                    <td>&nbsp;</td>
                    </tr></table>", dailyTot, bikeTot, otherTot);

            grandTotal = totalB + dailyTot + bikeTot + otherTot;

            sb.AppendFormat(@"<table style='width:100%'>
            <tr>
            <td style='width:67%'>
                <table style='width:100%'>
                    <tr>
                        <td colspan='2' style='text-align:center;'><b>ADDITIONAL ALLOWANCES (B)</b></td>
                    </tr>
                    <tr>
                        <td style='width:75%;'><b>MOBILE ALLOWANCE : </b></td>
                        <td style='width:25%; text-align:right;'><b>{0}</b></td>
                    </tr>
                    <tr>
                        <td style='width:75%;'><b>FARE ALLOWANCE : </b></td>
                        <td style='width:25%; text-align:right;'><b>{1}</b></td>
                    </tr>
                    <tr>
                        <td style='width:75%;'><b>STATIONERY : </b></td>
                        <td style='width:25%; text-align:right;'><b>{2}</b></td>
                    </tr>
                    <tr>
                        <td style='width:75%;'><b>CYBER WORK : </b></td>
                        <td style='width:25%; text-align:right;'><b>{3}</b></td>
                    </tr>
                    <tr>
                        <td style='width:75%;'><b>TOTAL (B) : </b></td>
                        <td style='width:25%; text-align:right;'><b>{4}</b></td>
                    </tr>
                    <tr>
                        <td style='width:75%;'><b>GRAND TOTAL (A+B) (In Rupees) : </b></td>
                        <td style='width:25%; text-align:right;'><b>{5}</b></td>
                    </tr>
                </table>
            </td>

            <td style='width:3%'>&nbsp;</td>

            <td style='width:30%;vertical-align: top;'>
                <table style='width:100%'>
                    <tr>
                        <td>APPROVED BY:<br /> <b>{6}</b></td>
                    </tr>
                    <tr>
                        <td style='height: 112px;vertical-align: top;'>APPROVER'S REMARKS:<br /> </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>", dataMobile, dataFare, dataStationery, dataCyber, totalB, grandTotal, ApprovedBy.ToUpper());


            sb.AppendFormat(@"<table style='width:100%'>
            <tr>
            <td style='text-align:left;'><b>GRAND TOTAL (A+B) (In Words) : {0}</b></td>
            </tr>
            </table></body></html>", Convert.ToString(grandTotal.ToWords()).ToUpper());

            return sb.ToString();
        }
    }
}
