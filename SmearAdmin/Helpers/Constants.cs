using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmearAdmin.Helpers
{
    public static class Constants
    {
        public static class Strings
        {
            public static class JwtClaimIdentifiers
            {
                public const string Roles = "roles", Id = "id";
            }

            public static class JwtClaims
            {
                public const string ApiAccess = "api_access";
            }
        }


        //Constant for Employee & Doctors
        public static class EmployeeConstant
        {
            public const string Department = "Department";
            public const string Desigination = "Desigination";
            public const string Grade = "Grade";
            public const string Region = "Region";
            public const string HQ = "HQ";
            public const string PresentType = "Present Type";
            public const string Daily = "Daily";
            public const string Bike = "Bike";
            public const string Mobile = "Mobile";
            public const string Fare = "Fare";
            public const string Cyber = "Cyber";
            public const string Stationery = "Stationery";

            public const string Qualification = "Qualification";
            public const string Speciality = "Speciality";
            public const string Brand = "Brand";
            public const string Class = "Class";
            public const string BestDayToMeet = "BestDayToMeet";
            public const string BestTimeToMeet = "BestTimeToMeet";
            public const string Gender = "Gender";
            public const string VisitFrequency = "VisitFrequency";
            public const string Community = "Community";
        }

        public static class DoctorConstant
        {
            public const string Qualification = "Qualification";
            public const string Speciality = "Speciality";
            public const string Brand = "Brand";
            public const string Class = "Class";
            public const string BestDayToMeet = "BestDayToMeet";
            public const string BestTimeToMeet = "BestTimeToMeet";
            public const string Gender = "Gender";
            public const string VisitFrequency = "VisitFrequency";
            public const string Community = "Community";
        }

        public static class ChemistConstant
        {
            public const string Region = "Region";            
        }

        public static class ReferenceTableNames
        {
            public const string EMPLOYEE = "aspnetusers";
            public const string DOCTOR = "doctor";
            public const string CHEMIST = "chemist";
            public const string STOCKIST = "stockist";
            public const string PATIENT = "patient";
        }

        public static class PresentType
        {
            public const string FULLDAY = "Full-Day";
            public const string HALFDAY = "Half-Day";
            public const string LEAVE = "Leave";
        }

        public enum EmployeeExpensesStatus
        {
            NotSubmitted = 0,
            Submitted = 1,
            Approved = 2,
            Rejected = 3
        }
    }
}
