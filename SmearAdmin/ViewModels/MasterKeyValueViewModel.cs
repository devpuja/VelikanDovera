using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmearAdmin.Data;
using SmearAdmin.Helpers;

namespace SmearAdmin.ViewModels
{
    public class MasterKeyValueViewModel
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public string Type { get; set; }
    }

    public class MasterKeyValueResult<T>
    {
        public IEnumerable<T> ItemsDep { get; set; }
        public IEnumerable<T> ItemsDes { get; set; }
        public IEnumerable<T> ItemsGrade { get; set; }
        public IEnumerable<T> ItemsHQ { get; set; }
        public IEnumerable<T> ItemsRegion { get; set; }

        public IEnumerable<T> ItemsPresentType { get; set; }
        public MasterKeyValue ItemsDaily { get; set; }
        public MasterKeyValue ItemsBike { get; set; }
        public MasterKeyValue HQ { get; set; }
        public IEnumerable<string> ItemsRegionName { get; set; }

        public IEnumerable<T> ItemsQualification { get; set; }
        public IEnumerable<T> ItemsSpeciality { get; set; }
        public IEnumerable<T> ItemsBrand { get; set; }
        public IEnumerable<T> ItemsClass { get; set; }
        public IEnumerable<T> ItemsBestDayToMeet { get; set; }
        public IEnumerable<T> ItemsBestTimeToMeet { get; set; }
        public IEnumerable<T> ItemsGender { get; set; }
        //public MasterKeyValue ItemsVisitFrequency { get; set; }
        public string ItemVisitFrequency { get; set; }
        public IEnumerable<T> ItemsCommunity { get; set; }
    }
}
