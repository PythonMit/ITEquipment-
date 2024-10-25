using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITEquipment.Models
{
    public class StatusList
    {
        public int Id { get; set; }
        public string StatusType { get; set; }
}
    public class StatusViewModel
    {
        
        public IEnumerable<StatusList> statusLists { get; set; }
    }
}
