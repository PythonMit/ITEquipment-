using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITEquipment.Utilities
{
    public class ModuleList
    {
        public const string USP_EQUIPMENT_STATUS = "GetStatusTypes";
        public const string USP_EQUIPMENT_ADDORUPDATE = "AddOrUpdateEquipment";
        public const string USP_EQUIPMENT_DELETE = "DeleteEquipment";
        public const string USP_EQUIPMENT_GETALLEQUIPMENT = "GetAllEquipment";
        public const string USP_EQUIPMENT_GETALLACCESSORIESBYEQUIPMENTID = "GetAllAccessoriesByEquipmentId";
        public const string USP_EQUIPMENT_GETUSERDETAILSBYID = "GetUserDetailsById";
        public const string USP_EQUIPMENT_UPDATEUSERDETAILS = "UpdateUserDetails";
        public const string USP_EQUIPMENT_EQUIPMENTBOOKING = "EquipmentBooking";
        public const string USP_EQUIPMENT_GETALLBOOKINGBYUSERID = "GetAllBookingByUserId";
        public const string USP_EQUIPMENT_BOOKINGRESCHEDULE = "EquipmentBookingReschedule";
        public const string USP_EQUIPMENT_BOOKINgEXTEND = "EquipmentBookingExtend";
        public const string USP_EQUIPMENT_BOOKINGCANCEL = "EquipmentBookingCancel";
        public const string USP_EQUIPMENT_GETSETTING = "GetSetting";
        public const string USP_GLOBAL_SAVEERRORLOG = "Global_SaveErrorLog";
        public const string USP_GLOBAL_GETUSERBYPARAMETERS = "GetUserByEmailOrPassword";
        public const string USP_GLOBAL_GETUSERBYEMAIL = "GetUserByEmail";
        public const string USP_LOCATIONS = "GetLocations";
        public const string USP_ROLES = "GetAllRoles";
        public const string USP_MANFACTURERS = "GetAllManufacturer";

        #region Accessories
        public const string USP_MANAGEACCESSORIES = "ManageAccessories";
        public const string USP_GETACCESSORIES = "GetAccessoriesMasterList";
        #endregion

        #region Equipment
        public const string USP_MANAGEEQUIPMENT = "ManageEquipment";
        public const string USP_GETEQUIPMENT = "GetEquipmentMasterList";
        public const string USP_DELETEEQUIPMENTFILE = "DeleteEquipmentFiles";
        public const string USP_COUNTLIST = "DashboardCountList";
        public const string USP_EQUIPMENTSTATUS = "GetEqipmentStatusMaster";
        public const string USP_UPDATEEQUIPMENTSTATUS = "UpdateEquipmentStatus";
        #endregion

        #region user
        public const string USP_MANAGEUSER = "GetUserMasterList";
        public const string USP_GETUSERDETAILSBYID = "WEB_GetUserDetailsById";
        public const string USP_UPDATEUSERROLE = "UpdateUserRole";
        #endregion

        #region Booking
        public const string USP_MANAGEBOOKING = "GetBookingMasterList";
        //public const string USP_UPDATEBOOKINGSTATUS = "UpdateBookingSataus";
        public const string usp_UpdateBookingStatus = "usp_UpdateBookingStatus";
        public const string USP_BOOKINGHISTORY = "GetBookingHistory";
        #endregion

        #region hangfire scheduler
        public const string USP_OVERDUEBOOKINGTIGGER = "GetExpiredBookings";
        #endregion

        public const string USP_EQUIPMENT_GETEQUIPMENTSTATUSWISECOUNTLIST = "GetEquipmentStatuswiseCountList";
        public const string USP_EQUIPMENT_GETTOPUSEDEQUIPMENTCOUNTLIST = "GetTopUsedEquipmentCountList";
        public const string USP_ACCESSORYMAPPING_DELETE = "DeleteAccessoryMapping";

        #region Manufacturer

        public const string USP_GETMANUFACTURER = "GetManufacturerMasterList";
        public const string USP_MANAGEMANUFACTURERS = "ManageManufacturers";
        #endregion

        #region LocationMaster
        public const string USP_GETLOCATION = "GeLocationMasterList";
        public const string USP_MANAGELOCATIONS = "ManageLocation";
        #endregion
    }
}
