using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITEquipment.Models
{
    public class BookingEquipmentViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int EquipmentId { get; set; }
        //public int AccessoriesId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        //public int StatusId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifyBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public List<BookingAccessoriesViewModel> bookingAccessoriesViewModels { get; set; }
    }
    public class BookingAccessoriesViewModel
    {
        public int Id { get; set; }
        // public int BookingId { get; set; }
        public int AccessoriesId { get; set; }
    }
    //public class BookingAccessoriesViewModel
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}

    public class RescheduleViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BookingId { get; set; }
        public DateTime startdate { get; set; }
        public DateTime Enddate { get; set; }
        public int CreatedBy { get; set; }
        public string BookingType { get; set; }
    }
    public class ExtendBookingViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BookingId { get; set; }
        public DateTime Enddate { get; set; }
        public int CreatedBy { get; set; }
        public string BookingType { get; set; }
    }
    public class GetSettingViewModel
    {
        public int Id { get; set; }
        public string SettingName { get; set; }
        public string SettingValue { get; set; }
    }


    public class BookingDTOViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string lastName { get; set; }
        public string Name { get; set; }
        public DateTime Startdate { get; set; }
        public DateTime Enddate { get; set; }
        public string StatusType { get; set; }
        public int EquipmentId { get; set; }
        public int UserId { get; set; }
        public string Email { get; set; }
    }

    public class BookingListDTOModel
    {
        public int noOfRecords { get; set; }
        public List<BookingDTOViewModel> BookingList { get; set; }
    }

    public class BookingHistoryModel
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public int EquipmentId { get; set; }
        public int FromStatusId { get; set; }
        public int ToStatusId { get; set; }
        public string Comment { get; set; }
        public string StatusType { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }

    public class BookingHistoryList
    {
        public List<BookingHistoryModel> bookingHistoryModels { get; set; }
    }
}
