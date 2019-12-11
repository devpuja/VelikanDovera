using System;
using System.Collections.Generic;

namespace SmearAdmin.Data
{
    public partial class HolidayList
    {
        public int Id { get; set; }
        public string FestivalName { get; set; }
        public DateTime? FestivalDate { get; set; }
        public string FestivalDay { get; set; }
        public string FestivalDescription { get; set; }
        public int? IsNationalFestival { get; set; }
        public int? BelongToCommunity { get; set; }
    }
}
