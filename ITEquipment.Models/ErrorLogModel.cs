using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITEquipment.Models
{
    public class ErrorLogModel
    {
        public int ErrorLogID { get; set; }
        public string ErrorGUID { get; set; }
        public string ErrorURL { get; set; }
        public string ErrorMessage { get; set; }
        public string InnerException { get; set; }
        public string StackTrace { get; set; }
        public string TargetSite { get; set; }
        public string ErrorSource { get; set; }
        public string BrowserDetails { get; set; }
        public string CookiesDetails { get; set; }
        public string IPAddress { get; set; }
        public string UserSession { get; set; }
        public DateTime CreatedDatetime { get; set; }
    }
    public class ErrorLogInsertViewModel
    {
        public Int64 ErrorLogID { get; set; }
        public string ErrorGUID { get; set; }
    }
    public class MessageModel
    {
        public MessageModel()
        {
            Success = false;
            Time = 5000;
            MessageType = "Error";
        }
        public bool Success { get; set; }
        public string ResultMessage { get; set; }
        public int Time { get; set; }
        public string MessageType { get; set; }
        public object Value { get; set; }
    }
}
