using System;

namespace SmearAdmin.ViewModels
{
    public class SMSLoggerViewModel
    {
        public long ID { get; set; }
        public string MobileNumber { get; set; }
        public string SMSText { get; set; }
        public string SendSMSTo { get; set; }
        public string Occasion { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public string JobID { get; set; }
        public DateTime SendSMSDate { get; set; }
        public string MessageData { get; set; }
    }
}
