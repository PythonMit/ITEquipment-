using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITEquipment.Models
{
    public class MailModel
    {

    }
    public class EmailFormModel
    {
        public string Subject { get; set; }
        public string Message { get; set; }
        public string ToAddress { get; set; }
        public string CcAddress { get; set; }
        public string BccAddress { get; set; }

        #region Approved or reject booking
        public string EquName { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Comment { get; set; }
        public string ReqType { get; set; }
        public string UserName { get; set; }
        #endregion
    }
}
