using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace ITEquipment.Models
{
    public class EquipmentList
    {
        public IEnumerable<EquipmentViewModel> equipmentViewModels;
    }
    public class EquipmentViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SerialNo { get; set; }
        public string ModelNo { get; set; }
        public string Image { get; set; }
        public int LocationId { get; set; }
        public string VideoUrl { get; set; }
        public int StatusId { get; set; }
        public string DocumentUrl { get; set; }
        public string Manufacturer { get; set; }
        public string AssetsTag { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedDateTime { get; set; }

    }

    public class AccessoriesList
    {
        public IEnumerable<AccessoriesViewModel> accessoriesViewModels;
    }
    public class AccessoriesViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [DisplayName("Model No")]
        public string ModelNo { get; set; }

        [Required(ErrorMessage = "SerialNo is required.")]
        [DisplayName("SerialNo No")]
        public string SerialNo { get; set; }
    }
    public class AccessoryListModel
    {
        public int noOfRecords { get; set; }
        public List<AccessoriesViewModel> AccessoryList { get; set; }
    }
    public class Bookingviewmodel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
        public string LocationName { get; set; }
        public string StatusType { get; set; }
        public string DocumentUrl { get; set; }
        public string VideoUrl { get; set; }
        public string Image { get; set; }
        public string PhoneNo { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string BookingStatus { get; set; }

    }
    public class BookingList
    {
        public List<Bookingviewmodel> bookingviewmodels { get; set; }
    }

    public class EquipmentModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [DisplayName("Model No")]
        public string ModelNo { get; set; }
        [Required(ErrorMessage = "SerialNo is required.")]
        [DisplayName("SerialNo No")]
        public string SerialNo { get; set; }
        [Required]
        [DisplayName("Location")]
        public int LocationId { get; set; }

        [DisplayName("Location")]
        public string LocationName { get; set; }
        public string Image { get; set; }
        public string VideoUrl { get; set; }
        [Required]
        [DisplayName("Status")]
        public int StatusId { get; set; }
        public string DocumentUrl { get; set; }
        public string StatusType { get; set; }
        [Required]
        [DisplayName("Manufacturer")]
        public int ManufacturerId { get; set; }
        [DisplayName("Manufacturer")]
        public string ManufacturerName { get; set; }

        [Required]
        [DisplayName("Assets Tag")]
        public string AssetsTag { get; set; }
    }
    public class EquipmentListModel
    {
        public int noOfRecords { get; set; }
        public List<EquipmentModel> EquipmentList { get; set; }
    }
    public class EquipmentDTOModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [DisplayName("Model No")]
        public string ModelNo { get; set; }
        [Required(ErrorMessage = "SerialNo is required.")]
        [DisplayName("Serial No")]
        public string SerialNo { get; set; }
        public string ImagePath { get; set; }
        public IFormFile? Image { get; set; }
        [Required]
        [DisplayName("Location")]
        public int LocationId { get; set; }
        public IFormFile? Video { get; set; }
        public string VideoPath { get; set; }
        public IFormFile? Document { get; set; }
        public string DocumentPath { get; set; }
        [Required]
        [DisplayName("Status")]
        public int StatusId { get; set; }


        [DisplayName("Location")]
        public string LocationName { get; set; }

        public string StatusType { get; set; }
        [Required]
        [DisplayName("Manufacturer")]
        public int ManufacturerId { get; set; }
        [DisplayName("Manufacturer")]
        public string ManufacturerName { get; set; }
        [Required]
        [DisplayName("Assets Tag")]
        public string AssetsTag { get; set; }

        public List<AccessoryListViewModel> AssetsList { get; set; }


    }
    public class AccessoryListDTO
    {
        public string AccessoryName { get; set; }
    }
    public class AccessoryListViewModel
    {
        public int Id { get; set; }
        public int EquipmentId { get; set; }
        public string AccessoryName { get; set; }
    }

    public class AccessoryDTO
    {
        //public int EquipmentId { get; set; }
        public string AccessoryName { get; set; }
    }
    public class DashboardModel
    {
        public List<DashboardCountList> AllCountList { get; set; }
        public List<EquipmentStatuswiseCountList> EquipmentStausWiseCountList { get; set; }
    }

    public class DashboardCountList
    {
        public int EquipmentsList { get; set; }
        public int AsseccoryList { get; set; }
        public int UsersList { get; set; }
        public int BookingList { get; set; }
    }

    public class EquipmentStatuswiseCountList
    {
        public string StatusType { get; set; }
        public int RecordCount { get; set; }

    }
    public class TopUsedEquipmentCountModal
    {
        public string Name { get; set; }
        public int BookingCount { get; set; }

    }
}

