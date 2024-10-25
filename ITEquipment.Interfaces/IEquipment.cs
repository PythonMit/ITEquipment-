using ITEquipment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ITEquipment.Models.Consts.EnumConst;

namespace ITEquipment.Interfaces
{
    public interface IEquipment
    {
        Task<bool> AddOrUpdateEquipment(EquipmentViewModel equipmentViewModel);
        Task<bool> RemoveEquipment(int equipmentId);
        Task<bool> RemoveEquipmentFile(int equipmentId, string FileType);
        Task<EquipmentList> GetAllEquipment();
        Task<AccessoriesList> GetAllAccessoriesByEquipmentId(int equipmentId);
        Task<bool> UpdateUserDetails(UserMasterViewModel userMasterViewModel);
        Task<int> BookingEquipment(BookingEquipmentViewModel bookingEquipmentViewModel);

        Task<BookingList> GetAllBookingListById(int userId);

        Task<int> BookingReschedule(RescheduleViewModel rescheduleViewModel);
        Task<int> BookingExtend(ExtendBookingViewModel extendBookingViewModel);
        Task<int> Bookingcancel(int userId, int bookingId);
        Task<UserMasterViewModel> GetUserDetailsById(int userId);

        Task<GetSettingViewModel> GetSetting();

        #region web project
        Task<EquipmentListModel> GetAllEquipmentList(int DisplayLength, int DisplayStart, string SortColumnName, string SortDirection, string SearchTextm, string EquipmentName, string AssetsTag, string Location, string Status);
        Task<bool> AddEquipment(EquipmentDTOModel accessories, string AccessoryListJson);
        Task<bool> UpdateEquipment(EquipmentDTOModel accessories, string AccessoryListJson);
        Task<bool> UpdateBookingStatus(int bookingId, int statusId, string Status = "" ,string Comment="");
        Task<bool> UpdateEquipmentStatus(int equiomentId, string StatusType);
        Task<EquipmentDTOModel> GetEquipmentById(int equipmentId);
        Task<DashboardCountList> GetDashboardcountList();
        Task<BookingListDTOModel> GetAllBookingList(int DisplayLength, int DisplayStart, string SortColumnName, string SortDirection, string SearchText, string UserSearch, string EquipmentSearch, string StatusSearch, string StartDateSearch, string EndDateSearch);

        Task<UserMasterDTOModel> ViewUserDetailsById(int userId);
        Task<List<EquipmentStatuswiseCountList>> GetEquipmentStatuswiseCountList();
        Task<List<TopUsedEquipmentCountModal>> GetTopUsedEquipmentCountList();
        Task<bool> RemoveAccessoryMapping(int Id);
        Task<BookingHistoryList> GetBookingHistory(int bookingId, int equipmentId);
        #endregion
    }
}
