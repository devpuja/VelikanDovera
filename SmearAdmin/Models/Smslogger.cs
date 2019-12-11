using System;
using System.Collections.Generic;

namespace SmearAdmin.Models
{
    public partial class Smslogger
    {
        public long Id { get; set; }
        public string MobileNumber { get; set; }
        public string Smstext { get; set; }
        public string SendSmsto { get; set; }
        public string Occasion { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public string JobId { get; set; }
        public DateTime? SendSmsdate { get; set; }
        public string MessageData { get; set; }
    }
}
