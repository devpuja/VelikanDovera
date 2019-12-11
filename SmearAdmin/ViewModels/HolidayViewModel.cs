using System;

namespace SmearAdmin.ViewModels
{
    public class HolidayViewModel
    {
        public int ID { get; set; }
        public string FestivalName { get; set; }
        public DateTime FestivalDate { get; set; }
        public string FestivalDay { get; set; }
        public string FestivalDescription { get; set; }
        public int? IsNationalFestival { get; set; }
        public int? BelongToCommunity { get; set; }
        public string CommunityName { get; set; }
    }
}
