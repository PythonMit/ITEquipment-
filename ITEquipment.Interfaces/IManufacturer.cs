using ITEquipment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITEquipment.Interfaces
{
    public interface IManufacturer
    {
        Task<ManufacturerNameListModel> GetAllManufacturer(int DisplayLength, int DisplayStart, string SortColumnName, string SortDirection, string SearchText, string ManufacturerName);
        Task<bool> AddManufacturer(ManufacturerModel manufacturerModel);
        Task<bool> UpdateManufacturer(ManufacturerModel manufacturerModel);
        Task<bool> RemoveManufacturer(int ManufacturerId);
        Task<ManufacturerModel> GetManufacturerById(int ManufacturerId);
        //Task<IEnumerable<AccessoriesViewModel>> SearchAccessory(string serchData);
    }
}
