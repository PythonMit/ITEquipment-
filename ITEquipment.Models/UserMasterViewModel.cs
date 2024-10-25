using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ITEquipment.Models.Consts.EnumConst;

namespace ITEquipment.Models
{
    public class UserMasterViewModel
    {

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNo { get; set; }
        public string FCMToken { get; set; }
        //public int BookingId { get; set; }
        public string DeviceId { get; set; }
        public string Password { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime? ModifiedDateTime { get; set; }
        public string AppVersion { get; set; }
        public string Os { get; set; }
        public int RoleId { get; set; }

    }
   public class Role
    {
        public RoleType roleId { get; set; }
    }
    public class UserMasterDTOModel
    {

        public int Id { get; set; }
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        public string Email { get; set; }
        [DisplayName("Phone No")]
        public string PhoneNo { get; set; }
        public string FCMToken { get; set; }
        //public int BookingId { get; set; }
        public string DeviceId { get; set; }
        public string AppVersion { get; set; }
        public string Os { get; set; }
        [DisplayName("Select Role")]
        public int RoleId { get; set; }
        [DisplayName("Role")]
        public string Name { get; set; }

    }
    public class UserListModel
    {
        public int noOfRecords { get; set; }
        public List<UserMasterDTOModel> userMasters { get; set; }
    }
}
