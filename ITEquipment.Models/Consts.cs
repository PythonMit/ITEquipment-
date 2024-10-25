using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITEquipment.Models
{
 public class Consts
    {
        public class EnumConst
        {
            public enum RoleType
            {

                Admin = 1,
                User = 2,
                Readonly = 3
                
            }
            public enum StatusType
            {

                Available = 1,
                InUse = 2,
                Damage = 3,
                InRepair = 4,
                NotAvailable=5,
                Requested=6,
                Returned=1006
            }
        }
    }
}
