using System.Collections.Generic;

namespace SmearAdmin.ViewModels
{
    public class AdminDashboardViewModel
    {
        public string SendSMSToName { get; set; }
        public int Count { get; set; }

    }

    public class AdminDashboardViewModelDTO
    {
        public List<AdminDashboardViewModel> SendSMSToNameItems { get; set; }
        public int SMSBalance { get; set; }
    }
}
